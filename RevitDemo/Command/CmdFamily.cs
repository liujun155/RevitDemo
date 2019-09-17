using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Caliburn.Micro;
using DotNet.REVIT.Extension;
using DotNet.REVIT.External;
using DotNet.REVIT.Generic;
using RevitDemo.Common;
using RevitDemo.Entity;
using RevitDemo.ViewModel;
using RevitDemo.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitDemo.Command
{
    [Transaction(TransactionMode.Automatic)]
    public class CmdFamily : RevitCommand
    {
        UIApplication _app;
        Document _doc;
        private FamilyEnt _selFamily;

        /// <summary>
        /// 判断命令是否可用
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>
        ///   <c>true</c> if [is command available] [the specified application]; otherwise, <c>false</c>.
        /// </returns>
        /// 创建人:刘俊  创建时间:2019/9/6 17:55
        public override bool IsCommandAvailable(UIApplication app)
        {
            var curdoc = app.ActiveUIDocument;
            if (curdoc == null) return false;
            return true;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="message">The message.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        /// 创建人:刘俊  创建时间:2019/9/6 17:55
        protected override Result ExecuteCommand(ExternalCommandData data, ref string message, ElementSet elements)
        {
            _app = data.Application;
            _doc = data.Application.ActiveUIDocument.Document;
            FamilyManageViewModel vm = new FamilyManageViewModel();
            vm.SetFamily = SetFamily;
            vm.SetMulFam = SetMulFamily;
            FamilyManageView frm = new FamilyManageView(vm);
            frm.ShowDialog();
            return Result.Succeeded;
        }

        /// <summary>
        /// 加载并放置族文件
        /// </summary>
        /// <param name="list"></param>
        public void SetFamily(List<FamilyEnt> list)
        {
            if (list == null || list.Count == 0) return;
            foreach (var item in list)
            {
                Family family = FindAndLoadFamily(_doc, item);
                //创建族实例
                FamilySymbol fSymbol = FindFamilySymbolByFamily(_doc, family);
                if (fSymbol != null)
                {
                    XYZ point = new XYZ(0, 0, 0);
                    //创建族实例
                    FamilyInstance fi = _doc.Create.NewFamilyInstance(point, fSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }
            }
            TaskDialog.Show("提示", "放置成功");
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
            catch (Exception)
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 查找某一房间
        /// </summary>
        /// <param name="doc">Revit Document</param>
        /// <param name="roomName">房间标记名称</param>
        /// <returns>返回房间标记</returns>
        public static RoomTag FindRoomTag(Document doc, string roomName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> collection = collector.OfClass(typeof(SpatialElementTag)).ToList();
            foreach (Element elem in collector)
            {
                if (elem is RoomTag)
                {
                    RoomTag roomTag = elem as RoomTag;
                    if (roomTag.Room.Name.Contains(roomName))
                    {
                        return roomTag;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 弹出配置框
        /// </summary>
        /// <param name="ent"></param>
        public void SetMulFamily(FamilyEnt ent)
        {
            if (ent == null)
            {
                TaskDialog.Show("提示", "请选择族");
                return;
            }
            _selFamily = ent;
            List<string> roomNames = new List<string>();
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            List<Element> collection = collector.OfClass(typeof(SpatialElementTag)).ToList();
            foreach (Element elem in collector)
            {
                if (elem is RoomTag)
                {
                    RoomTag roomTag = elem as RoomTag;
                    roomNames.Add(roomTag.Room.Name);
                }
            }
            FrmSetFamilyViewModel vm = new FrmSetFamilyViewModel(roomNames);
            vm.SetConfig = SetConfig;
            FrmSetFamilyView frm = new FrmSetFamilyView(vm);
            frm.ShowDialog();
        }

        /// <summary>
        /// 按配置放置族
        /// </summary>
        /// <param name="ent"></param>
        public void SetConfig(SetConfigEnt ent)
        {
            if (_selFamily == null) return;
            try
            {
                Family family = FindAndLoadFamily(_doc, _selFamily);
                //创建族实例
                FamilySymbol fSymbol = null;
                ISet<ElementId> idSet = family.GetFamilySymbolIds();
                if (idSet.Count > 0)
                {
                    fSymbol = _doc.GetElement(idSet.ElementAt<ElementId>(0)) as FamilySymbol;
                    if (!fSymbol.IsActive)
                        fSymbol.Activate();
                    //获取实例参数
                    FamilyInstance instance = _doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), fSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    string len = instance.LookupParameter("长度").AsValueString();
                    string wid = instance.LookupParameter("宽度").AsValueString();
                    string hei = instance.LookupParameter("高度").AsValueString();
                    double length = double.Parse(len);
                    double width = double.Parse(wid);
                    double height = double.Parse(hei);
                    _doc.Delete(instance.Id);

                    RoomTag rmtag = FindRoomTag(_doc, ent.RoomName);
                    Room room = rmtag.Room;
                    BoundingBoxXYZ boundingBoxXYZ = room.get_BoundingBox(_doc.ActiveView);
                    XYZ minPoint = boundingBoxXYZ.Min;
                    XYZ maxPoint = boundingBoxXYZ.Max;
                    XYZ minp = AUnit.FT2MM(minPoint);
                    XYZ maxp = AUnit.FT2MM(maxPoint);
                    var roomLength = maxp.X - minp.X;//房间长度
                    var roomWidth = maxp.Y - minp.Y;//房间宽度
                    double lenCount = Math.Floor(roomLength / length);
                    double widCount = Math.Floor(roomWidth / width);
                    if (ent.Num > lenCount * widCount)
                    {
                        TaskDialog.Show("提示", "当前房间仅能摆放此族" + lenCount * widCount + "个");
                        return;
                    }
                    if (ent.Direction == "横向")
                    {
                        double allCount = Math.Ceiling(ent.Num / lenCount);
                        for (int i = 1; i <= allCount; i++)
                        {
                            double y = 0;
                            if (i == 1)
                                y = width / 2;
                            else
                                y = width * i - width / 2;
                            for (int j = 1; j <= lenCount; j++)
                            {
                                if (i * j > ent.Num) break;
                                double x = 0;
                                if (j == 1)
                                    x = length / 2;
                                else
                                    x = length * j - length / 2;
                                XYZ point = minp + new XYZ(x, y, 0);
                                point = AUnit.MM2FT(point);
                                //创建族实例
                                FamilyInstance fi = _doc.Create.NewFamilyInstance(point, fSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            }
                        }
                    }
                    else if (ent.Direction == "纵向")
                    {
                        double allCount = Math.Ceiling(ent.Num / widCount);
                        for (int i = 1; i <= allCount; i++)
                        {
                            double x = 0;
                            if (i == 1)
                                x = length / 2;
                            else
                                x = length * i - length / 2;
                            for (int j = 1; j <= widCount; j++)
                            {
                                if (i * j > ent.Num) break;
                                double y = 0;
                                if (j == 1)
                                    y = width / 2;
                                else
                                    y = width * j - width / 2;
                                XYZ point = minp + new XYZ(x, y, 0);
                                point = AUnit.MM2FT(point);
                                //创建族实例
                                FamilyInstance fi = _doc.Create.NewFamilyInstance(point, fSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误", ex.Message);
                return;
            }
            TaskDialog.Show("提示", "创建成功");
        }

    }
}
