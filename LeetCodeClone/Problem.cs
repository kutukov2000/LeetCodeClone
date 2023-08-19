﻿namespace LeetCodeClone
{
    class Problem
    {
        public required string QuestionTitleSlug { get; set; }
        public required string QuestionTitle { get; set; }
        public int DifficultyLevel { get; set; }

        public override string ToString()
        {
            return $"{QuestionTitle} - {DifficultyLevel}";
        }
    }
}