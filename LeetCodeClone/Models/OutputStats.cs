using PropertyChanged;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class OutputStats
    {
        public int MemoryUsed { get; set; }
        public float TimeUsed { get; set; }
        public string? ExecuteStatus { get; set; }
        public string? Result { get; set; }
        public int DockPanelHeight { get; set; } = 0;
    }
}