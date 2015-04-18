using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 用户名助记
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


       string dbStr = ConfigurationManager.ConnectionStrings["dbCS"].ConnectionString;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt";
            if (ofd.ShowDialog() != true)
            {
                return;
            }
            string filename = ofd.FileName;
           DataTable tableB=new DataTable(); 
            IEnumerable<string> lines = File.ReadLines(filename);
            tableB.Columns.Add("Site1");
            tableB.Columns.Add("User_Name1");
            tableB.Columns.Add("User_Password1");
            tableB.Columns.Add("User_Others1");

               foreach (string line in lines)
               {
                   

                   string[] segs = line.Split(' ');
                   string site = segs[0];
                   string name = segs[1];
                   string password = segs[2];
                   string other = segs[3];
                   DataRow row = tableB.NewRow();
                   row["Site1"] = site;
                   row["User_Name1"] = name;
                   row["User_Password1"] = password;
                   row["User_Others1"] = other;
                   tableB.Rows.Add(row);
                  

               }
                using(SqlBulkCopy bulkCopy =new SqlBulkCopy(dbStr))
               {
                   bulkCopy.DestinationTableName = "T_UserNameAndPassword";
                  
                   bulkCopy.ColumnMappings.Add("Site1", "Site");
                   bulkCopy.ColumnMappings.Add("User_Name1", "User_Name");
                   bulkCopy.ColumnMappings.Add("User_Password1", "User_Password");
                   bulkCopy.ColumnMappings.Add("User_Others1", "User_Others");
                   bulkCopy.WriteToServer(tableB);
               }

            MessageBox.Show("成功导入数据：" + lines.Count() + "条");

        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            string dbStr = ConfigurationManager.ConnectionStrings["dbCS"].ConnectionString;
            string site = txtSite.Text;
            string result = "";
            if(site.Length<=0)
            {
                MessageBox.Show("要找哪里的账号和密码呢？");
                return;
            }
            using (SqlConnection conn = new SqlConnection(dbStr))
            {
                conn.Open();
                using(SqlCommand cmd=conn.CreateCommand())
                {
                    cmd.CommandText = "select * from T_UserNameAndPassword where Site like @Name";
                    cmd.Parameters.Add(new SqlParameter("@Name", '%' + site + '%'));
                   // cmd.ExecuteScalar();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    DataTable table = dataset.Tables[0];
                    DataRowCollection rows = table.Rows;
                    if(rows.Count<=0)
                    {
                        MessageBox.Show("数据库里没有存这个地方的用户名哟");
                        return;
                    }
                    if(rows.Count>=1)
                    {
                        foreach(DataRow row in rows)
                        {
                            string dbusername = (string)row["User_Name"];
                            string dbsite = (string)row["Site"];
                            string dbpassword = (string)row["User_Password"];
                            string dbothers = (string)row["User_Others"];
                            result += "在 " + dbsite + "\n用户名：" + dbusername + "\n密码：" + dbpassword + "\n备注："+dbothers+"\n";
                        }
                    }
                }
            }
            MessageBox.Show(result);
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addwin = new AddWindow();
            addwin.ShowDialog();
        }

        private void btn_updata_Click(object sender, RoutedEventArgs e)
        {
            UpdataWindow updatewin = new UpdataWindow();
            updatewin.ShowDialog();
        }
    }
}
