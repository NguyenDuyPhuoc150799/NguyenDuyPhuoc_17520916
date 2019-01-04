using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project.DAO;
using System.Security.Cryptography;

namespace Project
{
    public partial class FrmDangKy : Form
    {
        public FrmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if(txtTaiKhoan.Text == "")
            {
                MessageBox.Show("Xin vui lòng nhập tài khoản !");
                txtTaiKhoan.Focus();
            }
            else if(txtMatKhau.Text == "")
            {
                MessageBox.Show("Xin vui lòng nhập mật khẩu !");
                txtMatKhau.Focus();
            }
            else if (txtMatKhau.Text != txtXacNhan.Text)
            {
                MessageBox.Show("Xác Nhận Mật Khẩu Không Đúng ! Vui Lòng Nhập Lại !");
            }
            else
            {
                try
                {
                    if (DangKy(txtTaiKhoan.Text, txtMatKhau.Text))
                    {
                        MessageBox.Show("Bạn đã đăng ký tài khoản thành công ! ");
                    }
                }
                catch 
                {
                    MessageBox.Show("Tên Tài Khoản Đã Bị Trùng ! Xin Vui Lòng Nhập Lại Tài Khoản Mới !");
                }
            }
        }

        bool DangKy(string userName,string PassWord)
        {
            PassWord = getMd5Hash(PassWord);
           
            string sql = @"INSERT INTO dbo.Account        ( [UserName] ,  [PassWord]  ) VALUES  (	N'"+userName+"', N'"+PassWord+"')";
            int kq = DataProvider.Instance.ExecuteNonQuery(sql);
            if (kq > 0)
                return true;
            return false;
        }

        static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private void txtXacNhan_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnDangKy.PerformClick();
            }
        }
    }
}
