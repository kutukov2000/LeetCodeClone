using Newtonsoft.Json;
using PropertyChanged;

namespace LeetCodeClone.Models
{
    [AddINotifyPropertyChangedInterface]
    class InputStats
    {
        [JsonIgnore]
        private string? _sourceCode;
        [JsonProperty("source")]
        public string SourceCode
        {
            get => _sourceCode;
            set
            {
                _sourceCode = value;
                UpdateLineNumbers();
            }
        }
        [JsonIgnore]
        public string LineNumbers { get; set; }
        [JsonIgnore]
        public string[] Languages { get; }
        [JsonProperty("lang")]
        public string SelectedLanguage { get; set; }
        [JsonProperty("memory_limit")]
        public int MemoryLimit { get; set; }
        [JsonProperty("time_limit")]
        public int TimeLimit { get; set; }
        [JsonProperty("input")]
        public string? Input { get; set; }
        public InputStats()
        {
            LineNumbers = "1";

            Languages = new string[] { "C", "CPP14", "CPP17", "CLOJURE", "CSHARP", "GO", "HASKELL", "JAVA8", "JAVA14", "JAVASCRIPT_NODE", "KOTLIN", "OBJECTIVEC", "PASCAL", "PERL", "PHP", "PYTHON", "PYTHON3", "PYTHON3_8", "R", "RUBY", "RUST", "SCALA", "SWIFT", "TYPESCRIPT" };
            SelectedLanguage = Languages[17];

            MemoryLimit = 262144;
            TimeLimit = 5;
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