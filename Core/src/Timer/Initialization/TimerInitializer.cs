using Core.Initialization;

namespace Core.Timer.Initialization
{
	public sealed class TimerInitializer : BaseInitializer
	{
		protected override void Init() {}
		protected override void PostInit() => Destroy(gameObject);
		protected override void Deinit() {}
	}
}