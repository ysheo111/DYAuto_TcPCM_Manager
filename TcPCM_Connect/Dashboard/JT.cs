using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;

namespace TcPCM_Connect
{
    class JT
    {
        int success = 0, fail = 0;
        private Session theSession;
        private UFSession theUfSession;

        public JT()
        {
        }

        private void SetEnvironmentVariables()
        {
            string nxBinPath = @"C:\Program Files\Siemens\NX2312\NXBIN";
            string currentPath = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("UGII_BASE_DIR", @"C:\Program Files\Siemens\NX2312");
            Environment.SetEnvironmentVariable("PATH", nxBinPath + ";" + currentPath);
        }

        private bool InitializeNX()
        {
            try
            {
                theSession = Session.GetSession();
                theUfSession = UFSession.GetUFSession();
                return false;
            }
            catch (Exception initEx)
            {
                MessageBox.Show($"NX 초기화 중 오류가 발생했습니다: {initEx.Message}", "초기화 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return true;
        }

        public string FileLoading()
        {

            SetEnvironmentVariables();
            if (InitializeNX()) return null;

            string err = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = true;
                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return "선택한 파일 위치가 올바르지 않습니다. 다시 시도해주십시오";
                string[] fileList = dlg.FileNames;

                OpenFileDialog filedlg = new OpenFileDialog();
                filedlg.Title = "폴더를 선택하세요";
                filedlg.Filter = "폴더|*.none";
                filedlg.CheckFileExists = false;
                filedlg.CheckPathExists = true;
                filedlg.ValidateNames = false;

                DialogResult filedialog = filedlg.ShowDialog();
                if (filedialog == DialogResult.Cancel) return null;
                else if (filedialog != DialogResult.OK) return "선택한 폴더 위치가 올바르지 않습니다. 다시 시도해주십시오";

                string selectedFolderPath = System.IO.Path.GetDirectoryName(dlg.FileName);

                Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
                splashthread.IsBackground = true;
                splashthread.Start();

                long time = new DateTime().Second;

                err = JTCreate(fileList, selectedFolderPath).Result;
                MessageBox.Show($"걸린 시간 : {(new DateTime().Second) -time}");
            }
            catch (Exception e)
            {
                err = e.Message;
            }

            ExitNX();
            LoadingScreen.CloseSplashScreen();
            return err;
        }

        private async Task<string> JTCreate(string[] fileList, string folderPath)
        {
            string msg = $".............({success + fail}/{fileList.Length})";
            string detailMag = "";

            foreach (string file in fileList)
            {
                msg = $".............({success + fail + 1}/{msg.Split(new char[] { '/' }, 2)[1]}";
                detailMag += $"\r\n{file.Split('\\')[file.Split('\\').Length - 1]}";

                try
                {
                    await Task.Run(() => OpenNXFile(folderPath, file));
                }
                catch (System.Exception ex)
                {
                    //_log.Error(ex.Message, ex);
                    fail++;
                }
            }

            return $"JT 변환이 완료되었습니다.\n(Total : {fileList.Length} Success : {success} Fail : {fail})";
        }

        public void OpenNXFile(string folderPath, string file)
        {
            try
            {
                BasePart basePart1 = theSession.Parts.OpenActiveDisplay(file, DisplayPartOption.AllowAdditional, out _);
                theUfSession.Part.TranslateJt(basePart1.Tag, "", folderPath);
                basePart1.Close(BasePart.CloseWholeTree.True, BasePart.CloseModified.CloseModified, null);
                success++;
            }
            catch (System.Exception ex)
            {
                //_log.Error(ex.Message, ex);
                fail++;
            }
        }

        private void ExitNX()
        {
            if (theUfSession != null)
            {
                theUfSession = null;
            }

            if (theSession != null)
            {
                theSession = null;
            }
        }
    }
}