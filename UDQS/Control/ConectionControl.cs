using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UDQS.Models;

namespace UDQS.Control
{
    class ConectionControl
    {
        public string baseUrl = "http://98.254.97.228/"; //"https://localhost:44365/";
        HttpClient httpClient;
        public ConectionControl()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<FileInformation> ComparNewFile(List<FileInformation> fileInformation)
        {
            string apiResponse;
            var payload = JsonConvert.SerializeObject(fileInformation);
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(baseUrl + "api/Assambly/ComparNewFile", c).Result)
            {
                apiResponse = response.Content.ReadAsStringAsync().Result;
                fileInformation = JsonConvert.DeserializeObject<List<FileInformation>>(apiResponse);

            }
            return fileInformation;
        }

        public  void DonwloadFile(FileInformation fileInformation, string url)
        {
            string ruta = url + fileInformation.Name;
            var di = new DirectoryInfo(ruta).Parent;
            var payload = JsonConvert.SerializeObject(fileInformation);
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(baseUrl + "api/Assambly/DonwloadFile", c).Result)
            {
                if (!di.Exists)
                        Directory.CreateDirectory(di.FullName);
                using (var fs = new FileStream(ruta,FileMode.Create))
                {
                     response.Content.CopyToAsync(fs).Wait();
                }
            }
        }

    }
}
