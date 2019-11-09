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

            if (!ListLexicalErrors.Any())
            {
                if (ListToken.Any())
                {
                    parser = new Parser(ListToken);
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
            if (ListToken.Any())
            {
                htmlReport.generateReport("listadoTokens.html", ListToken);
                if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoTokens.html"))
                {
                    Process.Start(Directory.GetCurrentDirectory() + "\\listadoTokens.html");
                }
            }
        }

        private void errorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HTMLReport htmlReport = new HTMLReport();

            if (!ListLexicalErrors.Any())
            {
                if (ListSyntacticErrors.Any())
                {
                    htmlReport.generateReport("listadoErroresSintacticos.html", ListSyntacticErrors);

                    if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoErroresSintacticos.html"))
                    {
                        Process.Start(Directory.GetCurrentDirectory() + "\\listadoErroresSintacticos.html");
                    }
                }
            }
            else
            {
                htmlReport.generateReport("listadoTokens.html", ListToken);
                htmlReport.generateReport("listadoErroresLexicos.html", ListLexicalErrors);

                if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoTokens.html")
                    && File.Exists(Directory.GetCurrentDirectory() + "\\listadoErroresLexicos.html"))
                {
                    Process.Start(Directory.GetCurrentDirectory() + "\\listadoTokens.html");
                    Process.Start(Directory.GetCurrentDirectory() + "\\listadoErroresLexicos.html");
                }
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
    }
}
