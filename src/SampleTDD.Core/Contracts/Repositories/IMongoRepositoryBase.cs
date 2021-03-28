using MongoDB.Bson;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoRepositoryBase<TCollection> where TCollection : BaseCollection
	{
		void Add(TCollection collection);
		void Add(IEnumerable<TCollection> collectionList);
		TCollection Get(ObjectId ID);
		IEnumerable<TCollection> GetAll();
		bool Delete(ObjectId ID);
		IEnumerable<TCollection> GetAll(Expression<Func<TCollection, bool>> filter);
		IEnumerable<TCollection> GetAll(IClientSessionHandle session, Expression<Func<TCollection, bool>> filter);
		bool Any(Expression<Func<TCollection, bool>> filter);
		IEnumerable<TCollection> GetAll(Expression<Func<TCollection, bool>> filter, int skip, int take, out long total);
		bool Update(TCollection collection, bool writeOnPreviouse = true);
		void Add<TCollec>(TCollec collection) where TCollec : BaseCollection;
		void Add<TCollec>(IEnumerable<TCollec> collectionList) where TCollec : BaseCollection;
		TCollec Get<TCollec>(ObjectId ID) where TCollec : BaseCollection;
		bool Any<TCollec>(Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection;
		IEnumerable<TCollec> GetAll<TCollec>() where TCollec : BaseCollection;
		bool Delete<TCollec>(ObjectId ID) where TCollec : BaseCollection;
		IEnumerable<TCollec> GetAll<TCollec>(Expression<Func<TCollec, bool>> filter) where TCollec : BaseCollection;
		IEnumerable<TCollec> GetAll<TCollec>(Expression<Func<TCollec, bool>> filter, int skip, int take, out long total) where TCollec : BaseCollection;
		bool Update<TCollec>(TCollec collection, bool writeOnPreviouse = true) where TCollec : BaseCollection;
		TCollec GetDescending<TCollec>(ObjectId ID, Expression<Func<TCollec, object>> orderBy) where TCollec : BaseCollection;
		TCollection GetDescending(ObjectId ID, Expression<Func<TCollection, object>> orderBy);
		void StartTransaction(Action<IClientSessionHandle> action);
		void StartTransaction(Action<IClientSessionHandle> action, CancellationToken cancellationToken);
		TCollection Get(Expression<Func<TCollection, bool>> filter);
		void Add(IClientSessionHandle session, TCollection collection);
		bool Delete(IClientSessionHandle session, ObjectId ID);
		bool Update(IClientSessionHandle session, TCollection collection, bool writeOnPreviouse = true);
		TCollection Get(IClientSessionHandle session, ObjectId ID);
		bool Any(IClientSessionHandle session, Expression<Func<TCollection, bool>> filter);
	}
}