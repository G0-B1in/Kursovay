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
    /// Логика взаимодействия для addOrder.xaml
    /// </summary>
    public partial class addOrder : Window
    {
        private Order order;
        public addOrder(Order order)
        {
            InitializeComponent();
            if (order.ID_Order == 0)
            {
                DateTime now = DateTime.Now;
                order.Date = now;
                order.ID_Status = 1;
            }

            this.order = order;
            DataContext = order;
            cbClient.ItemsSource = MainWindow.baza.Client.ToList();
            cbBranch.ItemsSource = MainWindow.baza.Branch.ToList();
            cbPaymendMethod.ItemsSource = MainWindow.baza.PaymentMethod.ToList();
            cbStatus.ItemsSource = MainWindow.baza.OrderStatus.ToList();

            
            var dishes = MainWindow.baza.OrderDish.ToList();
            dgOrderedDishes.ItemsSource = dishes.Where(dish => dish.ID_Order == order.ID_Order);
            var employees = MainWindow.baza.Order_Employee.ToList();
            dgOrderEmployees.ItemsSource = employees.Where(dish => dish.ID_Order == order.ID_Order).OrderByDescending(p => p.Job);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbClient.SelectedItem == null)
                {
                    throw new Exception("Клиент не выбран");
                }
                if (cbBranch.SelectedItem == null)
                {
                    throw new Exception("Филиал не определен");
                }
                if (cbPaymendMethod.SelectedItem == null)
                {
                    throw new Exception("Способ оплаты не определен");
                }
                if (order.ID_Order == 0)
                {
                    MainWindow.baza.Order.Add(order);
                }
                MainWindow.baza.SaveChanges();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cbClient.SelectedItem == null)
                {
                    throw new Exception("Клиент не выбран");
                }
                if (cbBranch.SelectedItem == null)
                {
                    throw new Exception("Филиал не определен");
                }
                if (cbPaymendMethod.SelectedItem == null)
                {
                    throw new Exception("Способ оплаты не определен");
                }

                if (order.ID_Order == 0)
                {
                    MainWindow.baza.Order.Add(order);
                }
                MainWindow.baza.SaveChanges();

                Menu menu = new Menu();
                OrderDishes ord = new OrderDishes(menu, order);
                ord.ShowDialog();
                dgOrderedDishes.ItemsSource = null;
                var dishes = MainWindow.baza.OrderDish.ToList();
                dgOrderedDishes.ItemsSource = dishes.Where(dish => dish.ID_Order == order.ID_Order);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbClient.SelectedItem == null)
                {
                    throw new Exception("Клиент не выбран");
                }
                if (cbBranch.SelectedItem == null)
                {
                    throw new Exception("Филиал не определен");
                }
                if (cbPaymendMethod.SelectedItem == null)
                {
                    throw new Exception("Способ оплаты не определен");
                }

                if (order.ID_Order == 0)
            {
                MainWindow.baza.Order.Add(order);
            }
            MainWindow.baza.SaveChanges();

            Employee employee = new Employee();
            OrderEmplyees oemp = new OrderEmplyees(employee, order);
            oemp.ShowDialog();
            dgOrderEmployees.ItemsSource = null;
            var employees = MainWindow.baza.Order_Employee.ToList();
            dgOrderEmployees.ItemsSource = employees.Where(dish => dish.ID_Order == order.ID_Order);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedOrd = editButton.DataContext as OrderDish;

            selectedOrd.Count++;
            MainWindow.baza.SaveChanges();
            dgOrderedDishes.ItemsSource = null;
            var dishes = MainWindow.baza.OrderDish.ToList();
            dgOrderedDishes.ItemsSource = dishes.Where(dish => dish.ID_Order == order.ID_Order);
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedOrd = editButton.DataContext as OrderDish;

            if (selectedOrd.Count > 0)
                selectedOrd.Count--;
            else
                MainWindow.baza.OrderDish.Remove(selectedOrd);
            MainWindow.baza.SaveChanges();
            dgOrderedDishes.ItemsSource = null;
            var dishes = MainWindow.baza.OrderDish.ToList();
            dgOrderedDishes.ItemsSource = dishes.Where(dish => dish.ID_Order == order.ID_Order);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var delButton = sender as Button;
            var deletedOrd = delButton.DataContext as Order_Employee;

            MainWindow.baza.Order_Employee.Remove(deletedOrd);
            MainWindow.baza.SaveChanges();
            var employees = MainWindow.baza.Order_Employee.ToList();
            dgOrderEmployees.ItemsSource = employees.Where(dish => dish.ID_Order == order.ID_Order);
        }
    }
}
