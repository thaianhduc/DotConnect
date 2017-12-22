namespace DotConnect.LogGoose
{
    public class SearchResult
    {
        public SearchResult()
        {
            Found = true;
        }
        public int LineNumber { get; set; }
        public string FileName { get; set; }
        public bool Found { get; set; }
        public string FullText { get; set; }

        public static SearchResult NotFound()
        {
            return new SearchResult{Found = false};
        }
    }
}