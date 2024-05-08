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
using Excel = Microsoft.Office.Interop.Excel;

namespace доставка.reports
{
    /// <summary>
    /// Логика взаимодействия для reportEmplloyee.xaml
    /// </summary>
    public partial class reportEmplloyee : Page
    {
        Excel.Application excelapp;
        Excel.Workbook workBook;
        List<Order_Employee> listorders;
        List<Employee> startvalue;
        List<Employee> cbsort;
        public string user;
        public Branch branch;


        public reportEmplloyee(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            if (user != "Администратор")
            {
                this.branch = branch;
                startvalue = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
                cbsort = MainWindow.baza.Employee.Where(p => p.Branch.Adress == branch.Adress).ToList();
                cbBranch.Visibility = Visibility.Hidden;
                tbBranch.Visibility = Visibility.Hidden;
            }
            else
            {
                startvalue = MainWindow.baza.Employee.ToList();
                cbsort = MainWindow.baza.Employee.ToList();
                cbBranch.ItemsSource = MainWindow.baza.Branch.ToList();
            }
            cbJob.ItemsSource = MainWindow.baza.Job_Position.ToList();
            listorders = MainWindow.baza.Order_Employee.ToList();
            excelapp = new Excel.Application();
        }

        public void CBFiltres()
        {
            Branch selectedBranch = cbBranch.SelectedItem as Branch;
            Job_Position selectedJob = cbJob.SelectedItem as Job_Position;

            var sort = startvalue.ToList();

            if (cbBranch.SelectedItem != null)
            {
                sort = Enumerable.Where(startvalue, p => p.Branch.Adress == selectedBranch.Adress).ToList();
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
                sort = Enumerable.Where(startvalue, p => p.Job_Position.Name == selectedJob.Name).ToList();
                if (cbBranch.SelectedItem != null)
                {
                    cbsort = sort.Where(p => p.Branch.Adress == selectedBranch.Adress).ToList();
                }
                else
                {
                    cbsort = sort;
                }
            }
            else cbsort = startvalue;
        }


        private void cbBranch_Selected(object sender, SelectionChangedEventArgs e)
        {
            CBFiltres();
        }

        private void cbJob_Selected(object sender, SelectionChangedEventArgs e)
        {
            CBFiltres();
        }

        public void createExcel()
        {
            workBook = excelapp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            int row = 1;


            for (int i = 0; i < cbsort.Count; i++)
            {
                var allOrders = listorders.Where(t => t.ID_Employee == cbsort[i].ID_Employee).ToList();


                workSheet.Cells[row, 1] = $"Сотрудник: {cbsort[i].LName} {cbsort[i].FName} {cbsort[i].Patronymic}";
                workSheet.Cells[row, 2] = $"Должность: {cbsort[i].Job_Position.Name}";
                workSheet.Cells[row, 1].Font.Bold = true;
                row++;

                workSheet.Cells[row, 1] = "№ заказ";
                workSheet.Cells[row, 2] = "Дата";


                Excel.Range rangeTitle = workSheet.Range[$"A{row}:B{row}"];

                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbOrange;

                row++;
                for (int k = 0, col = 1; k < allOrders.Count; k++, row++)
                {
                    workSheet.Cells[row, col] = allOrders[k].ID_Order;
                    col++;
                    workSheet.Cells[row, col] = allOrders[k].Order.Date;
                    col = 1;
                }
                workSheet.Cells[row, 1] = "Итого заказов:";
                workSheet.Cells[row, 2] = allOrders.Count;
                rangeTitle = workSheet.Range[$"A{row}:B{row}"];
                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbYellow;
                row++;
            }
            Excel.Range range = workSheet.Range[$"A{2}:B{row}"];

            range.EntireColumn.AutoFit();
            range.EntireRow.AutoFit();

        }


        private void Excel1_Click(object sender, RoutedEventArgs e)
        {
            createExcel();
            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void SortReset(object sender, RoutedEventArgs e)
        {
            cbBranch.SelectedItem = null;
            cbJob.SelectedItem = null;
            cbsort = startvalue;
        }
    }
}
