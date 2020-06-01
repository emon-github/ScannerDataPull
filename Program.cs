using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDataPull
{
    class Program
    {
        public static ModelContext _db = new ModelContext();
        public static string _token = "";

        static void Main(string[] args)
        {
            SetLoginToken().Wait();

            GetDeviceData().Wait();
            GetStaffData().Wait();
            GetAccessData().Wait();
        }

        static public async Task SetLoginToken()
        {
            var token = "";

            try
            {
                var Loginurl = "http://175.143.69.73:8085/cloudIntercom/login";

                using (var client = new HttpClient())
                {
                    var parameters = new Dictionary<string, string> {
                    { "userName", "admin" },
                    { "password", "554206"}};
                    var encodedContent = new FormUrlEncodedContent(parameters);

                    // HttpResponseMessage result = await client.PostAsync(Loginurl, encodedContent);

                    var response = await client.PostAsync(Loginurl, encodedContent);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(responseContent);
                        token = data.token.ToString();
                        _token = token.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        static public async Task GetDeviceData()
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("token", _token);
                string jsonString = "";

                HttpResponseMessage result = await client.GetAsync("http://175.143.69.73:8085/cloudIntercom/selectGateEquipByQueryVo");

                if (result.IsSuccessStatusCode)
                {
                    jsonString = await result.Content.ReadAsStringAsync();
                    dynamic response = JsonConvert.DeserializeObject(jsonString);
                    List<Device> list = response.data.list.ToObject<List<Device>>();
                    string str = response.data.total.ToString();

                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var entity = _db.Devices.Where(_ => _.sn == item.sn).SingleOrDefault();
                            if (entity == null)
                            {
                                _db.Devices.Add(item);
                            }
                        }
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        static public async Task GetStaffData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("token", _token);
                    string jsonString = "";
                    var url = "http://175.143.69.73:8085/cloudIntercom/selectPersonByQueryVo";

                    HttpResponseMessage result = await client.GetAsync(url);

                    if (result.IsSuccessStatusCode)
                    {
                        jsonString = await result.Content.ReadAsStringAsync();
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonString);
                        int totalPage = Convert.ToInt32(jsonData.data.pages.ToString());

                        for (int i = 1; i <= totalPage; i++)
                        {
                            var parameters = new Dictionary<string, string> {
                            { "pageNum", i.ToString() },
                            { "pageSize", "10"}};
                            var encodedContent = new FormUrlEncodedContent(parameters);

                            var response = await client.PostAsync(url, encodedContent).ConfigureAwait(false);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                                List<Staff> list = responseData.data.list.ToObject<List<Staff>>();
                                if (list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        if (!string.IsNullOrEmpty(item.photoUrl))
                                        {
                                            var entity = _db.Staffs.Where(_ => _.phone == item.phone).SingleOrDefault();
                                            if (entity == null)
                                            {
                                                _db.Staffs.Add(item);
                                            }
                                        }
                                    }
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        static public async Task GetAccessData()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.Timeout = TimeSpan.FromMinutes(2);
                    client.MaxResponseContentBufferSize = 216784921;
                    string jsonString = "";
                    client.DefaultRequestHeaders.Add("token", _token);

                    var url = "http://175.143.69.73:8085/cloudIntercom/selectAccessRecord";

                    HttpResponseMessage result = await client.GetAsync(url);

                    if (result.IsSuccessStatusCode)
                    {
                        jsonString = await result.Content.ReadAsStringAsync();
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonString);
                        int totalPage = Convert.ToInt32(jsonData.data.pages.ToString());

                        for (int i = 1; i <= totalPage; i++)
                        {
                            var parameters = new Dictionary<string, string> {
                            { "pageNum", i.ToString() },
                            { "pageSize", "20"}};
                            var encodedContent = new FormUrlEncodedContent(parameters);

                            var response = await client.PostAsync(url, encodedContent).ConfigureAwait(false);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                                List<AccessRecord> list = responseData.data.list.ToObject<List<AccessRecord>>();
                                if (list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        var entity = _db.AccessRecords.Where(_ => _.id == item.id).SingleOrDefault();
                                        if (entity == null)
                                        {
                                            _db.AccessRecords.Add(item);
                                        }
                                    }
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
