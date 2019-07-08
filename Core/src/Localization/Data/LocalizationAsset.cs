using UnityEngine;

namespace Core.Localization.Data
{
	[CreateAssetMenu(fileName = "LocalizationAsset", menuName = "Localization/LocalizationAsset")]
	public sealed class LocalizationAsset : ScriptableObject
	{
		[SerializeField] private StringStringDictionary dictionary;
		
		public StringStringDictionary Dictionary => dictionary;
	}
}