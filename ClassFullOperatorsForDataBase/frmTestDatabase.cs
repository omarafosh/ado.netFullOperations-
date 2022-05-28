using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.IO;

namespace ClassFullOperatorsForDataBase
{
    public partial class frmTestDatabase : DevExpress.XtraEditors.XtraForm
    {
        public frmTestDatabase()
        {
            InitializeComponent();
        }


        string strSql = "";
        OleDbConnection dbcon = new OleDbConnection();

        private void frmTestDatabase_Load(object sender, EventArgs e)
        {
            AdoCalss connstring = new AdoCalss();
            string strConn=connstring.SelectConnectionString("accessnew", "", "", "db");
            dbcon.ConnectionString = strConn;
       
            strSql = "select * from tbl";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AdoCalss dd = new AdoCalss();
            DataTable  dt = new DataTable();
            dt.Clear();
            dt = dd.GetData(strSql, dbcon);
            Control[] carr = new Control[3] { textEdit4, textEdit1, checkEdit1 };
            string[] farr = new string[3] { "Photo", "fName", "IsSatate" };
            dd.fromDB(carr, farr, dt,0);
            dbcon.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AdoCalss ad = new AdoCalss();
            Control[] carr = new Control[3] { textEdit4, textEdit1, checkEdit1 };
            string[] farr = new string[3] { "Photo", "fName", "IsSatate" };
            ad.InsertData(carr, farr, "tbl", dbcon);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            AdoCalss ad = new AdoCalss();
            Control[] carr = new Control[3] { textEdit4, textEdit1, checkEdit1 };
            string[] farr = new string[3] { "Photo", "fName", "IsSatate" };
           ad.InsertData(carr, farr, "tbl",dbcon);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            AdoCalss ad = new AdoCalss();
            Control[] carr = new Control[2] { textEdit4, checkEdit1 };
            string[] farr = new string[2] { "Photo", "IsSatate" };
            //DataTable dSelect = new DataTable();
            //strSql = "select ID from tbl ID=" + t;
            //dSelect = ad.GetData(strSql, dbcon);
            ad.UpdateData(carr, farr, "tbl", "fName='" + textEdit1.Text + "'", dbcon);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Exit Button
            Application.Exit();
        }
    }
}