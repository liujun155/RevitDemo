using Caliburn.Micro;
using DotNet.REVIT.External;
using RevitDemo.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.ViewModel
{
    public class MainViewModel : Screen
    {
        public RevitCommand Test { get; set; }
        public RevitCommand LoadRoom { get; set; }
        public RevitCommand Door { get; set; }
        public RevitCommand Connector { get; set; }
        public RevitCommand GetSelection { get; set; }

        public MainViewModel()
        {
            Test = new CmdFamily();
            LoadRoom = new CmdRoom();
            Door = new CmdDoor();
            Connector = new CmdConnector();
            GetSelection = new CmdSelected();
        }
    }
}
