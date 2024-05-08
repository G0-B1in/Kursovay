using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;


namespace доставка.reports
{
    /// <summary>
    /// Логика взаимодействия для reportBranch.xaml
    /// </summary>
    public partial class reportBranch : Page
    {

        Excel.Application excelapp;
        Excel.Workbook workBook;
        List<Branch> listbranches;
        List<Order> listorders;

        public reportBranch()
        {
            InitializeComponent();

            excelapp = new Excel.Application();
            listbranches = MainWindow.baza.Branch.ToList();
            listorders = MainWindow.baza.Order.ToList();
            chartBranches.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Филиалы")
            {
                IsValueShownAsLabel = true
            };
            chartBranches.Series.Add(currentSeries);
        }

        public void createExcel()
        {
            workBook = excelapp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            int row = 1;

            workSheet.Cells[row, 1] = "Филиал";
            workSheet.Cells[row, 2] = "Выручка, руб.";
            Excel.Range rangeTitle = workSheet.Range[$"A{row}:B{row}"];
            rangeTitle.Interior.Color = Excel.XlRgbColor.rgbOrange;

            decimal itogo = 0;
            row++;
            for (int i = 0; i < listbranches.Count; i++)
            {
                var allOrders = listorders.Where(t => t.ID_Branch == listbranches[i].ID_branch).Sum(p => p.Price);

                workSheet.Cells[row, 1] = listbranches[i].Adress;
                workSheet.Cells[row, 2] = allOrders;
                rangeTitle = workSheet.Range[$"A{row}:B{row}"];
                rangeTitle.EntireColumn.AutoFit();
                rangeTitle.EntireRow.AutoFit();
                row++;
                itogo += allOrders;
            }

            workSheet.Cells[row, 1] = "Итого:";
            workSheet.Cells[row, 2] = itogo.ToString("F2");
            rangeTitle = workSheet.Range[$"A{1}:B{row}"];
            rangeTitle.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

           

        }


        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            createExcel();
            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Series currentSeries = chartBranches.Series.FirstOrDefault();
            currentSeries.ChartType = SeriesChartType.Pie;
            currentSeries.Points.Clear();

            for (int i = 0; i < listbranches.Count; i++)
            {
                var allOrders = listorders.Where(t => t.ID_Branch == listbranches[i].ID_branch).Sum(p => p.Price);
                currentSeries.Points.AddXY(listbranches[i].Adress, allOrders);
            }
        }
    }
}
