using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SampleTDD.Core.Contracts;
using System;

namespace SampleTDD.Core.Collections
{
	public class BaseCollection : IBaseCollection
	{
		[BsonId]
		public ObjectId _id { get; set; }

		public bool IsDeleted { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime CreationDate { get; set; }
	}
}
