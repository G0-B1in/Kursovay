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

namespace доставка.PAGES
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        List<Client> chbsort;
        List<Client> tbsort;
        public string user;
        public Branch branch;


        public ClientsPage(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            this.branch = branch;
            tbsort = MainWindow.baza.Client.ToList();
            chbsort = MainWindow.baza.Client.ToList();
            dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
            if(user != "Администратор")
            {
                btReport.Visibility = Visibility.Hidden;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var deletedClient = dgpizza.SelectedItem as Client;

            if (deletedClient != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Client.Remove(deletedClient);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgpizza.ItemsSource = null;
                    dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
                }

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedClient = editButton.DataContext as Client;

            adding.addClient edit = new adding.addClient(selectedClient);
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
            edit.Title = "Окно редактирования: Клиент";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
        }

        private int currentPage = 1;
        private int countPosition = 10;
        private int maxPages;
        private void Refresh()
        {
            var listPizza = chbsort;
            maxPages = (int)Math.Ceiling(listPizza.Count * 1.0 / countPosition);

            var listPizzaPage = listPizza.Skip((currentPage - 1) * countPosition).Take(countPosition).ToList();

            TxtCurrentPage.Text = currentPage.ToString();
            LblTotalPages.Content = "из " + maxPages;
            dgpizza.ItemsSource = listPizzaPage;
        }

        private void GoToFirstPage(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Refresh();

        }

        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            if (currentPage <= 1) currentPage = 1;
            else
                currentPage--;
            Refresh();
        }

        private void GoToNextPage(object sender, RoutedEventArgs e)
        {
            if (currentPage >= maxPages) currentPage = maxPages;
            else
                currentPage++;
            Refresh();
        }

        private void GoToLastPage(object sender, RoutedEventArgs e)
        {
            currentPage = maxPages;
            Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbSearch.Visibility = Visibility.Hidden;
            tbsearch();
        }

        private void cbdiscount_Checked(object sender, RoutedEventArgs e)
        {
            tbsearch();
            CHBFilter();
        }

        private void cbdiscount_Unchecked(object sender, RoutedEventArgs e)
        {
            tbsearch();
            CHBFilter();
        }

        private void CategorySortReset(object sender, RoutedEventArgs e)
        {
            cbdiscount.IsChecked = false;
            seek.Text = null;
            tbSearch.Visibility = Visibility.Visible;
            dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
        }

        public void tbsearch()
        {
            if (seek.Text == null)
            {
                tbsort = MainWindow.baza.Client.ToList();
            }
            else
            {
                tbsort = Enumerable.Where(MainWindow.baza.Client.ToList(), n => n.FullName.Contains(seek.Text) | n.Patronymic.Contains(seek.Text) | n.LName.Contains(seek.Text) | n.FName.Contains(seek.Text) | n.Phone.Contains(seek.Text) | n.Adress.Contains(seek.Text)).ToList();
            }
            CHBFilter();
        }

        public void CHBFilter()
        {
            if(cbdiscount.IsChecked == false)
            {
                chbsort = tbsort;
            }
            else
            {
                chbsort = Enumerable.Where(tbsort, n => n.Discount != 0).ToList();
            }

            dgpizza.ItemsSource = chbsort;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Client client = new Client();
            adding.addClient edit = new adding.addClient(client);
            edit.Title = "Окно добавления: Клиент";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Client.ToList();
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            reportWindow reportWindow = new reportWindow(branch, user);
            reportWindow.cbReports.SelectedItem = reportWindow.cbReports.Items[2];
            reportWindow.Title = "Отчет по клиента";
            reportWindow.ShowDialog();
        }
    }
}
