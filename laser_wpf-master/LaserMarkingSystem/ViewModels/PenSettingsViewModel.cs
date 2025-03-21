using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InIWorkspace.ViewModels
{
    public class PenSettingsViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public PenSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            Name = string.Empty;
        }

    }
}
