using DWHDashboard.SharedKernel.Model;
using System;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Utility
{
    public class TableauEntityComparer<T> : IEqualityComparer<T> where T : TableauEntity
    {
        public bool Equals(T x, T y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            return x != null && y != null && x.TableauId.ToLower().Trim().Equals(y.TableauId.ToLower().Trim());
        }

        public int GetHashCode(T obj)
        {
            int hashTableauId = obj.TableauId == null ? 0 : obj.TableauId.GetHashCode();
            return hashTableauId;
        }
    }
}