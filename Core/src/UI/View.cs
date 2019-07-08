using UnityEngine;

namespace Core.UI
{
	public abstract class View : MonoBehaviour
	{
		[SerializeField] protected GameObject objectToHide;

		protected virtual void Awake()
		{
			if (objectToHide == null) objectToHide = gameObject;
		}

		protected void OnDestroy() => Hide();

		public void Show()
		{
			if (objectToHide != null) objectToHide.SetActive(true);
			OnShown();
		}

		protected virtual void OnShown() {}

		protected virtual void BeforeHide() {}

		public void Hide()
		{
			BeforeHide();
			if (objectToHide != null) objectToHide.SetActive(false);
		}
	}
}