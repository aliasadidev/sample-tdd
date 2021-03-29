using Newtonsoft.Json;
using Xunit;

namespace SampleTDD.UnitTest.Modules
{
	public static class Extensions
	{
		public static void IsEqula(this object expectedObj, object actualObj)
		{
			var obj1 = JsonConvert.SerializeObject(expectedObj);
			var obj2 = JsonConvert.SerializeObject(actualObj);
			Assert.Equal(obj1, obj2);
		}
	}
}