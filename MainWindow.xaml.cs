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
using System.IO;

namespace доставка
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PizzaEntities1 baza;
        public Branch branch;
        List<Users> user;
        public MainWindow()
        {
            InitializeComponent();

            baza = new PizzaEntities1();
            cbbranch.ItemsSource = baza.Branch.ToList();
            cbuser.ItemsSource = baza.Users.ToList();
            user = baza.Users.ToList();
        }

        private void singin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("registrationlist.txt");
                string registrations = sr.ReadToEnd();
                sr.Close();

                StreamWriter sw = new StreamWriter("registrationlist.txt");

                if (cbuser.SelectedItem == null)
                {
                    throw new Exception("Укажите пользователя");
                }
                if(tbPassword.Password == null | tbPassword.Password == "")
                {
                    throw new Exception("Введите пароль");
                }

                if (cbuser.SelectedItem == cbuser.Items[0])
                {
                    if (tbPassword.Password != user[0].password)
                    {
                        throw new Exception("Неверный пароль");
                    }
                    registrations += $"\nВход: Администратор {DateTime.Now}";
                    adminWindow aw = new adminWindow("Администратор");
                    cbbranch.SelectedItem = null;
                    aw.WindowState = this.WindowState;
                    aw.Show();
                }

                if (cbuser.SelectedItem == cbuser.Items[1])
                {
                    if (cbbranch.SelectedItem == null)
                    {
                        throw new Exception("Укажите Ваш филиал");
                    }
                    if (tbPassword.Password != user[1].password)
                    {
                        throw new Exception("Неверный пароль");
                    }
                    registrations += $"\nВход: Управляющий {branch.Adress} {DateTime.Now}";
                    managerWindow mw = new managerWindow(branch, "Управляющий");
                    mw.WindowState = this.WindowState;
                    mw.Show();
                }

                sw.Write(registrations);
                sw.Close();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbuser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbuser.SelectedItem != null)
            {
                if (cbuser.SelectedItem != cbuser.Items[0])
                {
                    cbbranch.IsEnabled = true;
                }
                else
                {
                    cbbranch.SelectedItem = null;
                    cbbranch.IsEnabled = false;
                }
            }
           
        }

        private void cbbranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            branch = cbbranch.SelectedItem as Branch;
        }
    }
}
