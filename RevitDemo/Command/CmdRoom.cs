using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DotNet.REVIT.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.REVIT.Extension;
using DotNet.REVIT.Helper;

namespace RevitDemo.Command
{
    [Transaction(TransactionMode.Manual)]
    public class CmdRoom : RevitCommand
    {
        public override bool IsCommandAvailable(UIApplication app)
        {
            return true;
        }

        protected override Result ExecuteCommand(ExternalCommandData data, ref string message, ElementSet elements)
        {
            try
            {
                data.Application.OpenAndActivateDocument(@"E:\Revit二次开发\简易房间.rvt");
                //data.Application.PostCommand(RevitCommandId.LookupPostableCommandId(PostableCommand.Default3DView));

                Document doc = data.Application.ActiveUIDocument.Document;
                View3D view3D = doc.GetView3D();

                data.Application.ActiveUIDocument.ActiveView = view3D;


                //doc.InvokeTransaction(()=> 
                //{
                //    //...
                //});

                //TaskCommand.Invoke((m)=> 
                //{

                //});

                //using (Transaction tr = new Transaction(doc))
                //{
                //    try
                //    {
                //        tr.Start("");
                //        //...
                //        tr.Commit();
                //    }
                //    catch (Exception)
                //    {
                //        tr.RollBack();
                //    }
                //}
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Test", ex.Message);
            }

            return Result.Succeeded;
        }
    }
}
