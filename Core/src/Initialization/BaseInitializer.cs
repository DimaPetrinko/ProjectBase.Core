using Core.Application;
using Core.SceneManagement;
using UnityEngine;

namespace Core.Initialization
{
	public abstract class BaseInitializer : MonoBehaviour
	{
		private void Awake()
		{
			if (IsApplicationNotInitialized())
			{
				ScenesLoader.LoadScene(0, true, false, false);
				return;
			}

			Debug.Log($"[{nameof(BaseInitializer)}]: Initializing {gameObject.name}...");
			Init();
		}

		private void Start() => PostInit();

		private void OnDestroy()
		{
			Debug.Log($"[{nameof(BaseInitializer)}]: Deinitializing {gameObject.name}...");
			Deinit();
		}

		protected abstract void Init();
		protected abstract void PostInit();
		protected abstract void Deinit();

		private static bool IsApplicationNotInitialized() => !BaseApplicationManager.IsInitialized &&
			ScenesLoader.CurrentSceneIndex != 0;
	}
}