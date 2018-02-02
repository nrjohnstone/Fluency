using FluentAssertions;
using Xunit;

namespace Fluency.Net.Framework._461.Tests
{

    public class BuilderTests
    {
        internal class TestEntity
        {
            public string PropertyA { get; set; }
        }

        [Fact]
        public void Builders_ShouldWork_With_NetFramework_461()
        {
            DynamicFluentBuilder<TestEntity> builder = new DynamicFluentBuilder<TestEntity>();

            TestEntity result = builder.With(x => x.PropertyA, "Foo").build();

            result.PropertyA.Should().Be("Foo");
        }
    }
}