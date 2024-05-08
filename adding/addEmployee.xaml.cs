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
    /// Логика взаимодействия для addEmployee.xaml
    /// </summary>
    public partial class addEmployee : Window
    {
        private Employee employee;
        public addEmployee(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            DataContext = employee;
            cbBranch.ItemsSource = MainWindow.baza.Branch.ToList();
            cbJobPosition.ItemsSource = MainWindow.baza.Job_Position.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbFName.Text == null | tbFName.Text == "")
                {
                    throw new Exception("Укажите имя сотрудника");
                }
                if (tbLName.Text == null | tbLName.Text == "")
                {
                    throw new Exception("Укажите фамилию сотрудника");
                }
                if (tbBerthday.Text == null | tbBerthday.Text == "")
                {
                    throw new Exception("Укажите дату рождения сотрудника");
                }
                if (tbCard.Text == null | tbCard.Text == "")
                {
                    throw new Exception("Укажите номер банковской карты сотрудника");
                }
                if (tbCard.Text.Length != 16)
                {
                    throw new Exception("Номер банковской карты не определен");
                }
                if (cbBranch.SelectedItem == null)
                {
                    throw new Exception("Укажите место работы сотрудника");
                }
                if (cbJobPosition.SelectedItem == null)
                {
                    throw new Exception("Укажите должность для сотрудника");
                }
                if (DateTime.Now.Year - Convert.ToDateTime(tbBerthday.Text).Year < 18)
                {
                    throw new Exception("Сотрудник должне быть совершеннолетним");
                }
                if (employee.ID_Employee == 0)
                {
                    MainWindow.baza.Employee.Add(employee);
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
    }
}
