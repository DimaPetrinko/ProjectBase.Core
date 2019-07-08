using System;

namespace Core.Timer
{
	public struct TimerBadge
	{
		public int Id { get; }
		public DateTime StartTime { get; }
		public float Duration { get; }
		public bool Running { get; private set; }

		public TimerBadge(int id, DateTime startTime, float duration = -1)
		{
			Id = id;
			StartTime = startTime;
			Duration = duration;
			Running = true;
		}

		public void Expire() => Running = false;
	}
}