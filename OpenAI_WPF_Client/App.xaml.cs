using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();
        public static List<CultureInfo> Languages
        {
            get => m_Languages;
        }
        public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US")); //neutral culture for this project
            m_Languages.Add(new CultureInfo("uk-UA"));
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
                    //MessageBox.Show("СРАКА");
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }
    }
}
