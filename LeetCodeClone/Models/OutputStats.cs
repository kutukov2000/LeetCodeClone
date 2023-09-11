using HackerEarthApi.JsonModels;
using PropertyChanged;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    public class OutputStats
    {
        public int MemoryUsed { get; set; }
        public float TimeUsed { get; set; }
        public string? ExecuteStatus { get; set; }
        public string? Result { get; set; }

        public void SetOutputStats(HackerEarthApiOutput hackerEarthApiOutput)
        {
            MemoryUsed = hackerEarthApiOutput.MemoryUsed;
            TimeUsed = hackerEarthApiOutput.TimeUsed;
            ExecuteStatus = hackerEarthApiOutput.ExecutionStatus;
        }
    }
}