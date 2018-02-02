using System;
using Fluency.Probabilities;
using FluentAssertions;
using Xunit;

namespace Fluency.Net.Standard.Tests.Probabilities
{
    public class PercentChanceOfTests
    {
        public class Declaring_a_percent_chance_that_is_greater_than_100
        {
            static Exception exception;

            [Fact]
            public void should_fail() =>
                Catch.Exception(() => new OutcomeSpecification<int>(101, 0)).Should()
                    .BeOfType<ArgumentException>();
        }

        public class Declaring_a_percent_chance_that_is_less_than_0
        {
            [Fact]
            public void should_fail() =>
                Catch.Exception(() => new OutcomeSpecification<int>(-1, 0)).Should()
                    .BeOfType<ArgumentException>();
        }

        public class Declaring_a_percent_chance_that_is_0
        {
            public Declaring_a_percent_chance_that_is_0()
            {
                _result = new OutcomeSpecification<int>(0, 0);
            }

            [Fact]
            public void should_succeed() => _result.Should().NotBeNull();

            [Fact]
            public void should_have_a_zero_percent_chance() => _result.PercentChance.Should().Be(0);

            private readonly OutcomeSpecification<int> _result;
        }

        public class Declaring_a_percent_chance_that_is_100
        {
            public Declaring_a_percent_chance_that_is_100()
            {
                _result = new OutcomeSpecification<int>(100, 0);
            }

            [Fact]
            public void should_succeed() => _result.Should().NotBeNull();

            [Fact]
            public void should_have_a_100_percent_chance() => _result.PercentChance.Should().Be(100);

            private readonly OutcomeSpecification<int> _result;
        }
    }
}