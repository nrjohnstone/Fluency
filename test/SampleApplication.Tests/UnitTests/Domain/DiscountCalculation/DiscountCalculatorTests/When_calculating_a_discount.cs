// Copyright 2011 Chris Edwards
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Fluency.Utils;
using FluentAssertions;
using SampleApplication.Domain;
using SampleApplication.Domain.DiscountCalculation;
using SampleApplication.Tests.FluentBuilders;
using Xunit;

namespace SampleApplication.Tests.UnitTests.Domain.DiscountCalculation.DiscountCalculatorTests
{
    public class When_calculating_a_tiered_discount
    {
        readonly DiscountCalculator _calculator;

        public When_calculating_a_tiered_discount()
        {
            IDiscountStrategy tieredDiscountStrategy = DiscountStrategyBuilder.BuildTieredStrategy()
                .Where.OrdersGreaterThanOrEqualTo(100.dollars()).GetDiscountOf(10.Percent())
                .Build();

            _calculator = new DiscountCalculator(tieredDiscountStrategy);
        }

        [ Fact ]
        public void For_an_order_less_than_100_dollars()
        {
            Order order = an.Order
                    .With( a.LineItem
                                .Costing( 100.dollars() ).build()
                    )
                    .build();

            double discount = _calculator.CalculateDiscount( order );

            discount.Should().Be( 10.dollars() );
        }
    }
}