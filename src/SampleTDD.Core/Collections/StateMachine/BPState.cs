using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SampleTDD.Core.Constants;

namespace SampleTDD.Core.Collections.StateMachine
{
	public class BPState : BaseCollection
	{
		public ObjectId BPID { get; set; }
		public StateTypes StateID { get; set; }
		public bool IsCompleted { get; set; }
		public RoleTypes RoleID { get; set; }
		public long? UserID { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? CompletedDateTime { get; set; }

	}
}
