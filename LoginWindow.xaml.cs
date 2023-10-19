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
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace MedicalCenter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SqlConnection connection { private set; get; }
        static MainAppWindow mainWindow;
        static private List<string> doctors = new List<string>();
        static public (int, string) selectedUser { private set; get; }
        static public MainWindow LoginWindow { private set; get; }
        public MainWindow()
        {
            OpenSQLConnection();

            InitializeComponent();
            mainWindow = new MainAppWindow();
            LoginWindow = this;
        }
        private void OpenSQLConnection()
        {
            connection = new SqlConnection(@"Data Source=(localdb)\LocalForLabs;Initial Catalog=MedicalCenter;Integrated Security=True");
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void DoctorsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("Select vrach_id, name_ from Vrach", connection);
            SqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                doctors.Add(reader.GetString(1));
            }
            reader.Close(); 

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = doctors;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorsComboBox.SelectedIndex != -1)
            {
                this.Visibility = Visibility.Hidden;
                selectedUser = (DoctorsComboBox.SelectedIndex, DoctorsComboBox.SelectedItem.ToString());
                mainWindow.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
