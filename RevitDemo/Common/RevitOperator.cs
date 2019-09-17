using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Common
{
    public class RevitOperator
    {
        /// <summary>
        /// 获得指定设备的所有连接键
        /// </summary>
        /// <param name="revitDoc">Revit Document</param>
        /// <param name="instId">实例ID</param>
        public static List<RevitConnector> GetRevitConnectors(Document revitDoc, ElementId instId)
        {
            FamilyInstance oInst = revitDoc.GetElement(instId) as FamilyInstance;
            Family family = oInst.Symbol.Family;
            Document familyDoc = revitDoc.EditFamily(family);
            List<ConnectorElement> elems = GetConnectorElement(familyDoc);
            Transform transfrom = GetTransform(revitDoc, oInst);
            if (transfrom == null) return null;
            List<RevitConnector> rcList = new List<RevitConnector>();
            foreach (Element elem in elems)
            {
                ConnectorElement ce = elem as ConnectorElement;
                Parameter para = ce.get_Parameter(BuiltInParameter.RBS_CONNECTOR_DESCRIPTION);
                if (para != null)
                {
                    RevitConnector rc = new RevitConnector();
                    rc.Description = para.AsString();
                    rc.Direction = transfrom.OfPoint(ce.CoordinateSystem.BasisZ);
                    rc.LocalPoint = transfrom.OfPoint(ce.Origin);
                    rc.Connector = GetConnector(oInst, rc.LocalPoint);
                    rcList.Add(rc);
                }
            }
            return rcList;
        }

        /// <summary>
        /// 获取设备上指定点的连接键
        /// </summary>
        /// <param name="elem">设备</param>
        /// <param name="inspt">特定点</param>
        /// <returns>连接键</returns>
        public static Connector GetConnector(Element elem, XYZ inspt)
        {
            if (elem == null || inspt == null)
            {
                return null;
            }
            ConnectorSet cSet = GetConnectorSet(elem);
            ConnectorSetIterator csi = cSet.ForwardIterator();
            while (csi.MoveNext())
            {
                Connector curConn = csi.Current as Connector;
                if (curConn.Origin.IsAlmostEqualTo(inspt))
                {
                    return curConn;
                }
            }
            return null;
        }

        /// <summary>
        /// 用来找到连接器
        /// </summary>
        /// <param name="element">设备</param>
        /// <returns>连接件集合</returns>
        public static ConnectorSet GetConnectorSet(Element element)
        {
            if (element == null)
            {
                return null;
            }
            FamilyInstance fi = element as FamilyInstance;
            if (fi != null && fi.MEPModel.ConnectorManager != null)
            {
                return fi.MEPModel.ConnectorManager.Connectors;
            }
            MEPSystem system = element as MEPSystem;
            if (system != null)
            {
                return system.ConnectorManager.Connectors;
            }
            MEPCurve duct = element as MEPCurve;
            if (duct != null)
            {
                return duct.ConnectorManager.Connectors;
            }
            return null;
        }

        /// <summary>
        /// 获得族中的连接件
        /// </summary>
        /// <param name="familyDoc">Family Document</param>
        /// <returns>返回连接件集合</returns>
        public static List<ConnectorElement> GetConnectorElement(Document familyDoc)
        {
            return new FilteredElementCollector(familyDoc).WherePasses(
                new ElementClassFilter(typeof(ConnectorElement))).Cast<ConnectorElement>().ToList();
        }

        /// <summary>
        /// 获取变换矩阵
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="oInst">The o inst.</param>
        /// <returns></returns>
        public static Transform GetTransform(Document doc, FamilyInstance oInst)
        {
            Options opt = new Options();
            opt.ComputeReferences = false;
            opt.View = doc.ActiveView;
            GeometryElement geoElement = oInst.get_Geometry(opt);
            foreach (GeometryObject geoObj in geoElement)
            {
                if (geoObj is GeometryInstance)
                {
                    GeometryInstance geoInst = geoObj as GeometryInstance;
                    return geoInst.Transform;
                }
            }
            return null;
        }
    }
}
