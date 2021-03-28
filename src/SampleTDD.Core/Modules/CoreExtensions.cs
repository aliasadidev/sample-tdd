using AutoMapper;
using MongoDB.Bson;
using SampleTDD.Core.Collections;
using SampleTDD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using SampleTDD.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SampleTDD.Core.Modules
{
	public static class CoreExtensions
	{
		public static int ToInt(this Enum eum) => Convert.ToInt32(eum);

		public static IMappingExpression<TDestination, TSource> ChangeNaming<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
			where TSource : class, IBaseCollection
			where TDestination : class, IBaseCollectionDTO

		{
			return mapping.ForMember(d => d.ID, opt => opt.MapFrom(src => src._id.ToString()))
								   .ReverseMap()
								   .ForPath(s => s._id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ID) ? ObjectId.Empty : ObjectId.Parse(src.ID)));
		}


		public static BPDTO MergeObject(object source, BPDTO target, IEnumerable<string> ignoreList = null)
		{
			var properties = source.GetType().GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
			if (ignoreList != null)
			{
				properties = properties.Where(x => !ignoreList.Contains(x.Name));
			}
			foreach (var prop in properties)
			{
				var value = prop.GetValue(source, null);
				var t = target.GetType().GetProperties().Where(propx => propx.CanRead && propx.CanWrite && propx.Name == prop.Name).FirstOrDefault();
				if (t != null)
				{
					t.SetValue(target, value);
				}
			}
			return target;
		}

	}

}
