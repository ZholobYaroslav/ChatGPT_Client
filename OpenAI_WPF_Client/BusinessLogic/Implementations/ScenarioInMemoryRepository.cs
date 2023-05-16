using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
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
                    new Scenario(1,"Землетрус", Scenario.UseDescriptionTemplate("Землетрус"), Scenario.Severity.Significant),
                    new Scenario(2, "Цунамі", Scenario.UseDescriptionTemplate("Цунамі"), Scenario.Severity.Significant),
                    new Scenario(3, "Ураган", Scenario.UseDescriptionTemplate("Ураган"), Scenario.Severity.Significant),
                    new Scenario(4, "Ядерна небезпека", Scenario.UseDescriptionTemplate("Ядерна небезпека"), Scenario.Severity.Significant),
                };
            }
            else
            {
                _scenarios = new List<Scenario>
                {
                    new Scenario(11, "Earthquake", Scenario.UseDescriptionTemplate("Earthquake"), Scenario.Severity.Significant),
                    new Scenario(12, "Tsunami", Scenario.UseDescriptionTemplate("Tsunami"), Scenario.Severity.Significant),
                    new Scenario(13, "Hurricane", Scenario.UseDescriptionTemplate("Hurricane"), Scenario.Severity.Significant),
                    new Scenario(14, "Nuclear Threat", Scenario.UseDescriptionTemplate("Nuclear Threat"), Scenario.Severity.Significant),
                };
            }
        }

        public ScenarioInMemoryRepository()
        {
            this.ScenarioDataSeeding();
        }

        public void AddScenario(Scenario scenario)
        {
            var ids = Scenarios.Select(s => s.Id).AsEnumerable<int>();
            if (ids.Contains(scenario.Id))
            {
                int idNew = int.MinValue;
                do
                {
                    idNew = new Random(10).Next(0, ids.Max() + 1);
                } while (!ids.Contains(idNew));

                scenario.Id = idNew;
            }
            Scenarios.Add(scenario);
        }
        public void RemoveScenario(int id)
        {
            Scenarios.Remove(Scenarios.First(s => s.Id == id));
        }
        public void EditScenario(int id, Scenario updatedScenario)
        { 
            Scenarios.First(s => s.Id == id).UpdateScenario(updatedScenario);
        }
    }
}
