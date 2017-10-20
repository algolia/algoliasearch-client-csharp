using Algolia.Search.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algolia.Search.Iterators
{
    public class RulesIterator : IEnumerable<JObject>
    {
        Index _index;
        int _hitsPerPage;

        public RulesIterator(Index index, int hitsPerPage = 1000)
        {
            this._index = index;
            this._hitsPerPage = hitsPerPage;
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
        Index _index;
        JObject _answer;
        RuleQuery _ruleQuery;
        int _pos;
        JObject _rule;

        public RulesEnumerator(Index index, int hitsPerPage = 1000)
        {
            this._index = index;
            this._ruleQuery = new RuleQuery();
            this._ruleQuery.Page = 0;
            this._ruleQuery.HitsPerPage = hitsPerPage;
            Reset();
        }

        private void LoadNextPage()
        {
            _answer = _index.SearchRulesAsync(null, _ruleQuery).GetAwaiter().GetResult();
            _pos = 0;
            _ruleQuery.Page += 1;
        }

        public JObject Current
        {
            get { return _rule; }
        }

        object IEnumerator.Current
        {
            get { return _rule; }
        }

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
            this._ruleQuery.Page = 0;
            LoadNextPage();
        }

        public void Dispose()
        {
            // Nothing to do
        }
    }
}
