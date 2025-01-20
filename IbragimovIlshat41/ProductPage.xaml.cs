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

namespace IbragimovIlshat41
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        List<Product> Tablelist;
        int RecordsCount;
        public ProductPage()
        {
            InitializeComponent();

            var currentAgents = IbragimovIlshat41Entities.GetContext().Product.ToList();
            Tablelist = IbragimovIlshat41Entities.GetContext().Product.ToList();

            ProductListView.ItemsSource = currentAgents;

            ComboType.SelectedIndex = 0;

            UpdateProducts();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void UpdateProducts()
        {
            var currentProducts = IbragimovIlshat41Entities.GetContext().Product.ToList();

            if (ComboType.SelectedIndex == 0)
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100).ToList();

            if (ComboType.SelectedIndex == 1)
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();

            if (ComboType.SelectedIndex == 2)
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();

            if (ComboType.SelectedIndex == 3)
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100).ToList();


            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();


            if (RBdown.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }

            if (RBup.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }

            ProductListView.ItemsSource = currentProducts.ToList();

            TBCount.Text = "кол-во " + currentProducts.Count.ToString();
            TBAllRecords.Text = "из " + Tablelist.Count.ToString();
        }

        private void RBup_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }


        private void RBdown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }
    }
}
