using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public interface IScenarioRepository
    {
        void ScenarioDataSeeding();
        void AddScenario(Scenario scenario);
        void RemoveScenario(Scenario scenario);
        void EditScenario(Scenario scenario, Scenario updatedScenario);
        List<Scenario> Scenarios { get; }
    }
}
