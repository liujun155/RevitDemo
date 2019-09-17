using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DotNet.REVIT.External;
using RevitDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Command
{
    [Transaction(TransactionMode.Manual)]
    public class CmdDoor : RevitCommand
    {
        UIApplication _app;
        Document _doc;

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
            try
            {
                Reference refer = _app.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "请选择墙");
                Element elem = _doc.GetElement(refer);
                Wall awall = elem as Wall;//把选择的元素转换为墙
                Line line = (awall.Location as LocationCurve).Curve as Line;//获取墙的线
                XYZ midPoint = (line.GetEndPoint(0) + line.GetEndPoint(1)) / 2;//获取中点
                Level wallLevel = _doc.GetElement(awall.LevelId) as Level;//获取标高，元素转换为标高

                //创建过滤器
                FamilySymbol familySymbol = null;
                FilteredElementCollector doorCollector = new FilteredElementCollector(_doc);
                //创建过滤收集器
                ElementCategoryFilter doorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
                //过滤出门类别
                ElementClassFilter familySymbolFilter = new ElementClassFilter(typeof(FamilySymbol));
                //过滤出门
                LogicalAndFilter doorAndFilter = new LogicalAndFilter(doorFilter, familySymbolFilter);
                //逻辑过滤器
                doorCollector.WherePasses(doorAndFilter);
                foreach (FamilySymbol symbol in doorCollector)
                {
                    if (symbol.Name == "600 x 1800mm")
                    {
                        familySymbol = symbol;
                        break;
                    }
                }
                if (!familySymbol.IsActive)
                    familySymbol.Activate();
                _doc.Create.NewFamilyInstance(midPoint, familySymbol, elem, wallLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                TaskDialog.Show("提示", "创建成功");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("提示", ex.Message);
            }

            //FamilyInstance banz = null;
            //FilteredElementCollector collector = new FilteredElementCollector(_doc);
            //ICollection<Element> collection = collector.OfClass(typeof(FamilyInstance)).ToElements();
            //foreach (Element e in collection)
            //{
            //    if (e.Name == "DDF数字配线架-20系统-B型-贝能达")
            //    {
            //        banz = e as FamilyInstance;
            //        List<RevitConnector> connectors = RevitOperator.GetRevitConnectors(_doc, banz.Id);
            //    }
            //}

            return Result.Succeeded;
        }

    }
}
