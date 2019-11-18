using System;
using RuntimeCheck;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class TestArgumentValidation
    {
        [Test]
        public void TestRequiresInvalidArgument()
        {
            try
            {
                MethodInteger1(2000);
            }
            catch(ViolatedContractException e)
            {
                Console.WriteLine(e.Message);
                var messageLines = e.Message.Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                NUnit.Framework.Assert.True(messageLines.Length == 2);
                NUnit.Framework.Assert.True(messageLines[0] == string.Format(Contract.ArgumentValidationFailedMsgTemplate, "TestArgumentValidation.MethodInteger1(..)"));
                NUnit.Framework.Assert.True(messageLines[1] == "Parametername: a");
                return;
            }
            // we should never get here
            NUnit.Framework.Assert.Fail();
        }

        [Test]
        public void TestRequiresNotNull()
        {
            try
            {
                MethodObject1(null);
            }
            catch(ArgumentException e)
            {
                return;
            }
            NUnit.Framework.Assert.Fail("no exception was thrown");
        }

        void MethodInteger1(int a)
        {
            Contract.Requires(a < 100, "a");
        }

        void MethodObject1(object o)
        {
            Contract.RequiresArgumentNotNull(o, "o");
        }
    }
}
