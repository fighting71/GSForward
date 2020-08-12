using IdentityServer4.Extensions;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AuthApi.Middleware
{
    public static class IdentityServerExt
    {
        public static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        public static string EnsureTrailingSlash(this string url)
        {
            if (url != null && !url.EndsWith("/"))
            {
                return url + "/";
            }
            return url;
        }
		internal static ICollection<string> FindMatchingSigningAlgorithms(this IEnumerable<ApiResource> apiResources)
		{
			List<ApiResource> list = apiResources.ToList();
			if (list.IsNullOrEmpty())
			{
				return new List<string>();
			}
			if (list.Count == 1)
			{
				return list.First().AllowedAccessTokenSigningAlgorithms;
			}
			List<ICollection<string>> list2 = (from r in list
											   where r.AllowedAccessTokenSigningAlgorithms.Any()
											   select r.AllowedAccessTokenSigningAlgorithms).ToList();
			if (list2.Any())
			{
				IEnumerable<string> source = IntersectLists(list2);
				if (source.Any())
				{
					return source.ToHashSet();
				}
				throw new InvalidOperationException("Signing algorithms requirements for requested resources are not compatible.");
			}
			return new List<string>();
		}
		private static IEnumerable<T> IntersectLists<T>(IEnumerable<IEnumerable<T>> lists)
		{
			return lists.Aggregate((IEnumerable<T> l1, IEnumerable<T> l2) => l1.Intersect(l2));
		}
	}
}
