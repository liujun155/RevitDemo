using Caliburn.Micro;
using RevitDemo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.ViewModel
{
    public class FrmSetFamilyViewModel : Screen
    {
        public System.Action<SetConfigEnt> SetConfig;

        private SetConfigEnt _setConfig = new SetConfigEnt();
        public SetConfigEnt CurrentEnt
        {
            get { return _setConfig; }
            set
            {
                _setConfig = value;
                NotifyOfPropertyChange(nameof(CurrentEnt));
            }
        }

        private List<string> _directions = new List<string>() { "横向", "纵向" };
        public List<string> Directions
        {
            get { return _directions; }
            set
            {
                _directions = value;
                NotifyOfPropertyChange("Directions");
            }
        }

        private List<string> _rooms = new List<string>() { "客厅", "主卧", "次卧" };
        public List<string> Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                NotifyOfPropertyChange("Rooms");
            }
        }

        public void BtnClick()
        {
            if (SetConfig != null && CurrentEnt != null)
                SetConfig(CurrentEnt);
        }

        public FrmSetFamilyViewModel() { }

        public FrmSetFamilyViewModel(List<string> roomNames)
        {
            Rooms = new List<string>(roomNames);
        }
    }
}
