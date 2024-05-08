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
    /// Логика взаимодействия для OrderDishes.xaml
    /// </summary>
    public partial class OrderDishes : Window
    {
        private Order order;
        private OrderDish orderDish;
        private Menu menu;
        public OrderDishes(Menu menu, Order order)
        {
            InitializeComponent();
            this.order = order;
            this.orderDish = new OrderDish();
            this.menu = menu;
            DataContext = menu;
            dgpizza.ItemsSource = MainWindow.baza.Menu.ToList();
        }

        private void orddish(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedmenu = editButton.DataContext as Menu;
            orderDish.ID_Order = order.ID_Order;            
            orderDish.ID_OrderDish = selectedmenu.ID_dish;
            orderDish.Count = 1;
            orderDish.Menu = selectedmenu;
            orderDish.Order = order;
            
            if(orderDish.ID == 0)
            {
                MainWindow.baza.OrderDish.Add(orderDish);
            }
            MainWindow.baza.SaveChanges();
            this.Close();
        }
    }
}
