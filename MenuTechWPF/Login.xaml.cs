using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MenuTechWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private const string URL = "https://localhost:44327/api/Login/";
        public string returmMsg = "";
        private Customer customer = new Customer();
        public Login(Customer customer1)
        {
            
            customer = customer1;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           
            customer.userName = txtUserName.Text;
            customer.password = txtPassword.Password;

           
            var bytePassword = System.Text.Encoding.UTF8.GetBytes(customer.password);
            string base64Password = Convert.ToBase64String(bytePassword);

            HttpClient http = new HttpClient();

            http.BaseAddress=new Uri(URL);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = http.GetAsync(URL + customer.userName + "/" + base64Password).Result;

            var dataObj = response.Content.ReadAsStringAsync().Result;

           
            if (dataObj=="false")
            {
                customer.loginMsg = "User does not exist!";
                MessageBox.Show(customer.loginMsg, "Login failes", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

                lblUserMsg.Content = customer.loginMsg;
            }
            else
            {
                
               customer.userName = JsonSerializer.Deserialize<Customer>(dataObj).userName;
               customer.password = JsonSerializer.Deserialize<Customer>(dataObj).password;
               customer.customerId = JsonSerializer.Deserialize<Customer>(dataObj).customerId;
               customer.loginMsg = "Welcome " + customer.userName + "!";
               Close();
            }



            
        }
    }
}
