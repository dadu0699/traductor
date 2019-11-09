using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traductor.models;

namespace traductor.util
{
    class Translate
    {
        private int index;
        private int counterTabulations;
        private Token token;
        private List<Token> ListToken;

        public StringBuilder Code { get; set; }

        public Translate()
        {
            index = 9;
            counterTabulations = 0;
            Code = new StringBuilder();
            ListToken = new List<Token>();
        }

        public void start(List<Token> listToken)
        {
            ListToken = listToken;
            checkInstruction();
        }

        public void checkInstruction()
        {
            assignToken();

            if (token.TypeToken == Token.Type.IDENTIFICADOR)
            {
                asignacion();
            }
            else if (token.TypeToken == Token.Type.COMENTARIO_UNA_LINEA)
            {
                addIndentation();
                singleLineComment();
            }
            else if (token.TypeToken == Token.Type.COMENTARIO_MULTILINEA)
            {
                addIndentation();
                blockComment();
            }


            if (index < ListToken.Count - 1)
            {
                index++;
                checkInstruction();
            }
        }

        public void asignacion()
        {
            if (ListToken[index + 1].TypeToken == Token.Type.SIMBOLO_IGUAL)
            {
                Code.Append(token.Value);
                Code.Append(" ");
                index++;
                assignToken();
                Code.Append(token.Value);
                Code.Append(" ");
                index++;
                assignToken();


                if (token.TypeToken == Token.Type.SIMBOLO_LLAVE_IZQ)
                {
                    Code.Append("[");
                    index++;
                    assignToken();

                    while (token.TypeToken != Token.Type.SIMBOLO_LLAVE_DCHO)
                    {
                        Code.Append(token.Value);
                        index++;
                        assignToken();
                    }
                    Code.Append("]");
                    index++;
                    assignToken();
                }
                else if (token.TypeToken == Token.Type.RESERVADA_NEW)
                {
                    Code.Append("[");
                    index++;
                    assignToken();
                    Code.Append("]");
                    index++;
                    assignToken();
                }
                else
                {
                    while (token.TypeToken != Token.Type.SIMBOLO_PUNTO_Y_COMA)
                    {
                        if (token.TypeToken != Token.Type.SIMBOLO_COMA)
                        {
                            Code.Append(token.Value);
                            if (ListToken[index + 1].TypeToken == Token.Type.SIMBOLO_COMA
                                && ListToken[index + 2].TypeToken == Token.Type.IDENTIFICADOR)
                            {
                                Code.Append("\n");
                            }
                            else
                            {
                                Code.Append(" ");
                            }
                        }
                        index++;
                        assignToken();
                    }
                }
                Code.Append("\n");
            }
        }

        public void blockComment()
        {
            StringBuilder comment = new StringBuilder(token.Value);
            comment.Replace("\t", "");
            comment.Replace("/*", "'''");
            comment.Replace("*/", "'''");

            string[] splitComment = comment.ToString().Split('\n');

            Code.Append(splitComment[0]);
            Code.Append("\n");
            counterTabulations++;
            for (int i = 1; i < (splitComment.Length - 1); i++)
            {
                Code.Append("    ");
                Code.Append(splitComment[i].Trim());
                Code.Append("\n");
            }
            counterTabulations--;
            Code.Append(splitComment[splitComment.Length - 1].Trim());
            Code.Append("\n");
        }

        public void singleLineComment()
        {
            StringBuilder comment = new StringBuilder(token.Value);
            comment.Replace("\n", "");
            comment.Replace("//", "#");
            Code.Append(comment);
            Code.Append("\n");
        }

        public void addIndentation()
        {
            for (int i = 0; i < counterTabulations; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Code.Append(" ");
                }
            }
        }

        public void assignToken()
        {
            token = ListToken[index];
            Console.WriteLine(token.Value);
        }
    }
}
