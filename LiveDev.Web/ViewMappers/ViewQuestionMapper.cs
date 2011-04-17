using System;
using System.Collections.Generic;
using System.Linq;
using LiveDev.Domain;
using LiveDev.Infrastructure.Repositories;
using LiveDev.Web.Models;

namespace LiveDev.Web.ViewMappers
{
    public class ViewQuestionMapper
    {
        private QuestionsRepository _questionsRepository;

        public ViewQuestionMapper()
        {
            _questionsRepository = new QuestionsRepository();
        }

        public ViewQuestionMapper(QuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public virtual ViewQuestion GetViewQuestion(Question question)
        {
            return new ViewQuestion
                       {
                           Id = question.Id,
                           TextQuestion = question.TextQuestion,
                           SourceCode = question.GetContractDefinitionSourceCode()
                       };
        }

        public virtual Question GetQuestion(ViewQuestion viewQuestion)
        {
            Question mappedQuestion = _questionsRepository.GetById(viewQuestion.Id);
            mappedQuestion.SourceCode = viewQuestion.SourceCode;
            return mappedQuestion;
        }

        public virtual IList<ViewQuestion> GetViewQuestionsFromList(IEnumerable<Question> questionList)
        {
            return questionList.Select(GetViewQuestion).ToList();
        }
    }
}