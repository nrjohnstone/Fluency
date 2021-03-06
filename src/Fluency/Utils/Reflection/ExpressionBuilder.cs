﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluency.Utils.Reflection
{
    public class ExpressionBuilder
    {
        public static Expression<Func<T, object>> Create<T>(PropertyInfo property)
        {
            return (Expression<Func<T, object>>)Create(property, typeof(T));
        }

        public static object Create(PropertyInfo property, Type type)
        {
            ParameterExpression param = Expression.Parameter(type, "entity");
            MemberExpression expression = Expression.Property(param, property);
            UnaryExpression castedProperty = Expression.Convert(expression, typeof(object));
            return Expression.Lambda(castedProperty, param);
        }
    }
}