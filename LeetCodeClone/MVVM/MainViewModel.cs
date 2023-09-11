using LeetCodeClone.Models;
using PropertyChanged;
using System.Windows;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        private const string _clientSecret = "acc5789876823ef7de80d3a2dd8892bed9cc1b35 ";
        private HackerEarth HackerEarth;
        private LeetCodeApi LeetCode;

        public RelayCommand RunCodeCommand { get; }
        public OutputStats OutputStats { get; set; }
        public InputStats InputStats { get; set; }
        public List<LeetCodeProblem>? Problems { get; set; }

        private LeetCodeProblem? _selectedProblem;
        public LeetCodeProblem SelectedProblem
        {
            get => _selectedProblem;
            set
            {
                _selectedProblem = value;
                LoadSelectedProblemAsync();
            }
        }

        public MainViewModel()
        {
            HackerEarth = new HackerEarth(_clientSecret);
            LeetCode = new LeetCodeApi();

            InputStats = new InputStats();
            OutputStats = new OutputStats();

            RunCodeCommand = new RelayCommand(o => { RunCode(); });

            GetProblemsAsync();
        }

        private async Task LoadSelectedProblemAsync()
        {
            SelectedProblem.Description = await LeetCode.GetProblemDetailAsync(SelectedProblem.QuestionTitleSlug);
        }
        private async Task GetProblemsAsync()
        {
            Problems = await LeetCode.GetProblemsAsync();
            SelectedProblem = Problems[121];
        }
        private async Task RunCode()
        {
            OutputStats.ExecuteStatus = string.Empty;

            string id = await HackerEarth.ExecuteCodeAsync(InputStats);

            while (true)
            {
                OutputStats.ExecuteStatus = await HackerEarth.GetStatusAsync(id);
                if (OutputStats.ExecuteStatus == RequestStatus.INITIATED ||
                    OutputStats.ExecuteStatus == RequestStatus.QUEUED ||
                    OutputStats.ExecuteStatus == RequestStatus.COMPILED)
                    continue;
                else if (OutputStats.ExecuteStatus == RequestStatus.FAILED)
                {
                    MessageBox.Show("REQUEST_FAILED");
                    break;
                }
                else if (OutputStats.ExecuteStatus == RequestStatus.COMPLETED)
                {
                    string output = await HackerEarth.GetOutputStringAsync(id);
                    OutputStats = await HackerEarth.GetStatsAsync(id);
                    OutputStats.Result = await HackerEarth.GetOutputAsync(output);

                    if (OutputStats.MemoryUsed > InputStats.MemoryLimit)
                    {
                        MessageBox.Show("Memory Limit Exceeded");
                    }
                    if (OutputStats.TimeUsed > InputStats.TimeLimit)
                    {
                        MessageBox.Show("Time Limit Exceeded");
                    }
                    break;
                }
            }
        }
    }
}