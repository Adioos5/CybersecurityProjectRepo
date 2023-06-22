using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace Watchdog;

// Define the class for your object
class Politician
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    // Add other properties as needed
}

public partial class SQLWindow : Window
{
    private HttpClient client;
    public SQLWindow()
    {
        var sqlcontext = new SQLWindowContext();
        this.DataContext = sqlcontext;
        InitializeComponent();
        this.Find<Button>("Ready").Click += (object? sender, RoutedEventArgs e) =>
        {
            sqlcontext.Running = true;
        };

        this.Find<Button>("Search").Click += (object? sender, RoutedEventArgs e) =>
        {
            var query = this.Find<TextBox>("Input")!.Text;

            sqlcontext.LastResult = "(Pobieranie...)";
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("query", query);
            try
            {
                string result = webClient.DownloadString("http://10.13.13.103:4000/api/politicians");

                var parsedJson = JObject.Parse(result);
                var token = parsedJson["result"].ToString(Newtonsoft.Json.Formatting.None);

                List<Politician> responseList = JsonConvert.DeserializeObject<List<Politician>>(token);

                sqlcontext.LastResult = $"(Zwrócono polityków: {responseList.Count})\n";
                foreach (Politician obj in responseList)
                    sqlcontext.LastResult += $"   ID: {obj.Id}, Polityk: {obj.Name} {obj.Surname}\n";
            }
            catch (Exception ex)
            {
                sqlcontext.LastResult = $"(Błąd: {ex.Message})";
            }
        };

        this.Find<Button>("AddUnion").Click += (object? sender, RoutedEventArgs e) =>
        {
            this.Find<TextBox>("Input")!.Text = "Adam' UNION SELECT * FROM Politicians --";

            // WebClient webClient = new WebClient();
            // webClient.QueryString.Add("query", "Adam' UNION SELECT * FROM Politicians --");
            // string result = webClient.DownloadString("https://localhost:7221/api/politicians");

            // var parsedJson = JObject.Parse(result);
            // var token = parsedJson["result"].ToString(Newtonsoft.Json.Formatting.None);

            // List<Politician> responseList = JsonConvert.DeserializeObject<List<Politician>>(token);

            // foreach (Politician obj in responseList)
            // {
            //     Console.WriteLine($"Object ID: {obj.Id}, Name: {obj.Name}");
            // }
        };

        this.Find<Button>("DeleteAll").Click += (object? sender, RoutedEventArgs e) =>
        {
            this.Find<TextBox>("Input")!.Text = "Adam'; DELETE FROM Politicians --";

            // WebClient webClient = new WebClient();
            // webClient.QueryString.Add("query", "Adam' UNION SELECT * FROM Politicians --");
            // string result = webClient.DownloadString("https://localhost:7221/api/politicians");

            // var parsedJson = JObject.Parse(result);
            // var token = parsedJson["result"].ToString(Newtonsoft.Json.Formatting.None);

            // List<Politician> responseList = JsonConvert.DeserializeObject<List<Politician>>(token);

            // foreach (Politician obj in responseList)
            // {
            //     Console.WriteLine($"Object ID: {obj.Id}, Name: {obj.Name}");
            // }
        };
    }
}