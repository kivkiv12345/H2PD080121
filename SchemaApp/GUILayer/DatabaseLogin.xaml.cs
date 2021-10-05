using MySql.Data.MySqlClient;
using SchemaClasses;
using System;
using System.Collections.Generic;
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

namespace GUILayer
{
    /// <summary>
    /// Interaction logic for DatabaseLogin.xaml
    /// </summary>
    class ConnectionObject
    {
        public string Host { get; set; } = "localhost";
        public string Port { get; set; } = "3306";
        public string User { get; set; } = "root";
        public string Password { get; set; } = "";
    }

    public partial class DatabaseLogin : UserControl
    {
        ConnectionObject connObject = new ConnectionObject();
        public DatabaseLogin()
        {
            InitializeComponent();
            this.DataContext = this.connObject;
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            connObject.Password = passwordTextBox.Password;  // Assign the password from the PasswordBox, as it doesn't allow for databinding.
            DatabaseManager dbmanager = new DatabaseManager(connObject.Password);
            dbmanager.username = connObject.User;
            dbmanager.host = connObject.Host;
            dbmanager.port = Convert.ToInt32(connObject.Port);
            DataController.DBManager = dbmanager;
            // TODO Kevin: Check that the connection works, before enabling the editor stuff.
            MySqlConnection conn = new MySqlConnection(dbmanager.ConnectionString);
            try
            {
                conn.Open();
                ((MainWindow)((DockPanel)((ContentArea)this.Parent).Parent).Parent).editorButtonStack.IsEnabled = true;
                MessageBox.Show("Connection successfully established.\nControls have been enabled.\nWelcome!", "Connection Successful!");
            }
            catch (MySqlException error)
            {
                ((MainWindow)((DockPanel)((ContentArea)this.Parent).Parent).Parent).editorButtonStack.IsEnabled = false;
                MessageBox.Show(error.Message, "Connection failed!");
            }
            finally
            {
                conn.Close();
            }
            
        }
    }
}
