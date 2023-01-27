using Microsoft.AspNetCore.Mvc;
using lab_13.Models;
using lab_13.Utils;

namespace lab_13.Controllers
{
    public class MockupsController : Controller
    {
        private int QuestionCount
        {
            get
            {
                return HttpContext.Session.Get<int>(nameof(QuestionCount)) switch
                {
                    < 0 => throw new Exception(" Invalid question count"),
                    { } count => count
                };
            }
        }

        private QuizQuestionModel NextQuestion
        {
            get
            {
                var question = QuizQuestionModel.RandomQuizQuestion;

                var count = QuestionCount;
                HttpContext.Session.Set($"Question{count}", question);
                count += 1;
                HttpContext.Session.Set(nameof(QuestionCount), count);

                return question;
            }
        }

        private QuizQuestionModel LastQuestion
        {
            get
            {
                var count = QuestionCount - 1;
                return HttpContext.Session.Get<QuizQuestionModel>($"Question{count}");
            }
        }

        private QuizResultModel Result
        {
            get
            {
                var result = new QuizResultModel { Questions = new() };
                for (var i = 0; i < QuestionCount; i++)
                {
                    var question = HttpContext.Session.Get<QuizQuestionModel>($"Question{i}");
                    result.Questions.Add(question);
                }

                return result;
            }
        }

        private void SaveAnswer(int? answer)
        {
            var lastQuestion = LastQuestion;
            lastQuestion.UserAnswer = answer;
            HttpContext.Session.Set($"Question{QuestionCount - 1}", lastQuestion);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Quiz()
        {
            var question = QuestionCount switch
            {
                0 => NextQuestion,
                _ => LastQuestion
            };
            ViewBag.Question = question.ToString();
            return View();
        }

        [HttpPost]
        public IActionResult Quiz(QuizAnswerModel answerModel, string action)
        {
            if (ModelState.IsValid)
            {
                if (answerModel.Answer < -10)
                {
                    ModelState.AddModelError("Answer", $"  {answerModel.Answer} Too low number for this");
                    ViewBag.Question = LastQuestion;
                    return View();
                }
                SaveAnswer(answerModel.Answer);

                if (action == "Next")
                {
                    ViewBag.Question = NextQuestion;
                    return RedirectToAction("Quiz");
                }
                else
                {
                    return RedirectToAction("QuizResult");
                }
            }
            else
            {
                ViewBag.Question = LastQuestion;
                return View();
            }
        }
        public IActionResult QuizResult()
        {
            var result = Result;

            HttpContext.Session.Clear();

            return View(result);
        }
    }
}