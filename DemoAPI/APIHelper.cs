using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemoAPI
{
    class APIHelper
    {

        string baseUrl = "https://localhost:44355/api/customers";
            // Hàm yêu cầu sửa thông tin 1 khách hàng trên server 
            // Hàm có 3 tham số truyền vào là ID, Tên, Địa chỉ, Số điện thoại khách hàng
        public Customer GetCustomerAPI(int id)
        {

            using (var wb = new WebClient())
            {
                try
                {
                    string response = wb.DownloadString(baseUrl + "/" + id);
                    Customer customer = JsonConvert.DeserializeObject<Customer>(response);
                    return customer;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public List<Customer> GetListCustomerAPI()
        {
            using (var wb = new WebClient())
            {
                try
                {
                    string response = wb.DownloadString(baseUrl);
                    List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(response);
                    return customers;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public bool CreateCustomerAPI(string name, string address, string phonenumber)
        {
            using (var wb = new WebClient())
            {
                try
                {
                    wb.QueryString.Add("name", name);
                    wb.QueryString.Add("adress", address);
                    wb.QueryString.Add("phoneNumber", phonenumber);
                    var response = wb.UploadValues(baseUrl, "POST", wb.QueryString);
                    string responseInString = Encoding.UTF8.GetString(response);
                    return responseInString.Equals("true");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool DeleteCustomerAPI(int id)
        {
            using (var wb = new WebClient())
            {
                try
                {
                    wb.QueryString.Add("id", id.ToString());
                    var response = wb.UploadValues(baseUrl, "DELETE", wb.QueryString);
                    string responseInString = Encoding.UTF8.GetString(response);
                    return responseInString.Equals("true");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool UpdateCustomerAPI(int id, string name, string address, string phonenumber)
        {
            using (var wb = new WebClient())
            {
                try
                {
                    wb.QueryString.Add("id", id.ToString());
                    wb.QueryString.Add("name", name);
                    wb.QueryString.Add("adress", address);
                    wb.QueryString.Add("phoneNumber", phonenumber);

                    var response = wb.UploadValues(baseUrl, "PUT", wb.QueryString);
                    string responseInString = Encoding.UTF8.GetString(response);
                    return responseInString.Equals("true");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
