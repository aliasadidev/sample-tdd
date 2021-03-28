using System.Collections.Generic;

namespace SampleTDD.Core.DTOs.BPIs.BPISabteDarkhast
{
	public class BPISabteDarkhastDTO
	{
		/// <summary>
		/// نوع خسارت
		/// </summary>
		public ushort NoeKhesaratTypeID { get; set; }
		/// <summary>
		/// کد ملی بیمار
		/// </summary>   
		public string NationalID { get; set; }
		/// <summary>
		/// نام و نام خانوادگی
		/// </summary>
		public string FullName { get; set; }



		/// <summary>
		/// شماره تماس بیمار
		/// </summary>       
		public string PhoneNumber { get; set; }

		public string HomeNumber { get; set; }

		/// <summary>
		/// استان
		/// </summary>
		public ushort ProvinceID { get; set; }
		/// <summary>
		/// شهر
		/// </summary>
		public ushort CityID { get; set; }

		/// <summary>
		/// نام شعبه
		/// </summary>
		public string BranchName { get; set; }
		/// <summary>
		/// کد شعبه
		/// </summary>
		public ushort BranchID { get; set; }


	}
}

