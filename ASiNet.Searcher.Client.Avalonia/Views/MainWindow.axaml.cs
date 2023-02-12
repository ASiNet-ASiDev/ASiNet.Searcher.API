using ASiNet.Searcher.Client.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace ASiNet.Searcher.Client.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        viewModel.MoveSelectionCommand.Execute(-1);
                        ResultScrollViewer.LineUp();
                        break;
                    case Key.Down:
                        viewModel.MoveSelectionCommand.Execute(1);
                        ResultScrollViewer.LineDown();
                        break;
                    case Key.Enter:
                        if(e.KeyModifiers.HasFlag(KeyModifiers.Shift))
                        {
                            viewModel.SelectItemCommand.Execute(null);
                            InputBox.CaretIndex = InputBox.Text.Length;

                            viewModel.ExecuteSelectedCommand.Execute(null);
                        }
                        else
                        {
                            viewModel.SelectItemCommand.Execute(null);
                            InputBox.CaretIndex = InputBox.Text.Length;
                        }
                        break;
                }
            }
        }
    }
}