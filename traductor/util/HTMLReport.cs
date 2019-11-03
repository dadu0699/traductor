using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traductor.models;

namespace traductor.util
{
    class HTMLReport
    {
        private FileStream fileStream;
        private StreamWriter streamWriter;

        public HTMLReport()
        {
        }

        public void header()
        {
            streamWriter = null;
            streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

            streamWriter.WriteLine("<!doctype html>");
            streamWriter.WriteLine("<html lang=\"es\">");
            streamWriter.WriteLine("<head>");
            streamWriter.WriteLine("<meta charset=\"utf - 8\">");
            streamWriter.WriteLine("<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1, shrink - to - fit = no\">");
            streamWriter.WriteLine("<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.3/css/bootstrap.css\">");
            streamWriter.WriteLine("<link rel=\"stylesheet\" href=\"https://cdn.datatables.net/1.10.20/css/dataTables.bootstrap4.min.css\">");
        }

        public void footer()
        {
            streamWriter.WriteLine("<script src=\"https://code.jquery.com/jquery-3.3.1.js\"></script>");
            streamWriter.WriteLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js\"></script>");
            streamWriter.WriteLine("<script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js\"></script>");
            streamWriter.WriteLine("<script src=\"https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js\"></script>");
            streamWriter.WriteLine("<script src=\"https://cdn.datatables.net/1.10.20/js/dataTables.bootstrap4.min.js\"></script>");
            streamWriter.WriteLine("<script> $(document).ready(function () {$('#example').DataTable();});</script>");
            streamWriter.WriteLine("</body>");
            streamWriter.WriteLine("</html>");

            streamWriter.Close();
            fileStream.Close();
        }

        public void generateReport(string filename, List<Token> listTokens)
        {
            fileStream = null;
            fileStream = new FileStream(filename, FileMode.Create);
            header();

            streamWriter.WriteLine("<title>Tokens</title>");
            streamWriter.WriteLine("</head>");
            streamWriter.WriteLine("<body>");
            streamWriter.WriteLine("<div class=\"container\"><br>");
            streamWriter.WriteLine("<h1>Listado de Tokens " + DateTime.Now.ToString("M/d/yyyy") + "</h1><hr>");
            streamWriter.WriteLine("<table id=\"example\" class=\"table table - striped table - bordered\" style=\"width: 100 % \">");
            streamWriter.WriteLine("<thead><tr><th>#</th><th>Lexema</th><th>Tipo</th><th>Fila</th><th>Columna</th></tr></thead>");
            streamWriter.WriteLine("<tbody>");

            foreach (var item in listTokens)
            {
                streamWriter.WriteLine("<tr>");
                streamWriter.WriteLine("<th>" + item.IdToken + "</th>");
                streamWriter.WriteLine("<th>" + item.Value + "</th>");
                streamWriter.WriteLine("<th>" + item.TypeToken + "</th>");
                streamWriter.WriteLine("<th>" + item.Row + "</th>");
                streamWriter.WriteLine("<th>" + item.Column + "</th>");
                streamWriter.WriteLine("</tr>");
            }

            streamWriter.WriteLine("</tbody>");
            streamWriter.WriteLine("</table>");
            streamWriter.WriteLine("</div>");

            footer();
        }

        public void generateReport(string filename, List<Error> listError)
        {
            fileStream = new FileStream(filename, FileMode.Create);
            header();

            streamWriter.WriteLine("<title>Errores</title>");
            streamWriter.WriteLine("</head>");
            streamWriter.WriteLine("<body>");
            streamWriter.WriteLine("<div class=\"container\"><br>");
            streamWriter.WriteLine("<h1>Listado de Errores " + DateTime.Now.ToString("M/d/yyyy") + "</h1><hr>");
            streamWriter.WriteLine("<table id=\"example\" class=\"table table - striped table - bordered\" style=\"width: 100 % \">");
            streamWriter.WriteLine("<thead><tr><th>#</th><th>Error</th><th>Descripción</th><th>Fila</th><th>Columna</th></tr></thead>");
            streamWriter.WriteLine("<tbody>");

            foreach (var item in listError)
            {
                streamWriter.WriteLine("<tr>");
                streamWriter.WriteLine("<th>" + item.IdError + "</th>");
                streamWriter.WriteLine("<th>" + item.Character + "</th>");
                streamWriter.WriteLine("<th>" + item.Description + "</th>");
                streamWriter.WriteLine("<th>" + item.Row + "</th>");
                streamWriter.WriteLine("<th>" + item.Column + "</th>");
                streamWriter.WriteLine("</tr>");
            }

            streamWriter.WriteLine("</tbody>");
            streamWriter.WriteLine("</table>");
            streamWriter.WriteLine("</div>");

            footer();
        }
    }
}

