using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace InIWorkspace.Events
{
    public class NewKeyMessage
    {
        public int Id {  get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public bool IsUpdate { get; set; }
    }
}
