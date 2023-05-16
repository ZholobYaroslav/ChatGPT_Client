using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public class Scenario
    {
        public enum Severity
        {
            Low,
            Minor,
            Significant,
            Critical
        }

        public override string ToString()
        {
            return Caption;
        }

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public Severity SeverityLevel { get; set; }

        public Scenario(Scenario scenario)
        {
            Id = scenario.Id;
            Caption = scenario.Caption;
            Description = scenario.Description;
            SeverityLevel = scenario.SeverityLevel;
        }
        public Scenario(int id, string caption, string description, Severity severityLevel)
        {
            Id = id;
            Caption = caption;
            Description = description;
            SeverityLevel = severityLevel;
        }
        public static string UseDescriptionTemplate(string situation)
        {
            return App.Language.Name.Equals("uk-UA") 
                ?
                $"скажи мені що робити, щоб зберегти моє життя у випадку {situation}. " +
                $"Напиши підказки, будь лаконічним і починай свою відповідь одразу із пунктів 1... 2... без жодних узагальнюючих фраз. Вперед!"
                :
                $"tell me what actions I should do to save my life in case of {situation}. " +
                $"Give bullet points, stay concise and start your response already with 1... 2... without any sum-up talks. Go!";
        }
        public void UpdateScenario(Scenario scenario)
        {
            Caption = scenario.Caption;
            Description = scenario.Description;
            SeverityLevel = scenario.SeverityLevel;
        }
    }
}
