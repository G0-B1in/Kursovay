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
using System.IO;

namespace доставка
{
    /// <summary>
    /// Логика взаимодействия для adminWindow.xaml
    /// </summary>
    public partial class adminWindow : Window
    {      
        public Branch branch;
        public string user;
        public adminWindow(string user)
        {
            InitializeComponent();
            branch = new Branch();
            this.user = user;
            tbuser.Text = $"{user}";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.DishesPage(branch, user));
            PressedColor(btDishes);
            RefreshStyle(btBranches);
            RefreshStyle(btClients);
            RefreshStyle(btEmployees);
            RefreshStyle(btOrders);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.BranchPage(branch, user));
            PressedColor(btBranches);
            RefreshStyle(btDishes);
            RefreshStyle(btClients);
            RefreshStyle(btEmployees);
            RefreshStyle(btOrders);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.EmployeePage(branch, user));
            PressedColor(btEmployees);
            RefreshStyle(btBranches);
            RefreshStyle(btClients);
            RefreshStyle(btDishes);
            RefreshStyle(btOrders);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.OrderPage(branch, user));
            PressedColor(btOrders);
            RefreshStyle(btBranches);
            RefreshStyle(btClients);
            RefreshStyle(btDishes);
            RefreshStyle(btEmployees);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.ClientsPage(branch, user));
            PressedColor(btClients);
            RefreshStyle(btBranches);
            RefreshStyle(btOrders);
            RefreshStyle(btDishes);
            RefreshStyle(btEmployees);
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                   "Вы точно хотете выйти из системы?",
                   "Внимание!",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        public void PressedColor(Button bt)
        {
            Brush brush = new SolidColorBrush(Color.FromRgb(244, 161, 91));
            bt.Background = brush;
            bt.FontWeight = FontWeights.Bold;            
        }

        public void RefreshStyle(Button bt)
        {
            Brush brush = new SolidColorBrush(Color.FromRgb(191, 68, 61));
            bt.Background = brush;
            bt.FontWeight = FontWeights.Normal;
        }

        private void registrationslist_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PAGES.RegistrationListPage());
            RefreshStyle(btBranches);
            RefreshStyle(btOrders);
            RefreshStyle(btDishes);
            RefreshStyle(btEmployees);
            RefreshStyle(btClients);
        }

    }
}
