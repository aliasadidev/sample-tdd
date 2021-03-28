using MongoDB.Bson;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace SampleTDD.Infrastructure.Data.Mongo
{
	public class MongoRepositoryBase<TCollection> : IMongoRepositoryBase<TCollection>
		where TCollection : BaseCollection
	{
		protected readonly IMongoSampleTDDContext _db;
		protected readonly IMongoCollection<TCollection> _currentCollec;

		public MongoRepositoryBase(IMongoSampleTDDContext mongoSampleTDDContext)
		{
			_db = mongoSampleTDDContext;
			_currentCollec = mongoSampleTDDContext.Set<TCollection>();
		}


		/// <summary>
		/// Session property
		/// </summary>
		public IClientSessionHandle Session { get; private set; }

		/// <summary>
		/// Start Transaction
		/// </summary>
		protected void StartTransaction()
		{
			//_transactionStartDateTime = DateTime.Now;

			Session = _db.Client.StartSession();
			Session.StartTransaction();
		}

		public void StartTransaction(Action<IClientSessionHandle> action)
		{
			using IClientSessionHandle session = _db.Client.StartSession();
			var transactionOptions = new TransactionOptions(
						 readConcern: ReadConcern.Snapshot,
						 writeConcern: WriteConcern.WMajority,
						 readPreference: ReadPreference.Primary
						 );

			var cancellationToken = default(CancellationToken); // normally a real token would be used
			session.WithTransaction((ses, ct) =>
			{
				action(ses);
				return string.Empty;
			}, transactionOptions, cancellationToken);

		}

		public void StartTransaction(Action<IClientSessionHandle> action, CancellationToken cancellationToken)
		{
			using IClientSessionHandle session = _db.Client.StartSession();
			var transactionOptions = new TransactionOptions(
						 readConcern: ReadConcern.Snapshot,
						 writeConcern: WriteConcern.WMajority,
						 readPreference: ReadPreference.Primary
						 );
			session.WithTransaction((ses, ct) =>
			{
				action(ses);
				return string.Empty;
			}, transactionOptions, cancellationToken);

		}

		public virtual void Add(TCollection collection) => Add<TCollection>(collection);
		public virtual void Add(IClientSessionHandle session, TCollection collection) => Add<TCollection>(session, collection);
		public virtual void Add(IEnumerable<TCollection> collectionList) => Add<TCollection>(collectionList);
		public virtual TCollection Get(ObjectId ID) => Get<TCollection>(ID);
		public virtual TCollection Get(IClientSessionHandle session, ObjectId ID) => Get<TCollection>(session, ID);
		public virtual TCollection Get(Expression<Func<TCollection, bool>> filter) => Get<TCollection>(filter);
		public virtual IEnumerable<TCollection> GetAll() => GetAll<TCollection>();
		public virtual bool Delete(ObjectId ID) => Delete<TCollection>(ID);
		public virtual bool Delete(IClientSessionHandle session, ObjectId ID) => Delete<TCollection>(session, ID);
		public virtual IEnumerable<TCollection> GetAll(Expression<Func<TCollection, bool>> filter) => GetAll<TCollection>(filter);
		public virtual IEnumerable<TCollection> GetAll(Expression<Func<TCollection, bool>> filter, int skip, int take, out long total) => GetAll<TCollection>(filter, skip, take, out total);
		public virtual bool Update(TCollection collection, bool writeOnPreviouse = true) => Update<TCollection>(collection, writeOnPreviouse);
		public virtual bool Update(IClientSessionHandle session, TCollection collection, bool writeOnPreviouse = true) => Update<TCollection>(session, collection, writeOnPreviouse);
		public virtual TCollection GetDescending(ObjectId ID, Expression<Func<TCollection, object>> orderBy) => GetDescending<TCollection>(ID, orderBy);

		public virtual void Add<TCollec>(TCollec collection) where TCollec : BaseCollection
		{
			collection.CreationDate = DateTime.Now;
			collection.IsDeleted = false;
			_db.Set<TCollec>().InsertOne(collection);
		}

		public virtual void Add<TCollec>(IClientSessionHandle session, TCollec collection) where TCollec : BaseCollection
		{
			collection.CreationDate = DateTime.Now;
			collection.IsDeleted = false;
			_db.Set<TCollec>().InsertOne(session, collection);
		}

		public virtual void Add<TCollec>(IEnumerable<TCollec> collectionList) where TCollec : BaseCollection
		{
			foreach (var collection in collectionList)
			{
				collection.CreationDate = DateTime.Now;
				collection.IsDeleted = false;
			}
			_db.Set<TCollec>().InsertMany(collectionList);
		}

		public virtual TCollec Get<TCollec>(ObjectId ID) where TCollec : BaseCollection
		{
			var collection = _db.Set<TCollec>().Find(x => x._id == ID).FirstOrDefault();
			return collection;
		}

		public virtual TCollec Get<TCollec>(IClientSessionHandle session, ObjectId ID) where TCollec : BaseCollection
		{
			var collection = _db.Set<TCollec>().Find(session, x => x._id == ID).FirstOrDefault();
			return collection;
		}

		public virtual TCollec Get<TCollec>(Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection
		{
			var collection = _db.Set<TCollec>().Find(filter).FirstOrDefault();
			return collection;
		}

		public virtual TCollec GetDescending<TCollec>(ObjectId ID, Expression<Func<TCollec, object>> orderBy) where TCollec : BaseCollection
		{
			var collection = _db.Set<TCollec>().Find(x => x._id == ID).SortByDescending(orderBy).FirstOrDefault();
			return collection;
		}


		public virtual IEnumerable<TCollec> GetAll<TCollec>() where TCollec : BaseCollection
		{
			var collectionList = _db.Set<TCollec>().Find(new BsonDocument()).ToList();
			return collectionList;
		}

		public virtual bool Delete<TCollec>(ObjectId ID) where TCollec : BaseCollection
		{
			_db.Set<TCollec>().FindOneAndUpdate(g => g._id == ID, new UpdateDefinitionBuilder<TCollec>().Set(x => x.IsDeleted, true));
			return true;
		}
		public virtual bool Delete<TCollec>(IClientSessionHandle session, ObjectId ID) where TCollec : BaseCollection
		{
			_db.Set<TCollec>().FindOneAndUpdate(session, g => g._id == ID, new UpdateDefinitionBuilder<TCollec>().Set(x => x.IsDeleted, true));
			return true;
		}
		public virtual IEnumerable<TCollec> GetAll<TCollec>(Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection
		{
			var lst = _db.Set<TCollec>().Find(filter).ToList();
			return lst;
		}

		public virtual IEnumerable<TCollec> GetAll<TCollec>(Expression<Func<TCollec, bool>> filter, int skip, int take, out long total) where TCollec : BaseCollection
		{
			total = _db.Set<TCollec>().CountDocuments(filter);
			var lst = _db.Set<TCollec>().Find(filter)
									.Skip(skip)
									.Limit(take)
									.ToList();
			return lst;
		}





		public virtual bool Update<TCollec>(TCollec collection, bool writeOnPreviouse = true) where TCollec : BaseCollection
		{
			var temp = _db.Set<TCollec>().Find(x => x._id == collection._id).FirstOrDefault();
			ReplaceOneResult updateResult;

			if (temp == null)
				throw new ArgumentNullException("Not found");

			if (writeOnPreviouse)
			{
				updateResult = _db.Set<TCollec>().ReplaceOne(filter: g => g._id == collection._id, replacement: collection);
			}
			else
			{
				temp.IsDeleted = true;
				updateResult = _db.Set<TCollec>().ReplaceOne(filter: g => g._id == collection._id, replacement: temp);
				Add<TCollec>(collection);
			}

			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}

		public virtual bool Update<TCollec>(IClientSessionHandle session, TCollec collection, bool writeOnPreviouse = true) where TCollec : BaseCollection
		{
			var temp = _db.Set<TCollec>().Find(session, x => x._id == collection._id).FirstOrDefault();
			ReplaceOneResult updateResult;

			if (temp == null)
				throw new ArgumentNullException("Not found");

			if (writeOnPreviouse)
			{
				updateResult = _db.Set<TCollec>().ReplaceOne(session, filter: g => g._id == collection._id, replacement: collection);
			}
			else
			{
				temp.IsDeleted = true;
				updateResult = _db.Set<TCollec>().ReplaceOne(session, filter: g => g._id == collection._id, replacement: temp);
				Add<TCollec>(session, collection);
			}

			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}

		public bool Any(Expression<Func<TCollection, bool>> filter) => Any<TCollection>(filter);
		public bool Any(IClientSessionHandle session, Expression<Func<TCollection, bool>> filter) => Any<TCollection>(session, filter);


		public bool Any<TCollec>(Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection
		{
			var hasOne = _db.Set<TCollec>().Find(filter).Any();
			return hasOne;
		}

		public bool Any<TCollec>(IClientSessionHandle session, Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection
		{
			var hasOne = _db.Set<TCollec>().Find(session, filter).Any();
			return hasOne;
		}

		public IEnumerable<TCollection> GetAll(IClientSessionHandle session, Expression<Func<TCollection, bool>> filter)
		{
			return _currentCollec.Find(session, filter).ToList();
		}
	}
}
