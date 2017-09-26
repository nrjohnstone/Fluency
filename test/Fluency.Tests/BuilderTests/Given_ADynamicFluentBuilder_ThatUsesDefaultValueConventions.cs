﻿using System;
using Fluency.DataGeneration;
using FluentAssertions;
using Xunit;

namespace Fluency.Tests.BuilderTests
{
    public class Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions : DynamicFluentBuilderTests
    {
        public class WhenBuildingTheObject : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;

            public WhenBuildingTheObject()
            {
                _result = Sut.build();
            }

            [Fact]
            public void It_Should_UseDefaultConventionFor_FirstNameProperty() =>
                _result.FirstName.Should().BeOneOf(RandomData.FirstNames);

            [Fact]
            public void It_Should_UseDefaultConventionFor_LastNameProperty() =>
                _result.LastName.Should().BeOneOf(RandomData.LastNames);

            [Fact]
            public void It_Should_UseDefaultConventionFor_DateTimeProperty() =>
                _result.DateTimeProperty.Should().NotBe(default(DateTime));

            [Fact]
            public void It_Should_UseDefaultConventionFor_IntegerProperty() =>
                _result.IntegerProperty.Should().NotBe(0);

            [Fact]
            public void It_Should_UseDefaultConventionFor_StringProperty() =>
                _result.StringProperty.Length.Should().Be(20);
        }

        public class
            WhenBuildingTheObject_WithProvidedDateTime : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly DateTime _providedDateTime = DateTime.Parse("1-Jan-2000 10:00");

            public WhenBuildingTheObject_WithProvidedDateTime()
            {
                _result = Sut.With(x => x.DateTimeProperty, _providedDateTime).build();
            }

            [Fact]
            public void It_Should_UseProvidedDateTime() => 
                _result.DateTimeProperty.Should().Be(_providedDateTime);
        }

        public class
            WhenBuildingTheObject_WithProvidedInteger : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly int _providedInteger = 1234;

            public WhenBuildingTheObject_WithProvidedInteger()
            {
                _result = Sut.With(x => x.IntegerProperty, _providedInteger).build();
            }

            [Fact]
            public void It_Should_UseProvidedInteger() =>
                _result.IntegerProperty.Should().Be(_providedInteger);
        }

        public class
            WhenBuildingTheObject_WithProvidedString : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly string _providedString = "1234";

            public WhenBuildingTheObject_WithProvidedString()
            {
                _result = Sut.With(x => x.StringProperty, _providedString).build();
            }

            [Fact]
            public void It_Should_UseProvidedString() =>
                _result.StringProperty.Should().Be(_providedString);
        }

        public class
            WhenBuildingTheObject_WithProvidedValueUsingFor : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly string _providedString = "1234";

            public WhenBuildingTheObject_WithProvidedValueUsingFor()
            {
                _result = Sut.For(x => x.StringProperty, _providedString).build();
            }

            [Fact]
            public void It_Should_UseProvidedValue() =>
                _result.StringProperty.Should().Be(_providedString);
        }

        public class
            WhenBuildingTheObject_WithProvidedValueUsingHaving : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly string _providedString = "1234";

            public WhenBuildingTheObject_WithProvidedValueUsingHaving()
            {
                _result = Sut.Having(x => x.StringProperty, _providedString).build();
            }

            [Fact]
            public void It_Should_UseProvidedValue() =>
                _result.StringProperty.Should().Be(_providedString);
        }

        public class
            WhenBuildingTheObject_AfterSpecifyingDynamicPropertyBuilder_UsingWith : Given_ADynamicFluentBuilder_ThatUsesDefaultValueConventions
        {
            private readonly TestClass _result;
            private readonly TestClass _expectedValue;

            public WhenBuildingTheObject_AfterSpecifyingDynamicPropertyBuilder_UsingWith()
            {
                _expectedValue = new TestClass { FirstName = "Bob", LastName = "Smith" };
                FluentBuilder<TestClass> propertyBuilder = new DynamicFluentBuilder<TestClass>().AliasFor(_expectedValue);

                _result = Sut.With(x => x.ReferenceProperty, propertyBuilder).build();
            }

            [Fact]
            public void It_Should_UseProvidedValue() =>
                _result.ReferenceProperty.Should().Be(_expectedValue);
        }
    }
}