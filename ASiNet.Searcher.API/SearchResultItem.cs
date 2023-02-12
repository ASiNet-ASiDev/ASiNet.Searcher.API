using ASiNet.Searcher.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASiNet.Searcher.API;
public class SearchResultItem
{
    public SearchResultItem(string path, SearchResultType type)
    {
        Type = type;
        Path = path;
    }

    public SearchResultType Type { get; init; }

    public string Path { get; init; }

}
