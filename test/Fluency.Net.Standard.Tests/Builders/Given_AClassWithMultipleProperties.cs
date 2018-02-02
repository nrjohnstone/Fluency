using Fluency.Utils;
using FluentAssertions;
using Xunit;

namespace Fluency.Net.Standard.Tests.Builders
{
    public class Given_AClassWithMultipleProperties
    {
        private class AClassWithTwoProperties
        {
            public bool PropertyASetterCalled;
            public bool PropertyBSetterCalled;
            private string _propertyA;
            private string _propertyB;

            public string PropertyA
            {
                get { return _propertyA; }
                set
                {
                    PropertyASetterCalled = true;
                    _propertyA = value;
                }
            }

            public string PropertyB
            {
                get { return _propertyB; }
                set
                {
                    PropertyBSetterCalled = true;
                    _propertyB = value;
                }
            }
        }

        private class AClassWithTwoPropertiesBuilder : FluentBuilder<AClassWithTwoProperties>
        {
            public AClassWithTwoPropertiesBuilder WithPropertyA(string value)
            {
                SetProperty(x => x.PropertyA, value);
                return this;
            }
        }

        public class When_builder_is_not_configured_to_ignore_any_property : Given_AClassWithMultipleProperties
        {
            [Fact]
            public void should_populate_all_properties()
            {
                var builder = new AClassWithTwoPropertiesBuilder();

                var instance = builder.build();

                instance.PropertyASetterCalled.Should().Be(true);
                instance.PropertyBSetterCalled.Should().Be(true);
            }

            [Fact]
            public void and_a_value_has_not_been_set_should_return_the_default_value()
            {
                var builder = new AClassWithTwoPropertiesBuilder();

                builder.GetValue(x => x.PropertyA).Should().NotBeEmpty();
            }
        }

        public class When_builder_is_configured_to_ignore_a_property : Given_AClassWithMultipleProperties
        {
            [Fact]
            public void should_not_call_setter_for_ignored_property()
            {
                var builder = new AClassWithTwoPropertiesBuilder();
                builder.IgnoreProperty(x => x.PropertyB);

                var instance = builder.build();

                instance.PropertyASetterCalled.Should().Be(true);
                instance.PropertyBSetterCalled.Should().Be(false);
            }

            [Fact]
            public void and_a_value_is_set_for_a_property_should_return_the_value()
            {
                var builder = new AClassWithTwoPropertiesBuilder();
                builder.IgnoreProperty(x => x.PropertyB);
                builder.SetProperty(x => x.PropertyB, "Foo");

                builder.GetValue(x => x.PropertyB).Should().Be("Foo");
            }
        }

        public class When_builder_is_configured_to_ignore_multiple_properties : Given_AClassWithMultipleProperties
        {
            [Fact]
            public void should_not_call_setter_for_any_property()
            {
                var builder = new AClassWithTwoPropertiesBuilder();
                builder.IgnoreProperty(x => x.PropertyA);
                builder.IgnoreProperty(x => x.PropertyB);

                var instance = builder.build();

                instance.PropertyASetterCalled.Should().Be(false);
                instance.PropertyBSetterCalled.Should().Be(false);
            }
        }

        public class When_builder_is_configured_to_ignore_all_properties : Given_AClassWithMultipleProperties
        {
            [Fact]
            public void should_not_call_setter_for_any_property()
            {
                var builder = new AClassWithTwoPropertiesBuilder();
                builder.IgnoreAllProperties();

                var instance = builder.build();

                instance.PropertyASetterCalled.Should().Be(false);
                instance.PropertyBSetterCalled.Should().Be(false);
            }

            [Fact]
            public void and_IgnoreAllProperties_called_after_explicitly_set_values_should_still_set_explicit_properties()
            {
                var builder = new AClassWithTwoPropertiesBuilder();

                builder.WithPropertyA("Foo");
                builder.IgnoreAllProperties();

                var instance = builder.build();

                instance.PropertyA.Should().Be("Foo");
                instance.PropertyBSetterCalled.Should().Be(false);
            }
        }

        public class
            When_builder_is_configured_to_ignore_all_properties_and_a_property_is_set_explicitly :
                Given_AClassWithMultipleProperties
        {
            [Fact]
            public void should_set_explicit_property()
            {
                var builder = new AClassWithTwoPropertiesBuilder();
                builder.IgnoreAllProperties();
                builder.WithPropertyA("Foo");

                var instance = builder.build();

                instance.PropertyA.Should().Be("Foo");
                instance.PropertyBSetterCalled.Should().Be(false);
            }
        }

    }
}