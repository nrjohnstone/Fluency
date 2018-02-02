using System;
using System.Linq;
using Fluency.Probabilities;
using FluentAssertions;
using Xunit;

namespace Fluency.Net.Standard.Tests.Probabilities
{
    public class ProbabilityTests
    {
        public class Defining_a_probability_with_percent_chances_totalling_greater_than_100
        {
            [Fact]
            public void should_fail() =>
                Catch.Exception(() =>
                        Probability.Of<int>()
                            .PercentOutcome(50, 0)
                            .PercentOutcome(51, 1))
                    .Should().BeOfType<ArgumentException>();
        }

        public class Defining_a_probability_with_percent_chances_totalling_100
        {
            public Defining_a_probability_with_percent_chances_totalling_100()
            {
                result =
                    Probability.Of<int>()
                        .PercentOutcome(50, 0)
                        .PercentOutcome(50, 1);
            }

            [Fact]
            public void should_return_a_valid_probability_specification() =>
                result.Should().BeOfType<ProbabilitySpecification<int>>();

            [Fact]
            public void should_contain_each_of_the_specified_percent_chance_specifications() =>
                result.Outcomes.Count().Should().Be(2);

            private readonly ProbabilitySpecification<int> result;
        }
    }
}