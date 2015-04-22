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
    /// UpdataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdataWindow : Window
    {
        private List<Info> list;
        public UpdataWindow()
        {
            list = new List<Info>();
            InitializeComponent();
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(App.dbStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from T_UserNameAndPassword where Site like @Name";
                    cmd.Parameters.Add(new SqlParameter("@Name", '%' + txtSearch.Text + '%'));
                    // cmd.ExecuteScalar();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    DataTable table = dataset.Tables[0];
                    DataRowCollection rows = table.Rows;
                    if (rows.Count <= 0)
                    {
                        MessageBox.Show("数据库里没有存这个地方的用户名哟");
                        return;
                    }
                    if (rows.Count >= 1)
                    {
                        foreach (DataRow row in rows)
                        {
                            string dbusername = (string)row["User_Name"];
                            string dbsite = (string)row["Site"];
                            list.Add(new Info() { infoName = dbusername, infoSite = dbsite });
                        }
                    }
                    lbData.ItemsSource = list;
                }
            }
        }

        private void btn_Yes_Click(object sender, RoutedEventArgs e)
        {
            string dbStr = ConfigurationManager.ConnectionStrings["dbCS"].ConnectionString;
            string newpw = txtUpdate.Text;
            Info info = (Info)lbData.SelectedItem;
            //MessageBox.Show(info.info_name);
            //MessageBox.Show(info.info_site);
            using (SqlConnection conn = new SqlConnection(dbStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update T_UserNameAndPassword set User_Password=@New where Site=@upSite and User_Name=@upName";
                    cmd.Parameters.Add(new SqlParameter("@New", newpw));
                    cmd.Parameters.Add(new SqlParameter("@upSite", info.infoSite));
                    cmd.Parameters.Add(new SqlParameter("@upName", info.infoName));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("修改成功~");
                }
            }
        }
    }
}
