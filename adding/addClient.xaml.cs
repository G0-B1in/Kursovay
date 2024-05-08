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
    /// Логика взаимодействия для addClient.xaml
    /// </summary>
    public partial class addClient : Window
    {
        private Client client;
        public addClient(Client client)
        {
            InitializeComponent();
            this.client = client;
            DataContext = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(tbFName.Text == null | tbFName.Text == "")
                {
                    throw new Exception("Укажите имя кдиента");
                }
                if (tbLName.Text == null | tbLName.Text == "")
                {
                    throw new Exception("Укажите фамилию клиента");
                }
                if (tbAdress.Text == null | tbAdress.Text == "")
                {
                    throw new Exception("Укажите адрес доставки клиента");
                }
                if (tbPhone.Text == null | tbPhone.Text == "")
                {
                    throw new Exception("Укажите номер телефона клиента");
                }

                if (client.ID_Client == 0)
                {
                    MainWindow.baza.Client.Add(client);
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
