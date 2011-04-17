using AtentoFramework2008.Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Infrastructure.Tests
{
    [TestClass]
    public class BaseTest
    {
        [TestCleanup]
        public void TestCleanup()
        {
            CloseSessionHaciendoRollback();
        }

        public void CloseSessionHaciendoRollback()
        {
            NHibernateSessionManager.Instance.CloseSession(false);
        }

        public void CloseSessionHaciendoCommit()
        {
            NHibernateSessionManager.Instance.CloseSession(true);
        }
    }
}
