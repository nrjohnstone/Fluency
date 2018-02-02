using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Fluency.Net.Framework._40.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        internal class TestEntity
        {
            public string PropertyA { get; set; }
        }

        [Test]
        public void DynamicBuilder_ShouldBeUsable()
        {
            DynamicFluentBuilder<TestEntity> builder = new DynamicFluentBuilder<TestEntity>();

            TestEntity result = builder.With(x => x.PropertyA, "Foo").build();

            result.PropertyA.Should().Be("Foo");
        }
    }
}
