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
    /// Логика взаимодействия для BranchPage.xaml
    /// </summary>
    public partial class BranchPage : Page
    {
        public string user;
        public Branch branch;


        List<Branch> tbsort;
        public BranchPage(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            this.branch = branch;

            dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
            tbsort = MainWindow.baza.Branch.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var deletedBrannch = dgpizza.SelectedItem as Branch;

            if (deletedBrannch != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Branch.Remove(deletedBrannch);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgpizza.ItemsSource = null;
                    dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
                }

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedBranch = editButton.DataContext as Branch;

            adding.addBranch edit = new adding.addBranch(selectedBranch);
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
            edit.Title = "Окно  редактирования: Филиал";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
        }

        private int currentPage = 1;
        private int countPosition = 10;
        private int maxPages;
        private void Refresh()
        {
            var listPizza = tbsort;
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
            if (seek.Text != "")
                tbsort = Enumerable.Where(MainWindow.baza.Branch.ToList(), n => n.Adress.Contains(seek.Text)).ToList();
            else
                tbsort = MainWindow.baza.Branch.ToList();
            Refresh();
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Branch branch = new Branch();
            adding.addBranch edit = new adding.addBranch(branch);
            edit.Title = "Окно добавления: Филиал";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            reportWindow reportWindow = new reportWindow(branch, user);
            reportWindow.cbReports.SelectedItem = reportWindow.cbReports.Items[3];
            reportWindow.Width = 850;
            reportWindow.Height = 550;
            reportWindow.Title = "Отчет по филиалам";
            reportWindow.ShowDialog();
        }

        private void SortReset(object sender, RoutedEventArgs e)
        {
            seek.Text = null;
            tbSearch.Visibility = Visibility.Visible;
            dgpizza.ItemsSource = MainWindow.baza.Branch.ToList();
        }
    }
}
