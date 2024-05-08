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
    /// Логика взаимодействия для addBranch.xaml
    /// </summary>
    public partial class addBranch : Window
    {
        private Branch branch;
        public addBranch(Branch branch)
        {
            InitializeComponent();
            this.branch = branch;
            DataContext = branch;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(tbAdress.Text == null | tbAdress.Text == "")
                {
                    throw new Exception("Укажите адрес");
                }
                if (tbPhone.Text == null | tbPhone.Text == "")
                {
                    throw new Exception("Укажите номер телефона");
                }

                if (branch.ID_branch == 0)
                {
                    MainWindow.baza.Branch.Add(branch);
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
