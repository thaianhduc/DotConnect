using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DotConnect.Loggy
{
    class Program
    {
        static void Main(string[] args)
        {
            var filter = LogFilter.FromArgs(args);
            if (string.IsNullOrWhiteSpace(filter.Location))
                filter.Location = AppDomain.CurrentDomain.BaseDirectory;
            var files = Directory.GetFiles(filter.Location, filter.SearchPattern, SearchOption.AllDirectories);
            if (filter.From.HasValue)
            {
                files = files.Where(x => new FileInfo(x).LastWriteTime.Date >= filter.From.Value).ToArray();
            }
            if (filter.To.HasValue)
            {
                files = files.Where(x => new FileInfo(x).LastWriteTime.Date <= filter.To.Value).ToArray();
            }

            var result = BuildExpression(filter, files);

            var sw = Stopwatch.StartNew();
            Console.WriteLine("Hold on .... and smile!");
            var defaultColor = Console.ForegroundColor;
            int totalTransaction = 0;
            int totalExecution = 0;
            double totalTime = 0;
            foreach (var r in result)
            {
                totalExecution += r.NumberOfExecution;
                totalTime += r.TotalExecutionTime;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(r.TransactionType);
                Console.ForegroundColor = defaultColor;
                Console.WriteLine("      {0} times - in {1}", r.NumberOfExecution, TimeSpan.FromMilliseconds(r.TotalExecutionTime));
                Console.WriteLine("");
                totalTransaction++;
            }
            sw.Stop();
            Console.WriteLine("Total transaction: {0}. Average: {2}. Done in: {1}", 
                totalTransaction, sw.Elapsed,
                TimeSpan.FromMilliseconds(totalTime/totalExecution));
            Console.WriteLine("Press any key to exit ...");
        }

        private static IEnumerable<PipelineResult> BuildExpression(LogFilter filter, string[] files)
        {
            if (filter.HasFilter)
            {
                return from file in files
                    select ReadFile(file)
                    into results
                    from r in results
                    where IsMatched(r, filter)
                    select r;
            }

            return from file in files
                select ReadFile(file)
                into results
                from p in results
                group p by p.TransactionType
                into g
                select new PipelineResult
                {
                    TransactionType = g.Key,
                    TotalExecutionTime = g.Sum(x => x.TotalExecutionTime),
                    NumberOfExecution = g.Sum(x => x.NumberOfExecution)
                };
        }

        private static bool IsMatched(PipelineResult result, LogFilter filer)
        {
            bool matched = true;
            if (filer.RunLongerThan != TimeSpan.Zero)
                matched = TimeSpan.FromMilliseconds(result.TotalExecutionTime) >= filer.RunLongerThan;
            if (filer.Transactions.Count > 0)
            {
                matched = matched && filer.Transactions.Any(t => result.TransactionType.IndexOf(t, StringComparison.InvariantCultureIgnoreCase) != -1);
            }
            return matched;
        }

        private static IEnumerable<PipelineResult> ReadFile(string filePath)
        {
            using (var st = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(st, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var tuple = ParseLine(line);
                            yield return new PipelineResult{TransactionType = tuple.Item1, TotalExecutionTime = tuple.Item2, NumberOfExecution = 1};
                        }
                    }
                }

            }
        }

        private static Tuple<string, int> ParseLine(string line)
        {
            var items = line.Split(new[] {",", ";"}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].IndexOf("urn:", StringComparison.InvariantCulture) > -1)
                {
                    return Tuple.Create(items[i], int.Parse(items[i+1].Trim()));
                }
            }

            return Tuple.Create("", 0);
        }

        private static void ProcessFile(string file, int ms)
        {
            using (var st = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(st, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var pieces = line.Split(new[] {",", ";"}, StringSplitOptions.RemoveEmptyEntries);
                            int v = 100;
                            int.TryParse(pieces[pieces.Length - 2], out v);
                            if (v >= ms)
                            {
                                Console.WriteLine(line);
                            }
                        }
                    }
                }

            }
        }
    }
}
