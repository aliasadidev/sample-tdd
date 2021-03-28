using MongoDB.Bson;

namespace SampleTDD.Core.Contracts
{
	public interface IBaseCollection
	{
		ObjectId _id { get; set; }
	}
}

