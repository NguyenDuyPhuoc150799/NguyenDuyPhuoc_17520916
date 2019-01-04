using Project.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (Login(txtTaiKhoan.Text, txtMatKhau.Text))
            {
                
                FrmGame frmGame = new FrmGame();
                frmGame._TaiKhoanThat = txtTaiKhoan.Text;
                this.Hide();
                frmGame.FormClosed += frmGame_FormClosed;
                frmGame.Show();
            }
            else
            {
                MessageBox.Show("Sai Tên Mật Khẩu hoặc Tài Khoản");
            }
        }

        private void frmGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void btnDangkyMoi_Click(object sender, EventArgs e)
        {
            FrmDangKy frmDangKy = new FrmDangKy();
            this.Hide();
            frmDangKy.ShowDialog();
            this.Show();
        }

        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }
    }
}
