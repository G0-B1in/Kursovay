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

namespace доставка.PAGES
{
    /// <summary>
    /// Логика взаимодействия для RegistrationListPage.xaml
    /// </summary>
    public partial class RegistrationListPage : Page
    {
        public RegistrationListPage()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader("registrationlist.txt");
            while (!sr.EndOfStream)
            {
                lb.Items.Add(sr.ReadLine());
            }            
            sr.Close();

        }
    }
}
