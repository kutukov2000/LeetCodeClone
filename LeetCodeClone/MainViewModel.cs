using PropertyChanged;
using System.Windows;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        private const string _clientSecret = "acc5789876823ef7de80d3a2dd8892bed9cc1b35 ";
        private HackerEarth HackerEarth { get; set; }
        private LeetCodeApi LeetCode { get; set; }

        public string LineNumbers { get; set; }

        private string _sourceCode;
        public string SourceCode
        {
            get { return _sourceCode; }
            set
            {
                _sourceCode = value;
                UpdateLineNumbers();
            }
        }

        public string ExecuteStatus { get; set; }
        public string[] Languages { get; }
        public string SelectedLanguage { get; set; }
        public string Input { get; set; }
        public string Result { get; set; }

        public int MemoryLimit { get; set; }
        public int TimeLimit { get; set; }
        public OutputStats OutputStats { get; set; }
        public RelayCommand RunCodeCommand { get; set; }
        public RelayCommand SourceCodeTextChanged { get; set; }
        public RelayCommand PreviewKeyDownCommand { get; set; }
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

            LineNumbers = "1";
            SourceCode = "";

            Languages = new string[] { "C", "CPP14", "CPP17", "CLOJURE", "CSHARP", "GO", "HASKELL", "JAVA8", "JAVA14", "JAVASCRIPT_NODE", "KOTLIN", "OBJECTIVEC", "PASCAL", "PERL", "PHP", "PYTHON", "PYTHON3", "PYTHON3_8", "R", "RUBY", "RUST", "SCALA", "SWIFT", "TYPESCRIPT" };
            SelectedLanguage = Languages[17];

            MemoryLimit = 262144;
            TimeLimit = 5;

            OutputStats = new OutputStats();

            LeetCode = new LeetCodeApi();

            GetProblems();
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
        public async Task LoadSelectedProblemAsync()
        {

            SelectedProblem.Description = await LeetCode.GetProblemDetailAsync(SelectedProblem.QuestionTitleSlug);

            if (string.IsNullOrEmpty(SelectedProblem.Description) || string.IsNullOrWhiteSpace(SelectedProblem.Description))
            {
                SelectedProblem.Description = @"<p>Is paid only problem</p>";
            }
        }
        public async Task GetProblems()
        {
            Problems = await LeetCode.GetProblemsAsync();
            SelectedProblem = Problems[121];
        }
        public async Task RunCode()
        {
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

                    if (OutputStats.MemoryUsed > MemoryLimit)
                    {
                        MessageBox.Show("Memory Limit Exceeded");
                    }
                    if (OutputStats.TimeUsed > TimeLimit)
                    {
                        MessageBox.Show("Time Limit Exceeded");
                    }
                    break;
                }
            }
        }
    }
}