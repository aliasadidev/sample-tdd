namespace SampleTDD.Core.DTOs.Security
{
	/// <summary>
	/// Permission
	/// </summary>
	public class PermissionDTO
	{
		/// <summary>
		/// Can Approve 
		/// </summary>
		public bool CanApprove { get; set; }
		/// <summary>
		/// Can Reject 
		/// </summary>
		public bool CanReject { get; set; }
		/// <summary>
		/// Can Start 
		/// </summary>
		public bool CanStart { get; set; }
		/// <summary>
		/// Can Close 
		/// </summary>
		public bool CanClose { get; set; }
		/// <summary>
		/// Can Finalize
		/// </summary>
		public bool CanFinalize { get; set; }
	}
}
