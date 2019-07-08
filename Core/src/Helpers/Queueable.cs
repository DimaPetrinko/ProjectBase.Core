using System;
using UnityEngine;

namespace Core.Helpers
{
	public sealed class Queueable<TType>
	{
		private TType value;

		public void Enqueue(TType valueToEnqueue)
		{
			if (value != null)
				Debug.LogWarning(
					$"A Queueable of type {typeof(TType)} overrides the previous value that was not dequeued");
			value = valueToEnqueue;
		}

		public TType Peek()
		{
			if (value == null)
				throw new ArgumentNullException($"A Queueable of type {typeof(TType)} was not previously enqueued");
			return value;
		}

		public TType Dequeue()
		{
			if (value == null)
				throw new ArgumentNullException($"A Queueable of type {typeof(TType)} was not previously enqueued");
			var valueToReturn = value;
			value = default;
			return valueToReturn;
		}
	}
}