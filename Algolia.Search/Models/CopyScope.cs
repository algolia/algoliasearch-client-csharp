using System.Runtime.Serialization;

namespace Algolia.Search.Models
{
	public enum CopyScope
	{
		/// <summary>
		/// copy settings.
		/// </summary>
		[EnumMember(Value = "settings")]
		SETTINGS,
		/// <summary>
		/// copy synonyms.
		/// </summary>
		[EnumMember(Value = "synonyms")]
		SYNONYMS,
		/// <summary>
		/// copy rules.
		/// </summary>
		[EnumMember(Value = "rules")]
		RULES
	}
}
