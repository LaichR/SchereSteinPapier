using System;
using NUnit.Framework;
using RuntimeCheck;

namespace UnitTest
{
    [TestFixture]
    public class ExceptionHelperTest
    {
        [Test]
        public void TestCreateArgumentNullException()
        {
            var exception = RuntimeCheck.ExceptionHelper.CreateArgumentException<ArgumentNullException>("object is null", "dummy");
            NUnit.Framework.Assert.IsNotNull(exception);
            NUnit.Framework.Assert.True(exception.Message == "object is null\r\nParametername: dummy");
        }

        [Test]
        public void TestCreateArgumentOutOfRangeException()
        {
            var exception = RuntimeCheck.ExceptionHelper.CreateArgumentException<ArgumentOutOfRangeException>("argument is not in specified range", "dummy");
            NUnit.Framework.Assert.IsNotNull(exception);
            NUnit.Framework.Assert.True(exception.Message == "argument is not in specified range\r\nParametername: dummy");
        }

        [Test]
        public void TestCreateException()
        {
            var exception = RuntimeCheck.ExceptionHelper.CreateException<System.IO.FileNotFoundException>("file {0} not found", "hello.txt");
            NUnit.Framework.Assert.IsNotNull(exception);
            NUnit.Framework.Assert.True(exception.Message == "file hello.txt not found");
        }
    }
}
