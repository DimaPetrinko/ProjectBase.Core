using System;

namespace Core.Helpers
{
	public interface IPairDictionary<TKey, TValue>
	{
		TValue this[TKey key] { get; }
		bool TryGetValue(TKey key, out TValue value);
	}

	[Serializable]
	public abstract class Pair<TKey, TValue>
	{
		public TKey key;
		public TValue value;
	}
}