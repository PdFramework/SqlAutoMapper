namespace IntegrationTests
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PeinearyDevelopment.Framework.Data;

    [TestClass]
    public class ResultValueTests
    {
        #region Int
        [TestMethod]
        public void TestExecuteWithValueResultInt()
        {
            var result = new SqlAutoMapper().AddCommandName("dbo.GetInts").ExecuteWithValueResult<int>();
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestExecuteWithValueResultsNullableInt()
        {
            var results = new SqlAutoMapper().AddCommandName("dbo.GetInts").ExecuteWithValueResults<int?>();
            Assert.AreEqual(15, results.Count());
            Assert.AreEqual(2, results.Count(result => result == 10));
            Assert.AreEqual(0, results.Count(result => result == 0));
            Assert.AreEqual(null, results.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TestExecuteWithValueResultsInt()
        {
            try
            {
                new SqlAutoMapper().AddCommandName("dbo.GetInts").ExecuteWithValueResults<int>();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(1, ex.InnerExceptions.Count);
                Assert.AreEqual(typeof(ArgumentNullException), ex.InnerExceptions.First().GetType());
                throw;
            }
        }
        #endregion

        #region String
        [TestMethod]
        public void TestExecuteWithValueResultString()
        {
            var result = new SqlAutoMapper().AddCommandName("dbo.GetStrings").ExecuteWithValueResult<string>();
            Assert.AreEqual("Zack", result);
        }

        [TestMethod]
        public void TestExecuteWithValueResultsNullableString()
        {
            var results = new SqlAutoMapper().AddCommandName("dbo.GetStrings").ExecuteWithValueResults<string>();
            Assert.AreEqual(14, results.Count());
            Assert.AreEqual(1, results.Count(result => result == "!@#$%^~`&*()-_+=|\\}]{[\"':;"));
            Assert.AreEqual(0, results.Count(result => result == "5"));
            Assert.AreEqual(null, results.Last());
        }
        #endregion

        #region Bool
        [TestMethod]
        public void TestExecuteWithValueResultBool()
        {
            var result = new SqlAutoMapper().AddCommandName("dbo.GetBools").ExecuteWithValueResult<bool>();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestExecuteWithValueResultsNullableBool()
        {
            var results = new SqlAutoMapper().AddCommandName("dbo.GetBools").ExecuteWithValueResults<bool?>();
            Assert.AreEqual(4, results.Count());
            Assert.AreEqual(2, results.Count(result => result == true));
            Assert.AreEqual(1, results.Count(result => result == false));
            Assert.AreEqual(null, results.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TestExecuteWithValueResultsBool()
        {
            try
            {
                new SqlAutoMapper().AddCommandName("dbo.GetBools").ExecuteWithValueResults<bool>();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(1, ex.InnerExceptions.Count);
                Assert.AreEqual(typeof(ArgumentNullException), ex.InnerExceptions.First().GetType());
                throw;
            }
        }
        #endregion

        #region DateTime
        [TestMethod]
        public void TestExecuteWithValueResultDateTime()
        {
            var result = new SqlAutoMapper().AddCommandName("dbo.GetDateTimes").ExecuteWithValueResult<DateTime>();
            Assert.AreEqual(new DateTime(1753, 1, 1, 0, 0, 0), result);
        }

        [TestMethod]
        public void TestExecuteWithValueResultsNullableDateTime()
        {
            var results = new SqlAutoMapper().AddCommandName("dbo.GetDateTimes").ExecuteWithValueResults<DateTime?>();
            Assert.AreEqual(5, results.Count());
            Assert.AreEqual(1, results.Count(result => result == new DateTime(1980, 5, 24, 0, 0, 0)));
            Assert.AreEqual(0, results.Count(result => result == DateTime.Now));
            Assert.AreEqual(null, results.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TestExecuteWithValueResultsDateTime()
        {
            try
            {
                new SqlAutoMapper().AddCommandName("dbo.GetDateTimes").ExecuteWithValueResults<DateTime>();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(1, ex.InnerExceptions.Count);
                Assert.AreEqual(typeof(ArgumentNullException), ex.InnerExceptions.First().GetType());
                throw;
            }
        }
        #endregion

        #region Guid
        [TestMethod]
        public void TestExecuteWithValueResultGuid()
        {
            var result = new SqlAutoMapper().AddCommandName("dbo.GetGuids").ExecuteWithValueResult<Guid>();
            Assert.AreEqual(Guid.Empty, result);
        }

        [TestMethod]
        public void TestExecuteWithValueResultsNullableGuid()
        {
            var results = new SqlAutoMapper().AddCommandName("dbo.GetGuids").ExecuteWithValueResults<Guid?>();
            Assert.AreEqual(4, results.Count());
            Assert.AreEqual(1, results.Count(result => result == Guid.Empty));
            Assert.AreEqual(0, results.Count(result => result == Guid.NewGuid()));
            Assert.AreEqual(null, results.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TestExecuteWithValueResultsGuid()
        {
            try
            {
                new SqlAutoMapper().AddCommandName("dbo.GetGuids").ExecuteWithValueResults<Guid>();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(1, ex.InnerExceptions.Count);
                Assert.AreEqual(typeof(ArgumentNullException), ex.InnerExceptions.First().GetType());
                throw;
            }
        }
        #endregion

    }
}
