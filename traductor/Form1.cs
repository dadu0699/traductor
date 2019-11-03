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
using traductor.util;

namespace traductor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer();
            HTMLReport htmlReport = new HTMLReport();
            string content = textEditor.Text;
            lexicalAnalyzer.scanner(content);

            if (!lexicalAnalyzer.ListError.Any())
            {
                if (lexicalAnalyzer.ListToken.Any())
                {
                    htmlReport.generateReport("listadoTokens.html", lexicalAnalyzer.ListToken);
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\listadoTokens.html"))
                    {
                        Process.Start(Directory.GetCurrentDirectory() + "\\listadoTokens.html");
                    }
                }
            }
            else
            {
                htmlReport.generateReport("listadoTokens.html", lexicalAnalyzer.ListToken);
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
    }
}
