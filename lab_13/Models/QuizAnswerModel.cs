using lab_13.Utils;

namespace lab_13.Models
{
    public class QuizAnswerModel
    {
        [LessThanOrNull(101, ErrorMessage = " There cannot be such a large number")]
        public int? Answer { get; set; }
    }
}