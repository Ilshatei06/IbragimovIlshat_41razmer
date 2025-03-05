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
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();


        int newOrderId;
        public ProductPage(User user)
        {
            InitializeComponent();
            if (selectedProducts.Count == 0)
            {
                OrderBtn.Visibility = Visibility.Hidden;
            }

            if (user == null)
            {
                FIOTB.Text = "Вы не аторизованы";
                RoleTB.Text = "Роль: Гость";
            }
            else
            {
                FIOTB.Text = "Вы аторизованы как " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Роль: Клиент"; break;
                    case 2:
                        RoleTB.Text = "Роль: Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Роль: Администратор"; break;
                }
            }


            var currentAgents = IbragimovIlshat41Entities.GetContext().Product.ToList();
            Tablelist = IbragimovIlshat41Entities.GetContext().Product.ToList();

            ProductListView.ItemsSource = currentAgents;

            ComboType.SelectedIndex = 0;

            UpdateProducts();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {


            if (ProductListView.SelectedIndex >= 0)
            {
                var prod = ProductListView.SelectedItem as Product;
                selectedProducts.Add(prod);

                //int newOrderID = selectedOrderProducts.Last().Order.OrderID;
                var newOrderProd = new OrderProduct();//новый заказ

                //номер продукта в новую запись
                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.ProductCount = 1;

                //проверии есть ли уже такой заказ
                var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));
                //MessageBox.Show(selOP.Count().ToString());
                if (selOP.Count() == 0)
                {
                    //MessageBox.Show(newOrderProd. OrderID.ToString() + " " + newOrderProd.ProductArticleNumber.ToString() + " " + newOrderProd.Quantity.ToString());
                    selectedOrderProducts.Add(newOrderProd);
                    //MessageBox.Shom("колво в selecteOP = " + selectedOrderProducts.Count().ToString());
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                            p.ProductCount++;
                    //MessageBox.Show("колво = " + p.Quantity.ToString());
                }

                OrderBtn.Visibility = Visibility.Visible;
                ProductListView.SelectedIndex = -1;

                UpdateProducts();
            }

        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow OrdWind = new OrderWindow(selectedOrderProducts, selectedProducts, FIOTB.Text);
            OrdWind.ShowDialog();
            UpdateProducts();
        }
    }
}


