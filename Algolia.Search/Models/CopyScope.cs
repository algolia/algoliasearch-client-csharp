using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
