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
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace MedicalCenter
{
    /// <summary>
    /// Interaction logic for MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : Window
    {
        public MainAppWindow()
        {
            InitializeComponent();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userNameLabel_textblock.Text = MainWindow.selectedUser.Item2;
        }

        private void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoginWindow.Visibility = Visibility.Visible;
            this.Hide();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            userNameLabel_textblock.Text = MainWindow.selectedUser.Item2;
        }

        private void CardStorageButton_Click(object sender, RoutedEventArgs e)
        {
            CardStorageWindow storageWindow = new CardStorageWindow();
            childeWindowContainer.Content = storageWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdmissionWindow admissionWindow = new AdmissionWindow();
            childeWindowContainer.Content = admissionWindow;
        }

        private void PatientRegistrationButton(object sender, RoutedEventArgs e)
        {
            PatientRegistrationWindow patientRegistration = new PatientRegistrationWindow();
            childeWindowContainer.Content = patientRegistration;
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            SheduleWindow sheduleWindow = new SheduleWindow();
            childeWindowContainer.Content = sheduleWindow;
        }
    }
}
