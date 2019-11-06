using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traductor.models
{
    class Token
    {
        public enum Type
        {
            RESERVADA_BOOL,
            RESERVADA_BREAK,
            RESERVADA_CASE,
            RESERVADA_CHAR,
            RESERVADA_CLASS,
            RESERVADA_DEFAULT,
            RESERVADA_ELSE,
            RESERVADA_FALSE,
            RESERVADA_FLOAT,
            RESERVADA_FOR,
            RESERVADA_IF,
            RESERVADA_INT,
            RESERVADA_NEW,
            RESERVADA_NULL,
            RESERVADA_STATIC,
            RESERVADA_STRING,
            RESERVADA_SWITCH,
            RESERVADA_TRUE,
            RESERVADA_VOID,
            RESERVADA_WHILE,
            RESERVADA_CONSOLE,
            RESERVADA_WRITELINE,
            SIMBOLO_LLAVE_IZQ,
            SIMBOLO_LLAVE_DCHO,
            SIMBOLO_PARENTESIS_IZQ,
            SIMBOLO_PARENTESIS_DCHO,
            SIMBOLO_CORCHETE_DCHO,
            SIMBOLO_CORCHETE_IZQ,
            SIMBOLO_COMA,
            SIMBOLO_PUNTO,
            SIMBOLO_DOS_PUNTOS,
            SIMBOLO_PUNTO_Y_COMA,
            SIMBOLO_IGUAL,
            SIMBOLO_MAS,
            SIMBOLO_MENOS,
            SIMBOLO_MULTIPLICACION,
            SIMBOLO_DIVISION,
            SIMBOLO_COMPARACION,
            SIMBOLO_DIFERENTE,
            SIMBOLO_MAYOR_QUE,
            SIMBOLO_MENOR_QUE,
            SIMBOLO_MAS_MAS,
            SIMBOLO_MENOS_MENOS,
            SIMBOLO_MAYOR_IGUAL,
            SIMBOLO_MENOR_IGUAL,
            NUMERO,
            IDENTIFICADOR,
            CADENA,
            COMENTARIO_UNA_LINEA,
            COMENTARIO_MULTILINEA
        }

        private int idToken;
        private int row;
        private int column;
        private Type typeToken;
        private string value;

        public int IdToken { get => idToken; set => idToken = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public string Value { get => value; set => this.value = value; }
        public string TypeToken
        {
            get
            {
                switch (typeToken)
                {
                    case Type.RESERVADA_BOOL:
                        return "Reservada bool";
                    case Type.RESERVADA_BREAK:
                        return "Reservada break";
                    case Type.RESERVADA_CASE:
                        return "Reservada case";
                    case Type.RESERVADA_CHAR:
                        return "Reservada char";
                    case Type.RESERVADA_CLASS:
                        return "Reservada class";
                    case Type.RESERVADA_DEFAULT:
                        return "Reservada default";
                    case Type.RESERVADA_ELSE:
                        return "Reservada else";
                    case Type.RESERVADA_FALSE:
                        return "Reservada false";
                    case Type.RESERVADA_FLOAT:
                        return "Reservada float";
                    case Type.RESERVADA_FOR:
                        return "Reservada for";
                    case Type.RESERVADA_IF:
                        return "Reservada if";
                    case Type.RESERVADA_INT:
                        return "Reservada int";
                    case Type.RESERVADA_NEW:
                        return "Reservada new";
                    case Type.RESERVADA_NULL:
                        return "Reservada null";
                    case Type.RESERVADA_STATIC:
                        return "Reservada static";
                    case Type.RESERVADA_STRING:
                        return "Reservada string";
                    case Type.RESERVADA_SWITCH:
                        return "Reservada switch";
                    case Type.RESERVADA_TRUE:
                        return "Reservada true";
                    case Type.RESERVADA_VOID:
                        return "Reservada void";
                    case Type.RESERVADA_WHILE:
                        return "Reservada while";
                    case Type.RESERVADA_CONSOLE:
                        return "Reservada Console";
                    case Type.RESERVADA_WRITELINE:
                        return "Reservada WriteLine";
                    case Type.SIMBOLO_LLAVE_IZQ:
                        return "Simbolo Llave Izquierda";
                    case Type.SIMBOLO_LLAVE_DCHO:
                        return "Simbolo Llave Derecha";
                    case Type.SIMBOLO_CORCHETE_IZQ:
                        return "Simbolo Corchete Izquierdo";
                    case Type.SIMBOLO_CORCHETE_DCHO:
                        return "Simbolo Corchete Derecho";
                    case Type.SIMBOLO_PARENTESIS_IZQ:
                        return "Simbolo Parentesis Izquierdo";
                    case Type.SIMBOLO_PARENTESIS_DCHO:
                        return "Simbolo Parentesis Derecho";
                    case Type.SIMBOLO_COMA:
                        return "Simbolo Coma";
                    case Type.SIMBOLO_PUNTO:
                        return "Simbolo Punto";
                    case Type.SIMBOLO_DOS_PUNTOS:
                        return "Simbolo Dos Puntos";
                    case Type.SIMBOLO_PUNTO_Y_COMA:
                        return "Simbolo Punto y Coma";
                    case Type.SIMBOLO_IGUAL:
                        return "Simbolo Igual";
                    case Type.SIMBOLO_MAS:
                        return "Simbolo Mas";
                    case Type.SIMBOLO_MENOS:
                        return "Simbolo Menos";
                    case Type.SIMBOLO_MULTIPLICACION:
                        return "Simbolo Multiplicación";
                    case Type.SIMBOLO_DIVISION:
                        return "Simbolo División";
                    case Type.SIMBOLO_COMPARACION:
                        return "Simbolo Comparación";
                    case Type.SIMBOLO_DIFERENTE:
                        return "Simbolo Diferenciación";
                    case Type.SIMBOLO_MAYOR_QUE:
                        return "Simbolo Mayor Que";
                    case Type.SIMBOLO_MENOR_QUE:
                        return "Simbolo Menor Que";
                    case Type.SIMBOLO_MAS_MAS:
                        return "Simbolo Mas Mas";
                    case Type.SIMBOLO_MENOS_MENOS:
                        return "Simbolo Menos Menos";
                    case Type.SIMBOLO_MAYOR_IGUAL:
                        return "Simbolo Mayor Igual";
                    case Type.SIMBOLO_MENOR_IGUAL:
                        return "Simbolo Menor Igual";
                    case Type.COMENTARIO_UNA_LINEA:
                        return "Comentario de una Línea";
                    case Type.COMENTARIO_MULTILINEA:
                        return "Comentario Multilínea";
                    case Type.NUMERO:
                        return "Numero";
                    case Type.IDENTIFICADOR:
                        return "Identificador";
                    case Type.CADENA:
                        return "Cadena";
                    default:
                        return "Desconocido";
                }
            }
        }

        public Token(int idToken, int row, int column, Type typeToken, string value)
        {
            this.IdToken = idToken;
            this.Row = row;
            this.Column = column;
            this.typeToken = typeToken;
            this.Value = value;
        }
    }
}
