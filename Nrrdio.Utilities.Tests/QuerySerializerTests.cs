using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrrdio.Utilities.Web.Query;
using System;

namespace Nrrdio.Utilities.Tests {
    [TestClass]
    public class QuerySerializerTests {
        [TestMethod]
        public void Serialize() {
            var testObject = new {
                Thing1 = "Test",
                Thing2 = 12345,
                Thing3 = new DateTime(2021, 01, 25).ToString("yyyy-MM-dd")
            };

            var query = QuerySerializer.Serialize(testObject);

            Assert.AreEqual("Thing1=Test&Thing2=12345&Thing3=2021-01-25", query);
        }

        [TestMethod]
        public void SerializeSnake() {
            var testObject = new {
                Test1 = "Test",
                TestThing2 = 12345,
                LongTestThing3 = new DateTime(2021, 01, 25).ToString("yyyy-MM-dd")
            };

            var query = QuerySerializer.Serialize(testObject, new PascalToSnakeNamingPolicy());

            Assert.AreEqual("test1=Test&test_thing2=12345&long_test_thing3=2021-01-25", query);
        }
    }
}
