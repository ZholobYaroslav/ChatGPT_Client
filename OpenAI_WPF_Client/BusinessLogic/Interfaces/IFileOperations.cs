using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenAI_WPF_Client.BusinessLogic.Interfaces
{
    public interface IFileOperations
    {
        RichTextBox Rtb { get; set; }
        void OpenFile();
        void SaveFileAs();
    }
}
