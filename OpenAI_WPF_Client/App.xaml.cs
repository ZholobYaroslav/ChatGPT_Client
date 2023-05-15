using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Client;
using OpenAI_WPF_Client.BusinessLogic;
using OpenAI_WPF_Client.BusinessLogic.Interfaces;
using OpenAI_WPF_Client.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OpenAI_WPF_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private static List<CultureInfo> m_Languages = new List<CultureInfo>();
        public static List<CultureInfo> Languages
        {
            get => m_Languages;
        }
        public App()
        {

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices( (hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddHttpClient();
                    services.AddTransient<IScenarioRepository, ScenarioInMemoryRepository>();
                    services.AddTransient<IEmailOperations, EmailOperations>();
                    services.AddTransient<IFileOperations, FileOperations>();
                })
                .Build();

            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US")); //neutral culture for this project
            m_Languages.Add(new CultureInfo("uk-UA"));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();

            await AppHost!.StartAsync();
            var startUpForm = AppHost.Services.GetRequiredService<MainWindow>();
            startUpForm.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

        public static event EventHandler LanguageChanged;
        public static CultureInfo Language
        {
            get => System.Threading.Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dictionary = new ResourceDictionary();
                switch (value.Name)
                {
                    case "uk-UA":
                        dictionary.Source = new Uri($"Resources/lang.{value.Name}.xaml", UriKind.Relative);
                        break;
                    default:
                        dictionary.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                //ResourceDictionary oldDictionary = (from d in Application.Current.Resources.MergedDictionaries
                //                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                //                              select d).First();

                var notNullDicts = Application.Current.Resources.MergedDictionaries
                                                      .Where(d => d is not null)
                                                      .Select(d => d).ToList();
                //var res = "";
               // notNullDicts.ForEach(d => res+="\n"+d.Source.ToString());

                //MessageBox.Show(res, " CultureInfo Language SET");

                var oldDictionary =  notNullDicts.FirstOrDefault(d => d.Source.OriginalString.StartsWith("Resources/lang."));
                                                    //.First(d => d.Source is not null && d.Source.OriginalString.StartsWith("Resources/lang."));

                if (oldDictionary != null)
                {
                    //MessageBox.Show("NOT NULL");
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dictionary);
                }
                else
                {
                    //MessageBox.Show("Very bad");
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }
    }
}
