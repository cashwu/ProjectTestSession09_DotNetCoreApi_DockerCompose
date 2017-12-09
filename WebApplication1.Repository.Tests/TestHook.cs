using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Repository.Tests.Setup;

namespace WebApplication1.Repository.Tests
{
    [TestClass()]
    public class TestHook
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            TestLocalDbProcess.CreateDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestLocalDbProcess.DestroyDatabase();
        }
    }
}

