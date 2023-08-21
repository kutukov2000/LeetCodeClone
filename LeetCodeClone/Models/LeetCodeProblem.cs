using PropertyChanged;

namespace LeetCodeClone
{
    [AddINotifyPropertyChangedInterface]
    class LeetCodeProblem
    {
        public required string QuestionTitleSlug { get; set; }
        public required string QuestionTitle { get; set; }
        public int DifficultyLevel { get; set; }
        public LeetCodeProblemDescription? Description { get; set; }
        //public string? Description { get; set; }
    }
}