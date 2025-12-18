using System.Collections.Generic;

namespace CommandBridge.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> WithoutNulls<T>(this IEnumerable<T?> source) 
            where T : class
        {
            foreach (var item in source) 
            { 
                if (item != null) 
                    yield return item;
            }
        }
    }
}