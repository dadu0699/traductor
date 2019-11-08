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

        public Form1()
        {
            InitializeComponent();

            ListToken = new List<Token>();
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer();
            Parser parser;
            HTMLReport htmlReport = new HTMLReport();
            string content = textEditor.Text;
            lexicalAnalyzer.scanner(content);

            ListToken = lexicalAnalyzer.ListToken;

            if (!lexicalAnalyzer.ListError.Any())
            {
                if (ListToken.Any())
                {
                    parser = new Parser(ListToken);
                }
            }
            else
            {
                htmlReport.generateReport("listadoTokens.html", ListToken);
                htmlReport.generateReport("listadoErrores.html", lexicalAnalyzer.ListError);

                MessageBox.Show("El archivo de entrada posee errores", "Error");

                if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoTokens.html")
                    && File.Exists(Directory.GetCurrentDirectory() + "\\listadoErrores.html"))
                {
                    Process.Start(Directory.GetCurrentDirectory() + "\\listadoTokens.html");
                    Process.Start(Directory.GetCurrentDirectory() + "\\listadoErrores.html");
                }
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
    }
}
