using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TcPCM_Connect_Global
{
    public class SoildEdge : IsolatedTaskProxy
    {
        public bool DoOpenSave(string filePath)
        {
            return InvokeSTAThread<string,bool>(SaveJT, filePath);
        }

        bool SaveJT(string filePath)
        {
            bool result = false;
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;            
            SolidEdgeFramework.SolidEdgeDocument document = null;
            SolidEdgePart.PartDocument partDocument = null;

            //SolidEdgePart.PartDocument document = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                if (filePath.Contains(".stp"))
                {
                    application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);                    
                    application = (SolidEdgeFramework.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("SolidEdge.Application");
                }
                else
                {
                    application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true);
                    application.DisplayAlerts = false;
                }

                documents = application.Documents;
                document = (SolidEdgeFramework.SolidEdgeDocument)application.Documents.Open(filePath);

                if (document != null)
                {
                    DocumentHelper.SaveAsJT(document, filePath);
                }
                else
                {
                    throw new System.Exception("No active document.");
                }
                application.DisplayAlerts = false;
                documents.Close();
                Marshal.FinalReleaseComObject(documents);
                application.Quit();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                // Additional cleanup.
                try
                {
                    Marshal.FinalReleaseComObject(application);
                    SolidEdgeCommunity.OleMessageFilter.Unregister();
                    result = true;
                }
                catch (System.Exception ex)
                {
                    
                }                          

            }

            return result;
        }
    }
}
