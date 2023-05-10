using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenAI_WPF_Client.BusinessLogic;

namespace OpenAI_WPF_Client.Windows
{
    /// <summary>
    /// Interaction logic for ScenariosWindow.xaml
    /// </summary>
    public partial class ScenariosWindow : Window
    {
        private ScenariosRepository _scenariosRepository;
        private ComboBox _scenariosComboBox;
        public ScenariosWindow(ScenariosRepository _scenariosRepository, ComboBox comboBox)
        {
            InitializeComponent();
            this._scenariosRepository = _scenariosRepository;
            this._scenariosComboBox = comboBox;
            scenariosDataGrid.ItemsSource = this._scenariosRepository.Scenarios;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _scenariosComboBox.Items.Refresh();
            Owner.Show();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string caption = scenarioCaptionTB.Text.Trim();
            string description = scenarioTB.Text.Trim();
            if(!Enum.TryParse(severityComboBox.Text, out Scenario.Severity severity))
            {
                MessageBox.Show("Can not TryParse enum");
            }

            Scenario scenario = new Scenario(caption, description, severity);
            _scenariosRepository.AddScenario(scenario);
            scenariosDataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string caption = scenarioCaptionTB.Text.Trim();
            string description = scenarioTB.Text.Trim();
            if (!Enum.TryParse(severityComboBox.Text, out Scenario.Severity severity))
            {
                MessageBox.Show("Can not TryParse enum");
            }
            Scenario? scenarioToUpdate = scenariosDataGrid.SelectedItem as Scenario;
            Scenario scenarioUpdated = new Scenario(caption, description, severity);
            _scenariosRepository.EditScenario(scenarioToUpdate, scenarioUpdated);

            scenariosDataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string caption = scenarioCaptionTB.Text.Trim();
            string description = scenarioTB.Text.Trim();
            if (!Enum.TryParse(severityComboBox.Text, out Scenario.Severity severity))
            {
                MessageBox.Show("Can not TryParse enum");
            }
            Scenario? scenarioToDelete = scenariosDataGrid.SelectedItem as Scenario;
            _scenariosRepository.RemoveScenario(scenarioToDelete);

            scenariosDataGrid.Items.Refresh();
        }

        private void descriptionTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if(App.Language.Name.Equals("uk-UA"))
            {
                scenarioTB.Text = Scenario.UseDescriptionTemplate("ВСТАВТЕ_ВАШУ_СИТУАЦІЮ_ТУТ");
            }
            else
            scenarioTB.Text = Scenario.UseDescriptionTemplate("PASTE_YOUR_SITUATION_HERE");
        }
    }
}
