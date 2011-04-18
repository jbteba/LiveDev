using System.Web.Mvc;
using LiveDev.Domain;
using LiveDev.Infrastructure.Processes;
using LiveDev.Infrastructure.Repositories;
using LiveDev.Web.Models;
using LiveDev.Web.ViewMappers;

namespace LiveDev.Web.Controllers
{
    public class ExamsController : Controller
    {
        private readonly QuestionsRepository _questionsRepository;
        private readonly CorrectionProcess _correctionProcess;
        private readonly ViewQuestionMapper _viewQuestionMapper;
        
        public ExamsController()
        {
            _questionsRepository = new QuestionsRepository();
            _correctionProcess = new CorrectionProcess();
            _viewQuestionMapper = new ViewQuestionMapper();
        }
        
        public ExamsController(QuestionsRepository questionsRepository, CorrectionProcess correctionProcess, ViewQuestionMapper viewQuestionMapper)
        {
            _questionsRepository = questionsRepository;
            _correctionProcess = correctionProcess;
            _viewQuestionMapper = viewQuestionMapper;
        }

        public ActionResult Index()
        {
            var allQuestions = _questionsRepository;
            return View(_viewQuestionMapper.GetViewQuestionsFromList(allQuestions));
        }

        public ActionResult Resolve(int id)
        {
            Question question = _questionsRepository.GetById(id);
            return View(_viewQuestionMapper.GetViewQuestion(question));
        }

        [HttpPost]
        public ActionResult Resolve(ViewQuestion viewQuestion)
        {
            Question questionWithAnswer = _viewQuestionMapper.GetQuestion(viewQuestion);
            var correctionResult = _correctionProcess.CheckAnswer(questionWithAnswer);
            if (correctionResult.HasErrors())
            {
                ViewBag.Errors = correctionResult.Errors;
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
            else
            {
                return RedirectToAction("Result");
            }
            return View(viewQuestion);
        }

        public ActionResult Result()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Question question)
        {
            _questionsRepository.Add(question);
            return RedirectToAction("Index");
        }

    }
}
