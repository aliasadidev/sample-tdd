using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using System;
using SampleTDD.Infrastructure.Data.Mongo;
using SampleTDD.Core.DTOs.Settings;
using SampleTDD.Core.Collections.StateMachine;
using System.Threading.Tasks;
using System.Collections.Generic;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Collections;
using System.Linq;
using MongoDB.Bson;

namespace SampleTDD.UnitTest.Modules
{
	public class MongoSampleTDDContextTest : MongoSampleTDDContext, IMongoSampleTDDContextTest
	{

		public MongoSampleTDDContextTest(IOptions<AppSettings> options) : base(options) { }



		public void SeedData()
		{
			clean();
			var list = new Task[] {
				Task.Run(createBPCollection),
				Task.Run(createBPStateCollection),

				Task.Run(addStates),
				Task.Run(addChangeStateRules),
				Task.Run(newBP),
			};

			Task.WaitAll(list);

		}

		private void clean()
		{
			ChangeStateRules.DeleteMany(x => true);
			States.DeleteMany(x => true);
			BPs.DeleteMany(x => true);
			BPStates.DeleteMany(x => true);
		}

		private async Task addStates()
		{
			await States.InsertManyAsync(new List<State>
			{
				new State {
						ID =  StateTypes.Start,
						EnglishTitle =  "Start",
						IsDeleted =  false,
						CreationDate =  DateTime.Now,
						HCode =  "1",
					},
				new State {
						ID =  StateTypes.RequestNewJob,
						EnglishTitle =  "Request New Job",
						IsDeleted =  false,
						CreationDate =  DateTime.Now,
						HCode =  "1.1",

				},
					new State {
						ID =  StateTypes.Closed,
						EnglishTitle =  "Closed",
						IsDeleted =  false,
						CreationDate =  DateTime.Now,
						HCode =  "1.1.1",

				}

			});
		}

		private async Task addChangeStateRules()
		{
			await ChangeStateRules.InsertManyAsync(new List<ChangeStateRule>
			{
				new ChangeStateRule {
							RoleID  = RoleTypes.InsuredCustomer,
							OperatinID  = OperationTypes.Start,
							CurrentState  = StateTypes.Start,
							NextState  = StateTypes.RequestNewJob,
							IsDeleted  = false,
							CreationDate  = DateTime.Now
						},
				new ChangeStateRule {
					RoleID  = RoleTypes.InsuredCustomer,
					OperatinID  = OperationTypes.Approve,
					CurrentState  = StateTypes.Start,
					NextState  = StateTypes.RequestNewJob,
					IsDeleted  = false,
					CreationDate  = DateTime.Now
				},
					new ChangeStateRule {
					RoleID  = RoleTypes.InsuredCustomer,
					OperatinID  = OperationTypes.Approve,
					CurrentState  = StateTypes.RequestNewJob,
					NextState  = StateTypes.Closed,
					IsDeleted  = false,
					CreationDate  = DateTime.Now
				}
			});
		}
		public static readonly ObjectId BPID = ObjectId.GenerateNewId();
		public static readonly long UserID = -2;
		private async Task newBP()
		{
			await BPs.InsertOneAsync(new BP
			{
				_id = BPID,
				CreationDate = DateTime.UtcNow,
				CreationTime = DateTime.UtcNow,
				IsDeleted = false,
				SabteDarkhast = new Core.Collections.BPIs.BPISabteDarkhast.BPISabteDarkhast
				{

					BranchID = 1,
					BranchName = "Berlin",
					CityID = 150,
					FullName = "Ali Asadi",
					PhoneNumber = "+989126449201",
					ProvinceID = 158
				},
				UserID = UserID,
			});

			await BPStates.InsertOneAsync(new BPState
			{
				_id = ObjectId.GenerateNewId(),
				UserID = UserID,
				BPID = BPID,
				CompletedDateTime = null,
				CreationDate = DateTime.UtcNow,
				IsCompleted = false,
				IsDeleted = false,
				RoleID = RoleTypes.InsuredCustomer,
				StateID = StateTypes.Start
			});
		}

		private async Task createBPCollection()
		{
			var collectionList = Database.ListCollectionNames().ToList();
			string collecName = nameof(BP);

			if (collectionList.All(x => x != collecName))
				await Database.CreateCollectionAsync(collecName);

		}

		private async Task createBPStateCollection()
		{
			var collectionList = Database.ListCollectionNames().ToList();
			string collecName = nameof(BPState);

			if (collectionList.All(x => x != collecName))
				await Database.CreateCollectionAsync(collecName);

		}

	}
}
