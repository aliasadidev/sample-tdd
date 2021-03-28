using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using MongoDB.Driver;

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

			var list = new Task[] {

				Task.Run(AddStates),
				Task.Run(AddChangeStateRules),
			};

			Task.WaitAll(list);
		}

		private async Task AddStates()
		{
			await _db.States.InsertManyAsync(new List<State>
			{
				new State {
						ID =  StateTypes.Start,
						EnglishTitle =  "Start",
						HisDate =  13990905,
						IsDeleted =  false,
						CreationDate =  DateTime.Now,
						HCode =  "1"
					},
				new State {
						ID =  StateTypes.RequestSampleTDD,
						EnglishTitle =  "RequestSampleTDD",
						HisDate =  13990905,
						IsDeleted =  false,
						CreationDate =  DateTime.Now,
						HCode =  "1.1",

				}

			});
		}

		private async Task AddChangeStateRules()
		{
			await _db.ChangeStateRules.InsertManyAsync(new List<ChangeStateRule>
			{
				new ChangeStateRule {
							RoleID  = RoleTypes.InsuredCustomer,
							OperatinID  = OperationTypes.Start,
							CurrentState  = StateTypes.Start,
							NextState  = StateTypes.RequestSampleTDD,
							HisDate  = 13990201,
							IsDeleted  = false,
							CreationDate  = DateTime.Now
						}
			});
		}
	}
}