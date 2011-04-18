using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using LiveDev.Domain;
using LiveDev.Infrastructure.Processes;
using LiveDev.Infrastructure.Repositories;
using LiveDev.Web.Controllers;
using LiveDev.Web.Models;
using LiveDev.Web.ViewMappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using AtentoFramework2008.TestTools.Helpers;

namespace LiveDev.Tests.Controllers
{
    [TestClass]
    public class ExamsControllerTest
    {
        [TestMethod]
        public void ExamsController_InitializesQuestionsRepository()
        {
            var examsController = new ExamsController();
            Assert.IsNotNull(examsController.GetFieldValue<QuestionsRepository>("_questionsRepository"));
        }

        [TestMethod]
        public void ExamsController_InitializesCorrectionProcess()
        {
            var examsController = new ExamsController();
            Assert.IsNotNull(examsController.GetFieldValue<CorrectionProcess>("_correctionProcess"));
        }

        [TestMethod]
        public void ExamsController_InitializesViewQuestionMapper()
        {
            var examsController = new ExamsController();
            Assert.IsNotNull(examsController.GetFieldValue<ViewQuestionMapper>("_viewQuestionMapper"));
        }

        [TestMethod]
        public void Index_GetsViewQuestionObjects()
        {
            var mockViewQuestionMapper = MockRepository.GenerateMock<ViewQuestionMapper>();
            var stubQuestionsRepository = MockRepository.GenerateStub<QuestionsRepository>();

            var controller = new ExamsController(stubQuestionsRepository, null, mockViewQuestionMapper);
            controller.Index();

            mockViewQuestionMapper.AssertWasCalled(m => m.GetViewQuestionsFromList(stubQuestionsRepository));
        }

        [TestMethod]
        public void Index_ReturnAllViewQuestions()
        {
            var mockViewQuestionMapper = MockRepository.GenerateMock<ViewQuestionMapper>();
            var stubViewQuestionList = MockRepository.GenerateStub<List<ViewQuestion>>();
            mockViewQuestionMapper.Stub(s => s.GetViewQuestionsFromList(Arg<IEnumerable<Question>>.Is.Anything)).Return(
                stubViewQuestionList);
            
            var controller = new ExamsController(null, null, mockViewQuestionMapper);
            var result = controller.Index() as ViewResult;

            Assert.AreEqual(stubViewQuestionList, result.ViewData.Model);
        }

        [TestMethod]
        public void Create_ReturnsAView()
        {
            var controller = new ExamsController();

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePost_AddsQuestionToTheRepository()
        {
            var mockQuestionsRepository = MockRepository.GenerateMock<QuestionsRepository>();
            var stubQuestion = MockRepository.GenerateStub<Question>();

            var controller = new ExamsController(mockQuestionsRepository, null, null);
            controller.Create(stubQuestion);

            mockQuestionsRepository.AssertWasCalled(m => m.Add(stubQuestion));
        }

        [TestMethod]
        public void CreatePost_ReturnsRedirectOnSuccess()
        {
            var stubQuestionsRepository = MockRepository.GenerateMock<QuestionsRepository>();
            
            var controller = new ExamsController(stubQuestionsRepository, null, null);
            ActionResult result = controller.Create(null);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Resolve_GetsQuestionById()
        {
            var mockQuestionsRepository = MockRepository.GenerateStub<QuestionsRepository>();
            var stubViewQuestionMapper = MockRepository.GenerateStub<ViewQuestionMapper>();

            var controller = new ExamsController(mockQuestionsRepository, null, stubViewQuestionMapper);
            controller.Resolve(1);

            mockQuestionsRepository.AssertWasCalled(m => m.GetById(1));
        }

        [TestMethod]
        public void Resolve_GetsAViewModelObject()
        {
            var mockViewQuestionMapper = MockRepository.GenerateMock<ViewQuestionMapper>();
            var stubQuestionsRepository = MockRepository.GenerateStub<QuestionsRepository>();
            var stubQuestion = MockRepository.GenerateStub<Question>();
            stubQuestionsRepository.Stub(s => s.GetById(Arg<int>.Is.Anything)).Return(stubQuestion);

            var controller = new ExamsController(stubQuestionsRepository, null, mockViewQuestionMapper);
            controller.Resolve(0);

            mockViewQuestionMapper.AssertWasCalled(m=>m.GetViewQuestion(stubQuestion));
        }

        [TestMethod]
        public void Resolve_ReturnsQuestionView()
        {
            var stubQuestionsRepository = MockRepository.GenerateStub<QuestionsRepository>();
            var stubViewQuestionMapper = MockRepository.GenerateMock<ViewQuestionMapper>();
            var mockViewQuestion = MockRepository.GenerateMock<ViewQuestion>();
            stubViewQuestionMapper.Stub(s => s.GetViewQuestion(Arg<Question>.Is.Anything)).Return(mockViewQuestion);
            
            var controller = new ExamsController(stubQuestionsRepository, null, stubViewQuestionMapper);
            var result = controller.Resolve(0) as ViewResult;

            Assert.AreEqual(mockViewQuestion, result.ViewData.Model);
        }

        [TestMethod]
        public void ResolvePost_GetsQuestionWithViewQuestionMapper()
        {
            var mockViewQuestionMapper = MockRepository.GenerateMock<ViewQuestionMapper>();
            var stubViewQuestion = MockRepository.GenerateStub<ViewQuestion>();
            var stubCorrectionProcess = MockRepository.GenerateStub<CorrectionProcess>();
            stubCorrectionProcess.Stub(s => s.CheckAnswer(Arg<Question>.Is.Anything)).Return(
                MockRepository.GenerateStub<CorrectionResult>());

            var controller = new ExamsController(null, stubCorrectionProcess, mockViewQuestionMapper);
            controller.Resolve(stubViewQuestion);

            mockViewQuestionMapper.AssertWasCalled(m=>m.GetQuestion(stubViewQuestion));
        }

        [TestMethod]
        public void ResolvePost_ChecksAnswer()
        {
            var mockCorrectionProcess = MockRepository.GenerateMock<CorrectionProcess>();
            mockCorrectionProcess.Stub(s => s.CheckAnswer(Arg<Question>.Is.Anything)).Return(
                MockRepository.GenerateStub<CorrectionResult>());
            var stubQuestion = MockRepository.GenerateStub<Question>();
            var stubViewQuestionMapper = MockRepository.GenerateStub<ViewQuestionMapper>();
            stubViewQuestionMapper.Stub(s => s.GetQuestion(Arg<ViewQuestion>.Is.Anything)).Return(stubQuestion);
            var stubViewQuestion = MockRepository.GenerateStub<ViewQuestion>();
            
            var controller = new ExamsController(null, mockCorrectionProcess, stubViewQuestionMapper);
            controller.Resolve(stubViewQuestion);

            mockCorrectionProcess.AssertWasCalled(m => m.CheckAnswer(stubQuestion));
        }

        [TestMethod]
        public void ResolvePost_WhenSourceCodeCannotBeExecuted_ShowsTheErrorList()
        {
            var mockCorrectionProcess = MockRepository.GenerateStub<CorrectionProcess>();
            mockCorrectionProcess.Stub(s => s.CheckAnswer(Arg<Question>.Is.Anything)).Return(new CorrectionResult
                                                                                                 {
                                                                                                     Errors =
                                                                                                         new List
                                                                                                         <string>
                                                                                                             {
                                                                                                                 "Error1",
                                                                                                                 "Error2"
                                                                                                             }
                                                                                                 });
            var stubViewQuestionMapper = MockRepository.GenerateStub<ViewQuestionMapper>();

            var controller = new ExamsController(null, mockCorrectionProcess, stubViewQuestionMapper);
            controller.Resolve(null);
            Assert.AreEqual("Error1", controller.ModelState[""].Errors[0].ErrorMessage);
            Assert.AreEqual("Error2", controller.ModelState[""].Errors[1].ErrorMessage);
        }

        [TestMethod]
        public void ResolvePost_WhenThereAreNoErrors_ReturnsRedirectOnSuccess()
        {
            var stubCorrectionProcess = MockRepository.GenerateStub<CorrectionProcess>();
            var stubCorrectionResult = MockRepository.GenerateStub<CorrectionResult>();
            stubCorrectionProcess.Stub(s => s.CheckAnswer(Arg<Question>.Is.Anything)).Return(stubCorrectionResult);
            var stubViewQuestionMapper = MockRepository.GenerateStub<ViewQuestionMapper>();

            var controller = new ExamsController(null, stubCorrectionProcess, stubViewQuestionMapper);
            ActionResult result = controller.Resolve(null);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Result", redirectResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Result_ReturnsAView()
        {
            var controller = new ExamsController();

            var result = controller.Result() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
