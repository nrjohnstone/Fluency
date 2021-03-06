﻿using System.Collections.Generic;
using System.Linq;
using Fluency.Probabilities;
using Fluency.Utils;
using FluentAssertions;
using Xunit;

namespace Fluency.Net.Standard.Tests.Probabilities
{
    public class ProbabilitySpecificationTests
    {
        public class Getting_the_outcome_of_a_probability_with_a_single_100_percent_chance_outcome
        {
            public Getting_the_outcome_of_a_probability_with_a_single_100_percent_chance_outcome()
            {
                _probability = new ProbabilitySpecification<int>().PercentOutcome(100, expectedOutcome);
                outcome = _probability.GetOutcome();
            }

            [Fact]
            public void should_return_the_single_100_percent_outcome() =>
                outcome.Should().Be(expectedOutcome);

            private const int expectedOutcome = 1;
            private ProbabilitySpecification<int> _probability;
            private readonly int outcome;
        }

        public class When_getting_10_outcomes_of_a_probability_having_two_50_50_outcomes
        {
            public When_getting_10_outcomes_of_a_probability_having_two_50_50_outcomes()
            {
                _probability = new ProbabilitySpecification<int>()
                    .PercentOutcome(50, outcome1)
                    .PercentOutcome(50, outcome2);
                _outcomes = 10.Times().Select(x => _probability.GetOutcome());
            }

            [Fact]
            public void should_return_at_least_one_of_each_outcome()
            {
                _outcomes.Should().Contain(outcome1);
                _outcomes.Should().Contain(outcome2);
            }

            private const int outcome1 = 1;
            private const int outcome2 = 2;
            private ProbabilitySpecification<int> _probability;
            private readonly IEnumerable<int> _outcomes;
        }

        public class When_getting_10_outcomes_of_a_probability_having_a_zero_percent_outcome
        {
            public When_getting_10_outcomes_of_a_probability_having_a_zero_percent_outcome()
            {
                probability = new ProbabilitySpecification<int>().PercentOutcome(0, outcome);
                outcomes = 10.Times().Select(x => probability.GetOutcome());
            }

            [Fact]
            public void should_never_return_the_zero_percent_outcome()
            {
                outcomes.Should().NotContain(outcome);
            }

            const int outcome = 1;
            static ProbabilitySpecification<int> probability;
            static IEnumerable<int> outcomes;
        }
    }
}