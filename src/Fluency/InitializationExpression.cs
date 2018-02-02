using System;
using System.Collections.Generic;
using Fluency.Conventions;
using Fluency.DataGeneration;
using Fluency.IdGenerators;

namespace Fluency
{
    public class InitializationExpression
    {
        private readonly FluencyConfiguration config = new FluencyConfiguration();


        internal FluencyConfiguration GetConfiguration()
        {
            return config;
        }


        public void IdGeneratorIsConstructedBy(Func<IIdGenerator> func)
        {
            config.ConstructIdGenerator = func;
        }


        public void UseDefaultValueConventions()
        {
            config.DefaultValueConventions = new List<IDefaultConvention>
            {
                // Strings
                Convention.ByName( "Firstname", p=> ARandom.FirstName() ),
                Convention.ByName( "LastName", p => ARandom.LastName() ),
                Convention.ByName( "FullName", p => ARandom.FullName() ),
                Convention.ByName( "City", p => ARandom.City() ),
                Convention.ByName( "State", p => ARandom.StateCode() ),
                Convention.ByName( "Zip", p => ARandom.ZipCode() ),
                Convention.ByName( "ZipCode", p => ARandom.ZipCode() ),
                Convention.ByName( "PostalCode", p => ARandom.ZipCode() ),
                Convention.ByName( "Email", p => ARandom.Email() ),
                Convention.ByName( "Address", p => String.Format( "{0} {1} {2}",
                    ARandom.IntBetween( 10, 9999 ),
                    ARandom.LastName(),
                    ARandom.ItemFrom( "Street", "Lane", "Ave.", "Blvd." ) ) ),
                Convention.ByName( "Phone", p => ARandom.StringPattern( "999-999-9999" ) ),
                Convention.ByName( "HomePhone", p => ARandom.StringPattern( "999-999-9999" ) ),
                Convention.ByName( "WorkPhone", p => ARandom.StringPattern( "999-999-9999" ) ),
                Convention.ByName( "BusinessPhone", p => ARandom.StringPattern( "999-999-9999" ) ),
                Convention.ByName( "Fax", p => ARandom.StringPattern( "999-999-9999" ) ),
                Convention.String( 20 ),
                // Dates
                Convention.ByName( "BirthDate", p => ARandom.BirthDate() ),
                Convention.DateType(),
                Convention.IntegerType(),
                Convention.ByType< Decimal>( p => ARandom.CurrencyAmount()  )
            };
        }
    }
}