using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DotNet.REVIT.External;
using DotNet.REVIT.Generic;
using RevitDemo.Common;
using RevitDemo.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Command
{
    [Transaction(TransactionMode.Manual)]
    public class CmdConnector : RevitCommand
    {
        UIApplication _app;
        Document _doc;

        FamilyEnt con1 = new FamilyEnt()
        {
            Id = Guid.NewGuid(),
            FileName = Path.GetFileNameWithoutExtension(@"E:\Revit二次开发\双向连接件.rfa"),
            FilePath = @"E:\Revit二次开发\双向连接件.rfa"
        };
        FamilyEnt con2 = new FamilyEnt()
        {
            Id = Guid.NewGuid(),
            FileName = Path.GetFileNameWithoutExtension(@"E:\Revit二次开发\流出连接件.rfa"),
            FilePath = @"E:\Revit二次开发\流出连接件.rfa"
        };

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

            RevitConnector connector = null;
            Transaction ts1 = new Transaction(_doc, "CtCon1");
            try
            {
                ts1.Start();
                Family family = FindAndLoadFamily(_doc, con1);
                FamilySymbol fSymbol = FindFamilySymbolByFamily(_doc, family);
                XYZ point = new XYZ(0, 0, 0);
                //创建族实例
                FamilyInstance fi = _doc.Create.NewFamilyInstance(point, fSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                ts1.Commit();
                //获取连接件
                List<RevitConnector> connectors = RevitOperator.GetRevitConnectors(_doc, fi.Id);
                connector = connectors == null ? null : connectors[0];
            }
            catch (Exception ex)
            {
                ts1.RollBack();
            }
            Transaction ts2 = new Transaction(_doc, "CtCon2");
            try
            {
                ts2.Start();
                Family family1 = FindAndLoadFamily(_doc, con2);
                FamilySymbol fSymbol1 = FindFamilySymbolByFamily(_doc, family1);
                FamilyInstance fi = _doc.Create.NewFamilyInstance(connector.LocalPoint, fSymbol1, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                ts2.Commit();
                //获取连接件
                List<RevitConnector> connectors = RevitOperator.GetRevitConnectors(_doc, fi.Id);
                RevitConnector connector2 = connectors == null ? null : connectors[0];
                ts2.Start();
                if (connector2 != null)
                {
                    LocationPoint lp = fi.Location as LocationPoint;
                    XYZ p2 = lp.Point.Subtract(connector2.LocalPoint);
                    ElementTransformUtils.MoveElement(_doc, fi.Id, p2);
                }
                ts2.Commit();
            }
            catch (Exception ex)
            {
                ts2.RollBack();
            }
            return Result.Succeeded;
        }

        /// <summary>
        /// 加载族文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyName"></param>
        /// <returns></returns>
        private Family FindAndLoadFamily(Document doc, FamilyEnt ent)
        {
            try
            {
                Family family = null;
                //查找是否已经载入族
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(Family)).ToElements();
                foreach (Element e in collection)
                {
                    Family fam = e as Family;
                    if (fam != null)
                    {
                        if (fam.Name == ent.FileName)
                        {
                            family = fam;
                            break;
                        }
                    }
                }
                //载入族文件
                if (family == null)
                    _doc.LoadFamily(ent.FilePath, new FamilyLoadOptions(), out family);
                return family;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private FamilySymbol FindFamilySymbolByFamily(Document doc, Family family)
        {
            try
            {
                FamilySymbol fSymbol = null;
                ISet<ElementId> idSet = family.GetFamilySymbolIds();
                if (idSet.Count > 0)
                {
                    fSymbol = doc.GetElement(idSet.ElementAt<ElementId>(0)) as FamilySymbol;
                    if (!fSymbol.IsActive)
                        fSymbol.Activate();
                    return fSymbol;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
