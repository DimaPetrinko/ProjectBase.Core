namespace Core.UI
{
	public abstract class TargetView<TTarget> : View
	{
		protected TTarget Target { get; private set; }

		public void Show(TTarget target)
		{
			Target = target;
			base.Show();
		}

		public new void Hide()
		{
			Target = default;
			base.Hide();
		}
	}
}