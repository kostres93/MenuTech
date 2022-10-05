using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MenuTechWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//TODO: stavi logout dugme
        //TODO: stavi url u config fajl  pa izvuci a ne zakucati
        private const string URL = "https://localhost:44327/store";
        private const string URLTransactions = "https://localhost:44327/api/Transactions/";
        public Customer customer = new Customer();
        private HttpClient http = new HttpClient();


        public MainWindow()
        {

            InitializeComponent();
            SetCurrencyList();
            optionGrid.IsEnabled = false;


            http.BaseAddress = new Uri(URL);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetCurrencyList()
        {
            ddlCurrency.SelectedValuePath = "Key";
            ddlCurrency.DisplayMemberPath = "Value";
            ddlCurrency.Items.Add(new KeyValuePair<int, string>(941, "RSD"));
            ddlCurrency.Items.Add(new KeyValuePair<int, string>(978, "EUR"));
            ddlCurrency.Items.Add(new KeyValuePair<int, string>(840, "USD"));
            ddlCurrency.SelectedIndex = 0;

        }
        private void StoreAccBalance_Click(object sender, RoutedEventArgs e)
        {

            HttpResponseMessage response = http.GetAsync(URL+ "/GetStoreAccBalance").Result;
            txbRequestSend.Text = response.StatusCode.ToString() + response.Content.Headers.ToString();
            var dataObj = response.Content.ReadAsStringAsync().Result;
            
            MessageBox.Show("Store account balance is " + dataObj.ToString(), "Store account balance", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

            if (response.IsSuccessStatusCode)
            {
                var dataObject = response.Content.ReadAsStringAsync().Result;
            }
            txbRequestRecieved.Text = dataObj;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(customer);

            login.ShowDialog();

            lblLoginMsg.Content = login.returmMsg;

            if (customer.customerId != 0)
            {
                optionGrid.IsEnabled = true;
                GetTransactions(customer.customerId);
            }
            lblLoginMsg.Content = customer.loginMsg;

            login.Close();
            btnSignOut.IsEnabled = true;


        }

        private void btnCustomerAccBalance_Click(object sender, RoutedEventArgs e)
        {
            var userString = Encoding.UTF8.GetBytes(customer.userName + ":" + customer.password);
            string base64UserString = Convert.ToBase64String(userString);

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64UserString);

            HttpResponseMessage response = http.GetAsync(URL + "/GetCustomerAccBalance/" + customer.customerId).Result;
            var dataObj = response.Content.ReadAsStringAsync().Result;
            if (dataObj.ToString()!="")
            {
                MessageBox.Show("Customer account balance is " + dataObj.ToString(), "Customer account balance", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

            }

        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {

            var userString = Encoding.UTF8.GetBytes(customer.userName + ":" + customer.password);
            string base64UserString = Convert.ToBase64String(userString);

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64UserString);

            var obj =  new 
             {
                customerId = customer.customerId,
                currency =ddlCurrency.SelectedValue,
                amount = txtAmount.Text.ToString()
             };


            var json = JsonSerializer.Serialize(obj);
            
            var content = new StringContent(json.ToString(),Encoding.UTF8, "application/json");

            HttpResponseMessage response = http.PostAsync(URL+"/Pay" , content).Result;
            var dataObj = response.Content.ReadAsStringAsync().Result;

            MessageBox.Show("Transaction id: "+dataObj.ToString(), "Payment information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

            GetTransactions(customer.customerId);

        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            // /Regex regex = new Regex(@"^\d*(\.\d+)?$");
            // e.Handled = !regex.IsMatch(e.Text);

        }

        private void GetTransactions(int CustomerId)
        {
            HttpClient http = new HttpClient();

            http.BaseAddress = new Uri(URLTransactions);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = http.GetAsync(URLTransactions + customer.customerId).Result;

            var dataObj = response.Content.ReadAsStringAsync().Result.ToString();

            Customer[] customers = JsonSerializer.Deserialize<Customer[]>(dataObj);

            dgTransactions.ItemsSource = customers;

            foreach (var customer in customers)
            {
                if (customer.transactionPlus != null && customer.refund != 1)
                    btnRefund.IsEnabled = true;
            }
           

        }

        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            var userString = Encoding.UTF8.GetBytes(customer.userName + ":" + customer.password);
            string base64UserString = Convert.ToBase64String(userString);

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64UserString);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var obj  = new
            {
                customerId = customer.customerId,
                transactionId = txtTransactionId.Text,
                amount = txtAmount.Text.ToString()
                
            };


            var json = JsonSerializer.Serialize(obj);

            var content = new StringContent(json.ToString(),Encoding.UTF8,"application/json");

            HttpResponseMessage response = http.PostAsync(URL + "/Refund", content).Result;
            var dataObj = response.Content.ReadAsStringAsync().Result;
            if (!Convert.ToBoolean(dataObj))
            {
                MessageBox.Show("Refund not possible!", "Refund information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

            }
            GetTransactions(customer.customerId);
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            optionGrid.IsEnabled = false;
            dgTransactions.ItemsSource=null;
            dgTransactions.Items.Refresh();
            lblLoginMsg.Content = "";
            btnSignOut.IsEnabled = false;
            customer = new Customer();
        }
    }
}
