using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using traductor.analyzers;
using traductor.models;
using traductor.util;

namespace traductor
{
    public partial class Form1 : Form
    {
        private List<Token> ListToken; 
        private List<Error> ListLexicalErrors; 
        private List<Error> ListSyntacticErrors; 

        public Form1()
        {
            InitializeComponent();

            ListToken = new List<Token>();
            ListLexicalErrors = new List<Error>();
            ListSyntacticErrors = new List<Error>();
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer();
            Parser parser;
            HTMLReport htmlReport = new HTMLReport();
            string content = textEditor.Text;
            lexicalAnalyzer.scanner(content);

            ListToken = lexicalAnalyzer.ListToken;
            ListLexicalErrors = lexicalAnalyzer.ListError;


            MessageBox.Show("Análisis léxico completado");
            if (!ListLexicalErrors.Any())
            {
                if (ListToken.Any())
                {
                    parser = new Parser(ListToken);
                    MessageBox.Show("Análisis sintáctico completado");
                    Console.WriteLine("Syntactic analysis completed");
                    ListSyntacticErrors = parser.ListError;

                    if (ListSyntacticErrors.Any())
                    {
                        MessageBox.Show("El archivo de entrada posee errores sintácticos", "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("El archivo de entrada posee errores lexicos", "Error");
            }
        }

        private void tokenReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HTMLReport htmlReport = new HTMLReport();

            htmlReport.generateReport("listadoTokens.html", ListToken);
            if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoTokens.html"))
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\listadoTokens.html");
            }
        }

        private void errorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HTMLReport htmlReport = new HTMLReport();

            htmlReport.generateReport("listadoErroresLexicos.html", ListLexicalErrors);
            htmlReport.generateReport("listadoErroresSintacticos.html", ListSyntacticErrors);

            if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoErroresLexicos.html"))
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\listadoErroresLexicos.html");
            }

            if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoErroresSintacticos.html"))
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\listadoErroresSintacticos.html");
            }
        }

        private void translateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Translate translate = new Translate();
            if (!ListLexicalErrors.Any() && !ListSyntacticErrors.Any()
                && ListToken.Count > 0 && ListSyntacticErrors.Count == 0)
            {
                translate.start(ListToken);
                translateTextBox.Text = translate.Code.ToString();
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("201801266  Didier Alfredo Domínguez Urías", "Datos");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void limpiarDocumentosRecientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textEditor.Clear();
            translateTextBox.Clear();
            commandLineTextBox.Clear();
            ListToken.Clear();
            ListLexicalErrors.Clear();
            ListSyntacticErrors.Clear();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                FileName = "",
                DefaultExt = "cs",
                Filter = "Archivos CS (*.cs)|*.cs"
            };

            string line = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textEditor.Clear();
                StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                while (line != null)
                {
                    line = streamReader.ReadLine();
                    if (line != null)
                    {
                        textEditor.AppendText(line);
                        textEditor.AppendText(Environment.NewLine);
                    }
                }
                streamReader.Close();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                FileName = "",
                DefaultExt = "cs",
                Filter = "Archivos CS (*.cs)|*.cs"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = saveFileDialog.OpenFile();
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(textEditor.Text);
                streamWriter.Close();
                fileStream.Close();
            }
        }

        private void guardarTraduccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                FileName = "",
                DefaultExt = "py",
                Filter = "Archivos PY (*.py)|*.py"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = saveFileDialog.OpenFile();
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(translateTextBox.Text);
                streamWriter.Close();
                fileStream.Close();
            }
        }
    }
}
