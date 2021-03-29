using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.Modules;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace SampleTDD.UnitTest.CoreTest
{
	public class ExtensionMethodTests
	{

		[Fact]
		public void ChangeNaming_ConvertObjectIdTypeToStringAndViceVersa()
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
			var actualResult = mapper.Map<IBaseCollectionDTO>(collection.Object);
			Assert.Equal(collection.Object._id.ToString(), actualResult.ID);
		}



		[Fact]
		public void MergeObject_MergeTwoObjectIsCorrect()
		{
			string objID = ObjectId.GenerateNewId().ToString();
			const string stateName = "Start";

			var stateDTO = new Mock<StateDTO>();
			stateDTO.Setup(x => x.StateName).Returns(stateName);


			var bpDTO = new BPDTO() { ID = objID };

			//Test case 1
			BPDTO mergeResult = Core.Modules.CoreExtensions.MergeObject(stateDTO.Object, bpDTO);
			var expectedResult = JsonConvert.SerializeObject(new BPDTO { StateName = stateName, ID = objID });
			var actualResult = JsonConvert.SerializeObject(mergeResult);
			Assert.Equal(expectedResult, actualResult);
		}


		[Fact]
		public void MergeObject_MergeTwoObjectIsCorrect_Test2()
		{
			string objID = ObjectId.GenerateNewId().ToString();
			const string stateName = "Start";

			var stateDTO = new Mock<StateDTO>();
			stateDTO.Setup(x => x.StateName).Returns(stateName);


			var bpDTO = new BPDTO() { ID = objID };



			//Test case 2
			const StateTypes stateType = StateTypes.Start;
			stateDTO.Setup(x => x.StateType).Returns(stateType);
			BPDTO mergeResult = Core.Modules.CoreExtensions.MergeObject(stateDTO.Object, bpDTO, new List<string>() {
				nameof(StateDTO.StateType)
			});
			var expectedResult = JsonConvert.SerializeObject(new BPDTO { StateName = stateName, ID = objID, StateType = stateType });
			var actualResult = JsonConvert.SerializeObject(mergeResult);
			Assert.NotEqual(mergeResult.StateType, stateType);
			Assert.NotEqual(expectedResult, actualResult);
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