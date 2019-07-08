using Core.Initialization;

namespace Core.Application
{
	public abstract class BaseApplicationManager : IManager
	{
		public static bool IsInitialized { get; protected set; }
		
		public abstract void Start();
		public abstract void Stop();
	}
}