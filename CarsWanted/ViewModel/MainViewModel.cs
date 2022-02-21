using CarsWanted.Core;
using CarsWanted.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarsWanted.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
            types = new ObservableCollection<string>();
            types.Add("vehiclenumber");
            types.Add("bodynumber");
            types.Add("chassisnumber");
            types.Add("enginenumber");
            Items = new ObservableCollection<Car>();
        }

        #region Properties

        public ObservableCollection<Car> Items { get; set; }
        public ObservableCollection<string> types { get; set; }

        private object selectedItem;
        public object SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }




        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        #endregion


        #region Commands

        private ICommand command;
        public ICommand Command
        {
            get
            {
                return command ?? (command = new CommandHandler(param => CommandAction(param), true));
            }
        }

        public void CommandAction(object _parpam)
        {
            if (_parpam != null)
            {
                switch (_parpam.ToString())
                {
                    case "search":
                        if (SelectedItem != null)
                        {
                            if (!String.IsNullOrWhiteSpace(SearchText))
                            {
                                Search(SearchText, SelectedItem.ToString());
                            }
                        }
                        break;

                    case "searchWeb":
                        if (SelectedItem != null)
                        {
                            if (!String.IsNullOrWhiteSpace(SearchText))
                            {
                                SearchWeb(SearchText, SelectedItem.ToString());
                            }
                        }
                        break;

                    case "download":
                        Download();
                        break;
                }
            }

            MessageBox.Show("Готово");
        }
 
        #endregion

        public void Search(string value, string index)
        {
            //1 vehiclenumber
            //2 bodynumber
            //3 chassisnumber
            //4 enginenumber
            Items.Clear();
            List<Car> cars = new List<Car>();
            using (Stream stream = File.OpenRead(@"carswanted.json"))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                while (reader.Read())
                {

                    try
                    {
                        Car c = serializer.Deserialize<Car>(reader);
                        if (c != null)
                        {

                            switch (index)
                            {
                                case "vehiclenumber":
                                    if (c.vehiclenumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "bodynumber":
                                    if (c.bodynumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "chassisnumber":
                                    if (c.chassisnumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "enginenumber":
                                    if (c.enginenumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            if (cars.Count > 0)
            {
                foreach (var car in cars)
                {
                    Items.Add(car);
                }
            }
        }

        public void SearchWeb(string value, string index)
        {
            //1 vehiclenumber
            //2 bodynumber
            //3 chassisnumber
            //4 enginenumber
            Items.Clear();
            List<Car> cars = new List<Car>();
            WebClient myWebClient = new();

            using (Stream stream = myWebClient.OpenRead("https://data.gov.ua/dataset/9b0e87e0-eaa3-4f14-9547-03d61b70abb6/resource/e43a82da-89e1-4bbb-820c-bd04ab7a0c89/download/carswanted.json"))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                while (reader.Read())
                {

                    try
                    {
                        Car c = serializer.Deserialize<Car>(reader);
                        Debug.WriteLine(c.vehiclenumber);
                        if (c != null)
                        {

                            switch (index)
                            {
                                case "vehiclenumber":
                                    if (c.vehiclenumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "bodynumber":
                                    if (c.bodynumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "chassisnumber":
                                    if (c.chassisnumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                                case "enginenumber":
                                    if (c.enginenumber.Contains(value))
                                    {
                                        cars.Add(c);
                                    }
                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            if (cars.Count > 0)
            {
                foreach (var car in cars)
                {
                    Items.Add(car);
                }
            }
        }

        private void Download()
        {
            string remoteUri = "https://data.gov.ua/dataset/9b0e87e0-eaa3-4f14-9547-03d61b70abb6/resource/e43a82da-89e1-4bbb-820c-bd04ab7a0c89/download/carswanted.json";
            string fileName = "carswanted.json", myStringWebResource = null;
            WebClient myWebClient = new WebClient();
            myStringWebResource = remoteUri + fileName;
            myWebClient.DownloadFile(myStringWebResource, fileName);
        }
    }
}
