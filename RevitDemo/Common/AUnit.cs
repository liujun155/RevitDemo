using Autodesk.Revit.DB;
using DevCore.Generic.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Common
{
    /// <summary>
    /// 单位转换
    /// </summary>
    public class AUnit
    {
        /// <summary>
        /// 英尺转毫米
        /// </summary>
        /// <param name="value">英尺</param>
        /// <returns>返回毫米</returns>
        public static XYZ FT2MM(XYZ value)
        {
            return new XYZ(Unit.FT2MM(value.X), Unit.FT2MM(value.Y), Unit.FT2MM(value.Z));
        }

        /// <summary>
        /// 英尺转米
        /// </summary>
        /// <param name="value">英尺</param>
        /// <returns>返回米</returns>
        public static XYZ FT2M(XYZ value)
        {
            XYZ xyz = FT2MM(value);
            return new XYZ(xyz.X / 1000, xyz.Y / 1000, xyz.Z / 1000);
        }

        /// <summary>
        /// 毫米转英尺
        /// </summary>
        /// <param name="value">毫米</param>
        /// <returns>返回英尺</returns>
        public static XYZ MM2FT(XYZ value)
        {
            return new XYZ(Unit.MM2FT(value.X), Unit.MM2FT(value.Y), Unit.MM2FT(value.Z));
        }

        /// <summary>
        /// 米转英尺
        /// </summary>
        /// <param name="value">米</param>
        /// <returns>返回英尺</returns>
        public static XYZ M2FT(XYZ value)
        {
            return new XYZ(Unit.M2FT(value.X), Unit.M2FT(value.Y), Unit.M2FT(value.Z));
        }

        /// <summary>
        /// 厘米转英尺
        /// </summary>
        /// <param name="value">厘米</param>
        /// <returns>返回英尺</returns>
        public static XYZ CM2FT(XYZ value)
        {
            return new XYZ(Unit.CM2FT(value.X), Unit.CM2FT(value.Y), Unit.CM2FT(value.Z));
        }

        /// <summary>
        /// 将Revit内部值转换为由DisplayUnitType表示的值
        /// </summary>
        /// <param name="value">Revit内部值</param>
        /// <param name="dut">DisplayUnitType</param>
        /// <returns>返回由DisplayUnitType表示的值</returns>
        public static XYZ Convert2FT(XYZ value, Autodesk.Revit.DB.DisplayUnitType dut)
        {
            if (dut == Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS)
            {
                return MM2FT(value);
            }
            else if (dut == Autodesk.Revit.DB.DisplayUnitType.DUT_CENTIMETERS)
            {
                return CM2FT(value);
            }
            else if (dut == Autodesk.Revit.DB.DisplayUnitType.DUT_METERS)
            {
                return M2FT(value);
            }
            else
                return value;
        }

        /// <summary>
        /// 将Revit内部值转换为由DisplayUnitType表示的值
        /// </summary>
        /// <param name="to">Revit内部值</param>
        /// <param name="value">DisplayUnitType</param>
        /// <returns>返回由DisplayUnitType表示的值</returns>
        private static double ConvertFromAPI(Autodesk.Revit.DB.DisplayUnitType to, double value)
        {
            return value *= ImperialDutRatio(to);
        }

        /// <summary>
        /// 将由DisplayUnitType表示的值转换为Revit内部值
        /// </summary>
        /// <param name="value">Revit内部值</param>
        /// <param name="from">DisplayUnitType </param>
        /// <returns> </returns>
        private static double ConvertToAPI(double value, Autodesk.Revit.DB.DisplayUnitType from)
        {
            return value /= ImperialDutRatio(from);
        }

        /// <summary>
        /// DisplayUnitType对应的转换值
        /// </summary>
        /// <param name="dut">DisplayUnitType</param>
        /// <returns>返回由DisplayUnitType表示的值</returns>
        private static double ImperialDutRatio(Autodesk.Revit.DB.DisplayUnitType dut)
        {
            switch (dut)
            {
                case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_FEET: return 1;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES: return 1;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_INCHES: return 12;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_FRACTIONAL_INCHES: return 12;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS: return 0.3048;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_CENTIMETERS: return 30.48;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS: return 304.8;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS_CENTIMETERS: return 0.3048;
                default: return 1;
            }
        }
    }
}
