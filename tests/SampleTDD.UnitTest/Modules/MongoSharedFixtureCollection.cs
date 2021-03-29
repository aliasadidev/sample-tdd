using Xunit;

namespace SampleTDD.UnitTest.Modules
{

	[CollectionDefinition(nameof(SharedFixtureCollection))]
	public class SharedFixtureCollection : ICollectionFixture<SharedFixture>
	{

	}
}