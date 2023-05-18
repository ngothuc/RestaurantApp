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
            LoadAccountList();
        }

        void LoadFoodList()
        {
            string query = "SELECT * FROM FOOD";


            dataGridViewFood.DataSource = DataProvider.Instance.ExcuteQuery(query, new object[] { "staff" });
        }

        void LoadAccountList()
        {

            string query = "EXEC dbo.USP_GetAccountByUserName @username";

            DataProvider provider = new DataProvider();

            dataGridViewAccount.DataSource = provider.ExecuteQuery(query, new object[]{"staff"});
        }


        private void FormAdmin_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonBill_Click(object sender, EventArgs e)
        {

        }

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
    }
}
