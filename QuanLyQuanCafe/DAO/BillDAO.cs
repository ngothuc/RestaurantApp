using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuanLyQuanCafe.DAO
{

    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }
        /// <summary>
        /// Thành công: bill ID
        /// Thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckBillIDbyTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Bill WHERE IDTable = " + id + " AND Status = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "update Bill set DateCheckOut = GETDATE(), Status = 1, " + "discount = " + discount + ", TotalPrice = " + totalPrice*1000 + " where ID = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
           return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[]{checkIn, checkOut});
        }

        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}