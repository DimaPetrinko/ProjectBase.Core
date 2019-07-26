using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Core.Localization.UI
{
	[AddComponentMenu("UI/LocalizedTextMeshPro - Text (UI)", 12)]
	public sealed class LocalizedTMPText : TextMeshProUGUI
	{
		protected override void Awake()
		{
			base.Awake();
			if (UnityEngine.Application.isPlaying) UpdateLocalization();
		}

		public bool autoLocalize { get; set; } = true;

		public new string text
		{
			get => base.text;
			set => base.text = autoLocalize && UnityEngine.Application.isPlaying ? LocalizeText(value) : value;
		}

		public void UpdateLocalization() => base.text = LocalizeText(text);

		private static string LocalizeText(string value)
		{
			if (!value.Contains("[") || !value.Contains("]")) return value;
			var matches = Regex.Matches(value, @"\[(.+?)\]").Cast<Match>();
			matches.ToList().ForEach(match =>
			{
				var key = match.ToString();
				value = value.Replace(key, Localization.GetValue(key));
			});
			return value;
		}
	}
}