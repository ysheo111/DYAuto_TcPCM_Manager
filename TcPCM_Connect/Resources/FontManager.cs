using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace TcPCM_Connect
{
    //1. 프로젝트 내의
    //s폴더(임의로 생성한 폴더)에 폰트파일(ttf) 복사
    //2. Resources.resx 에 복사한 ttf파일을 리소스 추가
    class FontManager
    {
        private static FontManager instance = new FontManager();
        public PrivateFontCollection privateFont = new PrivateFontCollection();
        public static FontFamily[] fontFamilys
        {
            get
            {
                return instance.privateFont.Families;
            }
        }

        public FontManager()
        {
            AddFontFromMemory();
        }

        private void AddFontFromMemory()
        {
            System.Resources.ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true);
            foreach (System.Collections.DictionaryEntry entry in resourceSet)
            {
                if (!entry.Key.ToString().Contains("Montserrat")) continue;

                string resourceKey = entry.Key.ToString();
                byte[] font = entry.Value as byte[];

                IntPtr fontBuffer = Marshal.AllocCoTaskMem(font.Length);
                Marshal.Copy(font, 0, fontBuffer, font.Length);
                privateFont.AddMemoryFont(fontBuffer, font.Length);

                Marshal.FreeHGlobal(fontBuffer);//메모리 해제
            }

            //foreach (var test in Properties.Resources)
            //{
            //    //ttf
            //}
            //List<byte[]> fonts = new List<byte[]>();
            //fonts.Add(Properties.Resources.Montserrat_Black);//한컴고딕 보통
            //fonts.Add(Properties.Resources.HMFMPYUN);//휴먼편지체 보통

            //foreach (byte[] font in fonts)
            //{
            //    IntPtr fontBuffer = Marshal.AllocCoTaskMem(font.Length);
            //    Marshal.Copy(font, 0, fontBuffer, font.Length);
            //    privateFont.AddMemoryFont(fontBuffer, font.Length);

            //    Marshal.FreeHGlobal(fontBuffer);//메모리 해제
            //}
        }
    }
}