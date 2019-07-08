using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
	public static class CollectionsExtensions
	{
		public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
		{
			var rng = new Random();
			var buffer = source.ToList();
			for (var i = 0; i < buffer.Count; i++)
			{
				var j = rng.Next(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
			}
		}

		public static TSource Random<TSource>(this IEnumerable<TSource> source)
		{
			var enumerable = source as TSource[] ?? source.ToArray();
			var index = UnityEngine.Random.Range(0, enumerable.Length);
			return enumerable.ElementAtOrDefault(index);
		}
		
		public static void ForEach<TSource>(this IList<TSource> source, Action<TSource, int> action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			for (var index = 0; index < source.Count; ++index) action(source[index], index);
		}
	}
}