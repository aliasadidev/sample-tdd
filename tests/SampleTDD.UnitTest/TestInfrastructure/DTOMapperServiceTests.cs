using System;
using SampleTDD.Core.Constants;
using Xunit;
using SampleTDD.Infrastructure.Services;
using SampleTDD.Core.DTOs.States.Approve;
using SampleTDD.Core.DTOs.States;
using SampleTDD.Core.Contracts.Services;

namespace SampleTDD.UnitTest.TestInfrastructure
{
	public class DTOMapperServiceTests
	{
		[Theory]
		[InlineData(StateTypes.Start, typeof(SabteDarkhastStateDTO))]
		public void Get_GetDTOTypeByState(StateTypes state, Type dtoType)
		{
			// Arrange
			IDTOMapperService dtoMapperService = new DTOMapperService();

			// Act
			Type bpiType = dtoMapperService.Get(state);

			// Assert
			Assert.Equal(dtoType, bpiType);
		}

		[Theory]
		[InlineData(StateTypes.Start, typeof(SabteDarkhastStateApproveDTO))]
		public void GetApproveType_GetApproveTypeIsCorrect(StateTypes state, Type dtoType)
		{
			// Arrange
			IDTOMapperService dtoMapperService = new DTOMapperService();

			// Act
			Type bpiType = dtoMapperService.GetApproveType(state);

			// Assert
			Assert.Equal(dtoType, bpiType);
		}

		[Theory]
		[InlineData(StateTypes.Start)]
		public void CheckApproveTypeIsMapped_ResultIsCorrect(StateTypes state)
		{
			// Arrange
			IDTOMapperService dtoMapperService = new DTOMapperService();

			// Act
			bool actualResult = dtoMapperService.CheckApproveTypeIsMapped(state);

			// Assert
			Assert.True(actualResult);
		}
	}
}