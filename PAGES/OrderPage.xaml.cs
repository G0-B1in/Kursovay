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
using word = Microsoft.Office.Interop.Word;



namespace доставка.PAGES
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Order> tbsort;
        List<Order> dtsort;
        List<Order> chbsort;
        List<Order> startvalue;
        List<OrderStatus> listOrderStatuses;
        public string user;
        public Branch branch;

        word.Application appWord = new word.Application();
        List<OrderDish> listorderdishes;
        List<Order_Employee> listorder_Employees;

        public OrderPage(Branch branch, string user)
        {
            InitializeComponent();

            this.user = user;
            if (user != "Администратор")
            {
                this.branch = branch;
                tbsort = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
                dtsort = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
                chbsort = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
                startvalue = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
            }
            else
            {
                tbsort = MainWindow.baza.Order.ToList();
                dtsort = MainWindow.baza.Order.ToList();
                chbsort = MainWindow.baza.Order.ToList();
                startvalue = MainWindow.baza.Order.ToList();
            }

            dgpizza.ItemsSource = startvalue.OrderByDescending(p => p.ID_Order).ToList();

            listOrderStatuses = MainWindow.baza.OrderStatus.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var delButton = sender as Button;
            var deletedOrd = delButton.DataContext as Order;

            if (deletedOrd != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Order.Remove(deletedOrd);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgpizza.ItemsSource = null;
                    dgpizza.ItemsSource = MainWindow.baza.Order.OrderByDescending(p => p.ID_Order).ToList();
                }

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedOrd = editButton.DataContext as Order;

            adding.addOrder edit = new adding.addOrder(selectedOrd);
            edit.Title = "Окно редактирования: Заказ";
            edit.ShowDialog();
            
            dgpizza.ItemsSource = null;
            dgpizza.ItemsSource = startvalue.OrderByDescending(p => p.ID_Order).ToList();
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
            dgpizza.ItemsSource = listPizzaPage.OrderByDescending(p => p.ID_Order).ToList(); ;
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

        public void tbsearch()
        {
            if (seek.Text == "")
            {
                tbsort = startvalue;
            }
            else
            {
                tbsort = Enumerable.Where(startvalue, p => p.Price >= decimal.Parse(seek.Text)).ToList();

            }
            DateFiltres();
        }

        public void DateFiltres()
        {
            DateTime start = datestart.DisplayDate;
            DateTime end = dateend.DisplayDate;

            if (datestart.SelectedDate == null && dateend.SelectedDate == null)
            {
                dtsort = tbsort;
            }
            else
            {
                if(dateend.SelectedDate == null)
                {
                    dtsort = tbsort.Where(p => p.Date >= start).ToList();
                }
                else if(datestart.SelectedDate == null)
                {
                    dtsort = tbsort.Where(p => p.Date <= end).ToList();
                }
                else
                {
                    dtsort = tbsort.Where(p => p.Date >= start & p.Date <= end).ToList();
                }
            }

            OrdDoneSort();
        }

        public void OrdDoneSort()
        {
            if (cbdone.IsChecked == true)
            {
                chbsort = dtsort.Where(p => p.OrderStatus.Name == "Доставлен").ToList();
            }
            else
            {
                chbsort = dtsort;
            }

            dgpizza.ItemsSource = chbsort.OrderByDescending(p => p.ID_Order).ToList(); ;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbsearch();
        }

        private void cbBranch_Selected(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
        }

        private void CategorySortReset(object sender, RoutedEventArgs e)
        {
            dgpizza.ItemsSource = startvalue.OrderByDescending(p => p.ID_Order).ToList(); ;
            seek.Text = null;
            cbdone.IsChecked = false;
            dateend.SelectedDate = null;
            datestart.SelectedDate = null;
            tbsort = startvalue;
            dtsort = startvalue;
            chbsort = startvalue;
        }

        private void datestart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
            tbsearch();
        }

        private void dateend_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
            tbsearch();
        }

        private void cbdone_Checked(object sender, RoutedEventArgs e)
        {
            OrdDoneSort();
            tbsearch();
            DateFiltres();
        }

        private void cbdone_Unchecked(object sender, RoutedEventArgs e)
        {
            OrdDoneSort();
            tbsearch();
            DateFiltres();
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            adding.addOrder edit = new adding.addOrder(order);
            edit.Title = "Окно добавления: Заказ";
            edit.ShowDialog();
            dgpizza.ItemsSource = null;
            if (user != "Администратор")
                dgpizza.ItemsSource = MainWindow.baza.Order.OrderByDescending(p => p.ID_Order).Where(p => p.ID_Branch == branch.ID_branch).ToList();
            else
                dgpizza.ItemsSource = MainWindow.baza.Order.OrderByDescending(p => p.ID_Order).ToList();
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            reportWindow reportWindow = new reportWindow(branch, user);
            reportWindow.cbReports.SelectedItem = reportWindow.cbReports.Items[0];
            reportWindow.Title = "Отчет по заказам";
            reportWindow.ShowDialog();
        }

        private void Word_Click(object sender, RoutedEventArgs e)
        {
            CreateWord(sender);
        }

        public void CreateWord(object sender)
        {
            int row = 1;
            var delButton = sender as Button;
            var selectedOrder = delButton.DataContext as Order;
            listorderdishes = MainWindow.baza.OrderDish.Where(p => p.ID_Order == selectedOrder.ID_Order).ToList();
            listorder_Employees = MainWindow.baza.Order_Employee.Where(p => p.ID_Order == selectedOrder.ID_Order).ToList();
            word.Application appWord = new word.Application();
            word.Document document = appWord.Documents.Add();
            word.Paragraph tebleParagraph = document.Paragraphs.Add();
            word.Range tableRange = tebleParagraph.Range;
            word.Table orderTable = document.Tables.Add(tableRange, 10 + (listorderdishes.Count * 5), 2);
            word.Range cellRange;
            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = "Кассовый чек № " + selectedOrder.ID_Order;
            cellRange = orderTable.Cell(row, 2).Range;
            cellRange.Text = selectedOrder.Date.ToString();
            row++;
            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = "ФИЛИАЛ адрес: ";
            cellRange = orderTable.Cell(row, 2).Range;
            cellRange.Text = selectedOrder.Branch.Adress;
            row++;
            for (int i = 0; i < listorder_Employees.Count; i++)
            {
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = listorder_Employees[i].Job + ":";
                cellRange = orderTable.Cell(row, 2).Range;
                cellRange.Text = listorder_Employees[i].Employee.FullName;
                row++;
            }
            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = "ПРИХОД";
            orderTable.Rows[row].Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
            orderTable.Rows[row].Range.Font.Size = 14;
            orderTable.Cell(row, 1).Merge(orderTable.Cell(row, 2));
            orderTable.Rows[row].Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingleWavy;
            orderTable.Rows[row - 1].Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleNone;

            row++;
            for (int i = 0; i < listorderdishes.Count; i++)
            {
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = listorderdishes[i].Menu.Category.Name + " " + listorderdishes[i].Menu.Name;
                row++;
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = listorderdishes[i].Count + " x " + listorderdishes[i].Menu.Price.ToString("F2");
                cellRange = orderTable.Cell(row, 2).Range;
                cellRange.Text = listorderdishes[i].price.ToString("F2");
                row++;
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = "Без НДС";
                row++;
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = "Признак способа расчета";
                cellRange = orderTable.Cell(row, 2).Range;
                cellRange.Text = "ПОЛНЫЙ РАСЧЕТ";
                row++;
                cellRange = orderTable.Cell(row, 1).Range;
                cellRange.Text = "Признак предмета расчета";
                cellRange = orderTable.Cell(row, 2).Range;
                cellRange.Text = "ТОВАР";
                orderTable.Rows[row].Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleDouble;
                orderTable.Rows[row - 1].Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleNone;
                row++;
            }

            row++;

            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = "ИТОГО:";
            cellRange = orderTable.Cell(row, 2).Range;
            cellRange.Text = selectedOrder.NewPrice.ToString("F2");
            row++;

            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = "СКИДКА:";
            cellRange = orderTable.Cell(row, 2).Range;
            cellRange.Text = selectedOrder.Client.Discount.ToString() + "%";
            row++;

            cellRange = orderTable.Cell(row, 1).Range;
            cellRange.Text = selectedOrder.PaymentMethod.Name + ":";
            cellRange = orderTable.Cell(row, 2).Range;
            cellRange.Text = selectedOrder.NewPrice.ToString("F2");
            
            orderTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
            orderTable.Range.Font.Name = "Bahnschrift Light SemiCondensed";
            appWord.Visible = true;
            
        }

    }
}
