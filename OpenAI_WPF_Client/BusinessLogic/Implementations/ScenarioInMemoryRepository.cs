using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public class ScenarioInMemoryRepository : IScenarioRepository
    {
        private List<Scenario> _scenarios;
        public List<Scenario> Scenarios { get => _scenarios; }

        public void ScenarioDataSeeding()
        {
            if (App.Language.Name.Equals("uk-UA"))
            {
                _scenarios = new List<Scenario>
                {
                    new Scenario("Землетрус", Scenario.UseDescriptionTemplate("Землетрус"), Scenario.Severity.Significant),
                    new Scenario("Цунамі", Scenario.UseDescriptionTemplate("Цунамі"), Scenario.Severity.Significant),
                    new Scenario("Ураган", Scenario.UseDescriptionTemplate("Ураган"), Scenario.Severity.Significant),
                    new Scenario("Ядерна небезпека", Scenario.UseDescriptionTemplate("Ядерна небезпека"), Scenario.Severity.Significant),
                };
            }
            else
            {
                _scenarios = new List<Scenario>
                {
                    new Scenario("Earthquake", Scenario.UseDescriptionTemplate("Earthquake"), Scenario.Severity.Significant),
                    new Scenario("Tsunami", Scenario.UseDescriptionTemplate("Tsunami"), Scenario.Severity.Significant),
                    new Scenario("Hurricane", Scenario.UseDescriptionTemplate("Hurricane"), Scenario.Severity.Significant),
                    new Scenario("Nuclear Threat", Scenario.UseDescriptionTemplate("Nuclear Threat"), Scenario.Severity.Significant),
                };
            }
        }

        public ScenarioInMemoryRepository()
        {
            this.ScenarioDataSeeding();
        }

        public void AddScenario(Scenario scenario)
        {
            Scenarios.Add(scenario);
        }
        public void RemoveScenario(Scenario scenario)
        {
            Scenarios.Remove(scenario);
        }
        public void EditScenario(Scenario scenario, Scenario updatedScenario)
        {
            var res = Scenarios.Find(s => s.Equals(scenario));
            res?.UpdateScenario(updatedScenario.Caption, updatedScenario.Description, updatedScenario.SeverityLevel);
        }
    }
}
