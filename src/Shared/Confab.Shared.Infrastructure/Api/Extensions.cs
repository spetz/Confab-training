﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Confab.Shared.Infrastructure.Api
{
    public static class Extensions
    {
        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value)
            => model.Bind<T, object>(expression, value);

        public static T BindId<T>(this T model, Expression<Func<T, string>> expression)
            => model.Bind(expression, Guid.NewGuid());

        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression,
            object value)
        {
            var memberExpression = expression.Body as MemberExpression ??
                                   ((UnaryExpression) expression.Body).Operand as MemberExpression;
            if (memberExpression is null)
            {
                return model;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();
            var field = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field is null)
            {
                return model;
            }

            field.SetValue(model, value);

            return model;
        }
    }
}