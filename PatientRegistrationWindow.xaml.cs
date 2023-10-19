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
using System.Data;

namespace MedicalCenter
{
    /// <summary>
    /// Interaction logic for PatientRegistrationWindow.xaml
    /// </summary>
    public partial class PatientRegistrationWindow : UserControl
    {
        DataTable dataCollection = null;
        static private int selectedID = 0;
        public PatientRegistrationWindow()
        {
            InitializeComponent();
        }
        private void UpdateDataGrid()
        {
            SqlCommand command = new SqlCommand("Alter index all on Cards rebuild Select patient_id as ID, name_ as Name, age as Age, policy_ as Policy, passport as PassCode, homeAddress as Address, phoneNumber as Phone from Patient", MainWindow.connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataCollection = dataTable;
            PatientDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void PatientDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"insert into Patient (birthDate, name_, age, policy_, passport, homeAddress, phoneNumber) values ('2000-01-01', '{NameTextBox.Text}', {AgeTextBox.Text}, {PolicyTextBox.Text}, {PassCodeTextBox.Text}, '{HomeAddressTextBox.Text}', {PhoneNumberTextBox.Text})", MainWindow.connection);
            command.ExecuteNonQuery();
            NameTextBox.Clear();
            AgeTextBox.Clear();
            PolicyTextBox.Clear();
            PassCodeTextBox.Clear();
            HomeAddressTextBox.Clear();
            PhoneNumberTextBox.Clear();
            UpdateDataGrid();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"delete from Patient where patient_id = {selectedID} alter index all on Cards rebuild", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }

        private void PatientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientDataGrid.SelectedItem != null && PatientDataGrid.SelectedItems.Count == 1)
            {
                DataRowView rowView = (DataRowView)PatientDataGrid.SelectedItem;
                DataRow row = rowView.Row;
                object columnData = row["ID"];
                selectedID = (int)columnData;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"update Patient set name_ = '{NameTextBox.Text}', age = {AgeTextBox.Text}, policy_ = {PolicyTextBox.Text}, passport = {PassCodeTextBox.Text}, homeAddress = '{HomeAddressTextBox.Text}', phoneNumber = {PhoneNumberTextBox.Text}  where patient_id = {selectedID} alter index all on Cards rebuild", MainWindow.connection);
            command.ExecuteNonQuery();

            UpdateDataGrid();
        }
    }
}
