using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASiNet.Searcher.API;
public class SearchResult
{
    public SearchResult(string inputText)
    {
        InputText = inputText;
        _results = new();
    }

    public SearchResultItem this[int index]
    {
        get => _results[index];
    }

    public string InputText { get; init; }

    private List<SearchResultItem> _results;
    private readonly object _locker = new();

    public IEnumerable<SearchResultItem> GetResults()
    {
        lock (_locker)
        {
            return _results;
        }
    }

    public void AddResult(SearchResultItem result)
    {
        lock (_locker)
        {
            _results.Add(result);
        }
    }
}
