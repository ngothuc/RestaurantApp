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

namespace QuanLyQuanCafe
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
            //LoadAccountList();
            LoadDateTimePickerBill();
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
            LoadListFood();
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

        void LoadListFood()
        {
            dataGridViewFood.DataSource = FoodDAO.Instance.GetListFood();
        }


        #endregion

        #region events
        private void buttonBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dateTimePickerFromDate.Value, dateTimePickerDateAfter.Value);
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

        private void buttonViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
    }
}
