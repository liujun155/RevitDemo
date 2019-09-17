using Caliburn.Micro;
using RevitDemo.Entity;
using SeenSun.UIFramework.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.ViewModel
{
    public class FamilyManageViewModel : Screen
    {
        public System.Action<List<FamilyEnt>> SetFamily;
        public Action<FamilyEnt> SetMulFam;

        private List<FamilyEnt> _familyList;
        public List<FamilyEnt> FamilyList
        {
            get { return _familyList; }
            set
            {
                _familyList = value;
                NotifyOfPropertyChange(nameof(FamilyList));
            }
        }

        private FamilyEnt _selectedFam;
        public FamilyEnt SelFamily
        {
            get { return _selectedFam; }
            set { _selectedFam = value; NotifyOfPropertyChange(nameof(SelFamily)); }
        }

        public void LoadFamily()
        {
            if (FamilyList == null || FamilyList.Count == 0) return;
            var checkItems = FamilyList.FindAll(x => x.IsChecked);
            if (checkItems == null || checkItems.Count == 0)
                checkItems = new List<FamilyEnt>() { SelFamily };
            SetFamily(checkItems);
        }

        public void LoadMulFamily()
        {
            SetMulFam(SelFamily);
        }

        private void InitData()
        {
            FamilyList = new List<FamilyEnt>();
            FamilyEnt ent = new FamilyEnt()
            {
                Id = Guid.NewGuid(),
                FileName = Path.GetFileNameWithoutExtension(@"E:\Revit二次开发\做族\族达人速成光盘内容(2013)\4.族创建实例\4.3家具族\单人沙发.rfa"),
                FilePath = @"E:\Revit二次开发\做族\族达人速成光盘内容(2013)\4.族创建实例\4.3家具族\单人沙发.rfa"
            };
            FamilyEnt ent2 = new FamilyEnt()
            {
                Id = Guid.NewGuid(),
                FileName = Path.GetFileNameWithoutExtension(@"E:\Revit二次开发\简易参变族.rfa"),
                FilePath = @"E:\Revit二次开发\简易参变族.rfa"
            };
            FamilyList.Add(ent);
            FamilyList.Add(ent2);
        }

        public FamilyManageViewModel()
        {
            InitData();
        }
    }
}
