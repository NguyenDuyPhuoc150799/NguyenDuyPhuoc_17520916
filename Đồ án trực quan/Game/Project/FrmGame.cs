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

namespace Project
{
    public partial class FrmGame : Form
 
    {
        public  FrmGame()
        {
            InitializeComponent();
        }
        Button[,] Map;
        int level;
        int n;
        int SoLuongBooom;
        int SoLuongNuocDi;
        Button[] ListNuocDi;
        Button btnGo;
        Button btnQuaPhai;
        Button btnXuongDuoi;
        Button btnDelete;
        int idx = 0;
        int RowRobot = 0;
        int ColumnRobot = 0;

        private string TaiKhoanThat;
        public string _TaiKhoanThat { get => TaiKhoanThat; set => TaiKhoanThat = value; }

        private void btnTaoMap_Click(object sender, EventArgs e)
        {

            if (int.TryParse(txtLevel.Text, out level) == false)
            {
                MessageBox.Show("Level phải là kiểu số ! ");
                txtLevel.Focus();
                txtLevel.SelectAll();
            }
            else
            {
                int levelthatsu = 1;
                // 1. Kiểm tra txttaikhoan có bằng tài khoản trong database  ? 
                string sql = @"select * from Account";
                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
                //  bool CheckTaiKhoan = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 2 Tài khoản bằng nhau ==> Lấy Level của dòng đó ra
                    if (_TaiKhoanThat == (string)dt.Rows[i][0])
                    {
                        // txtLevel.Text = dt.Rows[i][2].ToString();
                        levelthatsu = (int)dt.Rows[i][2];
                        break;
                    }
                }


                // Kiểm tra level trong textbox level so với level trong database nếu lớn hơn thì không cho phép thực hiện việc tạo map
                bool Checklevel = true;
                if (level > levelthatsu)
                {
                    MessageBox.Show("Bạn không thể đi lên level cao hơn trừ khi chiến thắng ở hiện tại");
                    level = levelthatsu;
                    txtLevel.Text = level.ToString();
                    Checklevel = false;
                }
                if (Checklevel)
                {
                    txtLevel.Text = levelthatsu.ToString();
                    txtLevel.Focus();
                    txtLevel.SelectAll();
                    for (int i = 0; i < this.Controls.Count; ++i)
                    {
                        if (Controls[i].Name == "QuaPhai" || Controls[i].Name == "Map" || Controls[i].Name == "NuocDi" || Controls[i].Name == "QuaPhai" || Controls[i].Name == "XuongDuoi" || Controls[i].Name == "Go" || Controls[i].Name == "Delete" || Controls[i].Name == "Loop")
                            this.Controls.Remove(Controls[i--]);
                    }


                    n = (level - 1) / 3 + 4; // Khởi Tạo Map 4x4 và cách 3 level sẽ tăng Map++
                    Map = new Button[n, n];
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = 0; j < n; ++j)
                        {
                            Map[i, j] = new Button();
                            Map[i, j].Size = new Size(400 / n, 400 / n);
                            Map[i, j].Location = new Point(i * Map[i, j].Width, j * Map[i, j].Height + 150);
                            Map[i, j].Enabled = false;
                            Map[i, j].FlatStyle = FlatStyle.Flat;
                            Map[i, j].Name = "Map";
                            // Kiểm Tra Đích đến(dòng cuối và cột cuối) và tô màu
                            if (j == n - 1 || i == n - 1)
                            {
                                Map[i, j].BackColor = Color.BlueViolet;
                            }

                            this.Controls.Add(Map[i, j]);
                        }
                    }
                    // Hiển thị robot lên Map[0,0]
                    Map[0, 0].BackgroundImage = Properties.Resources.robot;
                    Map[0, 0].BackgroundImageLayout = ImageLayout.Stretch;
                    // Hiển thị chướng ngại vật
                    //B1 : Xác định số lượng chướng ngại vật : [ (n^2 - 2n)/4, (n^2-2n)/2 ]
                    Random rd = new Random();
                    SoLuongBooom = rd.Next((n * n - 2 * n) / 6, (n * n - 2 * n) / 3);
                    //  MessageBox.Show(SoLuongBooom.ToString());

                    //B2 : Hiển thị chướng ngại vật lên trên map

                    for (int i = 0; i < SoLuongBooom; ++i)
                    {
                        int RowBoom, ColumnBoom;
                        do
                        {
                            RowBoom = rd.Next(0, n - 1); // Bỏ Dòng cuối
                            ColumnBoom = rd.Next(0, n - 1); // Bỏ Cột Cuối    

                        } while (Map[RowBoom, ColumnBoom].BackgroundImage != null); // Không Được Trùng Với Vị Trí Robot và sẽ lặp lại nếu quả boom trùng nhau                           
                        Map[RowBoom, ColumnBoom].BackgroundImage = Properties.Resources.boom;
                        Map[RowBoom, ColumnBoom].BackgroundImageLayout = ImageLayout.Stretch;
                    }

                    // Hiển thị Nước Đi Được cho phép
                    //B1 : xác định số lượng nước đi (n/2,0.75n)
                    SoLuongNuocDi = rd.Next(n / 2, (int)(0.75 * n + 1));
                    //  MessageBox.Show(SoLuongNuocDi.ToString());
                    //B2 : Tạo Danh Sách Nước Đi Hiển Thị Lên Form
                    ListNuocDi = new Button[SoLuongNuocDi];
                    int SizeNuocDi;
                    if (SoLuongNuocDi <= 11)
                        SizeNuocDi = 50;
                    else
                        SizeNuocDi = 500 / SoLuongNuocDi;
                    for (int i = 0; i < ListNuocDi.Length; ++i)
                    {
                        ListNuocDi[i] = new Button();
                        ListNuocDi[i].Size = new Size(SizeNuocDi, SizeNuocDi);
                        ListNuocDi[i].Location = new Point(i * ListNuocDi[i].Width, Map[0, 0].Location.Y - ListNuocDi[i].Height);
                        ListNuocDi[i].Name = "NuocDi";
                        ListNuocDi[i].Tag = 0;
                        if (i >= SoLuongNuocDi / 2)
                        {
                            //Nước có thể bắt đầu đi
                            ListNuocDi[i].Text = "Go";
                            ListNuocDi[i].TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                        }
                        ListNuocDi[i].Enabled = false;
                        this.Controls.Add(ListNuocDi[i]);
                    }

                    //Hiển thị Button Qua Phải
                    btnQuaPhai = new Button();
                    btnQuaPhai.Size = new Size(50, 50);
                    btnQuaPhai.Location = new Point(ListNuocDi[0].Location.X, ListNuocDi[0].Location.Y - btnQuaPhai.Height);
                    btnQuaPhai.BackgroundImage = Properties.Resources.right;
                    btnQuaPhai.BackgroundImageLayout = ImageLayout.Stretch;
                    btnQuaPhai.Name = "QuaPhai";
                    btnQuaPhai.Click += btnQuaPhai_Click;
                    this.Controls.Add(btnQuaPhai);

                    //Hiển thị Button Xuống Dưới
                    btnXuongDuoi = new Button();
                    btnXuongDuoi.Size = new Size(50, 50);
                    btnXuongDuoi.Location = new Point(btnQuaPhai.Location.X + btnQuaPhai.Width, ListNuocDi[0].Location.Y - btnQuaPhai.Height);
                    btnXuongDuoi.BackgroundImage = Properties.Resources.down;
                    btnXuongDuoi.BackgroundImageLayout = ImageLayout.Stretch;
                    btnXuongDuoi.Name = "XuongDuoi";
                    btnXuongDuoi.Click += btnXuongDuoi_Click;
                    this.Controls.Add(btnXuongDuoi);

                    //Hiển thị Button Delete
                    btnDelete = new Button();
                    btnDelete.Size = new Size(50, 50);
                    btnDelete.Location = new Point(btnXuongDuoi.Location.X + btnXuongDuoi.Width, ListNuocDi[0].Location.Y - btnQuaPhai.Height);
                    btnDelete.BackgroundImage = Properties.Resources.Delete;
                    btnDelete.BackgroundImageLayout = ImageLayout.Stretch;
                    btnDelete.Name = "Delete";
                    btnDelete.Click += btnDelete_Click;
                    this.Controls.Add(btnDelete);

                    //Hiển thị Button Go
                    btnGo = new Button();
                    btnGo.Size = new Size(50, 50);
                    btnGo.Location = new Point(btnDelete.Location.X + btnDelete.Width, ListNuocDi[0].Location.Y - btnQuaPhai.Height);
                    btnGo.BackgroundImage = Properties.Resources.go;
                    btnGo.BackgroundImageLayout = ImageLayout.Stretch;
                    btnGo.Name = "Go";
                    btnGo.Visible = false;
                    btnGo.Click += btnGo_Click;
                    this.Controls.Add(btnGo);

                    //Hiển Thị picture Loop
                    PictureBox ptrLoop = new PictureBox();
                    ptrLoop.Size = new Size(SizeNuocDi, SizeNuocDi);
                    ptrLoop.Location = new Point(ListNuocDi[SoLuongNuocDi - 1].Location.X + ListNuocDi[SoLuongNuocDi - 1].Width, ListNuocDi[0].Location.Y);
                    ptrLoop.BackgroundImage = Properties.Resources.loop;
                    ptrLoop.BackgroundImageLayout = ImageLayout.Stretch;
                    ptrLoop.Name = "Loop";
                    this.Controls.Add(ptrLoop);
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            tmDiChuyen.Start();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = ListNuocDi.Count() - 1; i >= 0; --i)
            {
                if (i <= SoLuongNuocDi / 2 - 1)
                {
                    btnGo.Visible = false;
                }

                if (ListNuocDi[i].BackgroundImage != null)
                {
                    ListNuocDi[i].BackgroundImage = null;
                    ListNuocDi[i].Tag = 0;
                    break;
                }
            }
        }

        private void btnXuongDuoi_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListNuocDi.Count(); ++i)
            {
                if (i >= SoLuongNuocDi / 2 - 1)
                {
                    btnGo.Visible = true;

                }
                else
                {
                    btnGo.Visible = false;
                }
                if (ListNuocDi[i].BackgroundImage == null)
                {
                    ListNuocDi[i].BackgroundImage = Properties.Resources.down;
                    ListNuocDi[i].BackgroundImageLayout = ImageLayout.Stretch;
                    ListNuocDi[i].Tag = 1;
                    break;
                }
            }
        }

        private void btnQuaPhai_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListNuocDi.Count(); ++i)
            {
                if (i >= SoLuongNuocDi / 2 - 1)
                {
                    btnGo.Visible = true;

                }
                else
                {
                    btnGo.Visible = false;
                }
                if (ListNuocDi[i].BackgroundImage == null)
                {
                    ListNuocDi[i].BackgroundImage = Properties.Resources.right;
                    ListNuocDi[i].BackgroundImageLayout = ImageLayout.Stretch;
                    ListNuocDi[i].Tag = 2;
                    break;
                }
            }

        }
      
        private void tmDiChuyen_Tick(object sender, EventArgs e)
        {
            foreach (Button btn in ListNuocDi)
            {
                btn.BackColor = this.BackColor;
            }

            if ((int)ListNuocDi[idx].Tag == 2) //xuống dưới
            {
                // Xóa Ảnh robot
                Map[RowRobot, ColumnRobot].BackgroundImage = null;
                Map[0, 0].BackColor = Color.Blue;
                // Tăng dòng,giữ nguyên cột , cập nhật ảnh robot
                RowRobot++;

                // Đụng boom
                if (Map[RowRobot, ColumnRobot].BackgroundImage != null)
                {
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.bigbang;

                    tmDiChuyen.Stop();
                    btnGo.Enabled = false;
                    btnQuaPhai.Enabled = false;
                    btnXuongDuoi.Enabled = false;
                    btnDelete.Enabled = false;
                    MessageBox.Show("You Lose ! Click OK to play again ! ");

                    btnGo.Visible = false;
                    btnGo.Enabled = true;
                    btnQuaPhai.Enabled = true;
                    btnXuongDuoi.Enabled = true;
                    btnDelete.Enabled = true;
                    //  Cho Robot về lại ô 0 - 0
                    Map[0, 0].BackgroundImage = Properties.Resources.robot;
                    Map[0, 0].BackgroundImageLayout = ImageLayout.Stretch;


                    //2 / Reset lại màu nền của map là màu nền của form
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = 0; j < n; ++j)
                        {
                            Map[i, j].BackColor = this.BackColor;
                            if (i == n - 1 || j == n - 1)
                            {
                                Map[i, j].BackColor = Color.BlueViolet;
                            }
                        }

                    }
                    //3 / Chỗ bigbang hiện lên cho hiện lại quả boom trước đó
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.boom;
                    Map[RowRobot, ColumnRobot].BackgroundImageLayout = ImageLayout.Stretch;
                    //4 / Nước đi sẽ được reset trống lại từ đầu
                    foreach (Button bt in ListNuocDi)
                    {
                        bt.BackgroundImage = null;
                        bt.Tag = 0;
                    }
                    RowRobot = 0;
                    ColumnRobot = 0;
                    idx = 0;
                }

                else
                {
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.robot;
                    Map[RowRobot, ColumnRobot].BackgroundImageLayout = ImageLayout.Stretch;
                    Map[RowRobot, ColumnRobot].BackColor = Color.Green;
                    ListNuocDi[idx].BackColor = Color.OrangeRed;
                }
            }
            else if ((int)ListNuocDi[idx].Tag == 1) // qua phải
            {
                // Xóa Ảnh robot
                Map[RowRobot, ColumnRobot].BackgroundImage = null;
                // giữ nguyên dòng,tăng cột , cập nhật ảnh robot
                Map[0, 0].BackColor = Color.Blue;
                ColumnRobot++;

                //đụng boom
                if (Map[RowRobot, ColumnRobot].BackgroundImage != null)
                {
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.bigbang;

                    tmDiChuyen.Stop();
                    btnGo.Enabled = false;
                    btnQuaPhai.Enabled = false;
                    btnXuongDuoi.Enabled = false;
                    btnDelete.Enabled = false;
                    MessageBox.Show("You Lose ! Click OK to play again ! ");

                    btnGo.Visible = false;
                    btnGo.Enabled = true;
                    btnQuaPhai.Enabled = true;
                    btnXuongDuoi.Enabled = true;
                    btnDelete.Enabled = true;

                    //  Cho Robot về lại ô 0 - 0
                    Map[0, 0].BackgroundImage = Properties.Resources.robot;
                    Map[0, 0].BackgroundImageLayout = ImageLayout.Stretch;


                    //2 / Reset lại màu nền của map là màu nền của form
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = 0; j < n; ++j)
                        {
                            Map[i, j].BackColor = this.BackColor;
                            if (i == n - 1 || j == n - 1)
                            {
                                Map[i, j].BackColor = Color.BlueViolet;
                            }
                        }

                    }
                    //3 / Chỗ bigbang hiện lên cho hiện lại quả boom trước đó
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.boom;
                    Map[RowRobot, ColumnRobot].BackgroundImageLayout = ImageLayout.Stretch;
                    //4 / Nước đi sẽ được reset trống lại từ đầu
                    foreach (Button bt in ListNuocDi)
                    {
                        bt.BackgroundImage = null;
                        bt.Tag = 0;
                    }
                    RowRobot = 0;
                    ColumnRobot = 0;
                    idx = 0;
                }

                else
                {
                    Map[RowRobot, ColumnRobot].BackgroundImage = Properties.Resources.robot;
                    Map[RowRobot, ColumnRobot].BackgroundImageLayout = ImageLayout.Stretch;
                    Map[RowRobot, ColumnRobot].BackColor = Color.Green;
                    ListNuocDi[idx].BackColor = Color.OrangeRed;
                }
            }
            idx++;
            if (idx == SoLuongNuocDi || ListNuocDi[idx].BackgroundImage == null)
            {
                //ListNuocDi[idx].BackColor = Color.OrangeRed;
                Map[RowRobot, ColumnRobot].BackColor = Color.Blue;
                idx = 0;
            }
            // Chiến Thắng ==> update level lên database
            if (RowRobot == n - 1 || ColumnRobot == n - 1)
            {
                RowRobot = 0;
                ColumnRobot = 0;
                idx = 0;
                tmDiChuyen.Stop();
                btnGo.Enabled = false;
                btnQuaPhai.Enabled = false;
                btnXuongDuoi.Enabled = false;
                btnDelete.Enabled = false;
                MessageBox.Show("You Win ! Congratulations!");
                //Load map level tiếp theo
                txtLevel.Text = (++level).ToString();

                //Chỉ Update level vào database khi level đó lớn hơn level hiện tại đang chứa trong database còn nếu nhỏ hơn thì không lưu

                int levelthatsu = 1;
                // 1. Kiểm tra txttaikhoan có bằng tài khoản trong database  ? 
                string sql = @"select * from Account";
                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 2 Tài khoản bằng nhau ==> Lấy Level của dòng đó ra
                    if (_TaiKhoanThat == (string)dt.Rows[i][0])
                    {
                        levelthatsu = (int)dt.Rows[i][2];
                        break;
                    }
                }
                if (level > levelthatsu)
                {
                    // Update level mới vào database
                    string sql1 = "update Account set Level = " + level + " where UserName = N'" + _TaiKhoanThat + "' ";
                    DataProvider.Instance.ExecuteNonQuery(sql1);
                }

                btnTaoMap.PerformClick();
                txtLevel.Select(txtLevel.MaxLength, 1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Pink;
            // 1. Kiểm tra txttaikhoan có bằng tài khoản trong database  ? 
            string sql = @"select * from Account";
            DataTable dt = DataProvider.Instance.ExecuteQuery(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // 2 Tài khoản bằng nhau ==> Lấy Level của dòng đó ra
                if (_TaiKhoanThat == (string)dt.Rows[i][0])
                {
                    txtLevel.Text = dt.Rows[i][2].ToString();
                    break;
                }
            }
            btnTaoMap.PerformClick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmDiChuyen.Dispose();
        }

        private void txtLevel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTaoMap.PerformClick();
            }
        }
    }
}
