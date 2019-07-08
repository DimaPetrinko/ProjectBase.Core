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

		private List<Action> updateActionsList;

		public static bool Exists => instance != null;

		private void Awake()
		{
			coroutines = new Dictionary<TimerBadge, Coroutine>();
			updateActions = new Dictionary<TimerBadge, Action>();
			updateActionsList = new List<Action>();
			
			instance = this;
		}

		public static TimerBadge Set(float seconds, Action callback)
		{
			var timerBadge = new TimerBadge(instance.coroutines.Count, DateTime.Now, seconds);
			instance.coroutines.Add(timerBadge, instance.StartCoroutine(RunTimer(timerBadge, seconds, callback)));
			return timerBadge;
		}

		public static void StopAll()
		{
			instance.StopAllCoroutines();
			instance.coroutines.Clear();
			instance.updateActions.Clear();
		}

		public static void Stop(TimerBadge timerBadge)
		{
			if (!instance.coroutines.ContainsKey(timerBadge)) return;
			instance.StopCoroutine(instance.coroutines[timerBadge]);
			instance.coroutines.Remove(timerBadge);
			timerBadge.Expire();
		}

		public static TimerBadge RunInUpdate(Action action)
		{
			var timerBadge = new TimerBadge(instance.updateActions.Count, DateTime.Now);
			instance.updateActions.Add(timerBadge, action);
			instance.updateActionsList.Add(action);
			return timerBadge;
		}

		public static void StopUpdate(TimerBadge timerBadge)
		{
			if (!instance.updateActions.ContainsKey(timerBadge)) return;
			instance.updateActionsList.Remove(instance.updateActions[timerBadge]);
			instance.updateActions.Remove(timerBadge);
			timerBadge.Expire();
		}

		private static IEnumerator RunTimer(TimerBadge timerBadge, float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			instance.coroutines.Remove(timerBadge);
			callback?.Invoke();
		}

		private void Update() => updateActionsList.ToList().ForEach(a => a?.Invoke());

		private void OnDestroy() => StopAll();
	}
}