using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class AccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public AccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            textBoxUserName.Text = LoginAccount.UserName;
            textBoxDisplayName.Text = LoginAccount.DisplayName;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        void UpdateAccountInfo()
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(textBoxPassWord.Text);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string password = "";

            foreach (byte item in hasData)
            {
                password += item;
            }
            string displayName = textBoxDisplayName.Text;
            string newpass = "";
            string reenterPass = "";
            string userName = textBoxUserName.Text;

            byte[] tmp = ASCIIEncoding.ASCII.GetBytes(textBoxNewPassword.Text);
            byte[] has_tmp = new MD5CryptoServiceProvider().ComputeHash(tmp);
            foreach (byte item in has_tmp)
            {
                newpass += item;
            }

            byte[] retmp = ASCIIEncoding.ASCII.GetBytes(textBoxReEnterNewPW.Text);
            byte[] has_retmp = new MD5CryptoServiceProvider().ComputeHash(retmp);
            foreach (byte item in has_retmp)
            {
                reenterPass += item;
            }

            if(!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mất khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccountPassWord(userName, displayName, password, newpass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value;  }
            remove { updateAccount -= value; }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent:EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
