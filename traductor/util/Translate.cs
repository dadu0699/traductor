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
        private StringBuilder condition;
        private StringBuilder iterator;
        private Token token;
        private List<Token> ListToken;

        public StringBuilder Code { get; set; }

        public Translate()
        {
            index = 0;
            counterTabulations = 0;
            Code = new StringBuilder();
            condition = new StringBuilder();
            iterator = new StringBuilder();
            ListToken = new List<Token>();
        }

        public void start(List<Token> listToken)
        {
            ListToken = listToken;
            if (ListToken[7].TypeToken != Token.Type.RESERVADA_STRING)
            {
                index = 9;
            }
            else
            {
                index = 13;
            }
            checkInstruction();
        }

        public void checkInstruction()
        {
            assignToken();

            if (token.TypeToken == Token.Type.IDENTIFICADOR)
            {
                addIndentation();
                asignacion();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_CONSOLE)
            {
                addIndentation();
                print();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_IF)
            {
                addIndentation();
                sentIf();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_ELSE)
            {
                addIndentation();
                sentElse();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_SWITCH)
            {
                addIndentation();
                sentSwitch();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_CASE)
            {
                addIndentation();
                sentCase();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_DEFAULT)
            {
                addIndentation();
                sentDefault();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_FOR)
            {
                addIndentation();
                sentFor();
            }
            else if (token.TypeToken == Token.Type.RESERVADA_WHILE)
            {
                addIndentation();
                sentWhile();
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
            else if (token.TypeToken == Token.Type.RESERVADA_GRAFICARVECTOR)
            {
                addIndentation();
                while (token.TypeToken != Token.Type.SIMBOLO_PUNTO_Y_COMA)
                {
                    Code.Append(token.Value);
                    index++;
                    assignToken();
                }
                Code.Append("\n");
            }

            // FIX
            if (iterator.Length != 0)
            {
                counterTabulations++;
                addIndentation();
                Code.Append(iterator);
                Code.Append("\n");
                iterator.Clear();
                counterTabulations--;
            }

            if (token.TypeToken == Token.Type.SIMBOLO_LLAVE_IZQ)
            {
                counterTabulations++;
            }
            else if (token.TypeToken == Token.Type.SIMBOLO_LLAVE_DCHO
                || token.TypeToken == Token.Type.RESERVADA_BREAK)
            {
                counterTabulations--;
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
                    index+=2;
                    assignToken(); 
                    Code.Append(token.Value);
                    index++;
                    assignToken();

                    while (token.TypeToken != Token.Type.SIMBOLO_CORCHETE_DCHO)
                    {
                        Code.Append(token.Value);
                        index++;
                        assignToken();
                    }

                    Code.Append(token.Value);
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

        public void print()
        {
            index += 3;
            assignToken();
            Code.Append("print");
            while (token.TypeToken != Token.Type.SIMBOLO_PUNTO_Y_COMA)
            {
                Code.Append(token.Value);
                index++;
                assignToken();
            }
            Code.Append("\n");
        }

        public void sentIf()
        {
            Code.Append(token.Value);
            Code.Append(" ");
            index += 2;
            assignToken();
            while (token.TypeToken != Token.Type.SIMBOLO_PARENTESIS_DCHO)
            {
                Code.Append(token.Value);
                Code.Append(" ");
                index++;
                assignToken();
            }
            Code.Append(":");
            Code.Append("\n");
        }

        public void sentElse()
        {
            Code.Append(token.Value);
            Code.Append(":");
            Code.Append("\n");
        }

        public void sentSwitch()
        {
            condition.Clear();
            index += 2;
            assignToken();
            while (token.TypeToken != Token.Type.SIMBOLO_PARENTESIS_DCHO)
            {
                condition.Append(token.Value);
                condition.Append(" ");
                index++;
                assignToken();
            }

            index += 2;
            assignToken();
            if (token.TypeToken == Token.Type.RESERVADA_CASE)
            {
                Code.Append("if " + condition + "== ");
                index++;
                assignToken();
                Code.Append(token.Value);
                Code.Append(":");
                index++;
                assignToken();
                counterTabulations++;
            }
            Code.Append("\n");
        }

        public void sentCase()
        {
            Code.Append("elif " + condition + "== ");
            index++;
            assignToken();
            Code.Append(token.Value);
            Code.Append(":");
            index++;
            assignToken();
            counterTabulations++;
            Code.Append("\n");
        }

        public void sentDefault()
        {
            Code.Append("else:");
            counterTabulations++;
            Code.Append("\n");
        }

        public void sentFor()
        {
            index += 3;
            assignToken();
            asignacion();

            addIndentation();
            Code.Append("while ");
            index++;
            assignToken();
            while (token.TypeToken != Token.Type.SIMBOLO_PUNTO_Y_COMA)
            {
                Code.Append(token.Value);
                Code.Append(" ");
                index++;
                assignToken();
            }

            index++;
            assignToken();
            while (token.TypeToken != Token.Type.SIMBOLO_PARENTESIS_DCHO)
            {
                if (token.TypeToken == Token.Type.SIMBOLO_MAS_MAS)
                {
                    iterator.Append("+= 1");
                }
                else if (token.TypeToken == Token.Type.SIMBOLO_MENOS_MENOS)
                {
                    iterator.Append("-= 1");
                }
                else
                {
                    iterator.Append(token.Value);
                }

                iterator.Append(" ");
                index++;
                assignToken();
            }

            Code.Append(":");
            Code.Append("\n");
        }

        public void sentWhile()
        {
            Code.Append(token.Value);
            Code.Append(" ");
            index += 2;
            assignToken();
            while (token.TypeToken != Token.Type.SIMBOLO_PARENTESIS_DCHO)
            {
                Code.Append(token.Value);
                Code.Append(" ");
                index++;
                assignToken();
            }
            Code.Append(":");
            Code.Append("\n");
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
        }
    }
}
