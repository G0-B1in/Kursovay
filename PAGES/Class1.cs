//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace PostOffice.View
//{
//    /// <summary>
//    /// Логика взаимодействия для PageWatchPublicationNoReg.xaml
//    /// </summary>
//    public partial class PageWatchPublicationNoReg : Page
//    {
//        Model.DataBasePostOffice dataBasePostOffice;

//        List<Publication> _publications;

//        List<Publication> sortPublication;

//        List<Publication> temps;

//        private int _countPublication = 6;

//        public delegate void CloseWin();

//        public static event CloseWin closePage;

//        private int _maxPages;

//        private int _currentPage = 1;

//        public PageWatchPublicationNoReg()
//        {
//            InitializeComponent();

//            WinEntrance.closeWin += WinEntrance_closePage;

//            sortPublication = new List<Publication>();

//            temps = new List<Publication>();

//            try
//            {
//                dataBasePostOffice = new Model.DataBasePostOffice(MainWindow.postOfficeEntity);
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show($"{e.Message}");
//            }

//            _publications = dataBasePostOffice.postOfficeEntities.Publication.ToList();

//            comboBoxTypePublication(cbTypePublication, dataBasePostOffice.postOfficeEntities.TypePublication.ToList());

//            comboBoxTypeViewPubication(cbCatergoriaPublication, dataBasePostOffice.postOfficeEntities.TypeViewPublication.ToList());
//        }


//        private int CountEntryMax(int AllCountData) => (int)Math.Ceiling(AllCountData * 1.0 / _countPublication);

//        private void comboBoxTypePublication(ComboBox comboBox, List<TypePublication> list)
//        {
//            List<string> temp = new List<string>();

//            temp.Add("Все издания");

//            foreach (var item in list)
//            {
//                temp.Add(item.Name);
//            }

//            comboBox.ItemsSource = temp;

//            comboBox.SelectedItem = temp[0];
//        }

//        private void comboBoxTypeViewPubication(ComboBox comboBox, List<TypeViewPublication> list)
//        {
//            List<string> temp = new List<string>();

//            temp.Add("Все категории изданий");

//            foreach (var item in list)
//            {
//                temp.Add(item.Name);
//            }

//            comboBox.ItemsSource = temp;

//            comboBox.SelectedItem = temp[0];
//        }

//        private void Refresh()
//        {
//            var publication = sortPublication;

//            var publicationPages = sortPublication.Skip((_currentPage - 1) * _countPublication).Take(_countPublication).ToList();

//            _maxPages = CountEntryMax(publication.Count());

//            InfoPages.Content = $"Страница: {_currentPage} из {_maxPages}";

//            MyLv.ItemsSource = publicationPages;
//        }

//        private void ApplySearch()
//        {
//            sortPublication = _publications.Where(item => item.Name.StartsWith(tbSearch.Text)).ToList();
//            ApplyComboBoxFiltres();
//            Refresh();
//        }

//        private void btnNextPage(object sender, RoutedEventArgs e)
//        {
//            if (_currentPage >= _maxPages) _currentPage = _maxPages;
//            else
//            {
//                _currentPage++;
//            }
//            Refresh();
//        }

//        private void btnBackPage(object sender, RoutedEventArgs e)
//        {
//            if (_currentPage <= 1) _currentPage = 1;
//            else
//            {
//                _currentPage--;
//            }
//            Refresh();
//        }

//        private void btnFirstPage(object sender, RoutedEventArgs e)
//        {
//            _currentPage = 1;
//            Refresh();
//        }

//        private void btnLastPage(object sender, RoutedEventArgs e)
//        {
//            _currentPage = _maxPages;
//            Refresh();
//        }

//        private void TextChangedSearch(object sender, TextChangedEventArgs e)
//        {
//            ApplySearch();
//        }

//        private void cbCategoriaChanged(object sender, SelectionChangedEventArgs e)
//        {
//            ApplySearch();
//            ApplyComboBoxFiltres();
//        }

//        private void ApplyComboBoxFiltres()
//        {
//            string selectItem = cbCatergoriaPublication.SelectedItem as string;

//            string selectItem2 = cbTypePublication.SelectedItem as string;

//            if (selectItem != "Все категории изданий")
//            {
//                temps = _publications.Where(item => item.TypeViewPublication.Name == selectItem & item.Name.StartsWith(tbSearch.Text)).ToList();
//                if (selectItem2 != "Все издания")
//                {
//                    sortPublication = temps.Where(item => item.TypePublication.Name == selectItem2 & item.Name.StartsWith(tbSearch.Text)).ToList();
//                }
//                else
//                {
//                    sortPublication = temps;
//                }
//            }

//            if (selectItem2 != "Все издания")
//            {
//                temps = _publications.Where(item => item.TypePublication.Name == selectItem2 & item.Name.StartsWith(tbSearch.Text)).ToList();
//                if (selectItem != "Все категории изданий")
//                {
//                    sortPublication = temps.Where(item => item.TypeViewPublication.Name == selectItem & item.Name.StartsWith(tbSearch.Text)).ToList();
//                }
//                else
//                {
//                    sortPublication = temps;
//                }
//            }

//            Refresh();
//        }

//        private void cbTypeChanged(object sender, SelectionChangedEventArgs e)
//        {
//            ApplySearch();
//            ApplyComboBoxFiltres();
//        }


//        private void cbPriorityGenderChanged(object sender, SelectionChangedEventArgs e)
//        {
//            ApplySearch();
//            ApplyComboBoxFiltres();
//        }
//    }
//}