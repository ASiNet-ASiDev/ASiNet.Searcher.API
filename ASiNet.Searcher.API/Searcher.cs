using ASiNet.Searcher.API.Enums;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ASiNet.Searcher.API;
public partial class Searcher
{
    public Searcher(SearchOptions options)
    {
        Options = options;
    }

    public SearchOptions Options { get; set; }

    public async Task<SearchResult> SearchNew(string input, CancellationToken token = default, Action<SearchResult>? result = null)
    {
        var searchResult = new SearchResult(input);

        await Parallel.ForEachAsync(new[]
            {
                SearchFilesAndDirectories,

            },
            async (Func<string, Action<SearchResultItem>, Task> action, CancellationToken token) =>
                await action.Invoke(input, searchResult.AddResult));
        result?.Invoke(searchResult);
        return searchResult;
    }

    public static SearchResultType GetSearchStringType(string input)
    {
        if (FileSystemSearchPattern().IsMatch(input))
        {
            if(File.Exists(input))
                return SearchResultType.LocalFile;
            else if(Directory.Exists(input))
                return SearchResultType.LocalFolder;
            else
                return SearchResultType.None;
        }
        else if (WebSiteUriSearchPattern().IsMatch(input))
        {
            return SearchResultType.WebSite;
        }
        else
            return SearchResultType.None;
    }

    private async Task SearchFilesAndDirectories(string input, Action<SearchResultItem> addItem)
    {
        await Task.Run(() =>
        {
            try
            {
                if (FileSystemSearchPattern().IsMatch(input))
                    return;
                var baseFolder = Path.GetDirectoryName(input) ?? input;
                if (string.IsNullOrWhiteSpace(input))
                    return;
                if (Directory.Exists(baseFolder))
                {
                    var name = input.Substring(baseFolder.Length).Replace("\\", "").Replace("/", ""); 

                    var i = 0;
                    foreach (var folder in Directory.EnumerateDirectories(baseFolder, $"{name}*"))
                    {
                        if (i > Options.MaxFoldersVisualization)
                            break;
                        addItem.Invoke(new(folder, SearchResultType.LocalFolder));
                        i++;
                    }
                    i = 0;
                    foreach (var file in Directory.EnumerateFiles(baseFolder, $"{name}*"))
                    {
                        if (i > Options.MaxFoldersVisualization)
                            break;
                        addItem.Invoke(new(file, SearchResultType.LocalFile));
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                addItem.Invoke(new(ex.Message, SearchResultType.Error));
            }
        });
    }


    [GeneratedRegex(@"^\w\\:")]
    private static partial Regex FileSystemSearchPattern();
    [GeneratedRegex(@"https?://")]
    private static partial Regex WebSiteUriSearchPattern();
}
