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
    /// Логика взаимодействия для reportDish.xaml
    /// </summary>
    public partial class reportDish : Page
    {
        Excel.Application excelapp;
        Excel.Workbook workBook;

        List<Category> listcategories;
        List<Menu> listdishes;
        List<Menu> dtsort;

        public reportDish()
        {
            InitializeComponent();

            excelapp = new Excel.Application();
            listcategories = MainWindow.baza.Category.ToList();
            listdishes = MainWindow.baza.Menu.ToList();
            dtsort = MainWindow.baza.Menu.ToList();
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
            if (now.Month >= 1 & now.Month <= 3)
            {
                if (now.Month == 1)
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
                    StartMonth = now.Month - 2;
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


        public void DateFiltres()
        {
            DateTime start = datestart.DisplayDate;
            DateTime end = dateend.DisplayDate;

            if (datestart.SelectedDate == null && dateend.SelectedDate == null)
            {
                dtsort = listdishes;
            }
            else
            {
                dtsort = listdishes.Where(d => d.OrderDish.Any(p => p.Order.Date >= start & p.Order.Date <= end)).ToList();
            }

        }

        private void datestart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
        }

        private void dateend_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFiltres();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cbDate.SelectedItem == cbDate.Items[0])//за текущий день
            {
                other.Visibility = Visibility.Hidden;
                dtsort = listdishes.Where(d => d.OrderDish.Any(p => p.Order.Date == now)).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[1])// за текущую неделю
            {
                other.Visibility = Visibility.Hidden;
                WhatADayOfWeeh();
                dtsort = listdishes.Where(d => d.OrderDish.Any(p => p.Order.Date.DayOfYear >= weekStard & p.Order.Date.DayOfYear <= weekEnd & p.Order.Date.Year == now.Year)).ToList();

            }
            else if (cbDate.SelectedItem == cbDate.Items[2])// за текущий месяц
            {
                other.Visibility = Visibility.Hidden;
                dtsort = listdishes.Where(d => d.OrderDish.Any(p => p.Order.Date.Month == now.Month & p.Order.Date.Year == now.Year)).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[3])// за текущий квартал
            {
                other.Visibility = Visibility.Hidden;
                WhatAMonthOfQuarter();
                dtsort = listdishes.Where(d => d.OrderDish.Any(p => p.Order.Date.Month >= StartMonth & p.Order.Date.Month <= EndMonth & p.Order.Date.Year == now.Year)).ToList();
            }
            else if (cbDate.SelectedItem == cbDate.Items[4])// за текущий год
            {
                other.Visibility = Visibility.Hidden;
                dtsort = listdishes.Where( d => d.OrderDish.Any(p => p.Order.Date.Year == now.Year)).ToList();
            }
            else // произвольная дата 
            {
                other.Visibility = Visibility.Visible;
            }
        }


        public void createExcel()
        {
            workBook = excelapp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            int row = 1;


            for (int i = 0; i < listcategories.Count; i++)
            {
                var allDishes = dtsort.Where(t => t.ID_category == listcategories[i].ID_Category).OrderByDescending(p => p.OrderDish.Count).ToList();


                workSheet.Cells[row, 1] = $"{listcategories[i].Name}";
                workSheet.Cells[row, 1].Font.Bold = true;
                row++;

                workSheet.Cells[row, 1] = "Блюдо";
                workSheet.Cells[row, 2] = "Кол-во заказов";


                Excel.Range rangeTitle = workSheet.Range[$"A{row}:D{row}"];

                rangeTitle.EntireColumn.AutoFit();
                rangeTitle.EntireRow.AutoFit();
                rangeTitle.Interior.Color = Excel.XlRgbColor.rgbOrange;

                row++;
                for (int k = 0, col = 1; k < allDishes.Count; k++, row++)
                {
                    workSheet.Cells[row, col] = allDishes[k].Name;
                    col++;
                    workSheet.Cells[row, col] = allDishes[k].OrderDish.Count;
                    col = 1;
                }
                row++;
            }
        }


        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            createExcel();
            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void SortReset(object sender, RoutedEventArgs e)
        {
            cbDate.SelectedItem = null;
            dateend.SelectedDate = null;
            datestart.SelectedDate = null;
            dateend.Visibility = Visibility.Hidden;
            datestart.Visibility = Visibility.Hidden;
            dtsort = listdishes;
        }
    }
}
