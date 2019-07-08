using Core.Localization.Data;
using UnityEngine;
using LocalizationAsset = Core.Localization.Data.LocalizationAsset;

namespace Core.Localization
{
	public static class Localization
	{
		private static LanguageLocalizationAssetDictionary languageLocalizationDictionary;
		private static LocalizationAsset currentLocalizationAsset;

		public static SystemLanguage DefaultLanguage { get; set; } = SystemLanguage.English;

		public static void Init(LanguageLocalizationAssetDictionary dictionary) =>
			languageLocalizationDictionary = dictionary;

		public static void SetLanguage(SystemLanguage language)
		{
			var hasLocalization = languageLocalizationDictionary.TryGetValue(language, out var localizationAsset);

			currentLocalizationAsset = hasLocalization
				? localizationAsset
				: languageLocalizationDictionary[DefaultLanguage];
		}

		public static string GetValue(string key)
		{
			if (currentLocalizationAsset == null)
			{
				Debug.LogError("Localization asset was not set!");
				return key;
			}

			currentLocalizationAsset.Dictionary.TryGetValue(key.Substring(1, key.Length - 2), out var value);
			return value;
		}
	}
}