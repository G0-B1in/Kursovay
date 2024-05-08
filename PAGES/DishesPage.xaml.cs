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
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page
    {
        List<Menu> cbdishes;
        List<Menu> tbdishes;
        public string user;
        public Branch branch;

        public DishesPage(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            this.branch = branch;
            tbdishes = MainWindow.baza.Menu.ToList();
            cbdishes = MainWindow.baza.Menu.ToList();
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
            cbCategory.ItemsSource = MainWindow.baza.Category.ToList();
            if(user != "Администратор")
            {
                btRaport.Visibility = Visibility.Hidden;                
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {            
            var delButton = sender as Button;
            var deletedDish = delButton.DataContext as Menu;

            if (deletedDish != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Menu.Remove(deletedDish);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgpizza.ItemsSource = null;
                    dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
                }

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedDish = editButton.DataContext as Menu;

            adding.addDish edit = new adding.addDish(selectedDish);
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
            edit.Title = "Окно редактирования: Блюдо";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
        }

        private int currentPage = 1;
        private int countPosition = 10;
        private int maxPages;

        private void Refresh()
        {
            var listPizza = cbdishes;
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

        public void CBFiltres()
        {
            Category selectedCountry = cbCategory.SelectedItem as Category;
            if (cbCategory.SelectedItem != null)
            {
                cbdishes = Enumerable.Where(tbdishes, p => p.Category.ID_Category == selectedCountry.ID_Category).ToList();
            }
            else
            {
                cbdishes = tbdishes;
            }
            dgpizza.ItemsSource = cbdishes;
        }

        public void tbsearch()
        {
            if (seek.Text == "")
            {
                tbdishes = MainWindow.baza.Menu.ToList();
            }
            else
            {
                tbdishes = Enumerable.Where(MainWindow.baza.Menu.ToList(), n => n.Name.Contains(seek.Text)).ToList();

            }
            CBFiltres();
        }

        private void cbType_Selected(object sender, SelectionChangedEventArgs e)
        {
            tbCategory.Visibility = Visibility.Hidden;
            CBFiltres();
        }

        private void CategorySortReset(object sender, RoutedEventArgs e)
        {
            cbCategory.SelectedItem = null;
            seek.Text = null;
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            adding.addDish edit = new adding.addDish(menu);
            edit.Title = "Окно добавления: Блюдо";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            tbSearch.Visibility = Visibility.Visible;
            tbCategory.Visibility = Visibility.Visible;
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
        }

        private void Excel_CClick(object sender, RoutedEventArgs e)
        {
            reportWindow reportWindow = new reportWindow(branch, user);
            reportWindow.cbReports.SelectedItem = reportWindow.cbReports.Items[4];
            reportWindow.Title = "Отчет по блюдам";
            reportWindow.ShowDialog();
        }
    }
}
