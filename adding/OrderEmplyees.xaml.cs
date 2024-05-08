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
using System.Windows.Shapes;

namespace доставка.adding
{
    /// <summary>
    /// Логика взаимодействия для OrderEmplyees.xaml
    /// </summary>
    public partial class OrderEmplyees : Window
    {
        private Order order;
        private Order_Employee orderEmployee;
        private Employee employee;
        public OrderEmplyees(Employee employee, Order order)
        {
            InitializeComponent();
            this.order = order;
            this.orderEmployee = new Order_Employee();
            this.employee = employee;
            DataContext = employee;
            dgpizza.ItemsSource = MainWindow.baza.Employee.Where(p => p.ID_Branch == order.ID_Branch).OrderByDescending(p => p.Job_Position.Name).ToList();
        }

        private void click_addEmployee(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedemployee = editButton.DataContext as Employee;
            orderEmployee.ID_Order = order.ID_Order;
            orderEmployee.ID_Employee = selectedemployee.ID_Employee;
            orderEmployee.Employee = selectedemployee;
            orderEmployee.Order = order;

            if(orderEmployee.ID == 0)
            {
                MainWindow.baza.Order_Employee.Add(orderEmployee);
            }

            MainWindow.baza.SaveChanges();
            this.Close();
        }
    }
}
