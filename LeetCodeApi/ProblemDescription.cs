namespace LeetCodeClone
{
    public class ProblemDescription
    {
        private const string _htmlStart = @"<!DOCTYPE html><html><head><style>
                                  body {
                                    background-color:#3a3a3a;
                                    color: white;
                                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                  }
                                </style></head><body>";
        private const string _htmlEnd = @"</body></html>";
        public required string Title { get; set; }
        public required string Content { get; set; }
        public override string ToString()
        {
            string body = Title + Content;
            if (Title.Length == body.Length)
            {
                string isPainOnlyProblem = "It`s paid only problem";
                return _htmlStart + isPainOnlyProblem + _htmlEnd;
            }
            return _htmlStart + Title + Content + _htmlEnd;
        }
    }
}
