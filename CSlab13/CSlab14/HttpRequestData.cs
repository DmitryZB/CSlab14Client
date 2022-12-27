using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CSlab13
{
    public class TaxiDepotView
    {
        public string Address { get; set; }
        public int Id { get; set; }
        public List<TaxiGroupView> TaxiGroupViews { get; set; } = new List<TaxiGroupView>();
    }

    public class TaxiGroupView
    {
        public int Id { get; set; }
        public int TaxiDepotId { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class HttpRequestData
    {
        public static HttpClient client = new() { BaseAddress = new Uri("http://localhost:5204") };

        public static Car GetCarFromId(int id)
        {
            string url = $"http://localhost:5204/api/Car/{id}";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeCar = JsonConvert.DeserializeObject<Car>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);

            return deserializeCar;
        }

        public static List<Car> GetAllCars()
        {
            string url = "http://localhost:5204/api/Cars";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            var data = sr1.ReadToEnd();
            var deserializeListOfCars = JsonConvert.DeserializeObject<List<Car>>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);

            return deserializeListOfCars;
        }

        public static void AddCar(Car temp)
        {
            // client.BaseAddress = new Uri("http://localhost:5204");
            Task<HttpResponseMessage> request = client.PostAsJsonAsync($"api/Car", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static void ChangeCar(Car temp)
        {
            Task<HttpResponseMessage> request = client.PutAsJsonAsync($"api/Car", temp);
            string ans = request.Result.StatusCode.ToString();
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            Console.Write(ans);
        }

        public static void DeleteCar(Car temp)
        {
            string url = $"http://localhost:5204/api/Car/{temp.Id}";
            Task<HttpResponseMessage> request =
                new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static TaxiDepot GetTaxiDepotFromId(int id)
        {
            string url = $"http://localhost:5204/api/TaxiDepot/{id}";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeTaxiDepotView = JsonConvert.DeserializeObject<TaxiDepotView>(data);
            TaxiDepot temp = new TaxiDepot() { Id = deserializeTaxiDepotView.Id, Address = deserializeTaxiDepotView.Address };
            List<TaxiGroup> tempTaxiGroups = new List<TaxiGroup>();
            foreach (var VARIABLE in deserializeTaxiDepotView.TaxiGroupViews)
            {
                TaxiGroup taxiGroup = new TaxiGroup()
                {
                    Id = VARIABLE.Id,
                    TaxiDepotId = temp.Id,
                    TaxiDepot = temp,
                    Quantity = VARIABLE.Quantity,
                    // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                    CarId = VARIABLE.CarId,
                    Car = VARIABLE.Car
                };
                tempTaxiGroups.Add(taxiGroup);
            }

            temp.TaxiGroups = tempTaxiGroups;

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            
            
            return temp;
        }

        public static List<TaxiDepot> GetAllTaxiDepots()
        {
            string url = "http://localhost:5204/api/TaxiDepots";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            Console.WriteLine(data);
            var deserializeListTaxiDepotsView = JsonConvert.DeserializeObject<List<TaxiDepotView>>(data);
            if (deserializeListTaxiDepotsView == null)
                return new List<TaxiDepot>();
            List<TaxiDepot> list = new List<TaxiDepot>();
            foreach (var taxiDepotView in deserializeListTaxiDepotsView)
            {
                TaxiDepot temp = new TaxiDepot() { Id = taxiDepotView.Id, Address = taxiDepotView.Address };
                List<TaxiGroup> tempTaxiGroups = new List<TaxiGroup>();
                foreach (var VARIABLE in taxiDepotView.TaxiGroupViews)
                {
                    TaxiGroup taxiGroup = new TaxiGroup()
                    {
                        Id = VARIABLE.Id,
                        TaxiDepot = temp,
                        TaxiDepotId = temp.Id,
                        Quantity = VARIABLE.Quantity,
                        // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                        CarId = VARIABLE.CarId,
                        Car = VARIABLE.Car
                    };
                    tempTaxiGroups.Add(taxiGroup);
                }

                temp.TaxiGroups = tempTaxiGroups;
                list.Add(temp);
            }

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            return list;
        }

        public static void AddTaxiDepot(TaxiDepot taxiDepot)
        {
            List<TaxiGroup> taxiGroups = taxiDepot.TaxiGroups;
            List<TaxiGroupView> taxigroupViews = new List<TaxiGroupView>();
            for (int i = 0; i < taxiGroups.Count; i++)
            {
                TaxiGroupView taxiGroupView = new TaxiGroupView()
                {
                    TaxiDepotId = taxiGroups[i].TaxiDepotId,
                    Car = taxiGroups[i].Car,
                    Id = taxiGroups[i].Id,
                    Quantity = taxiGroups[i].Quantity,
                    CarId = taxiGroups[i].CarId,
                };
                taxigroupViews.Add(taxiGroupView);
            }

            TaxiDepotView temp = new TaxiDepotView()
            {
                Id = taxiDepot.Id,
                Address = taxiDepot.Address,
                TaxiGroupViews = taxigroupViews
            };

            Task<HttpResponseMessage> request = client.PostAsJsonAsync(
                $"api/TaxiDepot", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void ChangeTaxiDepot(TaxiDepot taxiDepot)
        {
            List<TaxiGroup> taxiGroups = taxiDepot.TaxiGroups;
            List<TaxiGroupView> taxiGroupViews = new List<TaxiGroupView>();
            for (int i = 0; i < taxiGroups.Count; i++)
            {
                TaxiGroupView taxiGroupView = new TaxiGroupView()
                {
                    TaxiDepotId = taxiGroups[i].TaxiDepotId,
                    Car = taxiGroups[i].Car,
                    Id = taxiGroups[i].Id,
                    Quantity = taxiGroups[i].Quantity,
                    CarId = taxiGroups[i].CarId,
                };
                taxiGroupViews.Add(taxiGroupView);
            }

            TaxiDepotView temp = new TaxiDepotView()
            {
                Id = taxiDepot.Id,
                Address = taxiDepot.Address,
                TaxiGroupViews = taxiGroupViews
            };

            Task<HttpResponseMessage> request = client.PutAsJsonAsync(
                $"api/TaxiDepot", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void DeleteTaxiDepot(TaxiDepot taxiDepot)
        {
            string url = $"http://localhost:5204/api/TaxiDepot/{taxiDepot.Id}";
            Task<HttpResponseMessage> request =
                new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }
    }
}