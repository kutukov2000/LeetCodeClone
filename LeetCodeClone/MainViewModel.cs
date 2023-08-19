using PropertyChanged;
using System.Windows;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        private const string _clientSecret = "Put your secret key here ";
        private HackerEarth HackerEarth { get; set; }
        private LeetCodeApi LeetCode { get; set; }

        public string LineNumbers { get; set; }

        public string SourceCode { get; set; }
        public string ExecuteStatus { get; set; }
        public string[] Languages { get; }
        public string SelectedLanguage { get; set; }
        public string Input { get; set; }
        public string Result { get; set; }

        public int MemoryLimit { get; set; }
        public int TimeLimit { get; set; }
        public OutputStats OutputStats { get; set; }
        public RelayCommand RunCodeCommand { get; set; }
        public List<Problem> Problems { get; set; }
        private Problem _selectedProblem;

        public Problem SelectedProblem
        {
            get { return _selectedProblem; }
            set
            {
                _selectedProblem = value;
                LoadSelectedProblemAsync();
            }
        }

        public MainViewModel()
        {
            HackerEarth = new HackerEarth(_clientSecret);
            RunCodeCommand = new RelayCommand(o => { RunCode(); });

            Languages = new string[] { "C", "CPP14", "CPP17", "CLOJURE", "CSHARP", "GO", "HASKELL", "JAVA8", "JAVA14", "JAVASCRIPT_NODE", "KOTLIN", "OBJECTIVEC", "PASCAL", "PERL", "PHP", "PYTHON", "PYTHON3", "PYTHON3_8", "R", "RUBY", "RUST", "SCALA", "SWIFT", "TYPESCRIPT" };
            SelectedLanguage = Languages[17];

            MemoryLimit = 262144;
            TimeLimit = 5;

            OutputStats = new OutputStats();

            LeetCode = new LeetCodeApi();

            GetProblems();
        }
        public async Task LoadSelectedProblemAsync()
        {
            SelectedProblem.Description = await LeetCode.GetProblemDetailAsync(SelectedProblem.QuestionTitleSlug);

            if (string.IsNullOrEmpty(SelectedProblem.Description))
            {
                SelectedProblem.Description = "Is paid only problem";
            }
        }
        public async Task GetProblems()
        {
            Problems = await LeetCode.GetProblemsAsync();
            SelectedProblem = Problems[121];
        }
        public async Task RunCode()
        {
            MessageBox.Show("We run code");
            RequestBody data = new RequestBody
            {
                Lang = SelectedLanguage,
                Source = SourceCode,
                MemoryLimit = MemoryLimit,
                TimeLimit = TimeLimit,
                Input = Input
            };
            string id = await HackerEarth.ExecuteCodeAsync(data);

            while (true)
            {
                ExecuteStatus = await HackerEarth.GetStatusAsync(id);
                if (ExecuteStatus == "REQUEST_INITIATED" || ExecuteStatus == "REQUEST_QUEUED" || ExecuteStatus == "CODE_COMPILED")
                    continue;
                else if (ExecuteStatus == "REQUEST_FAILED")
                {
                    MessageBox.Show("REQUEST_FAILED");
                    break;
                }
                else if (ExecuteStatus == "REQUEST_COMPLETED")
                {
                    string output = await HackerEarth.GetOutputStringAsync(id);
                    Result = await HackerEarth.GetOutputAsync(output);
                    OutputStats = await HackerEarth.GetStatsAsync(id);
                    break;
                }
            }

            //switch (status)
            //{
            //    case "AC": MessageBox.Show($"Accepted. The code executed successfully.\nOutput string: {output}"); break;
            //    case "MLE": MessageBox.Show($"Memory Limit Exceeded.\nOutput string: {output}"); break;
            //    case "TLE": MessageBox.Show($"Time Limit Exceeded.\nOutput string: {output}"); break;
            //    case "RE": MessageBox.Show($"FAIL.\nOutput string: {output}"); break;
            //    default: break;
            //}
        }
    }
}