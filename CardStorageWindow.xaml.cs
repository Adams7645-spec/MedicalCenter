using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace MedicalCenter
{
    /// <summary>
    /// Interaction logic for CardStorageWindow.xaml
    /// </summary>
    public partial class CardStorageWindow : UserControl
    {
        static private int selectedID = 0;
        static private List<string> patients = new List<string>();
        static private (int, string) selectedPatient = (0, null);
        DataTable dataCollection = null;
        public CardStorageWindow()
        {
            InitializeComponent();
        }

        private void CardStorageDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            SqlCommand command = new SqlCommand("Alter index all on Cards rebuild Select cards_id as CardID, Patient.name_ as PatientName from Cards inner join Disease_history on Cards.disease_history_id = Disease_history.disease_history_id inner join Patient on Patient.patient_id = Disease_history.patient_id", MainWindow.connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataCollection = dataTable;
            CardStorageDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"delete from Cards where cards_id = {selectedID} alter index all on Cards rebuild", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }

        private void CreateCardButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"insert into Cards (disease_history_id) values ({selectedPatient.Item1})", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }

        private void CardStorageDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CardStorageDataGrid.SelectedItem != null && CardStorageDataGrid.SelectedItems.Count == 1)
            {
                DataRowView rowView = (DataRowView) CardStorageDataGrid.SelectedItem;
                DataRow row = rowView.Row;
                object columnData = row["CardID"];
                selectedID = (int) columnData;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                UpdateDataGrid();
            }
            else
            {
                dataCollection.DefaultView.RowFilter = $"PatientName LIKE '%{searchText}%'";
                CardStorageDataGrid.ItemsSource = dataCollection.DefaultView;
            }
        }

        private void PatientsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT dh.disease_history_id AS ID, p.name_ AS Name FROM Disease_History dh INNER JOIN Patient p ON dh.patient_id = p.patient_id WHERE dh.disease_history_id NOT IN(SELECT disease_history_id FROM Cards)", MainWindow.connection);
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

        private void PatientsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            selectedPatient = (comboBox.SelectedIndex, patients[comboBox.SelectedIndex]);
        }
    }
}
