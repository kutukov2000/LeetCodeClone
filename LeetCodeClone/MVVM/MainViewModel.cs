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

            string requestId = await SendRequest();

            bool isRequesFinished = false;

            while (!isRequesFinished && requestId is not null)
            {
                var hackerEarthApiOutput = await HackerEarth.GetOutputStatsAsync(requestId);

                OutputStats.SetOutputStats(hackerEarthApiOutput);

                isRequesFinished = await HandleRequest(hackerEarthApiOutput);
            }
        }

        private async Task<string?> SendRequest()
        {
            try
            {
                return await HackerEarth.ExecuteCodeAsync(InputStats.ToRequestBody());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingButtonVisibility = Visibility.Collapsed;
                return null;
            }
        }

        private async Task<bool> HandleRequest(HackerEarthApiOutput hackerEarthApiOutput)
        {
            switch (OutputStats.ExecuteStatus)
            {
                case RequestStatus.INITIATED:
                case RequestStatus.QUEUED:
                case RequestStatus.COMPILED: return false;

                case RequestStatus.FAILED:
                    MessageBox.Show("REQUEST_FAILED");
                    LoadingButtonVisibility = Visibility.Collapsed;
                    return true;

                case RequestStatus.COMPLETED:
                    OutputStats.Result = await HackerEarth.GetOutputStringAsync(hackerEarthApiOutput.OutputString);
                    CheckLimits();
                    LoadingButtonVisibility = Visibility.Collapsed;
                    return true;
            }

            return false;
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