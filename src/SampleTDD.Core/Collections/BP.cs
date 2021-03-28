﻿using System;
using SampleTDD.Core.Constants;
using MongoDB.Bson.Serialization.Attributes;
using SampleTDD.Core.Collections.BPIs.BPISabteDarkhast;

namespace SampleTDD.Core.Collections
{
	public class BP : BaseCollection
	{
		public long UserID { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime CreationTime { get; set; }

		public BPISabteDarkhast SabteDarkhast { get; set; }

	}
}
