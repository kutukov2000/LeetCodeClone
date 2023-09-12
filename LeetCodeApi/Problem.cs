using PropertyChanged;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    public class Problem
    {
        public required string QuestionTitleSlug { get; set; }
        public required string QuestionTitle { get; set; }
        public int DifficultyLevel { get; set; }
        public ProblemDescription? Description { get; set; }
    }
}