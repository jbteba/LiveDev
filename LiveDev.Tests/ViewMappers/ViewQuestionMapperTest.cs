using System.Collections.Generic;
using AtentoFramework2008.TestTools.Helpers;
using LiveDev.Domain;
using LiveDev.Infrastructure.Repositories;
using LiveDev.Web.Models;
using LiveDev.Web.ViewMappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LiveDev.Tests.ViewMappers
{
    [TestClass]
    public class ViewQuestionMapperTest
    {
        [TestMethod]
        public void ViewQuestionMapper_InitializesQuestionsRepository()
        {
            var viewQuestionMapper = new ViewQuestionMapper();
            Assert.IsNotNull(viewQuestionMapper.GetFieldValue<QuestionsRepository>("_questionsRepository"));
        }

        [TestMethod]
        public void GetViewQuestion_MapsIdProperty()
        {
            var mockQuestion = MockRepository.GenerateMock<Question>();
            mockQuestion.Id = 1;

            var viewQuestionMapper = new ViewQuestionMapper();
            ViewQuestion result = viewQuestionMapper.GetViewQuestion(mockQuestion);

            Assert.AreEqual(mockQuestion.Id, result.Id);
        }

        [TestMethod]
        public void GetViewQuestion_MapsTextQuestionProperty()
        {
            var mockQuestion = MockRepository.GenerateMock<Question>();
            mockQuestion.TextQuestion = "textQuestion";

            var viewQuestionMapper = new ViewQuestionMapper();
            ViewQuestion result = viewQuestionMapper.GetViewQuestion(mockQuestion);

            Assert.AreEqual(mockQuestion.TextQuestion, result.TextQuestion);
        }
        
        [TestMethod]
        public void GetViewQuestion_MapsSourceCodeProperty()
        {
            var question =
                new Question(new Definition
                                 {ClassName = "stubClass", MethodName = "stubMethod", ReturnValue = "stubReturnValue"});

            var viewQuestionMapper = new ViewQuestionMapper();
            ViewQuestion result = viewQuestionMapper.GetViewQuestion(question);

            Assert.AreEqual(question.GetContractDefinitionSourceCode(), result.SourceCode);
        }

        [TestMethod]
        public void GetQuestion_CallsGetQuestion()
        {
            var mockQuestionsRepository = MockRepository.GenerateMock<QuestionsRepository>();
            mockQuestionsRepository.Stub(s => s.GetById(Arg<int>.Is.Anything)).Return(
                MockRepository.GenerateStub<Question>());
            var stubViewQuestion = MockRepository.GenerateStub<ViewQuestion>();
            stubViewQuestion.Id = 1;

            var viewQuestionMapper = new ViewQuestionMapper(mockQuestionsRepository);
            viewQuestionMapper.GetQuestion(stubViewQuestion);

            mockQuestionsRepository.AssertWasCalled(m=>m.GetById(stubViewQuestion.Id));
        }

        [TestMethod]
        public void GetQuestion_MapsSourceCodeProperty()
        {
            var stubQuestionsRepository = MockRepository.GenerateStub<QuestionsRepository>();
            stubQuestionsRepository.Stub(s => s.GetById(Arg<int>.Is.Anything)).Return(
                MockRepository.GenerateStub<Question>());
            var stubViewQuestion = MockRepository.GenerateStub<ViewQuestion>();
            stubViewQuestion.SourceCode = "stubSourceCode";

            var viewQuestionMapper = new ViewQuestionMapper(stubQuestionsRepository);
            var mappedQuestion = viewQuestionMapper.GetQuestion(stubViewQuestion);

            Assert.AreEqual(stubViewQuestion.SourceCode, mappedQuestion.SourceCode);
        }

        [TestMethod]
        public void GetViewQuestionsFromList_ReturnsViewQuestionElementsFromQuestionList()
        {
            var viewQuestionMapper = new ViewQuestionMapper();

            var questionOne = new Question
                                  {
                                      ContractDefinition =
                                          new Definition
                                              {
                                                  ClassName = "stubClass1",
                                                  MethodName = "stubMethod1",
                                                  ReturnValue = "stubReturnValue1"
                                              },
                                      Id = 1,
                                      TextQuestion = "textQuestion1"
                                  };
            var questionTwo = new Question
                                  {
                                      ContractDefinition =
                                          new Definition
                                              {
                                                  ClassName = "stubClass2",
                                                  MethodName = "stubMethod2",
                                                  ReturnValue = "stubReturnValue2"
                                              },
                                      Id = 2,
                                      TextQuestion = "textQuestion2"
                                  };
            IList<ViewQuestion> result =
                viewQuestionMapper.GetViewQuestionsFromList(new List<Question> {questionOne, questionTwo});

            Assert.AreEqual(questionOne.Id, result[0].Id);
            Assert.AreEqual(questionOne.TextQuestion, result[0].TextQuestion);
            Assert.AreEqual(questionOne.GetContractDefinitionSourceCode(), result[0].SourceCode);
            Assert.AreEqual(questionTwo.Id, result[1].Id);
            Assert.AreEqual(questionTwo.TextQuestion, result[1].TextQuestion);
            Assert.AreEqual(questionTwo.GetContractDefinitionSourceCode(), result[1].SourceCode);
        }
    }
}

