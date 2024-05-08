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
    /// Логика взаимодействия для addDish.xaml
    /// </summary>
    public partial class addDish : Window
    {
        private Menu menu;
        public addDish(Menu menu)
        {
            InitializeComponent();
            this.menu = menu;
            DataContext = menu;
            cbCategory.ItemsSource = MainWindow.baza.Category.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(tbName.Text == null | tbName.Text == "")
                {
                    throw new Exception("Укажите название блюда");
                }
                if (tbPrice.Text == null | tbPrice.Text == "")
                {
                    throw new Exception("Укажите стоимость блюда");
                }
                if (int.Parse(tbPrice.Text) <= 0)
                {
                    throw new Exception("Стоимость должна быть больше нуля");
                }
                if(cbCategory.SelectedItem == null)
                {
                    throw new Exception("Укажите категорию для блюда");
                }

                if (menu.ID_dish == 0)
                {
                    MainWindow.baza.Menu.Add(menu);
                }
                MainWindow.baza.SaveChanges();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Foto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Foto (*.png)|*.png|All files (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                var x = System.IO.File.ReadAllBytes(filename);
                menu.foto = x;
                DataContext = null;
                DataContext = menu;
            }
        }
    }
}
