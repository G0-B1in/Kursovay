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
    /// Логика взаимодействия для reportClient.xaml
    /// </summary>
    public partial class reportClient : Page
    {
        List<Order> listorders;
        List<Client> listclients;
        List<Client> cbsort;
        Excel.Application excelapp;
        Excel.Workbook workBook;

        public reportClient()
        {
            InitializeComponent();
            cbBranch.ItemsSource = MainWindow.baza.Branch.ToList();
            listorders = MainWindow.baza.Order.ToList();
            listclients = MainWindow.baza.Client.ToList();
            cbsort = MainWindow.baza.Client.ToList();
            excelapp = new Excel.Application();
        }

        public void createExcel()
        {
            workBook = excelapp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            int row = 1;


            for (int i = 0; i < cbsort.Count; i++)
            {
                var allOrders = listorders.Where(t => t.ID_Client == cbsort[i].ID_Client).ToList();


                workSheet.Cells[row, 1] = $"Клиент: {cbsort[i].FullName}";
                workSheet.Cells[row, 1].Font.Bold = true;
                row++;

                workSheet.Cells[row, 1] = "№ заказа";
                workSheet.Cells[row, 2] = "Дата";
                workSheet.Cells[row, 3] = "Стоимость";


                Excel.Range rangeTitle = workSheet.Range[$"A{row}:C{row}"];

                rangeTitle.EntireColumn.AutoFit();
                rangeTitle.EntireRow.AutoFit();
                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbOrange;

                row++;
                for (int k = 0, col = 1; k < allOrders.Count; k++, row++)
                {
                    workSheet.Cells[row, col] = allOrders[k].ID_Order;
                    col++;
                    workSheet.Cells[row, col] = allOrders[k].Date;
                    col++;
                    workSheet.Cells[row, col] = allOrders[k].Price;
                    col = 1;
                }
                workSheet.Cells[row, 1] = "Итого заказов:";
                workSheet.Cells[row, 3] = $"{allOrders.Count} на сумму {allOrders.Sum(p => p.Price).ToString("F2")}";
                rangeTitle = workSheet.Range[$"A{row}:C{row}"];
                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbYellow;

                row++;
            }
            Excel.Range range = workSheet.Range[$"A{1}:C{row}"];
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

        }


        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            createExcel();
            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void cbBranch_Selected(object sender, SelectionChangedEventArgs e)
        {
            CBFiltres();
        }

        public void CBFiltres()
        {
            Branch selectedBranch = cbBranch.SelectedItem as Branch;
            if (cbBranch.SelectedItem != null)
            {
                cbsort = Enumerable.Where(listclients, d => d.Order.Any(p => p.Branch.Adress == selectedBranch.Adress)).ToList();
            }
            else cbsort = listclients;
        }

        private void SortReset(object sender, RoutedEventArgs e)
        {
            cbBranch.SelectedItem = null;
            cbsort = listclients;
        }
    }
}
