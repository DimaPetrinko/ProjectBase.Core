using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Timer
{
	public sealed class Timer : MonoBehaviour
	{
		private static Timer instance;

		private Dictionary<TimerBadge, Coroutine> coroutines;
		private Dictionary<TimerBadge, Action> updateActions;
		private Dictionary<TimerBadge, Action> fixedUpdateActions;

		private List<Action> updateActionsList;
		private List<Action> fixedUpdateActionsList;

		public static bool Exists => instance != null;

		#region Set
		
		/// <summary>
		/// Execute callback after seconds
		/// </summary>
		/// <param name="seconds">Time for executing action</param>
		/// <param name="callback">Callback to invoke after seconds</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge Set(float seconds, Action callback)
		{
			var timerBadge = new TimerBadge(instance.coroutines.Count, DateTime.Now, seconds);
			instance.coroutines.Add(timerBadge, instance.StartCoroutine(RunTimer(timerBadge, seconds, callback)));
			return timerBadge;
		}

		#endregion

		#region Repeat

		/// <summary>
		/// Repeat action per time interval
		/// </summary>
		/// <param name="action">Action to repeat</param>
		/// <param name="interval">Time interval in seconds</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge Repeat(Action action, float interval)
		{
			var timerBadge = new TimerBadge(instance.coroutines.Count, DateTime.Now);
			instance.coroutines.Add(timerBadge, instance.StartCoroutine(RunInterval(interval, action)));
			return timerBadge;
		}

		/// <summary>
		/// Repeat action per time interval for seconds
		/// </summary>
		/// <param name="action">Action to repeat</param>
		/// <param name="interval">Time interval in seconds</param>
		/// <param name="seconds">Seconds to repeat</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge Repeat(Action action, float interval, float seconds)
		{
			var timerBadge = Repeat(action, interval);
			Set(seconds, () => Stop(timerBadge));
			return timerBadge;
		}

		#endregion

		#region RunInUpdate

		/// <summary>
		/// Execute action every frame forever
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInUpdate(Action action)
		{
			var timerBadge = new TimerBadge(instance.updateActions.Count, DateTime.Now);
			instance.updateActions.Add(timerBadge, action);
			instance.updateActionsList.Add(action);
			return timerBadge;
		}

		/// <summary>
		/// Execute action every frame for seconds
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <param name="seconds">Time for executing action</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInUpdate(Action action, float seconds)
		{
			var timerBadge = RunInUpdate(action);
			Set(seconds, () => StopUpdate(timerBadge));
			return timerBadge;
		}

		/// <summary>
		/// Execute action every frame for seconds
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <param name="seconds">Time for executing action</param>
		/// <param name="onComplete">Executed when timer is ended</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInUpdate(Action action, float seconds, Action onComplete)
		{
			var timerBadge = RunInUpdate(action);
			Set(seconds, () =>
			{
				StopUpdate(timerBadge);
				onComplete();
			});
			return timerBadge;
		}

		#endregion
		
		#region RunInFixedUpdate

		/// <summary>
		/// Execute action every frame forever
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInFixedUpdate(Action action)
		{
			var timerBadge = new TimerBadge(instance.fixedUpdateActions.Count, DateTime.Now);
			instance.fixedUpdateActions.Add(timerBadge, action);
			instance.fixedUpdateActionsList.Add(action);
			return timerBadge;
		}

		/// <summary>
		/// Execute action every frame for seconds
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <param name="seconds">Time for executing action</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInFixedUpdate(Action action, float seconds)
		{
			var timerBadge = RunInFixedUpdate(action);
			Set(seconds, () => StopUpdate(timerBadge));
			return timerBadge;
		}

		/// <summary>
		/// Execute action every frame for seconds
		/// </summary>
		/// <param name="action">Action to run</param>
		/// <param name="seconds">Time for executing action</param>
		/// <param name="onComplete">Executed when timer is ended</param>
		/// <returns>Returns timer badge used to stop the timer</returns>
		public static TimerBadge RunInFixedUpdate(Action action, float seconds, Action onComplete)
		{
			var timerBadge = RunInFixedUpdate(action);
			Set(seconds, () =>
			{
				StopUpdate(timerBadge);
				onComplete();
			});
			return timerBadge;
		}

		#endregion

		#region Stop

		public static void Stop(TimerBadge timerBadge)
		{
			if (!instance.coroutines.ContainsKey(timerBadge)) return;
			instance.StopCoroutine(instance.coroutines[timerBadge]);
			instance.coroutines.Remove(timerBadge);
			timerBadge.Expire();
		}

		public static void StopUpdate(TimerBadge timerBadge)
		{
			if (!instance.updateActions.ContainsKey(timerBadge)) return;
			instance.updateActionsList.Remove(instance.updateActions[timerBadge]);
			instance.updateActions.Remove(timerBadge);
			timerBadge.Expire();
		}

		public static void StopFixedUpdate(TimerBadge timerBadge)
		{
			if (!instance.fixedUpdateActions.ContainsKey(timerBadge)) return;
			instance.fixedUpdateActionsList.Remove(instance.fixedUpdateActions[timerBadge]);
			instance.fixedUpdateActions.Remove(timerBadge);
			timerBadge.Expire();
		}

		public static void StopAll()
		{
			StopAllRegular();
			StopAllUpdates();
			StopAllFixedUpdates();
		}

		public static void StopAllRegular()
		{
			instance.StopAllCoroutines();
			instance.coroutines.Clear();
		}

		public static void StopAllUpdates() => instance.updateActions.Clear();
		public static void StopAllFixedUpdates() => instance.fixedUpdateActions.Clear();

		#endregion

		#region IEnumerators

		private static IEnumerator RunTimer(TimerBadge timerBadge, float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			instance.coroutines.Remove(timerBadge);
			callback?.Invoke();
		}

		private static IEnumerator RunInterval(float interval, Action action)
		{
			var wait = new WaitForSeconds(interval);
			while (true)
			{
				action();
				yield return wait;
			}
		}

		#endregion

		#region Unity methods

		private void Awake()
		{
			coroutines = new Dictionary<TimerBadge, Coroutine>();
			updateActions = new Dictionary<TimerBadge, Action>();
			fixedUpdateActions = new Dictionary<TimerBadge, Action>();
			updateActionsList = new List<Action>();
			fixedUpdateActionsList = new List<Action>();

			instance = this;
		}

		private void Update() => updateActionsList.ToList().ForEach(a => a?.Invoke());
		private void FixedUpdate() => fixedUpdateActionsList.ToList().ForEach(a => a?.Invoke());
		private void OnDestroy() => StopAll();

		#endregion
	}
}