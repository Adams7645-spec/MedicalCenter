using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Data.SqlClient;

namespace MedicalCenter
{
    /// <summary>
    /// Interaction logic for AdmissionWindow.xaml
    /// </summary>
    public partial class AdmissionWindow : UserControl
    {
        static private List<string> patients = new List<string>();
        static private (int, string) selectedPatient = (0, null);
        DataTable dataCollection = null;
        public AdmissionWindow()
        {
            InitializeComponent();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            patients.Clear();
            SqlCommand command = new SqlCommand("Select patient_id, name_ from Patient", MainWindow.connection);
            SqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                patients.Add(reader.GetString(1));
            }
            reader.Close();

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = patients;
        }
        private void UpdateDataGrid()
        {
            SqlCommand command = new SqlCommand("Alter index all on Cards rebuild Select disease_history_id as ID, Patient.name_ as Name, date_ as Date, diagnosis, recommendations as guidance from Disease_history inner join Patient on Disease_history.patient_id = Patient.patient_id", MainWindow.connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataCollection = dataTable;
            AdmissinDataGrid.ItemsSource = dataTable.DefaultView;
        }
        private void AdmissinDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"insert into Disease_history (patient_id, date_, diagnosis, recommendations) values ({selectedPatient.Item1}, '2000-01-01', '{DiagTextBox.Text}', '{RecomTextBox.Text}')", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }

        private void PatientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            selectedPatient = (comboBox.SelectedIndex, patients[comboBox.SelectedIndex]);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"update Disease_history set diagnosis = '{DiagTextBox.Text}', recommendations = {RecomTextBox.Text} where patient_id = {selectedPatient.Item1} alter index all on Cards rebuild", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }
    }
}
