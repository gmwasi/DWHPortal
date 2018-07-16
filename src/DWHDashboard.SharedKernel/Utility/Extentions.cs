using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.SharedKernel.Utility
{
    public static class Extentions
    {
        /// <summary>
        /// Determines if a nullable Guid (Guid?) is null or Guid.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return !guid.HasValue || guid.Value == Guid.Empty;
        }

        /// <summary>
        /// Determines if Guid is Guid.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static IEnumerable<T> SniffOutNew<T>(this IEnumerable<T> newStuff, IEnumerable<T> oldStuff, IEqualityComparer<T> comparer)
        {
            return newStuff.Except(oldStuff, comparer).ToList();
        }

        public static IEnumerable<T> SniffOutUpdated<T>(this IEnumerable<T> oldStuff, IEnumerable<T> newStuff, IEqualityComparer<T> comparer)
        {
            return oldStuff.Intersect(newStuff, comparer).ToList();
        }

        public static IEnumerable<T> SniffOutVoided<T>(this IEnumerable<T> oldStuff, IEnumerable<T> newStuff, IEqualityComparer<T> comparer)
        {
            return oldStuff.Except(newStuff, comparer).ToList();
        }
    }
}