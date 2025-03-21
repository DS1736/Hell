using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<ICommandChangeMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private BaseViewModel _contentViewModel;
        private ICommand _createLineCommand;
        private ICommand _createPolylineCommand;
        private ICommand _fitCommand;
        private ICommand _fillCommand;
        private ICommand _oneHundredPercentCommand;
        private ICommand _zoomInCommand;
        private ICommand _zoomOutCommand;
        private ICommand _undoZoomCommand;
        private ICommand _redoZoomCommand;
        private ICommand _toggleGridSnapCommand;
        private int _gridUnitX;
        private int _gridUnitY;
        private double _memoryUsage;
        private Timer _memoryUpdateTimer;
        private ICommand _toggleElementSnapCommand;
        private bool _isElementSnapChecked;
        private ICommand _deleteCommand;

        public ICommand FitCommand
        {
            get { return _fitCommand; }
            set
            {
                if (Equals(value, _fitCommand)) return;
                _fitCommand = value;
                NotifyOfPropertyChange(() => FitCommand);
            }
        }

        public ICommand FillCommand
        {
            get { return _fillCommand; }
            set
            {
                if (Equals(value, _fillCommand)) return;
                _fillCommand = value;
                NotifyOfPropertyChange(() => FillCommand);
            }
        }

        public ICommand OneHundredPercentCommand
        {
            get { return _oneHundredPercentCommand; }
            set
            {
                if (Equals(value, _oneHundredPercentCommand)) return;
                _oneHundredPercentCommand = value;
                NotifyOfPropertyChange(() => OneHundredPercentCommand);
            }
        }

        public ICommand ZoomInCommand
        {
            get { return _zoomInCommand; }
            set
            {
                if (Equals(value, _zoomInCommand)) return;
                _zoomInCommand = value;
                NotifyOfPropertyChange(() => ZoomInCommand);
            }
        }

        public ICommand ZoomOutCommand
        {
            get { return _zoomOutCommand; }
            set
            {
                if (Equals(value, _zoomOutCommand)) return;
                _zoomOutCommand = value;
                NotifyOfPropertyChange(() => ZoomOutCommand);
            }
        }

        public ICommand UndoZoomCommand
        {
            get { return _undoZoomCommand; }
            set
            {
                if (Equals(value, _undoZoomCommand)) return;
                _undoZoomCommand = value;
                NotifyOfPropertyChange(() => UndoZoomCommand);
            }
        }

        public ICommand RedoZoomCommand
        {
            get { return _redoZoomCommand; }
            set
            {
                if (Equals(value, _redoZoomCommand)) return;
                _redoZoomCommand = value;
                NotifyOfPropertyChange(() => RedoZoomCommand);
            }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                if (Equals(value, _deleteCommand)) return;
                _deleteCommand = value;
                NotifyOfPropertyChange(() => DeleteCommand);
            }
        }

        public double MemoryUsage
        {
            get { return _memoryUsage; }
            set
            {
                if (value.Equals(_memoryUsage)) return;
                _memoryUsage = Math.Round(value, 2);
                NotifyOfPropertyChange(() => MemoryUsage);
            }
        }

        public BaseViewModel ContentViewModel
        {
            get { return _contentViewModel; }
            set
            {
                if (Equals(value, _contentViewModel)) return;
                _contentViewModel = value;

                CreateLineCommand = _contentViewModel.LineCommand;
                CreatePolylineCommand = _contentViewModel.PolylineCommand;
                DeleteCommand = _contentViewModel.DeleteCommand;
                GridUnitX = _contentViewModel.GridUnitX;
                GridUnitY = _contentViewModel.GridUnitY;

                ActivateItemAsync(_contentViewModel);

                FitCommand = ContentViewModel.FitCommand;

                NotifyOfPropertyChange(() => ContentViewModel);
            }
        }

        public ICommand CreateLineCommand
        {
            get { return _createLineCommand; }
            set
            {
                if (Equals(value, _createLineCommand)) return;
                _createLineCommand = value;
                NotifyOfPropertyChange(() => CreateLineCommand);
            }
        }

        public ICommand ToggleGridSnapCommand
        {
            get { return _toggleGridSnapCommand; }
            set
            {
                if (Equals(value, _toggleGridSnapCommand)) return;
                _toggleGridSnapCommand = value;

                NotifyOfPropertyChange(() => ToggleGridSnapCommand);
            }
        }

        public ICommand ToggleElementSnapCommand
        {
            get { return _toggleElementSnapCommand; }
            set
            {
                if (Equals(value, _toggleElementSnapCommand)) return;
                _toggleElementSnapCommand = value;
                NotifyOfPropertyChange(() => ToggleElementSnapCommand);
            }
        }

        public ICommand CreatePolylineCommand
        {
            get { return _createPolylineCommand; }
            set
            {
                if (Equals(value, _createPolylineCommand)) return;
                _createPolylineCommand = value;
                NotifyOfPropertyChange(() => CreatePolylineCommand);
            }
        }


        public int GridUnitX
        {
            get { return _gridUnitX; }
            set
            {
                if (value.Equals(_gridUnitX)) return;
                _gridUnitX = value;

                _contentViewModel.GridUnitX = _gridUnitX;

                NotifyOfPropertyChange(() => GridUnitX);

            }
        }

        public int GridUnitY
        {
            get { return _gridUnitY; }
            set
            {
                if (value.Equals(_gridUnitY)) return;
                _gridUnitY = value;
                _contentViewModel.GridUnitY = _gridUnitY;

                NotifyOfPropertyChange(() => GridUnitY);

            }
        }

        public bool IsElementSnapChecked
        {
            get { return _isElementSnapChecked; }
            set
            {
                if (value == _isElementSnapChecked) return;
                _isElementSnapChecked = value;
                NotifyOfPropertyChange(() => IsElementSnapChecked);
            }
        }


        public MainWindowViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _eventAggregator.Subscribe(this);
            ContentViewModel = new WorkspaceViewModel(windowManager,_eventAggregator);
        }        

        public async Task HandleAsync(ICommandChangeMessage message, CancellationToken cancellationToken)
        {
            //Be aware of command changes of the example views.
            FitCommand = message.FitCommand;
            FillCommand = message.FillCommand;
            OneHundredPercentCommand = message.OneHundredPercentCommand;
            ZoomInCommand = message.ZoomInCommand;
            ZoomOutCommand = message.ZoomOutCommand;
            UndoZoomCommand = message.UndoZoomCommand;
            RedoZoomCommand = message.RedoZoomCommand;
            await Task.CompletedTask;
        }
    }
}
