using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.Modules;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace SampleTDD.UnitTest.TestCore
{
	public class TestExtensionMethods
	{

		[Theory]
		[InlineData("Admin", testEnum.Admin)]
		[InlineData("UserTest", testEnum.User)]
		public void ToDescOrName_get_either_enum_desc_attr_or_name_is_correct(string expected, testEnum key)
		{
			var attrValue = key.ToDescOrName();
			Assert.Equal(expected, attrValue);
		}

		[Theory]
		[InlineData("AdminTest", testEnum.Admin)]
		[InlineData("Not found", testEnum.User)]
		public void ToNameAttr_get_name_attr_is_correct(string expected, testEnum key)
		{
			var attrValue = key.ToNameAttr("Not found");
			Assert.Equal(expected, attrValue);
		}

		[Fact]
		public void ChangeNaming_convert_value_of_objectId_type_to_string_and_reverse()
		{
			// Arrange
			Mock<IBaseCollection> collection = new Mock<IBaseCollection>();
			ObjectId objId = ObjectId.GenerateNewId();
			collection.Setup(x => x._id).Returns(objId);
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<IBaseCollection, IBaseCollectionDTO>().ChangeNaming();
			});

			// Act
			var mapper = mockMapper.CreateMapper();

			// Assert
			var result = mapper.Map<IBaseCollectionDTO>(collection.Object);
			Assert.Equal(collection.Object._id.ToString(), result.ID);
		}


		[Theory]
		[InlineData("Admin,User", new testEnum[] { testEnum.Admin, testEnum.User })]
		[InlineData("Admin,User,Customer", new testEnum[] { testEnum.Admin, testEnum.User })]
		[InlineData("Customer", new testEnum[] { })]
		public void ToEnumsList_string_with_delimeter_to_enum_list_is_correct(string input, testEnum[] expected)
		{
			var actual = input.ToEnumsList<testEnum>();
			Assert.Equal(expected, actual.ToArray());
		}

		[Fact]
		public void MergeObject_merge_two_object_is_correct()
		{
			string objID = ObjectId.GenerateNewId().ToString();
			const string stateName = "Start";

			var stateDTO = new Mock<StateDTO>();
			stateDTO.Setup(x => x.StateName).Returns(stateName);


			var bpDTO = new BPDTO() { ID = objID };

			//Test case 1
			BPDTO mergeResult = Core.Modules.CoreExtensions.MergeObject(stateDTO.Object, bpDTO);
			var j1 = JsonConvert.SerializeObject(mergeResult);
			var j2 = JsonConvert.SerializeObject(new BPDTO { StateName = stateName, ID = objID });
			Assert.Equal(j1, j2);

			//Test case 2
			const StateTypes stateType = StateTypes.Start;
			stateDTO.Setup(x => x.StateType).Returns(stateType);
			BPDTO mergeResult2 = Core.Modules.CoreExtensions.MergeObject(stateDTO.Object, bpDTO, new List<string>() {
				nameof(StateDTO.StateType)
			});
			var j3 = JsonConvert.SerializeObject(mergeResult2);
			var j4 = JsonConvert.SerializeObject(new BPDTO { StateName = stateName, ID = objID, StateType = stateType });
			Assert.NotEqual(mergeResult2.StateType, stateType);
			Assert.NotEqual(j3, j4);
		}

		[Fact]
		public void ChangeDTOAfterApproveCondition_is_correct()
		{
			// ObjectId objID = ObjectId.GenerateNewId();
			// var bpDTO = new BPDTO() { };
			// var bp = new BP() { _id = objID };

			// bpDTO.ChangeDTOAfterApproveCondition(bp, null, (BPDTO dto, BP coll) =>
			// {
			//     dto.ID = coll._id.ToString();
			//     return dto;
			// });
			// Assert.Equal(bpDTO.ID, bp._id.ToString());
		}


		#region test data and types

		public enum testEnum
		{
			[Display(Name = "AdminTest")]
			Admin,
			[Description("UserTest")]
			User
		}


		public interface StateDTO
		{
			string StateName { get; set; }
			StateTypes StateType { get; set; }
		}



		#endregion
	}
}

//MethodName_ExpectedBehavior_StateUnderTest
//isAdult_False_AgeLessThan18
//Should_ExpectedBehavior_When_StateUnderTest
//Should_ThrowException_When_AgeLessThan18
//When_StateUnderTest_Expect_ExpectedBehavior
//When_AgeLessThan18_Expect_isAdultAsFalse
//Test[Feature being tested].cs file name