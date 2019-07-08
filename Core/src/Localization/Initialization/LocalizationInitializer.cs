using Core.Initialization;
using Core.Localization.Data;
using Core.SceneManagement;
using UnityEngine;

namespace Core.Localization.Initialization
{
	public sealed class LocalizationInitializer : BaseInitializer
	{
		[SerializeField] private LanguageLocalizationAssetDictionary languageLocalizationDictionary;
		[SerializeField] private bool overrideLanguage;
		[SerializeField] private SystemLanguage languageToOverrideWith;
		
		protected override void Init()
		{
			Localization.Init(languageLocalizationDictionary);

			var language = !overrideLanguage ? UnityEngine.Application.systemLanguage : languageToOverrideWith;
			Localization.SetLanguage(language);
		}

		protected override void PostInit() => ScenesLoader.UnloadScene(gameObject.scene.name);

		protected override void Deinit() {}
	}
}