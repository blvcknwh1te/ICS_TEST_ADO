using System.Linq.Expressions;

namespace ICS_Test_Accounting.Tools
{
    public static class Extensions
    {
        public static string Capitalize(this string str)
        {
            if (str.Length == 0)
                return str;
            else if (str.Length == 1)
                str.ToUpper();
            else
                str = (char.ToUpper(str[0]) + str.Substring(1));

            return str;
        }

        public static IOrderedEnumerable<TSource> OrderByColumn<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedEnumerable<TSource>)result;
        }

    }
}
