using HackerEarthApi.JsonModels;
using PropertyChanged;

namespace LeetCodeClone.Models
{
    [AddINotifyPropertyChangedInterface]
    public class InputStats
    {
        private string? _sourceCode;
        public string SourceCode
        {
            get
            {
                return _sourceCode;
            }
            set
            {
                _sourceCode = value;
                UpdateLineNumbers();
            }
        }
        public string LineNumbers { get; set; }
        public string[] Languages { get; }
        public string SelectedLanguage { get; set; }
        public int MemoryLimit { get; set; }
        public int TimeLimit { get; set; }
        public string? Input { get; set; }
        public InputStats()
        {
            LineNumbers = "1";

            Languages = new string[] { "C", "CPP14", "CPP17", "CLOJURE", "CSHARP", "GO", "HASKELL", "JAVA8", "JAVA14", "JAVASCRIPT_NODE", "KOTLIN", "OBJECTIVEC", "PASCAL", "PERL", "PHP", "PYTHON", "PYTHON3", "PYTHON3_8", "R", "RUBY", "RUST", "SCALA", "SWIFT", "TYPESCRIPT" };
            SelectedLanguage = Languages[17];

            MemoryLimit = 262144;
            TimeLimit = 5;
        }
        public RequestBody ToRequestBody()
        {
            return new RequestBody
            {
                Language = this.SelectedLanguage,
                Source = this.SourceCode,
                Input = this.Input,
                MemoryLimit = this.MemoryLimit,
                TimeLimit = this.TimeLimit
            };
        }
        private void UpdateLineNumbers()
        {
            if (string.IsNullOrEmpty(SourceCode))
            {
                return;
            }
            LineNumbers = string.Empty;

            int lineCount = SourceCode.Split('\n').Length;

            for (int i = 1; i <= lineCount; i++)
            {
                LineNumbers += i + "\n";
            }
        }
    }
}