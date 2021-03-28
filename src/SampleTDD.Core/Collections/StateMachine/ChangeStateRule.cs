using SampleTDD.Core.Constants;

namespace SampleTDD.Core.Collections.StateMachine
{
	public class ChangeStateRule : BaseCollection
	{
		public RoleTypes RoleID { get; set; }
		public OperationTypes OperatinID { get; set; }
		public StateTypes CurrentState { get; set; }
		public StateTypes NextState { get; set; }
	}
}
