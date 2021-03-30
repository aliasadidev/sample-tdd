using System;
using System.Linq;
using MongoDB.Bson;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.DTOs.Security;
using SampleTDD.UnitTest.Modules;
using SampleTDD.UnitTest.Seeds;
using Xunit;
using Xunit.Priority;

namespace SampleTDD.UnitTest.InfrastructureTest
{
	[Collection(nameof(SharedFixtureCollection))]
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class WorkflowEngineServiceTests
	{
		private readonly SharedFixture sharedFixture;

		public WorkflowEngineServiceTests(SharedFixture mongoSharedFixture)
		{
			sharedFixture = mongoSharedFixture;
		}
		[Fact]
		public void GetPremissionsIsCorrectForInsuredCustomer()
		{
			// Arrange
			IWorkflowEngineService srv = sharedFixture.GetInstance<IWorkflowEngineService>();

			// Act
			PermissionDTO permission = srv.GetPremissions(DBSeed.BPID, RoleTypes.InsuredCustomer, DBSeed.UserID);

			// Assert
			Assert.True(permission.CanApprove);
		}

		[Fact]
		public void GetCurrentBpStateIsCorrectForInsuredCustomer()
		{
			//Arrange
			IWorkflowEngineService srv = sharedFixture.GetInstance<IWorkflowEngineService>();

			//Act
			StateTypes currentBPState = srv.GetCurrentBpState(DBSeed.BPID, RoleTypes.InsuredCustomer);
			
			//Assert
			Assert.Equal(StateTypes.Start, currentBPState);
		}

		[Fact]
		public void StartIsCorrectForInsuredCustomer()
		{
			//Arrange
			IWorkflowEngineService srv = sharedFixture.GetInstance<IWorkflowEngineService>();
			IMongoBPRepository bpRepo = sharedFixture.GetInstance<IMongoBPRepository>();
			IMongoBPStateRepository bpStaterepo = sharedFixture.GetInstance<IMongoBPStateRepository>();
			ObjectId bpID = ObjectId.GenerateNewId();
			var bpCollection = new BP
			{
				_id = bpID,
				CreationDate = DateTime.UtcNow,
				IsDeleted = false,
				SabteDarkhast = new Core.Collections.BPIs.BPISabteDarkhast.BPISabteDarkhast
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

			//Act
			bpRepo.StartTransaction(session =>
			{
				bpRepo.Add(session, bpCollection);
				srv.Start(session, bpID, RoleTypes.InsuredCustomer, DBSeed.UserID);
			});
			var bp = bpRepo.Get(bpID);
			var bpState = bpStaterepo.GetAll(x => x.BPID == bpID).Single();

			//Assert
			Assert.Equal(bpCollection.SabteDarkhast.BranchName, bp.SabteDarkhast.BranchName);
			Assert.False(bpState.IsCompleted);
			Assert.False(bpState.IsDeleted);
			Assert.Equal(RoleTypes.InsuredCustomer, bpState.RoleID);
			Assert.Equal(StateTypes.Start, bpState.StateID);

		}

	}
}