using ASiNet.Searcher.API;
using ASiNet.Searcher.API.Enums;
using ASiNet.Searcher.Client.Avalonia.Models;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ASiNet.Searcher.Client.Avalonia.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        _searcher = new(new());
    }

    public const int WIDTH = 600;
    public const int HEIGHT = 50;
    public const int MAX_HEIGHT = 550;
    public const int RESULT_ITEM_HEIGHT = 25;

    public string Input
    {
        get => _input;
        set
        {
            _input = value;
            OnUpdateInput();
            OnPropertyChanged();
        }
    }
    public ObservableCollection<ResultItem> Items { get; } = new();

    [ObservableProperty]
    private bool _showResults = true;
    [ObservableProperty]
    private int _width = 600;
    [ObservableProperty]
    private int _height = 50;


    private ResultItem? _selectedItem;
    private int _offset;

    private string _input = string.Empty;
    private API.Searcher _searcher;
    private CancellationTokenSource? _source;

    [RelayCommand]
    private void ExecuteSelected()
    {
        if(_selectedItem is not null)
        {
            switch (_selectedItem.Type)
            {
                case SearchResultType.WebSite:
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = _selectedItem.Path,
                    };
                    process.Start();
                    break;
                case SearchResultType.LocalFolder:
                    if (Directory.Exists(_selectedItem.Path))
                        Process.Start("explorer.exe", _selectedItem.Path);
                    break;
                case SearchResultType.LocalFile:
                    if(!File.Exists(_selectedItem.Path))
                        return;
                    process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = _selectedItem.Path,
                    };

                    process.Start();
                    break;
                default:
                    break;
            }
        }
        else if (!string.IsNullOrWhiteSpace(Input))
        {
            switch (API.Searcher.GetSearchStringType(Input))
            {
                case SearchResultType.WebSite:
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = Input,
                    };
                    process.Start();
                    return;
                case SearchResultType.LocalFolder:
                    if (Directory.Exists(Input))
                        Process.Start("explorer.exe", Input);
                    return;
                case SearchResultType.LocalFile:
                    if (!File.Exists(Input))
                        return;
                    process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = Input,
                    };

                    process.Start();
                    return;
                default:
                    return;
            }
        }
    }

    [RelayCommand]
    private void OnUpdateInput()
    {
        _source?.Cancel();
        _source = new();
        _ = _searcher.SearchNew(Input, _source.Token, OnSearchResult);
    }
    [RelayCommand]
    private void SelectItem()
    {
        Input = _selectedItem?.Type == SearchResultType.LocalFolder ? $"{_selectedItem?.Path}\\" ?? Input : _selectedItem?.Path ?? Input;
    }
    [RelayCommand]
    private void MoveSelection(int offset)
    {
        if(_offset + offset < Items.Count && _offset + offset >= 0)
        {
            _offset += offset;
            _selectedItem?.Unselect();
            _selectedItem = Items[_offset];
            _selectedItem.Select();
        }
    }

    private void OnSearchResult(SearchResult obj)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _showResults = true;
            Height = 50;
            Items.Clear();
            foreach (var item in obj.GetResults())
            {
                Items.Add(new(item));
                Height = Height <= MAX_HEIGHT ? Height += RESULT_ITEM_HEIGHT : MAX_HEIGHT;
            }
            if(Items.Count > 0)
            {
                _offset = 0;
                _selectedItem = Items[0];
                _selectedItem.Select();
            }
        }, DispatcherPriority.Normal);
    }
}