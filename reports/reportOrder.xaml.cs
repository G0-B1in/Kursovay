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
    /// Логика взаимодействия для reportOrder.xaml
    /// </summary>
    public partial class reportOrder : Page
    {
        Excel.Application excelapp;
        Excel.Workbook workBook;
        List<Order> startvalue;
        List<Order> dtsort;
        public string user;
        public Branch branch;
        List<OrderStatus> listOrderStatuses;

        public reportOrder(Branch branch, string user)
        {
            InitializeComponent();
            this.user = user;
            if (user != "Администратор")
            {
                this.branch = branch;
                startvalue = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
                dtsort = MainWindow.baza.Order.Where(p => p.Branch.Adress == branch.Adress).ToList();
            }
            else
            {
                startvalue = MainWindow.baza.Order.ToList();
                dtsort = MainWindow.baza.Order.ToList();
            }

            excelapp = new Excel.Application();
            listOrderStatuses = MainWindow.baza.OrderStatus.ToList();

        }

        public void DateFiltres()
        {
            DateTime start = datestart.DisplayDate;
            DateTime end = dateend.DisplayDate;

            if (datestart.SelectedDate == null && dateend.SelectedDate == null)
            {
                dtsort = startvalue;
            }
            else
            {
                dtsort = startvalue.Where(p => p.Date >= start & p.Date <= end).ToList();
            }

        }

        public void createExcel()
        {
            workBook = excelapp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            int row = 1;


            for (int i = 0; i < listOrderStatuses.Count; i++)
            {
                var allOrders = dtsort.Where(t => t.OrderStatus.ID_Status == listOrderStatuses[i].ID_Status).ToList();


                workSheet.Cells[row, 1] = $"Статус заказа: {listOrderStatuses[i].Name}";
                workSheet.Cells[row, 1].Font.Bold = true;
                row++;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Клиент";
                workSheet.Cells[row, 3] = "Дата";
                workSheet.Cells[row, 4] = "Стоимость";


                Excel.Range rangeTitle = workSheet.Range[$"A{row}:D{row}"];

                rangeTitle.EntireColumn.AutoFit();
                rangeTitle.EntireRow.AutoFit();
                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbOrange;

                row++;
                if (allOrders.Count != 0)
                {
                    for (int k = 0, col = 1; k < allOrders.Count; k++, row++)
                    {
                        workSheet.Cells[row, col] = allOrders[k].ID_Order;
                        col++;
                        workSheet.Cells[row, col] = allOrders[k].Client.FullName;
                        col++;
                        workSheet.Cells[row, col] = allOrders[k].Date;
                        col++;
                        workSheet.Cells[row, col] = allOrders[k].Price;
                        col = 1;
                    }
                    workSheet.Cells[row, 1] = "Итого заказов:";
                    workSheet.Cells[row, 4] = $"{allOrders.Count} на сумму {allOrders.Sum(p => p.Price).ToString("F2")}";
                    rangeTitle = workSheet.Range[$"A{row}:D{row}"];
                    rangeTitle.Interior.Color = Excel.XlRgbColor.rgbYellow;
                }
                else
                {
                    workSheet.Cells[row, 1] = "Нет заказов";
                }
                row++;
                row++;
            }
        }


        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            createExcel();
            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void datestart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
        }

        private void dateend_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
        }

        int weekStard;
        int weekEnd;
        DateTime now = DateTime.Now;

        public void WhatADayOfWeeh()
        {
            if (now.DayOfWeek == DayOfWeek.Monday)
            {
                weekStard = now.DayOfYear;
                weekEnd = now.DayOfYear + 6;
            }
            else if (now.DayOfWeek == DayOfWeek.Tuesday)
            {
                weekStard = now.DayOfYear - 1;
                weekEnd = now.DayOfYear + 5;
            }
            else if (now.DayOfWeek == DayOfWeek.Wednesday)
            {
                weekStard = now.DayOfYear - 2;
                weekEnd = now.DayOfYear + 4;
            }
            else if (now.DayOfWeek == DayOfWeek.Thursday)
            {
                weekStard = now.DayOfYear - 3;
                weekEnd = now.DayOfYear + 3;
            }
            else if (now.DayOfWeek == DayOfWeek.Friday)
            {
                weekStard = now.DayOfYear - 4;
                weekEnd = now.DayOfYear + 2;
            }
            else if (now.DayOfWeek == DayOfWeek.Saturday)
            {
                weekStard = now.DayOfYear - 5;
                weekEnd = now.DayOfYear + 1;
            }
            else
            {
                weekStard = now.DayOfYear - 6;
                weekEnd = now.DayOfYear;
            }

        }

        int StartMonth;
        int EndMonth;
        public void WhatAMonthOfQuarter()
        {
            if(now.Month >= 1 & now.Month <= 3)
            {
                if(now.Month == 1)
                {
                    StartMonth = now.Month;
                    EndMonth = now.Month + 2;
                }
                else if (now.Month == 2)
                {
                    StartMonth = now.Month - 1;
                    EndMonth = now.Month + 1;
                }
                else
                {
                    StartMonth = now.Month -2;
                    EndMonth = now.Month;
                }
            }
            else if (now.Month >= 4 & now.Month <= 6)
            {
                if (now.Month == 4)
                {
                    StartMonth = now.Month;
                    EndMonth = now.Month + 2;
                }
                else if (now.Month == 5)
                {
                    StartMonth = now.Month - 1;
                    EndMonth = now.Month + 1;
                }
                else
                {
                    StartMonth = now.Month - 2;
                    EndMonth = now.Month;
                }
            }
            else if (now.Month >= 7 & now.Month <= 9)
            {
                if (now.Month == 7)
                {
                    StartMonth = now.Month;
                    EndMonth = now.Month + 2;
                }
                else if (now.Month == 8)
                {
                    StartMonth = now.Month - 1;
                    EndMonth = now.Month + 1;
                }
                else
                {
                    StartMonth = now.Month - 2;
                    EndMonth = now.Month;
                }
            }
            else
            {
                if (now.Month == 10)
                {
                    StartMonth = now.Month;
                    EndMonth = now.Month + 2;
                }
                else if (now.Month == 11)
                {
                    StartMonth = now.Month - 1;
                    EndMonth = now.Month + 1;
                }
                else
                {
                    StartMonth = now.Month - 2;
                    EndMonth = now.Month;
                }
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(cbDate.SelectedItem == cbDate.Items[0])//за текущий день
            {
                other.Visibility = Visibility.Hidden;
                dtsort = startvalue.Where(p => p.Date == now).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[1])// за текущую неделю
            {
                other.Visibility = Visibility.Hidden;
                WhatADayOfWeeh();
                dtsort = startvalue.Where(p => p.Date.DayOfYear >= weekStard & p.Date.DayOfYear <= weekEnd & p.Date.Year == now.Year).ToList();

            }
            else if (cbDate.SelectedItem == cbDate.Items[2])// за текущий месяц
            {
                other.Visibility = Visibility.Hidden;
                dtsort = startvalue.Where(p => p.Date.Month == now.Month & p.Date.Year == now.Year).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[3])// за текущий квартал
            {
                other.Visibility = Visibility.Hidden;
                WhatAMonthOfQuarter();
                dtsort = startvalue.Where(p => p.Date.Month >= StartMonth & p.Date.Month <= EndMonth & p.Date.Year == now.Year).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[4])// за текущий год
            {
                other.Visibility = Visibility.Hidden;
                dtsort = startvalue.Where(p => p.Date.Year == now.Year).ToList();
            }            
            else // произвольная дата 
            {
                other.Visibility = Visibility.Visible;
            }
        }

        private void SortReset(object sender, RoutedEventArgs e)
        {
            cbDate.SelectedItem = null;
            dateend.SelectedDate = null;
            datestart.SelectedDate = null;
            dateend.Visibility = Visibility.Hidden;
            datestart.Visibility = Visibility.Hidden;
            dtsort = startvalue;
        }
    }
}
