using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traductor.models;

namespace traductor.analyzers
{
    class LexicalAnalyzer
    {
        private string auxiliary;
        private int state;
        private int idToken;
        private int idError;
        private int row;
        private int column;

        internal List<Token> ListToken { get; set; }
        internal List<Error> ListError { get; set; }

        public LexicalAnalyzer()
        {
            auxiliary = "";
            state = 0;
            idToken = 0;
            idError = 0;
            row = 1;
            column = 1;

            ListToken = new List<Token>();
            ListError = new List<Error>();
        }

        public void scanner(string entry)
        {
            char character;
            entry += "#";

            for (int i = 0; i < entry.Length; i++)
            {
                character = entry.ElementAt(i);
                switch (state)
                {
                    case 0:
                        // Reserved Word
                        if (char.IsLetter(character))
                        {
                            state = 1;
                            auxiliary += character;
                        }
                        // Digit
                        else if (char.IsDigit(character))
                        {
                            state = 2;
                            auxiliary += character;
                        }
                        // Chain
                        else if (character.Equals('"'))
                        {
                            state = 3;
                            auxiliary += character;
                        }
                        // Assignment AND Equal to
                        else if (character.Equals('='))
                        {
                            state = 4;
                            auxiliary += character;
                        }
                        // Inequality
                        else if (character.Equals('!'))
                        {
                            state = 5;
                            auxiliary += character;
                        }
                        // Less than AND Less than or Equal to
                        else if (character.Equals('<'))
                        {
                            state = 6;
                            auxiliary += character;
                        }
                        // Greater than AND Greater than or Equal to
                        else if (character.Equals('>'))
                        {
                            state = 7;
                            auxiliary += character;
                        }
                        // Single Line Comments AND Multiline Comment
                        else if (character.Equals('/'))
                        {
                            state = 8;
                            auxiliary += character;
                        }
                        // Character
                        else if (character.Equals('\''))
                        {
                            state = 12;
                            auxiliary += character;
                        }
                        // Plus and Increment operators
                        else if (character.Equals('+'))
                        {
                            state = 14;
                            auxiliary += character;
                        }
                        // Minus and Decrement operators
                        else if (character.Equals('-'))
                        {
                            state = 15;
                            auxiliary += character;
                        }
                        // Blanks and line breaks
                        else if (char.IsWhiteSpace(character))
                        {
                            state = 0;
                            auxiliary = "";
                            // Change row and restart columns in line breaks
                            if (character.CompareTo('\n') == 0)
                            {
                                column = 1;
                                row++;
                            }
                        }
                        // Symbol
                        else if (!addSymbol(character))
                        {
                            if (character.Equals('#') && i == (entry.Length - 1))
                            {
                                Console.WriteLine("Lexical analysis completed");
                            }
                            else
                            {
                                Console.WriteLine("Lexical Error: Not Found '" + character + "' in defined patterns");
                                addError(character.ToString());
                                state = 0;
                            }
                        }
                        break;
                    case 1:
                        if (char.IsLetter(character) || char.IsDigit(character))
                        {
                            state = 1;
                            auxiliary += character;
                        }
                        else
                        {
                            addWordReserved();
                            i--;
                        }
                        break;
                    case 2:
                        if (char.IsDigit(character))
                        {
                            state = 2;
                            auxiliary += character;
                        }
                        else if (character.Equals('.'))
                        {
                            state = 13;
                            auxiliary += character;
                        }
                        else
                        {
                            addToken(Token.Type.DIGITO);
                            i--;
                        }
                        break;
                    case 3:
                        if (!character.Equals('"'))
                        {
                            state = 3;
                            auxiliary += character;
                        }
                        else
                        {
                            auxiliary += character;
                            addToken(Token.Type.CADENA);
                        }
                        break;
                    case 4:
                        if (character.Equals('='))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_COMPARACION);
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_IGUAL);
                            i--;
                        }
                        break;
                    case 5:
                        if (character.Equals('='))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_DIFERENTE);
                        }
                        else
                        {
                            Console.WriteLine("Lexical Error: Not Found '" + auxiliary + "' in defined patterns");
                            addError(auxiliary);
                            auxiliary = "";
                            state = 0;
                            i--;
                        }
                        break;
                    case 6:
                        if (character.Equals('='))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_MENOR_IGUAL);
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_MENOR_QUE);
                            i--;
                        }
                        break;
                    case 7:
                        if (character.Equals('='))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_MAYOR_IGUAL);
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_MAYOR_QUE);
                            i--;
                        }
                        break;
                    case 8:
                        if (character.Equals('/'))
                        {
                            state = 9;
                            auxiliary += character;
                        }
                        else if (character.Equals('*'))
                        {
                            state = 10;
                            auxiliary += character;
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_DIVISION);
                            i--;
                        }
                        break;
                    case 9:
                        if (!character.Equals('\n'))
                        {
                            state = 9;
                            auxiliary += character;
                        }
                        else
                        {
                            auxiliary += character;
                            addToken(Token.Type.COMENTARIO_UNA_LINEA);
                        }
                        break;
                    case 10:
                        if (!character.Equals('*'))
                        {
                            state = 10;
                            auxiliary += character;
                        }
                        else
                        {
                            state = 11;
                            auxiliary += character;
                        }
                        break;
                    case 11:
                        if (!character.Equals('/'))
                        {
                            state = 10;
                            auxiliary += character;
                        }
                        else
                        {
                            auxiliary += character;
                            addToken(Token.Type.COMENTARIO_MULTILINEA);
                        }
                        break;
                    case 12:
                        if (!character.Equals('\''))
                        {
                            state = 12;
                            auxiliary += character;
                        }
                        else
                        {
                            auxiliary += character;
                            addToken(Token.Type.CARACTER);
                        }
                        break;
                    case 13:
                        if (char.IsDigit(character))
                        {
                            state = 13;
                            auxiliary += character;
                        }
                        else
                        {
                            addToken(Token.Type.DECIMAL);
                            i--;
                        }
                        break;
                    case 14:
                        if (character.Equals('+'))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_MAS_MAS);
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_MAS);
                            i--;
                        }
                        break;
                    case 15:
                        if (character.Equals('-'))
                        {
                            auxiliary += character;
                            addToken(Token.Type.SIMBOLO_MENOS_MENOS);
                        }
                        else
                        {
                            addToken(Token.Type.SIMBOLO_MENOS);
                            i--;
                        }
                        break;
                }
                column++;
            }
        }

        public bool addSymbol(char character)
        {
            if (character.Equals('{'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_LLAVE_IZQ);
                return true;
            }
            else if (character.Equals('}'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_LLAVE_DCHO);
                return true;
            }
            else if (character.Equals('('))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_PARENTESIS_IZQ);
                return true;
            }
            else if (character.Equals(')'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_PARENTESIS_DCHO);
                return true;
            }
            else if (character.Equals('['))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_CORCHETE_IZQ);
                return true;
            }
            else if (character.Equals(']'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_CORCHETE_DCHO);
                return true;
            }
            else if (character.Equals(','))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_COMA);
                return true;
            }
            else if (character.Equals('.'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_PUNTO);
                return true;
            }
            else if (character.Equals(';'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_PUNTO_Y_COMA);
                return true;
            }
            else if (character.Equals(':'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_DOS_PUNTOS);
                return true;
            }
            else if (character.Equals('*'))
            {
                auxiliary += character;
                addToken(Token.Type.SIMBOLO_MULTIPLICACION);
                return true;
            }
            return false;
        }

        public void addWordReserved()
        {
            if (auxiliary.Equals("bool", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_BOOL);
            }
            else if (auxiliary.Equals("break", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_BREAK);
            }
            else if (auxiliary.Equals("case", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_CASE);
            }
            else if (auxiliary.Equals("char", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_CHAR);
            }
            else if (auxiliary.Equals("class", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_CLASS);
            }
            else if (auxiliary.Equals("default", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_DEFAULT);
            }
            else if (auxiliary.Equals("else", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_ELSE);
            }
            else if (auxiliary.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_FALSE);
            }
            else if (auxiliary.Equals("float", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_FLOAT);
            }
            else if (auxiliary.Equals("for", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_FOR);
            }
            else if (auxiliary.Equals("if", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_IF);
            }
            else if (auxiliary.Equals("int", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_INT);
            }
            else if (auxiliary.Equals("new", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_NEW);
            }
            else if (auxiliary.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_NULL);
            }
            else if (auxiliary.Equals("static", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_STATIC);
            }
            else if (auxiliary.Equals("string", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_STRING);
            }
            else if (auxiliary.Equals("switch", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_SWITCH);
            }
            else if (auxiliary.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_TRUE);
            }
            else if (auxiliary.Equals("void", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_VOID);
            }
            else if (auxiliary.Equals("while", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_WHILE);
            }
            else if (auxiliary.Equals("Console", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_CONSOLE);
            }
            else if (auxiliary.Equals("WriteLine", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_WRITELINE);
            }
            else if (auxiliary.Equals("graficarVector", StringComparison.InvariantCultureIgnoreCase))
            {
                addToken(Token.Type.RESERVADA_GRAFICARVECTOR);
            }
            else
            {
                addToken(Token.Type.IDENTIFICADOR);
            }
        }

        public void addToken(Token.Type type)
        {
            idToken++;
            ListToken.Add(new Token(idToken, row, column - auxiliary.Length, type, auxiliary));
            auxiliary = "";
            state = 0;
        }

        public void addError(string chain)
        {
            idError++;
            ListError.Add(new Error(idError, row, column, chain, "Patrón desconocido"));
        }
    }
}
