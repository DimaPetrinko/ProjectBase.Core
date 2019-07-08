using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
	public static class ScenesLoader
	{
		public static event Action<Scene, LoadSceneMode> SceneLoaded;
		public static event Action<Scene> SceneUnloaded;

		public static Action<AsyncOperation> ShowLoadingScreenAction;

		private static readonly Dictionary<string, bool> SetActives = new Dictionary<string, bool>();

		/// <summary>
		///     Gets an active scene name
		/// </summary>
		/// <returns>Returns the name of the active scene</returns>
		public static string CurrentSceneName => SceneManager.GetActiveScene().name;
		public static int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

		#region LoadScene

		/// <summary>
		///     Loads a given scene
		/// </summary>
		/// <param name="sceneIndex">A scene to load</param>
		/// <param name="setActive">Whether to set this scene active after loading</param>
		/// <param name="loadAsync">Whether to load the scene asynchronously</param>
		/// <param name="additive">Can be either single or additional</param>
		public static void LoadScene(int sceneIndex, bool setActive = true, bool loadAsync = true, bool additive = true) =>
			LoadScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name, setActive, loadAsync, additive);

		/// <summary>
		///     Loads a given scene
		/// </summary>
		/// <param name="sceneName">A scene to load</param>
		/// <param name="setActive">Whether to set this scene active after loading</param>
		/// <param name="loadAsync">Whether to load the scene asynchronously</param>
		/// <param name="additive">Can be either single or additional</param>
		public static void LoadScene(string sceneName, bool setActive = true, bool loadAsync = true, bool additive = true)
		{
			SetActives.Add(sceneName, setActive);
			SceneManager.sceneLoaded += OnSceneLoaded;

			var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
			if (!loadAsync) SceneManager.LoadScene(sceneName, mode);
			else
			{
				var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
				ShowLoadingScreenAction(asyncOperation);
			}
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			var setActive = true;
			if (SetActives.ContainsKey(scene.name))
			{
				setActive = SetActives[scene.name];
				SetActives.Remove(scene.name);
			}
			if (setActive) SceneManager.SetActiveScene(scene);
			SetActives.Remove(scene.name);
			SceneLoaded?.Invoke(scene, mode);
		}

		#endregion

		#region SetActiveScene

		/// <summary>
		/// Sets the scene active
		/// </summary>
		/// <param name="sceneIndex">A scene to activate</param>
		public static void SetActiveScene(int sceneIndex) =>
			SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name);

		/// <summary>
		/// Sets the scene active
		/// </summary>
		/// <param name="sceneName">A scene to activate</param>

		public static void SetActiveScene(string sceneName) =>
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

		#endregion

		#region UnloadScene

		/// <summary>
		///     Unloads a given scene
		/// </summary>
		/// <param name="sceneIndex">A scene to unload</param>
		public static void UnloadScene(int sceneIndex) =>
			UnloadScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name);

		/// <summary>
		///     Unloads a given scene
		/// </summary>
		/// <param name="sceneName">A scene to unload</param>
		public static void UnloadScene(string sceneName)
		{
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			SceneManager.UnloadSceneAsync(sceneName);
		}

		private static void OnSceneUnloaded(Scene scene)
		{
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			SceneUnloaded?.Invoke(scene);
		}

		#endregion

		#region MoveObjectToScene

		/// <summary>
		///     Moves a gameObject to a given scene
		/// </summary>
		/// <param name="gameObject">gameObject to move</param>
		/// <param name="sceneIndex">The index of a scene to move the gameObject to</param>
		public static void MoveObjectToScene(GameObject gameObject, int sceneIndex) =>
			MoveObjectToScene(gameObject, SceneManager.GetSceneByBuildIndex(sceneIndex).name);

		/// <summary>
		///     Moves a gameObject to a given scene
		/// </summary>
		/// <param name="gameObject">gameObject to move</param>
		/// <param name="sceneName">The name of a scene to move the gameObject to</param>
		public static void MoveObjectToScene(GameObject gameObject, string sceneName) =>
			SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));

		#endregion
	}
}