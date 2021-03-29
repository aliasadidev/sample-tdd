using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using System.Linq;
using SampleTDD.UnitTest.Modules;
using MongoDB.Bson;

namespace SampleTDD.UnitTest.Seeds
{
	public class DBSeed
	{
		private readonly IMongoSampleTDDContextTest _db;
		public DBSeed(IMongoSampleTDDContextTest db)
		{
			_db = db;
		}



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
			_db.ChangeStateRules.DeleteMany(x => true);
			_db.States.DeleteMany(x => true);
			_db.BPs.DeleteMany(x => true);
			_db.BPStates.DeleteMany(x => true);
		}

		private async Task addStates()
		{
			await _db.States.InsertManyAsync(new List<State>
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
			await _db.ChangeStateRules.InsertManyAsync(new List<ChangeStateRule>
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
			await _db.BPs.InsertOneAsync(new BP
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

			await _db.BPStates.InsertOneAsync(new BPState
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
			string collecName = nameof(BP);
			await createCollection(collecName);
		}

		private async Task createBPStateCollection()
		{
			string collecName = nameof(BPState);
			await createCollection(collecName);
		}

		private async Task createCollection(string collectionName)
		{
			var collectionList = _db.Database.ListCollectionNames().ToList();
			if (collectionList.All(x => x != collectionName))
				await _db.Database.CreateCollectionAsync(collectionName);
		}

	}
}
