using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoAPI
{
    public partial class MainForm : Form
    {
        APIHelper apiHelper = new APIHelper();
        int currentID = 0;
        List<Customer> customers = null;
        public MainForm()
        {
            InitializeComponent();
            ReloadTable();
        }
        // Load lại dữ liệu mỗi khi khởi động hoặc thực hiện thêm, sửa, xóa 
        public void ReloadTable()
        {
            customers = apiHelper.GetListCustomerAPI();
            if (customers != null)
            {
                string searchString = textBoxSearch.Text;
                if (searchString != string.Empty)
                {
                    var customerFilterByName = customers.FindAll(
                    x => x.TenKhach.ToLower().Contains(searchString) ||
                    x.ID.ToString().Contains(searchString) ||
                    x.DiaChi.ToLower().Contains(searchString) ||
                    x.DienThoai.ToLower().Contains(searchString));
                    CustomerGridView.DataSource = customerFilterByName;
                }
                else
                {
                    CustomerGridView.DataSource = customers;
                }
            }
        }
        //Đặt lại mặc định cho các TextBox và Button 
        public void RefreshStatus()
        {
            buttonAdd.Enabled = true;
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
            buttonRefresh.Enabled = false;
            textBoxName.Text = string.Empty;
            textBoxAdress.Text = string.Empty;
            textBoxPhoneNumber.Text = string.Empty;
        }
        // Nút bỏ chọn
        private void button4_Click(object sender, EventArgs e)
        {
            RefreshStatus();
        }
        // CellClick cho DataGridView
        private void CustomerGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonAdd.Enabled = false;
            buttonUpdate.Enabled = true;
            buttonDelete.Enabled = true;
            buttonRefresh.Enabled = true;
            int row = e.RowIndex;

            if (row > -1 && row < CustomerGridView.RowCount)
            {
                try
                {
                    currentID = int.Parse(CustomerGridView.Rows[row].Cells[0].Value.ToString());
                }
                catch (Exception) { }
                string Name = CustomerGridView.Rows[row].Cells[1].Value.ToString();
                string Address = CustomerGridView.Rows[row].Cells[2].Value.ToString();
                string PhoneNumber = CustomerGridView.Rows[row].Cells[3].Value.ToString();
                textBoxName.Text = Name;
                textBoxAdress.Text = Address;
                textBoxPhoneNumber.Text = PhoneNumber;
            }
        }
        // Nút thêm
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string address = textBoxAdress.Text;
            string phonenumber = textBoxPhoneNumber.Text;
            try
            {
                int phone = int.Parse(phonenumber);
            }
            catch (Exception)
            {
                MessageBox.Show("Số điện thoại không đúng định dạng vui lòng nhập lại");
                return;
            }
            bool success = apiHelper.CreateCustomerAPI(name, address, phonenumber);
            if (success)
            {
                MessageBox.Show("Thêm thành công .");
                ReloadTable();

            }
            else
            {
                MessageBox.Show("Có lỗi từ server.");
            }
        }
        // Nút sửa
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string address = textBoxAdress.Text;
            string phonenumber = textBoxPhoneNumber.Text;
            try
            {
                int phone = int.Parse(phonenumber);
            }
            catch (Exception)
            {
                MessageBox.Show("Số điện thoại không đúng định dạng vui lòng nhập lại");
                return;
            }
            bool success = apiHelper.UpdateCustomerAPI(currentID, name, address, phonenumber);
            if (success)
            {
                MessageBox.Show("Sửa thành công .");
                ReloadTable();

            }
            else
            {
                MessageBox.Show("Có lỗi từ server.");
            }
        }
        // Nút xóa
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            DialogResult dialogResult = MessageBox.Show($"Có chắc muốn xóa khách hàng '{name}' có ID là {currentID}? ", "Thông báo ", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                bool success = apiHelper.DeleteCustomerAPI(currentID);
                if (success)
                {
                    MessageBox.Show("Xóa thành công .");
                    ReloadTable();

                }
                else
                {
                    MessageBox.Show("Có lỗi từ server.");
                }
            }
        }
        // Chức năng tìm kiếm
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchString = textBoxSearch.Text.ToLower();
            if (searchString != string.Empty)
            {
                var customerFilterByName = customers.FindAll(
                    x => x.TenKhach.ToLower().Contains(searchString) ||
                    x.ID.ToString().Contains(searchString) ||
                    x.DiaChi.ToLower().Contains(searchString) ||
                    x.DienThoai.ToLower().Contains(searchString));
                CustomerGridView.DataSource = customerFilterByName;
            }
            else
            {
                ReloadTable();
            }
        }

        private void CustomerGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
