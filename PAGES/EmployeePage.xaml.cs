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
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {

        List<Employee> cbsort;
        List<Employee> tbsort;
        List<Employee> startvalue;
        public string user;
        public Branch branch;

        public EmployeePage(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            if (user != "Администратор")
            {
                tbsort = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
                cbsort = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
                startvalue = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
                dgpizza.ItemsSource = startvalue;
                this.branch = branch;
                cbJob.ItemsSource = MainWindow.baza.Job_Position.ToList();
                cbBranch.Visibility = Visibility.Hidden;
                tbBranch.Visibility = Visibility.Hidden;
            }
            else
            {
                tbsort = MainWindow.baza.Employee.ToList();
                cbsort = MainWindow.baza.Employee.ToList();
                dgpizza.ItemsSource = MainWindow.baza.Employee.ToList();
                startvalue = MainWindow.baza.Employee.ToList();
                cbJob.ItemsSource = MainWindow.baza.Job_Position.ToList();
                cbBranch.ItemsSource = MainWindow.baza.Branch.ToList();
            }

           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var delButton = sender as Button;
            var deletedEmp = delButton.DataContext as Employee;

            if (deletedEmp != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Employee.Remove(deletedEmp);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgpizza.ItemsSource = null;
                    dgpizza.ItemsSource = MainWindow.baza.Employee.ToList();
                }

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedEmp = editButton.DataContext as Employee;

            adding.addEmployee edit = new adding.addEmployee(selectedEmp);
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = MainWindow.baza.Employee.ToList();
            edit.Title = "Окно редактирования: Сотрудник";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = startvalue;
        }

        private int currentPage = 1;
        private int countPosition = 10;
        private int maxPages;
        private void Refresh()
        {
            var listPizza = cbsort;
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

        private void cbJob_Selected(object sender, SelectionChangedEventArgs e)
        {
            tbJob.Visibility = Visibility.Hidden;
            CBFiltres();
        }

        private void cbBranch_Selected(object sender, SelectionChangedEventArgs e)
        {
            tbBranch.Visibility = Visibility.Hidden;
            CBFiltres();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbSearch.Visibility = Visibility.Hidden;
            tbsearch();
        }

        private void CategorySortReset(object sender, RoutedEventArgs e)
        {
            cbBranch.SelectedItem = null;
            cbJob.SelectedItem = null;
            seek.Text = null;
            dgpizza.ItemsSource = startvalue;
            cbsort = startvalue;
            tbsort = startvalue;
            tbBranch.Visibility = Visibility.Visible;
            tbJob.Visibility = Visibility.Visible;
            tbSearch.Visibility = Visibility.Visible;
        }

        public void tbsearch()
        {
            if (seek.Text == "" | seek.Text == null)
            {
                tbsort = startvalue;
            }
            else
            {
                tbsort = Enumerable.Where(startvalue, n => n.FName.Contains(seek.Text) | n.LName.Contains(seek.Text) | n.Patronymic.Contains(seek.Text) | n.FullName.Contains(seek.Text)).ToList();
            }
            CBFiltres();
        }

        public void CBFiltres()
        {
            Branch selectedBranch = cbBranch.SelectedItem as Branch;
            Job_Position selectedJob = cbJob.SelectedItem as Job_Position;

            var sort = tbsort.ToList();

            if (cbBranch.SelectedItem != null)
            {
                sort = Enumerable.Where(tbsort, p => p.Branch.Adress == selectedBranch.Adress).ToList();
                if (cbJob.SelectedItem != null)
                {
                    cbsort = sort.Where(p => p.Job_Position.Name == selectedJob.Name).ToList();
                }
                else
                {
                    cbsort = sort;
                }
            }
            else if (cbJob.SelectedItem != null)
            {
                sort = Enumerable.Where(tbsort, p => p.Job_Position.Name == selectedJob.Name).ToList();
                if (cbBranch.SelectedItem != null)
                {
                    cbsort = sort.Where(p => p.Branch.Adress == selectedBranch.Adress).ToList();
                }
                else
                {
                    cbsort = sort;
                }
            }
            else cbsort = tbsort;



            dgpizza.ItemsSource = cbsort;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee();
            adding.addEmployee edit = new adding.addEmployee(employee);
            edit.Title = "Окно добавления: Сотрудник";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            if (user != "Администратор")
            {
                dgpizza.ItemsSource = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
            }
            else
            {
                dgpizza.ItemsSource = MainWindow.baza.Employee.ToList();
            }
        }

        private void Excel1_Click(object sender, RoutedEventArgs e)
        {
            reportWindow reportWindow = new reportWindow(branch, user);
            reportWindow.cbReports.SelectedItem = reportWindow.cbReports.Items[1];
            reportWindow.Title = "Отчет по сотрудникам";
            reportWindow.ShowDialog();
        }
    }
}
