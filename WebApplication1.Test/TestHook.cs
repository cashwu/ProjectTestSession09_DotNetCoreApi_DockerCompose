using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Tests.Setup;

namespace WebApplication1.Tests
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

