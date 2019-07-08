using System;
using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using UnityEngine;

namespace Core.Localization.Data
{
	[Serializable]
	public sealed class LanguageLocalizationAssetDictionary : IPairDictionary<SystemLanguage, LocalizationAsset>
	{
		[Serializable]
		public sealed class LanguageLocalizationAssetPair : Pair<SystemLanguage, LocalizationAsset> {}

		[SerializeField] private List<LanguageLocalizationAssetPair> pairs;

		public LocalizationAsset this[SystemLanguage key] => pairs.First(p => p.key == key).value;

		public bool TryGetValue(SystemLanguage key, out LocalizationAsset value)
		{
			var contains = pairs.Any(p => p.key == key);
			value = contains ? this[key] : null;
			return contains;
		}
	}
}