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

        public FormAdmin()
        {
            InitializeComponent();
            //LoadAccountList();
            dataGridViewFood.DataSource = foodList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
            LoadListFood();
            LoadCategoryIntoComboBox(comboBoxCategory);
            AddFoodBinding();
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
            textBoxFoodName.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource, "Name"));
            textBoxFoodID.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource, "ID"));
            numericUpDownPrice.DataBindings.Add(new Binding("Value", dataGridViewFood.DataSource, "Price"));
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


        #endregion

        #region events
        private void buttonBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
        }

        private void buttonViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
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

      
    }
}
