using PropertyChanged;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class HackerEarthOutputStats
    {
        public int MemoryUsed { get; set; }
        public float TimeUsed { get; set; }
        public int DockPanelHeight { get; set; } = 0;
    }
}