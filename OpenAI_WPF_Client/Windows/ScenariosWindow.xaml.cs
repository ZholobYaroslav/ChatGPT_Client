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
        private IScenarioRepository _scenarioRepository;
        private ComboBox _scenariosComboBox;
        public ScenariosWindow(IScenarioRepository scenarioRepository, ComboBox comboBox)
        {
            InitializeComponent();
            this._scenarioRepository = scenarioRepository;
            this._scenariosComboBox = comboBox;
            scenariosDataGrid.ItemsSource = this._scenarioRepository.Scenarios;
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

            Scenario scenario = new Scenario(0, caption, description, severity);
            _scenarioRepository.AddScenario(scenario);
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
            Scenario scenarioUpdated = new Scenario(0, caption, description, severity);
            _scenarioRepository.EditScenario(scenarioToUpdate.Id, scenarioUpdated);

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
            _scenarioRepository.RemoveScenario(scenarioToDelete.Id);

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
