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

namespace доставка
{
    /// <summary>
    /// Логика взаимодействия для reportWindow.xaml
    /// </summary>
    public partial class reportWindow : Window
    {
        public string user;
        public Branch branch;

        public reportWindow(Branch branch, string user)
        {
            InitializeComponent();
            this.branch = branch;
            this.user = user;
            if (user != "Администратор")
            {
                cbReports.IsEnabled = false;
            }
        }

        private void cbReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbReports.SelectedItem == cbReports.Items[4])
            {
                frame.NavigationService.Navigate(new reports.reportDish());
                this.Width = 500;
                this.Title = "Отчет по блюдам";
            }

            if (cbReports.SelectedItem == cbReports.Items[3])
            {
                frame.NavigationService.Navigate(new reports.reportBranch());
                this.Width = 850;                
                this.Title = "Отчет по филиалам";
            }

            if (cbReports.SelectedItem == cbReports.Items[2])
            {
                frame.NavigationService.Navigate(new reports.reportClient());
                this.Width = 500;
                this.Title = "Отчет по клиента";
            }

            if (cbReports.SelectedItem == cbReports.Items[1])
            {
                frame.NavigationService.Navigate(new reports.reportEmplloyee(branch, user));
                this.Width = 500;
                this.Title = "Отчет по сотрудникам";
            }

            if (cbReports.SelectedItem == cbReports.Items[0])
            {
                frame.NavigationService.Navigate(new reports.reportOrder(branch, user));
                this.Width = 500;
                this.Title = "Отчет по заказам";
            }
        }
    }
}
