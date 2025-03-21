using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    public interface ICommandChangeMessage
    {
        ICommand FitCommand { get; }
        ICommand FillCommand { get; }
        ICommand OneHundredPercentCommand { get; }
        ICommand ZoomInCommand { get; }
        ICommand ZoomOutCommand { get; }
        ICommand UndoZoomCommand { get; }
        ICommand RedoZoomCommand { get; }

    }
}