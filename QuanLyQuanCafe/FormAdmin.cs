using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class FormAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource foodcategoryList = new BindingSource();
        BindingSource tableList = new BindingSource();

        public Account loginAccount;

        public FormAdmin()
        {
            InitializeComponent();
            //LoadAccountList();
            dataGridViewFood.DataSource = foodList;
            dataGridViewAccount.DataSource = accountList;
            dataGridViewCategory.DataSource = foodcategoryList;
            dataGridViewTable.DataSource = tableList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
            LoadListFood();
            LoadAccount();
            LoadCategory();
            LoadTable();
            LoadCategoryIntoComboBox(comboBoxCategory);
            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
        }


        /*void LoadFoodList()
        {
            string query = "SELECT * FROM FOOD";


            dataGridViewFood.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }

        void LoadAccountList()
        {

            string query = "EXEC dbo.USP_GetAccountByUserName @username";

            //DataProvider provider = new DataProvider();

            dataGridViewAccount.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[]{"staff"});
        }*/


        private void FormAdmin_Load(object sender, EventArgs e)
        {
            
        }

        #region methods
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            
            return listFood;
        }
        void AddTableBinding()
        {
            textBoxTableID.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "ID"));
            textBoxTableName.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "Name"));
            textBoxTableStatus.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "status"));
        }

        void AddCategoryBinding()
        {
            textBoxIDCategory.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "ID"));
            textBoxCategoryName.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "Name"));
        }

        void AddAccountBinding()
        {
            textBoxUserName.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            textBoxDisplayName.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDownType.DataBindings.Add(new Binding("Value", dataGridViewAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadTable()
        {
            tableList.DataSource = TableDAO.Instance.GetListTable();
        }
        void LoadCategory()
        {
            foodcategoryList.DataSource = CategoryDAO.Instance.GetCategoryList();
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dateTimePickerFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dateTimePickerDateAfter.Value = dateTimePickerFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
           dataGridViewBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            textBoxFoodName.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            textBoxFoodID.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            numericUpDownPrice.DataBindings.Add(new Binding("Value", dataGridViewFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }


        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if(AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if(loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản");
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }


        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }

        }
        #endregion

        #region events
        private void buttonViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void buttonEditAccount_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text;
            string displayName = textBoxDisplayName.Text;
            int type = (int)numericUpDownType.Value;
            EditAccount(userName, displayName, type);
        }

        private void buttonDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text;
            DeleteAccount(userName);
        }

        private void buttonAddAccount_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text;
            string displayName = textBoxDisplayName.Text;
            int type = (int)numericUpDownType.Value;
            AddAccount(userName, displayName, type);
        }

        private void buttonSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(textBoxSearchFood.Text);
        }

        private void buttonBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
        }

        private void buttonViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }


        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value;  }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        
        #endregion
        

        private void tabControlAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPageFoodCategory_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewFood.SelectedCells.Count > 0)
                {
                    int id = (int)dataGridViewFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;


                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    comboBoxCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in comboBoxCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    comboBoxCategory.SelectedIndex = index;

                }
            }

            catch { }
        }
        private void buttonViewCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void buttonAddFood_Click(object sender, EventArgs e)
        {
            string name = textBoxFoodName.Text;
            int categoryID = (comboBoxCategory.SelectedItem as Category).ID;
            float price = (float)numericUpDownPrice.Value;

            if(FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }

        private void buttonEditFood_Click(object sender, EventArgs e)
        {
            string name = textBoxFoodName.Text;
            int categoryID = (comboBoxCategory.SelectedItem as Category).ID;
            float price = (float)numericUpDownPrice.Value;
            int id = Convert.ToInt32(textBoxFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món");
            }
        }

        private void buttonDeleteFood_Click(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(textBoxFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }

        private void buttonResetPassword_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text;
            ResetPass(userName);
        }

        
       
       

       

        private void dataGridViewBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            string name = textBoxCategoryName.Text;
            if(CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục");
            }
        }

        private void buttonEditCategory_Click(object sender, EventArgs e)
        {
            string name = textBoxCategoryName.Text;
            int id = Convert.ToInt32(textBoxIDCategory.Text);
            if(CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi trong quá trình sửa danh mục");
            }
        }

        private void buttonDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxIDCategory.Text);

            if(CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục");
            }
        }

        private void buttonViewTable_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void buttonAddTable_Click(object sender, EventArgs e)
        {
            string name = textBoxTableName.Text;
            if(TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }

        private void buttonDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxTableID.Text);
            if(TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn");
            }
        }

        private void buttonEditTable_Click(object sender, EventArgs e)
        {
            string name = textBoxTableName.Text;
            int id = Convert.ToInt32(textBoxTableID.Text);
            if(TableDAO.Instance.UpdateTable(name, id))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }

        private void dateTimePickerFromDate_ValueChanged(object sender, EventArgs e)
        {

        }


       

  
       
        

      
    }
}
