using HackerEarthApi.JsonModels;
using LeetCodeClone.Models;
using PropertyChanged;
using System.Windows;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        public Visibility LoadingButtonVisibility { get; set; }

        public RelayCommand RunCodeCommand { get; }
        public OutputStats OutputStats { get; set; }
        public InputStats InputStats { get; set; }
        public List<Problem>? Problems { get; set; }

        private Problem? _selectedProblem;
        public Problem SelectedProblem
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
            LoadingButtonVisibility = Visibility.Collapsed;

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
            LoadingButtonVisibility = Visibility.Visible;

            OutputStats.ExecuteStatus = string.Empty;

            string id = string.Empty;

            try
            {
                id = await HackerEarth.ExecuteCodeAsync(InputStats.ToRequestBody());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingButtonVisibility = Visibility.Collapsed;
                return;
            }

            while (true)
            {
                HackerEarthApiOutput hackerEarthApiOutput = await HackerEarth.GetOutputStatsAsync(id);

                OutputStats.SetOutputStats(hackerEarthApiOutput);

                switch (OutputStats.ExecuteStatus)
                {
                    case RequestStatus.INITIATED:
                    case RequestStatus.QUEUED:
                    case RequestStatus.COMPILED: continue;

                    case RequestStatus.FAILED:
                        MessageBox.Show("REQUEST_FAILED");
                        LoadingButtonVisibility = Visibility.Collapsed;
                        return;

                    case RequestStatus.COMPLETED:
                        OutputStats.Result = await HackerEarth.GetOutputAsync(hackerEarthApiOutput.OutputString);
                        CheckLimits();
                        LoadingButtonVisibility = Visibility.Collapsed;
                        return;
                }
            }
        }
        private void CheckLimits()
        {
            if (OutputStats.MemoryUsed > InputStats.MemoryLimit)
            {
                MessageBox.Show("Memory Limit Exceeded");
            }
            if (OutputStats.TimeUsed > InputStats.TimeLimit)
            {
                MessageBox.Show("Time Limit Exceeded");
            }
        }
    }
}