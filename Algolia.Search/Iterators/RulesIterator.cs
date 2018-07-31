using Algolia.Search.Models;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Iterators
{
	public class RulesIterator : IEnumerable<JObject>
	{
	    IIndex _index;
		int _hitsPerPage;

		public RulesIterator(IIndex index, int hitsPerPage = 1000)
		{
			_index = index;
			_hitsPerPage = hitsPerPage;
		}

		public IEnumerator<JObject> GetEnumerator()
		{
			return new RulesEnumerator(_index, _hitsPerPage);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RulesEnumerator(_index, _hitsPerPage);
		}
	}

	public class RulesEnumerator : IEnumerator<JObject>
	{
	    IIndex _index;
		JObject _answer;
		RuleQuery _ruleQuery;
		int _pos;
		JObject _rule;

		public RulesEnumerator(IIndex index, int hitsPerPage = 1000)
		{
			_index = index;
			_ruleQuery = new RuleQuery();
			_ruleQuery.Page = 0;
			_ruleQuery.HitsPerPage = hitsPerPage;
			Reset();
		}

		private void LoadNextPage()
		{
			_answer = _index.SearchRulesAsync(null, _ruleQuery).GetAwaiter().GetResult();
			_pos = 0;
			_ruleQuery.Page += 1;
		}

		public JObject Current => _rule;

	    object IEnumerator.Current => _rule;

	    public bool MoveNext()
		{
			while (true)
			{
				if (_pos < ((JArray)_answer["hits"]).Count())
				{
					_rule = ((JArray)_answer["hits"])[_pos++].ToObject<JObject>();
					_rule.Remove("_highlightResult");
					return true;
				}
				if (((JArray)_answer["hits"]).Count != 0)
				{
					LoadNextPage();
					continue;
				}
				return false;
			}

		}

		public void Reset()
		{
			_pos = 0;
			_answer = new JObject();
			_ruleQuery.Page = 0;
			LoadNextPage();
		}

		public void Dispose()
		{
			// Nothing to do
		}
	}
}
