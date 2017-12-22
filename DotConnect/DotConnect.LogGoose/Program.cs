using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DotConnect.LogGoose
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome();
            var searchInfo = WaitingForSearchInfo(args);
            FullLookup(searchInfo);
            Console.WriteLine("Wanna try a new keyword? (y/n)");
            var answer = Console.ReadLine();
            while (answer == "y")
            {
                Console.WriteLine("Looking for? ");
                searchInfo.SearchTerm = Console.ReadLine();
                FullLookup(searchInfo);
                Console.WriteLine("Wanna try a new keyword? (y/n)");
                answer = Console.ReadLine();
            }
        }

        private static void Welcome()
        {
            Console.WriteLine("*****************  Goose is waiting to catch fish *********************");
            Console.WriteLine();
        }

        private static SearchInfo WaitingForSearchInfo(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Enter full path to the search location (a folder): ");
            }
            var path = args.Length > 0 ? args[0] : Console.ReadLine();
            while (!Directory.Exists(path))
            {
                Console.WriteLine("Invalid path. Try again! ");
                path = Console.ReadLine();
            }

            Console.WriteLine("Looking for? ");
            var searchTerm = Console.ReadLine();
            return new SearchInfo
            {
                Location = path,
                SearchTerm = searchTerm
            };
        }

        private static void FullLookup(SearchInfo searchInfo)
        {
            var root = new DirectoryInfo(searchInfo.Location);
            foreach (var result in Looking(root, searchInfo.SearchTerm))
            {
                PrintResult(result);
            }
            
            foreach (var directoryInfo in root.EnumerateDirectories())
            {
                FullLookup(new SearchInfo{Location = directoryInfo.FullName, SearchTerm = searchInfo.SearchTerm});                
            }            
        }

        private static void PrintResult(SearchResult topLevel)
        {
            Console.WriteLine($"BINGO!");
            WriteColor($"Line: {topLevel.LineNumber} File: {topLevel.FileName}");
            WriteColor(topLevel.FullText);
        }

        private static void WriteColor(string text)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = color;
        }

        private static IEnumerable<SearchResult> Looking(DirectoryInfo dicInfo, string searchTerm)
        {
            var pattern = new Regex(searchTerm, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            foreach (var file in dicInfo.EnumerateFiles())
            {
                Console.WriteLine($"Looking at: {file.FullName} ");
                using (var stream = File.OpenRead(file.FullName))
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        var lineNumber = 1;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line) && pattern.IsMatch(line))
                            {
                               yield return new SearchResult
                                {
                                    LineNumber = lineNumber,
                                    FileName = file.FullName,
                                    FullText = line
                                };
                            }

                            lineNumber++;
                        }
                    }
                }
            }
        }
    }
}
