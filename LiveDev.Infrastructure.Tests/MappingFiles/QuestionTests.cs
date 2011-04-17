using AtentoFramework2008.TestTools.DomainModel;
using LiveDev.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Infrastructure.Tests.MappingFiles
{
    [TestClass]
    public class QuestionTests
    {
        [TestMethod]
        public void Question_IsPersistent()
        {
            SpecifyOn.Entity<Question>().ShouldBePersistent().MappedToTable("Questions");
        }

        [TestMethod]
        public void Id_IsPersistent()
        {
            SpecifyOn.Entity<Question>().ShouldHaveProperty("Id").ShouldBePersistent().MappedAsIdentifier();
        }

        [TestMethod]
        public void TextQuestion_IsPersistent()
        {
            SpecifyOn.Entity<Question>().ShouldHaveProperty("TextQuestion").ShouldBePersistent().MappedToColumn(
                "TEXT_QUESTION");
        }
    }
}
