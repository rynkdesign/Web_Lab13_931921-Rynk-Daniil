namespace lab_13.Models
{
    public class QuizResultModel
    {
        public List<QuizQuestionModel> Questions { get; set; }

        public int CorrectAnswerCount
        {
            get
            {
                return Questions.Count(q => q.AnswerIsCorrect);
            }
        }

        public int Count => Questions.Count;
    }
}