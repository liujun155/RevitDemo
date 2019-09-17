using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Common
{
    public class RevitConnector
    {
        #region 变量
        private string _description;
        private XYZ _localPoint;
        private XYZ _direction;
        private Connector _connector;
        #endregion

        #region 属性
        /// 获取或设置连接键说明
        /// <summary>
        /// 获取或设置连接键说明
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        /// 获取或设置连接键坐标
        /// <summary>
        /// 获取或设置连接键坐标
        /// </summary>
        public XYZ LocalPoint
        {
            get { return this._localPoint; }
            set { this._localPoint = value; }
        }

        /// 获取或设置连接键方向
        /// <summary>
        /// 获取或设置连接键方向
        /// </summary>
        public XYZ Direction
        {
            get { return this._direction; }
            set { this._direction = value; }
        }

        /// 获取或设置连接键
        /// <summary>
        /// 获取或设置连接键
        /// </summary>
        public Connector Connector
        {
            get { return this._connector; }
            set { this._connector = value; }
        }
        #endregion
    }
}
