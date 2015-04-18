using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace 用户名助记
{
    /// <summary>
    /// AddWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddSite.Text == "")
            {
                MessageBox.Show("记得写地点哟");
                return;

            }
            if (txtAddName.Text == "")
            {
                MessageBox.Show("记得写用户名哟");
                return;

            }
            if (pwAddPassword.Password == "")
            {
                MessageBox.Show("密码呢？？");
                return;
            }
               
             string dbStr = ConfigurationManager.ConnectionStrings["dbCS"].ConnectionString;
            //与数据库对比，若不存在，则添加，否则返回
            using(SqlConnection conn =new  SqlConnection(dbStr) )
            {
                conn.Open();
                using(SqlCommand cmd=conn.CreateCommand())
                {
                    cmd.CommandText = "select * from T_UserNameAndPassword where Site=@Site and User_Name=@Username";
                    cmd.Parameters.Add(new SqlParameter("@Site", txtAddSite.Text));
                    cmd.Parameters.Add(new SqlParameter("@Username", txtAddName.Text));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    DataTable table = dataset.Tables[0];
                    DataRowCollection rows = table.Rows;
                    if(rows.Count>=1)
                    {
                        MessageBox.Show("这条信息添加过了~");
                        return;
                    }
                    if(rows.Count<=0)
                    {
                       using( SqlCommand cmd2=conn.CreateCommand())
                       {
                           cmd2.CommandText = "insert into T_UserNameAndPassword(Site,User_Name,User_Password,User_Others) values(@site,@username,@userpassword,@userothers)";
                           cmd2.Parameters.Add(new SqlParameter("@site", txtAddSite.Text));
                           cmd2.Parameters.Add(new SqlParameter("@username", txtAddName.Text));
                           cmd2.Parameters.Add( new SqlParameter("@userpassword", pwAddPassword.Password));
                           cmd2.Parameters.Add(new SqlParameter("@userothers", txtAddOther.Text));
                           cmd2.ExecuteNonQuery();
                       }
                       MessageBox.Show("添加成功~");
                       
                    }
                }
            }
        }
    }
}
