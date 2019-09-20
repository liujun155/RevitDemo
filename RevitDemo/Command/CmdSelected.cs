using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DotNet.REVIT.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Command
{
    [Transaction(TransactionMode.Automatic)]
    public class CmdSelected : RevitCommand
    {
        public UIApplication _app;
        public Document _doc;

        public override bool IsCommandAvailable(UIApplication app)
        {
            var curdoc = app.ActiveUIDocument;
            if (curdoc == null) return false;
            return true;
        }

        protected override Result ExecuteCommand(ExternalCommandData data, ref string message, ElementSet elements)
        {
            _app = data.Application;
            _doc = data.Application.ActiveUIDocument.Document;
            Selection selection = data.Application.ActiveUIDocument.Selection;
            List<ElementId> elementIds = selection.GetElementIds().ToList();
            if(elementIds.Count == 0)
            {
                TaskDialog.Show("提示", "您未选择任何图元");
            }
            else
            {
                string info = "您选择的图元ID列表：";
                foreach (var item in elementIds)
                {
                    info += "\n\t" + item.IntegerValue;
                }
                TaskDialog.Show("提示", info);
            }
            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter doorCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
            LogicalAndFilter logicalAndFilter = new LogicalAndFilter(familyInstanceFilter, doorCategoryFilter);
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            List<ElementId> doors = collector.WherePasses(logicalAndFilter).ToElementIds().ToList();
            string info2 = "文档中的门ID列表：";
            foreach (var item in doors)
            {
                info2+= "\n\t" + item.IntegerValue;
            }
            TaskDialog.Show("提示", info2);
            return Result.Succeeded;
        }
    }
}
