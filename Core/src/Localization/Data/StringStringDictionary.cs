using System;
using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using UnityEngine;

namespace Core.Localization.Data
{
	[Serializable]
	public sealed class StringStringDictionary : IPairDictionary<string, string>
	{
		[Serializable]
		public sealed class StringStringPair : Pair<string, string> {}

		[SerializeField] private List<StringStringPair> pairs;

		public string this[string key] => pairs.First(p => p.key == key).value;

		public bool TryGetValue(string key, out string value)
		{
			var contains = pairs.Any(p => string.Equals(p.key, key, StringComparison.OrdinalIgnoreCase));
			value = contains ? this[key] : key;
			return contains;
		}
	}
}