using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get {if (instance == null) instance = new TableDAO() ; return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 115;
        public static int TableHeight = 115;
        private TableDAO(){}

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }


        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
        public DataTable GetListTable()
        {
            return DataProvider.Instance.ExecuteQuery("Select * from TableFood");
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("insert into TableFood(Name) values (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateTable(string name, int id)
        {
            string query = string.Format("update TableFood set name = N'{0}' where ID = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteTable(int id)
        {

            BillDAO.Instance.DeleteBillByTableID(id);
            string query = string.Format("delete from TableFood where id = " + id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
