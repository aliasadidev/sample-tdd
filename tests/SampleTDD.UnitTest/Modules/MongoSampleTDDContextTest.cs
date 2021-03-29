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

	}
}
