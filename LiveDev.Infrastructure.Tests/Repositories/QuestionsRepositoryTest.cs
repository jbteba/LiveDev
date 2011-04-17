using LiveDev.Domain;
using LiveDev.Infrastructure.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class QuestionsRepositoryTest: BaseTest
    {
        [TestMethod]
        public void GetById_WhenQuestionNotExists_ReturnsNull()
        {
            var questionsRepository = new QuestionsRepository();
            var gotQuestion = questionsRepository.GetById(9999);

            Assert.IsNull(gotQuestion);
        }

        [TestMethod]
        public void GetById_WhenQuestionExists_ReturnsTheQuestion()
        {
            var stubQuestion = new Question();

            var questionsRepository = new QuestionsRepository {stubQuestion};
            var gotQuestion = questionsRepository.GetById(stubQuestion.Id);

            Assert.AreEqual(stubQuestion,gotQuestion);
        }
    }
}
