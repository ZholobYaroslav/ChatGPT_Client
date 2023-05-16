using GemBox.Document;
using Microsoft.Win32;
using OpenAI_WPF_Client.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenAI_WPF_Client.BusinessLogic
{
    public class FileOperations : IFileOperations
    {
        public RichTextBox Rtb { get; set; }

        public FileOperations() { }

        public void OpenFile()
        {
            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter =
                        "All Documents (*.docx;*.docm;*.doc;*.dotx;*.dotm;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt)|*.docx;*.docm;*.dotx;*.dotm;*.doc;*.dot;*.htm;*.html;*.rtf;*.xml;*.txt|" +
                        "Word Documents (*.docx)|*.docx|" +
                        "Word Macro-Enabled Documents (*.docm)|*.docm|" +
                        "Word 97-2003 Documents (*.doc)|*.doc|" +
                        "Word Templates (*.dotx)|*.dotx|" +
                        "Word Macro-Enabled Templates (*.dotm)|*.dotm|" +
                        "Word 97-2003 Templates (*.dot)|*.dot|" +
                        "Web Pages (*.htm;*.html)|*.htm;*.html|" +
                        "PDF (*.pdf)|*.pdf|" +
                        "Rich Text Format (*.rtf)|*.rtf|" +
                        "Flat OPC (*.xml)|*.xml|" +
                        "Plain Text (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                using (var stream = new MemoryStream())
                {
                    DocumentModel.Load(dialog.FileName).Save(stream, SaveOptions.RtfDefault);
                    stream.Position = 0;

                    Rtb.Document.Blocks.Clear();
                    var textRange = new TextRange(this.Rtb.Document.ContentStart, this.Rtb.Document.ContentEnd);
                    textRange.Load(stream, DataFormats.Rtf);
                }
            }
        }

        public void SaveFileAs()
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter =
                        "Word Document (*.docx)|*.docx|" +
                        "Word Macro-Enabled Document (*.docm)|*.docm|" +
                        "Word Template (*.dotx)|*.dotx|" +
                        "Word Macro-Enabled Template (*.dotm)|*.dotm|" +
                        "PDF (*.pdf)|*.pdf|" +
                        "XPS Document (*.xps)|*.xps|" +
                        "Web Page (*.htm;*.html)|*.htm;*.html|" +
                        "Single File Web Page (*.mht;*.mhtml)|*.mht;*.mhtml|" +
                        "Rich Text Format (*.rtf)|*.rtf|" +
                        "Flat OPC (*.xml)|*.xml|" +
                        "Plain Text (*.txt)|*.txt|" +
                        "Image (*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp"
            };

            if (dialog.ShowDialog() == true)
            {
                using (MemoryStream _stream = new MemoryStream())
                {
                    var textRange = new TextRange(this.Rtb.Document.ContentStart, this.Rtb.Document.ContentEnd);
                    textRange.Save(_stream, DataFormats.Rtf);
                    _stream.Position = 0;

                    DocumentModel.Load(_stream, LoadOptions.RtfDefault).Save(dialog.FileName);
                }
            }
        }
    }
}
