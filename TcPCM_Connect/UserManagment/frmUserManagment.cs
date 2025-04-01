using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmUserManagment : Form
    {
        public frmUserManagment()
        {
            InitializeComponent();
        }
        private void frmUserManagment_Load(object sender, EventArgs e)
        {
            SelectUserInfo();
        }

        private void dgv_UserInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView.Columns[dataGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            subfrmSignUp subfrmsignUp = new subfrmSignUp();
            subfrmsignUp.ShowDialog();

            SelectUserInfo();
        }

        private void dgv_UserInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UserEdit();
        }       

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            UserEdit();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (dgv_UserInfo.CurrentCell?.RowIndex == null)
            {
                CustomMessageBox.RJMessageBox.Show("삭제하고자하는 회원이 선택되지 않았습니다. \n다시 시도해주세요.", "회원 삭제"
    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string query = $@"Delete From UserInfo 
                              Where Idx = {dgv_UserInfo.Rows[dgv_UserInfo.CurrentCell.RowIndex].Cells["Idx"].Value}";

            if (global_DB.ScalarDeleteBool(query))
            {
                SelectUserInfo();
                CustomMessageBox.RJMessageBox.Show("삭제가 완료되었습니다.", "회원 삭제");
            }
            else
            {
                CustomMessageBox.RJMessageBox.Show("삭제에 실패하였습니다. 다시 시도해주세요.", "회원 삭제"
                    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public void SelectUserInfo()
        {
            string query = $@"  Select a.Idx, Authority as Level, ID, FirstName, LastName, CONVERT(CHAR(19),LastHistory,20) as 'Last Login Information'
                                from UserInfo as a";

            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;  
            dgv_UserInfo.Columns.Clear();
            dgv_UserInfo.DataSource = dataTable;
            dgv_UserInfo.Columns["Idx"].Visible = false;
        }


        public void UserEdit()
        {
            if (dgv_UserInfo.CurrentCell?.RowIndex == null)
            {
                CustomMessageBox.RJMessageBox.Show("수정하고자하는 회원이 선택되지 않았습니다. \n다시 시도해주세요.", "회원 정보 수정"
    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            subFrameUserEdit subFrameUser = new subFrameUserEdit();
            int rowIdx = dgv_UserInfo.CurrentCell.RowIndex;

            subFrameUser.Idx = (int)dgv_UserInfo.Rows[rowIdx].Cells["Idx"].Value;
            subFrameUser.id = dgv_UserInfo.Rows[rowIdx].Cells["ID"].Value?.ToString();
            subFrameUser.fristName = dgv_UserInfo
                .Rows[rowIdx].Cells["FirstName"].Value?.ToString();
            subFrameUser.lastName = dgv_UserInfo.Rows[rowIdx].Cells["LastName"].Value?.ToString();
            subFrameUser.auth = dgv_UserInfo.Rows[rowIdx].Cells["Level"].Value?.ToString();

            subFrameUser.ShowDialog();

            SelectUserInfo();
        }

        private void btn_TcPCM_Click(object sender, EventArgs e)
        {
            new frmTcPCM().Show();
        }
    }
}
