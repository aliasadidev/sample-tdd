using MongoDB.Bson;
using Moq;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Collections.BPIs.BPISabteDarkhast;
using Xunit;
using System;
using SampleTDD.UnitTest.Seeds;
using SampleTDD.UnitTest.Modules;
using SampleTDD.Core.Contracts.Repositories;
using Xunit.Priority;

namespace SampleTDD.UnitTest.InfrastructureTest.Repositories
{
	[Collection(nameof(SharedFixtureCollection))]
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class MongoBPRepositoryTest
	{
		private readonly SharedFixture sharedFixture;

		public MongoBPRepositoryTest(SharedFixture sharedFixture)
		{
			this.sharedFixture = sharedFixture;
		}

		[Fact, Priority(1)]
		public void GetReturnCorrectBP()
		{
			// Arrange
			var repoMock = new Mock<IMongoBPRepository>();
			ObjectId bpID = ObjectId.GenerateNewId();
			var bpCollection = new BP
			{
				_id = bpID,
				CreationDate = DateTime.UtcNow,
				IsDeleted = false,
				SabteDarkhast = new BPISabteDarkhast
				{

					BranchID = 10,
					BranchName = "UK",
					CityID = 150,
					FullName = "Mr. T",
					PhoneNumber = "+9999999",
					ProvinceID = 158
				},
				UserID = DBSeed.UserID,
			};

			// Act
			repoMock.Setup(collection => collection.Get(bpID)).Returns(bpCollection);
			BP result = repoMock.Object.Get(bpCollection._id);

			// Assert
			bpCollection.IsEqula(result);

		}

		[Fact, Priority(2)]
		public void AddNewBP()
		{
			// Arrange
			var repoMock = sharedFixture.GetInstance<IMongoBPRepository>();
			ObjectId bpID = ObjectId.GenerateNewId();
			var bpCollection = new BP
			{
				CreationDate = DateTime.UtcNow,
				SabteDarkhast = new BPISabteDarkhast
				{
					BranchID = 10,
					BranchName = "UK",
					CityID = 150,
					FullName = "Mr. T",
					PhoneNumber = "+9999999",
					ProvinceID = 158
				},
				UserID = DBSeed.UserID,
			};

			// Act
			repoMock.Add(bpCollection);

			// Assert
			Assert.True(bpCollection._id != ObjectId.Empty);

		}

	}
}