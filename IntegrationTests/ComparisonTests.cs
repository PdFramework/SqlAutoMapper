using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeinearyDevelopment.Framework.Data.MsSql;
using PeinearyDevelopment.Framework.Data.MySql;
using PeinearyDevelopment.Framework.Data.Postgres;

namespace IntegrationTests
{
	[TestClass]
	public class ComparisonTests
	{
		private const int NumberOfRuns = 10000;
		[TestMethod]
		public async Task MsSqlConnection()
		{
			for (var i = 0; i < NumberOfRuns; i++)
			{
				await new MsSqlAutoMapper("MsSqlDbConnectionString").AddCommandName("TestData.dbo.getpeople").ExecuteWithoutResultsAsync();
			}

			Assert.IsFalse(false);
		}

		[TestMethod]
		public async Task NpgSqlConnection()
		{
			for (var i = 0; i < NumberOfRuns; i++)
			{
				await new NpgSqlAutoMapper("NpgSqlDbConnectionString").AddCommandName("getpeople2").ExecuteWithoutResultsAsync();
			}

			Assert.IsFalse(false);
		}

		[TestMethod]
		public async Task MySqlConnection()
		{
			for (var i = 0; i < NumberOfRuns; i++)
			{
				await new MySqlAutoMapper("MySqlDbConnectionString").AddCommandName("test.getpeople").ExecuteWithoutResultsAsync();
			}

			Assert.IsFalse(false);
		}

		[TestMethod]
		public async Task MsSqlHydrate()
		{
			var people = await new MsSqlAutoMapper("MsSqlDbConnectionString").AddCommandName("TestData.dbo.getpeople").ExecuteWithResultsAsync<Person>();
			Assert.AreEqual(6, people.Count());
			AssertPeople(people);
		}

		[TestMethod]
		public async Task NpgSqlHydrate()
		{
			var people = await new NpgSqlAutoMapper("NpgSqlDbConnectionString").AddCommandName("getpeople2").ExecuteWithResultsAsync<Person>();
			Assert.AreEqual(6, people.Count());
			AssertPeople(people);
		}

		[TestMethod]
		public async Task MySqlHydrate()
		{
			var people = await new MySqlAutoMapper("MySqlDbConnectionString").AddCommandName("test.getpeople").ExecuteWithResultsAsync<Person>();
			Assert.AreEqual(6, people.Count());
			AssertPeople(people);
		}

		private void AssertPeople(IEnumerable<Person> people)
		{
			foreach (var person in people)
			{
				var ps = people.First(p => p.Id == person.Id);
				Assert.AreEqual(person.FirstName, ps.FirstName);
				Assert.AreEqual(person.LastName, ps.LastName);
			}
		}
	}

	public class Person
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
