using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using DotNet.REVIT.Dock;
using DotNet.REVIT.External;
using DotNet.REVIT.Helper;
using RevitDemo.Common;
using RevitDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo
{
    [Transaction(TransactionMode.Manual)]
    public class MainRun : RevitApp
    {
        public static MainRun ThisApp;
        public string AssemblyPath { get; set; }
        public DockHost DockLeft { get; set; }
        public DockHost DockRight { get; set; }

        public override Result OnRevitClose(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public override Result OnRevitStart(UIControlledApplication application)
        {
            OnMainViewBefore(application);
            RibbonRegisterHelper.Register(new MainView(), 0);
            OnMainViewAfter(application);
            return Result.Succeeded;
        }

        private void OnMainViewAfter(UIControlledApplication application)
        {
            ThisApp = this;
            AssemblyPath = System.IO.Path.GetDirectoryName(GetType().Assembly.Location);
            RVT.GuidHelper = new ElementHelper();
            RVT.InterfaceTypeHelper = new ElementHelper("c9789b43-7b46-4e83-b557-42ccfda75dda", "RevitDemo", "RevitPrimaryDemo");
        }

        private void OnMainViewBefore(UIControlledApplication application)
        {
            DockLeft = new DockHost(application, DockPosition.Left);
            DockRight = new DockHost(application, DockPosition.Right);
        }
    }
}
