using ASiNet.Searcher.API;
using ASiNet.Searcher.API.Enums;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASiNet.Searcher.Client.Avalonia.Models;
public partial class ResultItem : ObservableObject
{
    public ResultItem(SearchResultItem item)
    {
        Path = item.Path;

        Icon = item.Type switch
        {
            SearchResultType.LocalFolder => Tools.ThemifyIcons.Icon.Folder,
            SearchResultType.LocalFile => Tools.ThemifyIcons.Icon.File,
            SearchResultType.WebSite => Tools.ThemifyIcons.Icon.World,
            SearchResultType.Error => Tools.ThemifyIcons.Icon.Close,
            _ => Tools.ThemifyIcons.Icon.LayoutSidebarNone,
        };
        Type = item.Type;
        _bacgroundColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
    }

    public string Icon { get; private set; }
    public SearchResultType Type { get; private set; }

    [ObservableProperty]
    private IBrush _bacgroundColor;

    public string Path { get; init; }

    public void Select()
    {
        BacgroundColor = new SolidColorBrush(Color.FromArgb(255, 100, 200, 200));
    }

    public void Unselect()
    {
        BacgroundColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
    }

}
