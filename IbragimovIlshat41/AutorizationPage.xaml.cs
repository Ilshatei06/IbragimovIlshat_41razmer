using IbragimovIlshat41;
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
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        int countEror = 0;
        string capcha;
        public AutorizationPage()
        {
            InitializeComponent();

            TBoxCapcha.Visibility = Visibility.Hidden;
            
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = TBoxLogin.Text;
            string password = TBoxPassword.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля!");
                return;
            }

            if (countEror > 0)
            {
                if (string.IsNullOrEmpty(TBoxCapcha.Text))
                {
                    MessageBox.Show("Введите капчу!");
                    return;
                }

                if (TBoxCapcha.Text != capcha)
                {
                    countEror++;
                    MessageBox.Show("Капча введена неверно!");
                    TBoxCapcha.Text = "";
                    LoginBtn.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginBtn.IsEnabled = true;
                    return;
                }


            }

            User user = IbragimovIlshat41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                TBoxLogin.Text = "";
                TBoxPassword.Text = "";
                capchaOneWord.Text = "";
                capchaTwoWord.Text = "";
                capchaThreeWord.Text = "";
                capchaFourWord.Text = "";
                TBoxCapcha.Visibility = Visibility.Hidden;
            }
            else
            {
                countEror++;
                MessageBox.Show("Введены неверные данные!");

                Random rnd = new Random();
                string symb = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
                capchaOneWord.Text = symb[rnd.Next(symb.Length)].ToString();
                capchaTwoWord.Text = symb[rnd.Next(symb.Length)].ToString();
                capchaThreeWord.Text = symb[rnd.Next(symb.Length)].ToString();
                capchaFourWord.Text = symb[rnd.Next(symb.Length)].ToString();
                capcha = capchaOneWord.Text + capchaTwoWord.Text + capchaThreeWord.Text + capchaFourWord.Text;
                TBoxCapcha.Visibility = Visibility.Visible;

                if (countEror >= 2)
                {
                    TBoxCapcha.Text = "";
                    LoginBtn.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginBtn.IsEnabled = true;
                }
            }

            

        }
        

        private void NoLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = null;
            Manager.MainFrame.Navigate(new ProductPage(user));

            TBoxLogin.Text = "";
            TBoxPassword.Text = "";
            capchaOneWord.Text = "";
            capchaTwoWord.Text = "";
            capchaThreeWord.Text = "";
            capchaFourWord.Text = "";
            TBoxCapcha.Visibility = Visibility.Hidden;
            countEror = 0;
        }
    }
}
