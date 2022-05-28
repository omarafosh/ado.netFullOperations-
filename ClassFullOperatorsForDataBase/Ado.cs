using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassFullOperatorsForDataBase
{
    public class AdoCalss
    {

        public static string ConnString;
        //public string ConnString
        //{
        //    get
        //    {
        //        return _ConnString;
        //    }

        //    set
        //    {
        //        _ConnString = value;
        //    }
        //}
        public string SelectConnectionString(string Type, string username = "", string Password = "", string databaseName = "", string ipAddress = "127.0.0.1", int Port = 0, string datafile = "")
        {
            string str = "";
            if (Type == "accessold")
            {
                str = (Password == "") ? "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + databaseName + ".mdb" : "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + databaseName + ".mdb;Jet OLEDB:Database Password=" + Password;
            }
            else if (Type == "accessnew")
            {
                str = (Password == "") ? "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\" + databaseName + ".accdb" : "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\" + databaseName + ".accdb;Jet OLEDB:Database Password=" + Password;
            }

            else if (Type == "sqlserverce")
            {
                str = "Provider=Microsoft.SQLSERVER.CE.OLEDB.3.5;Data Source=" + Application.StartupPath + "\\" + databaseName + ".sdf";
            }
            else if (Type == "sqlservernet")
            {
                str = "Data Source=" + ipAddress + "," + Port + ";Network Library=DBMSSOCN;Initial Catalog=" + databaseName + ";User ID=" + username + ";Password=" + Password;
            }
            else if (Type == "sqlservernet")
            {
                str = @"Server=.\SQLExpress;AttachDbFilename=" + Application.StartupPath + "\\+" + datafile + ".mdf;Database=" + databaseName + ";Trusted_Connection=Yes";
            }
            return str;
        }
        public byte[]  SavePic(Image  img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }   
        }
        public Image ReadPic(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {

                return Image.FromStream(ms);
            }
        }
        public void fromDB(Control[] fControl , string[] lsfileds, DataTable dt,int index=0)
        {
            if (fControl.Length == 0 || dt.Rows.Count ==0)
                return;
            for (int i = 0; i < fControl.Length; i++)
            {
                if (Equals(fControl[i].GetType(), typeof(TextEdit)))
                {
                    TextEdit txt = (TextEdit)fControl[i];
                    txt.Text = dt.Rows[index][lsfileds[i]].ToString();
                }
                if (Equals(fControl[i].GetType(), typeof(PictureEdit)))
                {
                    PictureEdit pic = (PictureEdit)fControl[i];
                    if (pic.Image as object!=DBNull.Value)
                    pic.Image = ReadPic((byte[])dt.Rows[index][lsfileds[i]]); 
                }
                if (Equals(fControl[i].GetType(), typeof(CheckEdit)))
                {
                    CheckEdit chk = (CheckEdit)fControl[i];
                    chk.Checked = (bool)dt.Rows[index][lsfileds[i]];
                }
            }
        }
        public void OpenDBSafe(OleDbConnection dbcon)
        {
            if (dbcon.State == ConnectionState.Open)
                dbcon.Close();
            dbcon.Open();
        }
        public DataTable GetData(string str, OleDbConnection conn)
        {
            DataTable temp = new DataTable();
            OleDbDataAdapter dp = new OleDbDataAdapter(str, conn);
            OpenDBSafe(conn);
            dp.Fill(temp);
            conn.Close();
            return temp;
        }
        public void InsertData(Control[] fControl, string[] lsfileds, string tableName,OleDbConnection db)
        {
            string strField = "";
            string strvalue = "";
            for (int i = 0; i < fControl.Length; i++)
            {
                strField += lsfileds[i].ToString() + ",";
                strvalue += ("@" + lsfileds[i] + ",");
            }
            string str = "insert into " + tableName + "(" + strField.Substring(0, strField.Length - 1) + ") values (" + strvalue.Substring(0, strvalue.Length - 1) + ")";
            OleDbCommand cmd = new OleDbCommand(str, db);
            OpenDBSafe(db);
            for (int j=0;j< fControl.Length;j++)
            {
                if (Equals(fControl[j].GetType(), typeof(TextEdit)))
                {
                    TextEdit txt = (TextEdit)fControl[j];
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), txt.Text);
                }
                if (Equals(fControl[j].GetType(), typeof(PictureEdit)))
                {
                    PictureEdit pic = (PictureEdit)fControl[j];
                    if (pic.Image !=null)
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(),SavePic( pic.Image));
                    else
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), DBNull.Value);

                }
                if (Equals(fControl[j].GetType(), typeof(CheckEdit)))
                {
                    CheckEdit chk = (CheckEdit)fControl[j];
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), chk.Checked);
                }
            }
            cmd.ExecuteNonQuery();
            db.Close();     
        }
        public void UpdateData(Control[] fControl, string[] lsfileds, string tableName,string condition, OleDbConnection db)
        {
            string strField = "";
            for (int i = 0; i < fControl.Length; i++)
            {
                strField += lsfileds[i].ToString() + ("=@" + lsfileds[i]) + ",";
            }
            string str = "Update " + tableName + " set " + strField.Substring(0, strField.Length - 1) + " where " + condition;
            OleDbCommand cmd = new OleDbCommand(str, db);
            OpenDBSafe(db);
            for (int j = 0; j < fControl.Length; j++)
            {
                if (Equals(fControl[j].GetType(), typeof(TextEdit)))
                {
                    TextEdit txt = (TextEdit)fControl[j];
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), txt.Text);
                }
                if (Equals(fControl[j].GetType(), typeof(PictureEdit)))
                {
                    PictureEdit pic = (PictureEdit)fControl[j];
                    if (pic.Image !=null)
                        cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), SavePic(pic.Image));
                    else
                        cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), DBNull.Value);
                }
                if (Equals(fControl[j].GetType(), typeof(CheckEdit)))
                {
                    CheckEdit chk = (CheckEdit)fControl[j];
                    cmd.Parameters.AddWithValue("@" + lsfileds[j].ToString(), chk.Checked);
                }
            }
            cmd.ExecuteNonQuery();
            db.Close();

        }
    }
}
