using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; //Đóng gói Ctrl + R + E

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider(){}

        private string connectionSTR = @"Data Source=.\sqlexpress;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            //Chuyển khởi tạo data lên để return trong trường hợp có lỗi

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            //Tạo kết nối đến DataBase
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                //Tạo và thực thi câu truy vấn

                //DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);


                //Adapter là Trung gian thực hiện câu truy vấn
                connection.Close();
            }
            return data;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            //Chuyển khởi tạo data lên để return trong trường hợp có lỗi

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            //Tạo kết nối đến DataBase
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                //Tạo và thực thi câu truy vấn

                //DataTable data = new DataTable();
                //SqlDataAdapter adapter = new SqlDataAdapter(command);
                //adapter.Fill(data);
                data = command.ExecuteNonQuery();


                //Adapter là Trung gian thực hiện câu truy vấn
                connection.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            //Chuyển khởi tạo data lên để return trong trường hợp có lỗi

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            //Tạo kết nối đến DataBase
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                //Tạo và thực thi câu truy vấn

                //DataTable data = new DataTable();
                //SqlDataAdapter adapter = new SqlDataAdapter(command);
                //adapter.Fill(data);
                data = command.ExecuteScalar();


                //Adapter là Trung gian thực hiện câu truy vấn
                connection.Close();
            }
            return data;
        }
    }
}