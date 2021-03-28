using System.ComponentModel.DataAnnotations;

namespace SampleTDD.Core.Constants
{
	/// <summary>
	/// Roles
	/// </summary>
	public enum RoleTypes : long
	{

		/// <summary>
		/// Admin
		/// </summary>
		[Display(Name = "Admin")]
		Admin = 155,

		/// <summary>
		/// Insured Customer
		/// </summary>
		[Display(Name = "Insured Customer")]
		InsuredCustomer = 159,

	}
}
