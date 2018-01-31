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
	public class SynonymsIterator : IEnumerable<JObject>
	{
		Index _index;
		int _hitsPerPage;

		public SynonymsIterator(Index index, int hitsPerPage = 1000)
		{
			this._index = index;
			this._hitsPerPage = hitsPerPage;
		}

		public IEnumerator<JObject> GetEnumerator()
		{
			return new SynonymsEnumerator(_index, _hitsPerPage);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SynonymsEnumerator(_index, _hitsPerPage);
		}
	}

	public class SynonymsEnumerator : IEnumerator<JObject>
	{
		Index _index;
		JObject _answer;
		int _hitsPerPage;
		int _page;
		int _pos;
		JObject _synonym;

		public SynonymsEnumerator(Index index, int hitsPerPage = 1000)
		{
			this._index = index;
			this._page = 0;
			this._hitsPerPage = hitsPerPage;
			Reset();
		}

		private void LoadNextPage()
		{
			IEnumerable<Index.SynonymType> synonymType = null;
			_answer = _index.SearchSynonymsAsync(query: "", types: synonymType, page: _page, hitsPerPage: _hitsPerPage).GetAwaiter().GetResult();
			_pos = 0;
			_page += 1;
		}

		public JObject Current
		{
			get { return _synonym; }
		}

		object IEnumerator.Current
		{
			get { return _synonym; }
		}

		public bool MoveNext()
		{
			while (true)
			{
				if (_pos < ((JArray)_answer["hits"]).Count())
				{
					_synonym = ((JArray)_answer["hits"])[_pos++].ToObject<JObject>();
					_synonym.Remove("_highlightResult");
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
			_page = 0;
			LoadNextPage();
		}

		public void Dispose()
		{
			// Nothing to do
		}
	}
}
