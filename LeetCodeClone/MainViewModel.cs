using PropertyChanged;
using System.Windows;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        private const string _clientSecret = "Put your client secret here";
        private HackerEarth HackerEarth { get; set; }
        public string SourceCode { get; set; }

        public string[] Languages { get; }
        public string SelectedLanguage { get; set; }
        public string Result { get; set; }

        public int MemoryLimit { get; set; }
        public int TimeLimit { get; set; }

        public RelayCommand RunCodeCommand { get; set; }

        public MainViewModel()
        {
            HackerEarth = new HackerEarth(_clientSecret);
            RunCodeCommand = new RelayCommand(o => { RunCode(); });

            Languages = new string[] { "C", "CPP14", "CPP17", "CLOJURE", "CSHARP", "GO", "HASKELL", "JAVA8", "JAVA14", "JAVASCRIPT_NODE", "KOTLIN", "OBJECTIVEC", "PASCAL", "PERL", "PHP", "PYTHON", "PYTHON3", "PYTHON3_8", "R", "RUBY", "RUST", "SCALA", "SWIFT", "TYPESCRIPT" };
            SelectedLanguage = Languages[17];

            MemoryLimit = 262144;
            TimeLimit = 5;
        }
        public async Task RunCode()
        {
            MessageBox.Show($"Source code {SourceCode}");
            RequestBody data = new RequestBody
            {
                Lang = SelectedLanguage,
                Source = SourceCode,
                MemoryLimit = MemoryLimit,
                TimeLimit = TimeLimit
            };
            string id = await HackerEarth.ExecuteCodeAsync(data);

            (string status, string output) = await HackerEarth.GetStatusAsync(id);

            switch (status)
            {
                case "AC": MessageBox.Show($"Accepted. The code executed successfully.\nOutput string: {output}"); break;
                case "MLE": MessageBox.Show($"Memory Limit Exceeded.\nOutput string: {output}"); break;
                case "TLE": MessageBox.Show($"Time Limit Exceeded.\nOutput string: {output}"); break;
                case "RE": MessageBox.Show($"FAIL.\nOutput string: {output}"); break;
                default: break;
            }
            if (status == "AC")
            {
                Result = await HackerEarth.GetOutputAsync(output);
                await HackerEarth.GetStatsAsync(id);
            }
        }
    }
}