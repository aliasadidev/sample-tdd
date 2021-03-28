using System;
using System.Collections.Generic;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;

namespace SampleTDD.Core.Collections
{

	public class ConditionStateOperations
	{
		public Func<RoleTypes, BP, IEnumerable<ChangeStateRule>, IEnumerable<ChangeStateRule>> ApproveCondition { get; set; }
	}

}
