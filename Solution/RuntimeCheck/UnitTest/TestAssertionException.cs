using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuntimeCheck;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class TestAssertionException
    {
        [Test]
        public void TestAssert()
        {
            try
            {
                RuntimeCheck.Assert.True(false, "this is a test assertion");
            }
            catch(ViolatedAssertionException ex)
            {
                var stackTrace = ex.StackTrace;
                return;
            }
            NUnit.Framework.Assert.Fail();
        }
    }
}
