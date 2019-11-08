using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traductor.models;

namespace traductor.analyzers
{
    class Parser
    {
        private int index;
        private Token preAnalysis;
        private bool syntacticError;
        private List<Token> ListToken;

        public Parser(List<Token> listToken)
        {
            ListToken = listToken;
            index = 0;
            preAnalysis = listToken[index];
            syntacticError = false;
            inicio();
        }

        public void inicio()
        {
            parea(Token.Type.RESERVADA_CLASS);
            parea(Token.Type.IDENTIFICADOR);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            parea(Token.Type.RESERVADA_STATIC);
            parea(Token.Type.RESERVADA_VOID);
            parea(Token.Type.IDENTIFICADOR);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            argumento();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            instP();
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
        }

        public void argumento()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_STRING)
            {
                parea(Token.Type.RESERVADA_STRING);
                parea(Token.Type.SIMBOLO_CORCHETE_IZQ);
                parea(Token.Type.SIMBOLO_CORCHETE_DCHO);
                parea(Token.Type.IDENTIFICADOR);
            }
        }

        public void instP()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_STRING || preAnalysis.TypeToken == Token.Type.RESERVADA_INT
                || preAnalysis.TypeToken == Token.Type.RESERVADA_FLOAT || preAnalysis.TypeToken == Token.Type.RESERVADA_BOOL
                || preAnalysis.TypeToken == Token.Type.RESERVADA_CHAR || preAnalysis.TypeToken == Token.Type.IDENTIFICADOR
                || preAnalysis.TypeToken == Token.Type.RESERVADA_CONSOLE || preAnalysis.TypeToken == Token.Type.RESERVADA_IF
                || preAnalysis.TypeToken == Token.Type.RESERVADA_SWITCH || preAnalysis.TypeToken == Token.Type.RESERVADA_FOR
                || preAnalysis.TypeToken == Token.Type.RESERVADA_WHILE || preAnalysis.TypeToken == Token.Type.COMENTARIO_MULTILINEA
                || preAnalysis.TypeToken == Token.Type.COMENTARIO_UNA_LINEA)
            {
                inst();
                instP();
            }
        }

        public void inst()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_STRING || preAnalysis.TypeToken == Token.Type.RESERVADA_INT
                || preAnalysis.TypeToken == Token.Type.RESERVADA_FLOAT || preAnalysis.TypeToken == Token.Type.RESERVADA_BOOL
                || preAnalysis.TypeToken == Token.Type.RESERVADA_CHAR)
            {
                declaracion();
            }
            else if (preAnalysis.TypeToken == Token.Type.IDENTIFICADOR)
            {
                asignacion();
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_CONSOLE)
            {
                imprimir();
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_IF)
            {
                sentIf();
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_SWITCH)
            {
                sentSwitch();
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_FOR)
            {
                sentFor();
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_WHILE)
            {
                sentWhile();
            }
            else if (preAnalysis.TypeToken == Token.Type.COMENTARIO_MULTILINEA)
            {
                parea(Token.Type.COMENTARIO_MULTILINEA);
            }
            else if (preAnalysis.TypeToken == Token.Type.COMENTARIO_UNA_LINEA)
            {
                parea(Token.Type.COMENTARIO_UNA_LINEA);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'declaracion | asignacion | imprimir | if " +
                    "| for | switch | while | cadena | caracter' instead of '" + preAnalysis.toStringTypeToken + "', '"
                    + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void declaracion()
        {
            tipo();
            declaracionP();
        }

        public void tipo()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_STRING)
            {
                parea(Token.Type.RESERVADA_STRING);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_INT)
            {
                parea(Token.Type.RESERVADA_INT);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_FLOAT)
            {
                parea(Token.Type.RESERVADA_FLOAT);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_BOOL)
            {
                parea(Token.Type.RESERVADA_BOOL);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_CHAR)
            {
                parea(Token.Type.RESERVADA_CHAR);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'string | int | float | bool | char' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void declaracionP()
        {
            if (preAnalysis.TypeToken == Token.Type.IDENTIFICADOR)
            {
                parea(Token.Type.IDENTIFICADOR);
                asigVar();
                dclVariable();
                parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_CORCHETE_IZQ)
            {
                parea(Token.Type.SIMBOLO_CORCHETE_IZQ);
                parea(Token.Type.SIMBOLO_CORCHETE_DCHO);
                parea(Token.Type.IDENTIFICADOR);
                parea(Token.Type.SIMBOLO_IGUAL);
                dclArreglo();
                parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'id | []' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void asigVar()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_IGUAL)
            {
                parea(Token.Type.SIMBOLO_IGUAL);
                valor();
            }
        }

        public void dclVariable()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_COMA)
            {
                parea(Token.Type.SIMBOLO_COMA);
                parea(Token.Type.IDENTIFICADOR);
                asigVar();
                dclVariable();
            }
        }

        public void valor()
        {
            if (preAnalysis.TypeToken == Token.Type.CADENA)
            {
                parea(Token.Type.CADENA);
            }
            else if (preAnalysis.TypeToken == Token.Type.CARACTER)
            {
                parea(Token.Type.CARACTER);
            }
            else if (preAnalysis.TypeToken == Token.Type.IDENTIFICADOR)
            {
                parea(Token.Type.IDENTIFICADOR);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_TRUE)
            {
                parea(Token.Type.RESERVADA_TRUE);
            }
            else if (preAnalysis.TypeToken == Token.Type.RESERVADA_FALSE)
            {
                parea(Token.Type.RESERVADA_FALSE);
            }
            else if (preAnalysis.TypeToken == Token.Type.DIGITO || preAnalysis.TypeToken == Token.Type.DECIMAL
                || preAnalysis.TypeToken == Token.Type.SIMBOLO_PARENTESIS_IZQ)
            {
                expresion();
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'cadena | caracter | true | false | expresion | id' " +
                    "instead of '" + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void expresion()
        {
            termino();
            expresionP();
        }

        public void expresionP()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAS)
            {
                parea(Token.Type.SIMBOLO_MAS);
                termino();
                expresionP();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MENOS)
            {
                parea(Token.Type.SIMBOLO_MENOS);
                termino();
                expresionP();
            }
        }

        public void termino()
        {
            factor();
            terminoP();
        }

        public void terminoP()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MULTIPLICACION)
            {
                parea(Token.Type.SIMBOLO_MULTIPLICACION);
                factor();
                terminoP();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_DIVISION)
            {
                parea(Token.Type.SIMBOLO_DIVISION);
                factor();
                terminoP();
            }
        }

        public void factor()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_PARENTESIS_IZQ)
            {
                parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
                expresion();
                parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            }
            else if (preAnalysis.TypeToken == Token.Type.DIGITO)
            {
                parea(Token.Type.DIGITO);
            }
            else if (preAnalysis.TypeToken == Token.Type.DECIMAL)
            {
                parea(Token.Type.DECIMAL);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected '( | digito | decimal' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void dclArreglo()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_NEW)
            {
                parea(Token.Type.RESERVADA_NEW);
                tipo();
                parea(Token.Type.SIMBOLO_CORCHETE_IZQ);
                expresion();
                parea(Token.Type.SIMBOLO_CORCHETE_DCHO);
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_LLAVE_IZQ)
            {
                parea(Token.Type.SIMBOLO_LLAVE_IZQ);
                valor();
                asigArreglo();
                parea(Token.Type.SIMBOLO_LLAVE_DCHO);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'new | { ' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void asigArreglo()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_COMA)
            {
                parea(Token.Type.SIMBOLO_COMA);
                valor();
                asigArreglo();
            }
        }

        public void asignacion()
        {
            if (preAnalysis.TypeToken == Token.Type.IDENTIFICADOR)
            {
                parea(Token.Type.IDENTIFICADOR);
                asigVarAuDi();
                parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'id' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void iterador()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAS_MAS)
            {
                parea(Token.Type.SIMBOLO_MAS_MAS);
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MENOS_MENOS)
            {
                parea(Token.Type.SIMBOLO_MENOS_MENOS);
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected '++ | --' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void asigVarAuDi()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAS_MAS || preAnalysis.TypeToken == Token.Type.SIMBOLO_MENOS_MENOS)
            {
                iterador();
            }
            else
            {
                asigArregloP();
                parea(Token.Type.SIMBOLO_IGUAL);
                asignacionP();
            }
        }

        public void asigArregloP()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_CORCHETE_IZQ)
            {
                parea(Token.Type.SIMBOLO_CORCHETE_IZQ);
                expresion();
                parea(Token.Type.SIMBOLO_CORCHETE_DCHO);
            }
        }

        public void asignacionP()
        {
            if (preAnalysis.TypeToken == Token.Type.CADENA || preAnalysis.TypeToken == Token.Type.CARACTER
                || preAnalysis.TypeToken == Token.Type.IDENTIFICADOR || preAnalysis.TypeToken == Token.Type.DIGITO
                || preAnalysis.TypeToken == Token.Type.DECIMAL || preAnalysis.TypeToken == Token.Type.SIMBOLO_PARENTESIS_IZQ
                || preAnalysis.TypeToken == Token.Type.RESERVADA_TRUE || preAnalysis.TypeToken == Token.Type.RESERVADA_FALSE)
            {
                valor();
                concatenacion();
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'valor' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void concatenacion()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAS)
            {
                parea(Token.Type.SIMBOLO_MAS);
                valor();
                concatenacion();
            }
        }

        public void imprimir()
        {
            parea(Token.Type.RESERVADA_CONSOLE);
            parea(Token.Type.SIMBOLO_PUNTO);
            parea(Token.Type.RESERVADA_WRITELINE);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            asignacionP();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
        }

        public void sentIf()
        {
            parea(Token.Type.RESERVADA_IF);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            condicion();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            instP();
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
            sentElse();
        }

        public void sentElse()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_ELSE)
            {
                parea(Token.Type.RESERVADA_ELSE);
                parea(Token.Type.SIMBOLO_LLAVE_IZQ);
                instP();
                parea(Token.Type.SIMBOLO_LLAVE_DCHO);
                sentElse();
            }
        }

        public void condicion()
        {
            if (preAnalysis.TypeToken == Token.Type.CADENA || preAnalysis.TypeToken == Token.Type.CARACTER
                || preAnalysis.TypeToken == Token.Type.IDENTIFICADOR || preAnalysis.TypeToken == Token.Type.DIGITO
                || preAnalysis.TypeToken == Token.Type.DECIMAL || preAnalysis.TypeToken == Token.Type.SIMBOLO_PARENTESIS_IZQ
                || preAnalysis.TypeToken == Token.Type.RESERVADA_TRUE || preAnalysis.TypeToken == Token.Type.RESERVADA_FALSE)
            {
                valor();
                condicionP();
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'valor' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void condicionP()
        {
            if (preAnalysis.TypeToken == Token.Type.SIMBOLO_COMPARACION)
            {
                parea(Token.Type.SIMBOLO_COMPARACION);
                valor();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_DIFERENTE)
            {
                parea(Token.Type.SIMBOLO_DIFERENTE);
                valor();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAYOR_QUE)
            {
                parea(Token.Type.SIMBOLO_MAYOR_QUE);
                valor();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MENOR_QUE)
            {
                parea(Token.Type.SIMBOLO_MENOR_QUE);
                valor();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MAYOR_IGUAL)
            {
                parea(Token.Type.SIMBOLO_MAYOR_IGUAL);
                valor();
            }
            else if (preAnalysis.TypeToken == Token.Type.SIMBOLO_MENOR_IGUAL)
            {
                parea(Token.Type.SIMBOLO_MENOR_IGUAL);
                valor();
            }
        }

        public void sentSwitch()
        {
            parea(Token.Type.RESERVADA_SWITCH);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            valor();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            caseP();
            def();
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
        }

        public void caseP()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_CASE)
            {
                cas();
                caseP();
            }
        }

        public void cas()
        {
            parea(Token.Type.RESERVADA_CASE);
            valor();
            parea(Token.Type.SIMBOLO_DOS_PUNTOS);
            instP();
            parea(Token.Type.RESERVADA_BREAK);
            parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
        }

        public void def()
        {
            parea(Token.Type.RESERVADA_DEFAULT);
            parea(Token.Type.SIMBOLO_DOS_PUNTOS);
            instP();
            parea(Token.Type.RESERVADA_BREAK);
            parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
        }

        public void sentFor()
        {
            parea(Token.Type.RESERVADA_FOR);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            inicializador();
            parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
            condicion();
            parea(Token.Type.SIMBOLO_PUNTO_Y_COMA);
            iterador();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            instP();
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
        }

        public void inicializador()
        {
            if (preAnalysis.TypeToken == Token.Type.RESERVADA_STRING || preAnalysis.TypeToken == Token.Type.RESERVADA_INT
                || preAnalysis.TypeToken == Token.Type.RESERVADA_FLOAT || preAnalysis.TypeToken == Token.Type.RESERVADA_BOOL
                || preAnalysis.TypeToken == Token.Type.RESERVADA_CHAR)
            {
                declaracion();
            }
            else if (preAnalysis.TypeToken == Token.Type.IDENTIFICADOR)
            {
                asignacion();
            }
            else
            {
                Console.WriteLine("Syntactic Error: Was expected 'declaracion | asignacion' instead of '"
                    + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                syntacticError = true;
            }
        }

        public void sentWhile()
        {
            parea(Token.Type.RESERVADA_WHILE);
            parea(Token.Type.SIMBOLO_PARENTESIS_IZQ);
            condicion();
            parea(Token.Type.SIMBOLO_PARENTESIS_DCHO);
            parea(Token.Type.SIMBOLO_LLAVE_IZQ);
            instP();
            parea(Token.Type.SIMBOLO_LLAVE_DCHO);
        }

        public void parea(Token.Type type)
        {
            Console.WriteLine(preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
            if (syntacticError)
            {
                if (index < ListToken.Count - 1)
                {
                    index++;
                    preAnalysis = ListToken[index];
                    if (preAnalysis.TypeToken == Token.Type.SIMBOLO_PUNTO_Y_COMA)
                    {
                        syntacticError = false;
                    }
                }
            }
            else
            {
                if (index < ListToken.Count - 1)
                {
                    if (preAnalysis.TypeToken.Equals(type))
                    {
                        index++;
                        preAnalysis = ListToken[index];
                    }
                    else
                    {
                        Console.WriteLine("Syntactic Error: Was expected '" + type + "' instead of '"
                            + preAnalysis.toStringTypeToken + "', '" + preAnalysis.Value + "'");
                        syntacticError = true;
                    }
                }
            }
        }
    }
}
