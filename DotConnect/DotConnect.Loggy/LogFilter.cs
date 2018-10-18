using System;
using System.Collections.Generic;
using System.Globalization;
using TimeSpan = System.TimeSpan;

namespace DotConnect.Loggy
{
    public class LogFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Location { get; set; }
        public string SearchPattern { get; set; } = "*.*";
        /// <summary>
        /// Filter transactions ran longer than a certain value (TimeSpan).
        /// </summary>
        public TimeSpan RunLongerThan { get; set; } = TimeSpan.Zero;

        public IList<string> Transactions { get; set; } = new List<string>();

        public bool HasFilter => RunLongerThan != TimeSpan.Zero || Transactions.Count > 0;

        /// <summary>
        /// Constructs the filter from an array of arguments passed from console parameters
        /// -from
        /// -to
        /// -location
        /// -runLongerThan
        /// -trans (a comma separated list if expected transactions)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static LogFilter FromArgs(string[] args)
        {
            var filter = new LogFilter();
            for (var i = 0; i < args.Length - 1; i++)
            {
                var current = args[i].ToLowerInvariant();
                var next = args[i + 1];
                switch (current)
                {
                    case "-from":
                        if (!string.IsNullOrWhiteSpace(next))
                        {
                            filter.From = DateTime.ParseExact(next, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        break;
                    case "-to":
                        if (!string.IsNullOrWhiteSpace(next))
                        {
                            filter.To = DateTime.ParseExact(next, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        break;
                    case "-location":
                        filter.Location = next;
                        break;
                    case "-pattern":
                        filter.SearchPattern = next;
                        break;
                    case "-runlongerthan":
                        if (!string.IsNullOrWhiteSpace(next))
                        {
                            filter.RunLongerThan = TimeSpan.Parse(next);
                        }
                        break;
                    case "-trans":
                        if (!string.IsNullOrWhiteSpace(next))
                        {
                            filter.Transactions = next.Split(new[] {",", ";"}, StringSplitOptions.RemoveEmptyEntries);
                        }
                        break;
                }
            }
            if (string.IsNullOrWhiteSpace(filter.SearchPattern))
                filter.SearchPattern = "*.*";
            return filter;
        }
    }
}