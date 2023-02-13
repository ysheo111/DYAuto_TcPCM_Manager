using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TcPCM_Connect
{
    public enum TypeOfMessage
    {
        Success,
        Warning,
        Error,
    }

    public static class LoadingScreen
    {
        static Loading sf = null;

        public static void ShowSplashScreen()
        {
            if (sf == null)
            {
                sf = new Loading();
                sf.StartPosition = FormStartPosition.CenterScreen;
                sf.ShowSplashScreen();
            }
        }

        public static void CloseSplashScreen()
        {
            if (sf != null)
            {
                sf.CloseSplashScreen();
                sf = null;
            }
        }

        public static void UdpateStatusTextWithStatus(string Text, TypeOfMessage tom)
        {

            if (sf != null)
                sf.UdpateStatusTextWithStatus(Text, tom);
        }
    }
}

