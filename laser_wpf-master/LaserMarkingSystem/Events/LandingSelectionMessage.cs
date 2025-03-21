using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InIWorkspace.Events
{
    public class LandingSelectionMessage
    {
        public enum Option
        {
            Admin,
            Operator
        }
        public Option SelectedOption { get; set; }
    }

}
