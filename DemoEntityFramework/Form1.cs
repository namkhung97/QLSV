using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoEntityFramework
{
    public partial class Form1 : Form
    {
        QLSINHVIENEntities db = new QLSINHVIENEntities();

        public Form1()
        {
            InitializeComponent();
            LoadFrom();
            Edit_Column();
        }
        //Load form
       void LoadFrom()
        {
            dtgSinhVien.DataSource = db.SinhViens.ToList();
            comboBox1.DataSource = db.Khoas.ToList();
            comboBox1.ValueMember = "MaKH";
            comboBox1.DisplayMember = "TenKH";
            Edit_Column();
        }

        public void Clear()
        {
            txtMaSV.ReadOnly = false;
            txtMaSV.Text = "";
            txtHo.Text = "";
            txtTen.Text = "";
            txtNoisinh.Text = "";
            txtHocbong.Text = "";
        }
        void Edit_Column()
        {
            dtgSinhVien.Columns[0].HeaderText = "Họ";
            dtgSinhVien.Columns[1].HeaderText = "Tên";
            dtgSinhVien.Columns[2].HeaderText = "Giới tính";
        }

        void ThemSinhVien()
        {
            try
            {
                SinhVien sv = new SinhVien();

                sv.MaSV = txtMaSV.Text;
                sv.HoSV = txtHo.Text;
                sv.TenSV = txtTen.Text;
                //giới tính
                foreach (var i in groupBox1.Controls)
                {
                    RadioButton r = (RadioButton)i;
                    if (r.Checked)
                    {
                        switch (r.Text)
                        {
                            case "Nam":
                                sv.Phai = true;
                                break;
                            case "Nữ":
                                sv.Phai = false;
                                break;
                           
                        }
                    }
                }
                sv.NgaySinh = dateTimePicker1.Value;
                sv.NoiSinh = txtNoisinh.Text;
                sv.MaKH = comboBox1.SelectedValue.ToString();
                sv.HocBong = double.Parse(txtHocbong.Text);
                
                db.SinhViens.Add(sv);
                db.SaveChanges();
                MessageBox.Show("Thêm mới thành công");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi rồi : " , ex.ToString());
            }
        }

        void SuaSinhVien()
        {
            try
            {
                var query = db.SinhViens.Where(n=>n.MaSV == txtMaSV.Text);
                txtMaSV.ReadOnly = true;
                if (query.Count() > 0)
                {
                    SinhVien sv = query.SingleOrDefault();
                    
                    sv.HoSV = txtHo.Text;
                    sv.TenSV = txtTen.Text;
                    //giới tính
                    foreach (var i in groupBox1.Controls)
                    {
                        RadioButton r = (RadioButton)i;
                        if (r.Checked)
                        {
                            switch (r.Text)
                            {
                                case "Nam":
                                    sv.Phai = true;
                                    break;
                                case "Nữ":
                                    sv.Phai = false;
                                    break;
                            }
                        }
                    }
                    sv.NgaySinh = dateTimePicker1.Value;
                    sv.NoiSinh = txtNoisinh.Text;
                    sv.MaKH = comboBox1.SelectedValue.ToString();
                    sv.HocBong = double.Parse(txtHocbong.Text);

                    db.SaveChanges();
                    dtgSinhVien.DataSource = null;
                    dtgSinhVien.DataSource = db.SinhViens.ToList();
                    Edit_Column();
                    Clear();
                    MessageBox.Show("Cập nhập thành công");
                }
                else
                {
                    MessageBox.Show("oh no ! k cập nhập được rồi !");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi ngay từ vòng gửi xe vậy nè !",ex.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        void XoaSinhVien()
        {
            try
            {
                    if (MessageBox.Show("bạn có thật sự muốn xóa ","thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("bạn có thật sự muốn xóa ", "thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            string maSV = txtMaSV.Text;
                            SinhVien sv = db.SinhViens.Where(n => n.MaSV == maSV).SingleOrDefault();
                            db.SinhViens.Remove(sv);
                            db.SaveChanges();
                            LoadFrom();
                            Clear();
                            Edit_Column();
                        }
                    }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi ngay từ vòng gửi xe rồi","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        
      

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemSinhVien();
            LoadFrom();
            Clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SuaSinhVien();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            XoaSinhVien();
        }
        
        //hiện thông tin lên ô text khi được click
        private void dtgSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow r = dtgSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = r.Cells[0].Value.ToString();
                txtHo.Text = r.Cells[1].Value.ToString();
                txtTen.Text = r.Cells[2].Value.ToString();
                //check giới tính
                if ((bool)r.Cells[3].Value)
                {
                    foreach (RadioButton rdo in groupBox1.Controls)
                    {
                        if (rdo.Text == "Nam")
                        {
                            rdo.Checked = true;
                        }
                    }
                }
                else
                {
                    foreach (RadioButton rdo in groupBox1.Controls)
                    {
                        if (rdo.Text == "Nữ")
                        {
                            rdo.Checked = true;
                        }

                    }
                }
                dateTimePicker1.Value = (DateTime)r.Cells[4].Value;
                txtNoisinh.Text = r.Cells[5].Value.ToString();
                comboBox1.SelectedValue = r.Cells[6].Value.ToString();
                txtHocbong.Text = r.Cells[7].Value.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Chọn không đúng bản ghi , vui lòng chọn lại !","Thông báo" , MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
    }
}
