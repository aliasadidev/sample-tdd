using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using System.Linq;

namespace SampleTDD.IntegrationTest.Seeds
{
	public class DBSeed
	{
		private readonly IMongoSampleTDDContext _db;
		public DBSeed(IMongoSampleTDDContext db)
		{
			_db = db;
		}

		public void Init()
		{
			Task.Run(clean).Wait();
			var list = new Task[] {
				Task.Run(createBPCollection),
				Task.Run(createBPStateCollection),

				Task.Run(addStates),
				Task.Run(addChangeStateRules),
			};

			Task.WaitAll(list);
		}

		private async Task clean()
		{
			await _db.ChangeStateRules.DeleteManyAsync(x => true);
			await _db.States.DeleteManyAsync(x => true);
			await _db.BPs.DeleteManyAsync(x => true);
			await _db.BPStates.DeleteManyAsync(x => true);
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

		private async Task createBPCollection()
		{
			var collectionList = _db.Database.ListCollectionNames().ToList();
			string collecName = nameof(BP);

			if (collectionList.All(x => x != collecName))
				await _db.Database.CreateCollectionAsync(collecName);

		}

		private async Task createBPStateCollection()
		{
			var collectionList = _db.Database.ListCollectionNames().ToList();
			string collecName = nameof(BPState);

			if (collectionList.All(x => x != collecName))
				await _db.Database.CreateCollectionAsync(collecName);

		}
	}
}