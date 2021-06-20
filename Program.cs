using System;
using System.Collections.Generic;
using System.IO;

namespace NuevoCompilador
{
    public class AnalLex
    {

        private int numLinea = 1;
        private String lexema;
        private String[] reservadas = { "float", "write", "program", "until", "fi", "read", "bool", "if", "while", "do", "for", "class", "else", "true", "double", "final", "not", "and", "or", "String", "int" };
        private List<String> lstPalabrasReservadas;
        private char car;
        private int val;
        private StreamReader file;
        private TextWriter escribeLex;
        private TextWriter escribeSint;
        private TextWriter escribeErrores;
        private Boolean banderaErrores = false;
        private List<String> colaEntrada;///Cambiar lista a matriz para guardar el numero de linea
        private List<int> lineaToken;///Cambiar lista a matriz para guardar el numero de linea


        //Arbol sintactico
        private Arbol ArbolSintactico = new Arbol();
        private NodoArbol raiz = new NodoArbol();
        private NodoArbol nodoActual = new NodoArbol();

        public AnalLex(String nombreArchivo)
        {
            file = new StreamReader(nombreArchivo);
            escribeLex = new StreamWriter(@"/home/saul/Escritorio/NuevoCompilador/resultadoLexico.txt");
            escribeSint = new StreamWriter(@"/home/saul/Escritorio/NuevoCompilador/resultadoSintactico.txt");
            escribeErrores = new StreamWriter(@"/home/saul/Escritorio/NuevoCompilador/resultadoErrores.txt");
            lstPalabrasReservadas = new List<string>();
            colaEntrada = new List<string>();
            lineaToken = new List<int>();


            foreach (var item in reservadas)
            {
                lstPalabrasReservadas.Add(item);
            }

            lexema = "";

            val = file.Read();
            car = (Char)val;

            q0();
            AnalSint();
            file.Close();
            escribeLex.Close();
            escribeSint.Close();
            escribeErrores.Close();

            System.Environment.Exit(1);
        }

        private void q0()
        {
            if (car == ' ' || car == '\n' || car == '\r' || car == '\t')
            {
                q6();
            }

            if (Char.IsNumber(car))
            {
                q1();
            }

            if (Char.IsLetter(car))
            {
                q4();
            }

            if (car == '"')
            {
                q7();
            }

            if (car == '$')
            {
                q11();
            }

            switch (car)
            {
                case '\r': break;
                case '=':
                case '+':
                case '.':
                case '-':
                case '*':
                case '/':
                case '&':
                case ';': q9(); break;
                case '!'://///diferente de
                case '|':////igual que
                case '^':////menor igual que
                case '°':////mayor igual que
                case '<':
                case '>':
                case '(':
                case ')':
                case ',':
                case '{':
                case '}': q10(); break;
                default: break;
            }

        }

        private void q1()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q1();
            }
            else if (car == '.')
            {
                q2();
            }
            else
            {
                Mostrar("NUMERO");
                q0();
            }
        }

        private void q2()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q3();
            }
        }

        private void q3()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q3();
            }
            else
            {
                Mostrar("NUMERO");
                q0();
            }
        }

        private void q4()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car) || Char.IsLetter(car) || car == '$' || car == '_')
            {
                q4();
            }
            else
            {
                q5();
            }
        }

        private void q5()
        {
            if (lstPalabrasReservadas.Contains(lexema))
            {
                Mostrar("RESERVADA");
                q0();
            }
            else
            {
                Mostrar("Identificador");
                q0();
            }
        }

        public void q6()
        {
            val = file.Read();
            car = (Char)val;

            if (car == ' ' || car == '\n' || car == '\r' || car == '\t')
            {
                if (car == '\n')
                {
                    numLinea++;
                }

                q6();
            }
            else
            {
                q0();
            }

        }

        public void q7()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '"')
            {
                q8();
            }
            else
            {
                q7();
            }
        }

        private void q8()
        {
            lexema = lexema + car;

            Mostrar("CADENA");

            val = file.Read();
            car = (Char)val;

            q0();
        }

        private void q9()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            Mostrar("OPERADOR");
            q0();
        }

        private void q10()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            Mostrar("SIMBOLO");
            q0();
        }

        private void q11()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '/')
            {
                q12();
            }

            if (car == '*')
            {
                q13();
            }
        }

        private void q12()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '\n' || car == '\r')
            {
                Mostrar("COMENTARIO SENCILLO");
                q0();
            }
            else
            {
                q12();
            }
        }


        private void q13()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '\n')
            {
                numLinea++;
            }

            if (car == '*')
            {
                q14();
            }
            else
            {
                q13();
            }
        }

        private void q14()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '/')
            {
                Mostrar("COMENTARIO MULTIPLE");

                val = file.Read();
                car = (Char)val;

                q0();
            }
            else
            {
                q13();
            }

        }

        private void Mostrar(String msg)
        {
            colaEntrada.Add(lexema);
            lineaToken.Add(numLinea);
            escribeLex.WriteLine($"{numLinea} : {msg} ({lexema})");
            lexema = "";
        }
        private void AnalSint()
        {
            raiz = null;
            if (colaEntrada[0] != "program")
            {
                /////////////Lanzamos excepcion
                escribeErrores.WriteLine("Falta la funcion 'program' en la linea " + lineaToken[0]);
            }
            else
            {
                for (int i = 0; i < colaEntrada.Count; i++)
                {
                    if (raiz == null || colaEntrada[i] == "program")
                    {
                        raiz = ArbolSintactico.Insertar(colaEntrada[i], null);//Insertamos 'program'
                        nodoActual = raiz;
                        i++;
                        if (colaEntrada[i] != "{")
                        {
                            escribeErrores.WriteLine("Falta el simbolo '{' cerca de " + colaEntrada[i] + " en la linea " + lineaToken[i]);
                        }
                        else
                        {
                            i++;
                        }
                        if (colaEntrada[i] == "int" || colaEntrada[i] == "float" || colaEntrada[i] == "boolean")
                        {
                            i = listaDeclaracion(ArbolSintactico, nodoActual, colaEntrada, i);
                        }
                        else
                            i = listaSentencia(ArbolSintactico, nodoActual, colaEntrada, i);
                    }
                    else if (banderaErrores != true)
                    {
                        i = listaSentencia(ArbolSintactico, nodoActual, colaEntrada, i);
                    }

                }
            }
            if (banderaErrores != true)
                ArbolSintactico.PreOrden(raiz, escribeSint);
            Console.WriteLine("End of sintax");
        }
        private int listaDeclaracion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            try
            {
                do
                {
                    //Definir TIPO() segun regla gramatical
                    if (arrayValores[i] == "int" || arrayValores[i] == "float" || arrayValores[i] == "boolean")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);

                    }
                    ////////////////
                    //Lista-ID
                    else if (arrayValores[i] != "," && arrayValores[i] != "{" && arrayValores[i] != "}" && arrayValores[i] != ";" && colaEntrada[i] != "if" && colaEntrada[i] != "else" && colaEntrada[i] != "write" && colaEntrada[i] != "do" && colaEntrada[i] != "while" && colaEntrada[i] != "read")
                    {
                        arbol.Insertar(arrayValores[i], nodo);
                        if (arrayValores[i + 1] != "," && arrayValores[i + 1] != ";")
                        {
                            banderaErrores = true;
                            escribeErrores.WriteLine("Simbolo faltante cerca de " + colaEntrada[i] + " en la linea " + lineaToken[i] + ".¿Falta un ',' o ';'?");
                            break;
                        }
                    }
                    //////////////////////////
                    else if (colaEntrada[i] == "{")
                    {
                        banderaErrores = true;
                        escribeErrores.WriteLine("Simbolo '{' irreconocible cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i]);
                    }
                    else if (colaEntrada[i] == "}")
                    {
                        banderaErrores = true;
                        escribeErrores.WriteLine("Simbolo '}' irreconocible cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i]);
                    }
                    i++;
                } while (arrayValores[i] != ";" && (colaEntrada[i] != "if" && colaEntrada[i] != "else" && colaEntrada[i] != "write" && colaEntrada[i] != "do" && colaEntrada[i] != "while" && colaEntrada[i] != "read"));
                if (colaEntrada[i] == "if" || colaEntrada[i] == "do" || colaEntrada[i] == "write" || colaEntrada[i] == "do" || colaEntrada[i] == "while" || colaEntrada[i] == "read")
                    return i - 1;
                else
                    return i;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                banderaErrores = true;
                escribeErrores.WriteLine("Falta un ';' cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i - 1]);
                return i;
            }



        }

        private int listaSentencia(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            switch (colaEntrada[i])
            {
                case "{":
                    if (colaEntrada[i - 1] != "program")///BLOQUE
                    {
                        //i = listaDeclaracion(ArbolSintactico, nodo, colaEntrada, i);
                    }
                    else
                    {
                        i++;
                    }
                    break;
                case "}":
                    i++;
                    break;
                case "if":
                    i = seleccion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "else":
                    nodo = arbol.Insertar(arrayValores[i], nodo);
                    i += 2;
                    i = listaSentencia(arbol, nodo, colaEntrada, i);
                    break;
                case "while":
                    i = iteracion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "do":
                    i = repeticion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "read":
                    i = sentRead(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "write":
                    i = sentWrite(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                default:
                    i = asignacion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
            }
            return i;
        }
        private int seleccion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            NodoArbol nodoAux = new NodoArbol();
            NodoArbol nodoRaiz = new NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            bool errorFlag = false;
            ///Leer B-Expresion
            try
            {
                if (colaEntrada[i] == "(")
                {
                    i++;
                    while (colaEntrada[i] != ")")
                    {
                        if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                        {
                            nodo = arbol.Insertar(arrayValores[i], nodo);
                        }
                        else
                        {
                            nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                            arbol.Insertar(arrayValores[i], nodo);
                            arbol.Insertar(arrayValores[i + 2], nodo);
                            i += 2;
                        }
                        i++;
                    }
                }
                else
                    errorFlag = true;
                ////Leer THEN y luego BLOQUE
                i++;
                while (colaEntrada[i] != "fi")
                {
                    if (colaEntrada[i] == "then")
                    {
                        i++;
                        ////Iniciamos BLOQUE
                        if (colaEntrada[i] == "{")
                        {
                            i++;
                            i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
                        }
                        else
                            errorFlag = true;
                    }
                    else
                        errorFlag = true;
                    if (colaEntrada[i] == "else")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodoRaiz);
                        nodoAux = nodo;
                        i++;
                        ////Llamamos otro BLOQUE
                        if (colaEntrada[i] == "{")
                        {
                            i++;
                            i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
                        }
                        else
                            errorFlag = true;
                    }

                    if (i > colaEntrada.Count)
                    { // INSERTAR ERROR DE FI FALTANTE
                    }
                    i++;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                banderaErrores = true;
                escribeErrores.WriteLine("Falta un ')' cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i - 1]);
                return i;
            }


            return i;
        }
        private int iteracion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            NodoArbol nodoAux = new NodoArbol();
            NodoArbol nodoRaiz = new NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            ///Leer b-expresion
            bool errorFlag = false;
            if (colaEntrada[i] == "(")
            {
                i++;
                while (colaEntrada[i] != ")")
                {
                    if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);
                    }
                    else
                    {
                        nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                        arbol.Insertar(arrayValores[i], nodo);
                        arbol.Insertar(arrayValores[i + 2], nodo);
                        i += 2;
                    }
                    i++;
                }
            }
            else
                errorFlag = true;
            i++;
            ////Llamamos a un bloque
            if (colaEntrada[i] == "{")
            {
                i++;
                i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
            }
            else
                errorFlag = true;
            return i;
        }
        private int repeticion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            NodoArbol nodoAux = new NodoArbol();
            NodoArbol nodoRaiz = new NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            bool errorFlag = false;
            ///Leer b-expresion
            try
            {
                if (colaEntrada[i] == "{")
                {
                    i++;
                    i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
                }
                else
                    errorFlag = true;
                i += 2;
                if (colaEntrada[i] == "}")
                    i++;
                else
                    errorFlag = true;
                if (colaEntrada[i] == "until")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodoRaiz);
                    i++;
                    while (colaEntrada[i] != ";")
                    {
                        if (colaEntrada[i] == "(")
                        {
                            i++;
                            while (colaEntrada[i] != ")")
                            {
                                if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                                {
                                    nodo = arbol.Insertar(arrayValores[i], nodo);
                                }
                                else
                                {
                                    nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                                    arbol.Insertar(arrayValores[i], nodo);
                                    arbol.Insertar(arrayValores[i + 2], nodo);
                                    i += 2;
                                }
                                i++;
                            }
                        }
                        else
                            errorFlag = true;
                        i++;
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                banderaErrores = true;
                escribeErrores.WriteLine("Hay errores que verificar");
                return i;
            }



            return i;


        }
        private int sentRead(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            bool errorFlag = false;
            do
            {
                //Definir TIPO() segun regla gramatical
                if (arrayValores[i] == "read")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodo);

                }
                ////////////////
                //Lista-ID
                else if (arrayValores[i] != "," && arrayValores[i] != "{" && arrayValores[i] != "}" && arrayValores[i] != ";")
                    arbol.Insertar(arrayValores[i], nodo);
                //////////////////////////
                else if (colaEntrada[i] == "{")
                {

                }
                else if (colaEntrada[i] == "}")
                {

                }
                i++;
            } while (arrayValores[i] != ";" && errorFlag == false);
            return i;
        }
        private int sentWrite(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {

            NodoArbol nodoAux = new NodoArbol();
            List<string> listaSimbolos = new List<string>();
            nodo = arbol.Insertar(arrayValores[i], nodo);
            i++;
            bool errorFlag = false;
            ///Leer B-Expresion
            while (colaEntrada[i] != ";")
            {
                if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodo);
                }
                else
                {

                    ///Llamamos expresion
                    nodoAux = expresion(arbol, nodo, arrayValores, i);
                    nodo.Hijo = nodoAux;
                    i = nodoAux.AuxIteracion;
                }
            }


            return i;
        }

        private NodoArbol expresion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            Arbol arbolSec = new Arbol();
            NodoArbol trabajo = new NodoArbol(), temp = new NodoArbol();

            temp = termino(arbolSec, arrayValores, i);
            i = temp.AuxIteracion;

            ///Reinicializamos variables
            NodoArbol nuevo = new NodoArbol();

            if (arrayValores[i] == "+" || arrayValores[i] == "-")
            {
                while (arrayValores[i] == "+" || arrayValores[i] == "-")
                {
                    NodoArbol Aux = new NodoArbol();
                    Aux.Dato = arrayValores[i];
                    Aux.Hijo = arbolSec.raiz;
                    arbolSec.raiz = Aux;
                    nuevo = Aux;
                    i++;
                    temp = factor(arrayValores, i);
                    i++;
                    arbolSec.Insertar(temp.Dato, nuevo);
                    temp = nuevo;

                }
            }
            /*else
            {
                arbolSec.Insertar(temp.Dato, null);
                i++;
            }*/
            temp.AuxIteracion = i;
            return temp;
        }
        private NodoArbol expresion2(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            Arbol arbolSec = new Arbol();
            NodoArbol temp = new NodoArbol();
            temp = termino(arbolSec, arrayValores, i);
            i = temp.AuxIteracion;

            ///Reinicializamos variables
            NodoArbol nuevo = new NodoArbol();
            while (arrayValores[i] == "+" || arrayValores[i] == "-")
            {
                NodoArbol Aux = new NodoArbol();
                Aux.Dato = arrayValores[i];
                if (Aux.Hijo != null)
                    Aux.Hijo = arbolSec.raiz;
                arbolSec.raiz = Aux;
                if (arbolSec.raiz.Hijo == null)
                    arbolSec.Insertar(temp.Dato, arbolSec.raiz);
                nuevo = Aux;
                i++;
                temp = factor(arrayValores, i);
                i++;
                arbolSec.Insertar(temp.Dato, nuevo);
                temp = nuevo;

            }
            temp.AuxIteracion = i;
            return temp;
        }

        private NodoArbol termino(Arbol arbolSec, List<string> arrayValores, int i)
        {
            NodoArbol temp = new NodoArbol(), nuevo = new NodoArbol();////Variables temporales

            temp = factor(arrayValores, i);
            if (arrayValores[i + 1] == "*" || arrayValores[i + 1] == "/")
            {
                i++;
                while (arrayValores[i] == "*" || arrayValores[i] == "/")
                {
                    if (arbolSec.raiz.Dato == "")
                    {
                        nuevo = arbolSec.Insertar(arrayValores[i], null);//Ej. * y / como raiz
                        arbolSec.Insertar(temp.Dato, nuevo);//Hijo como identificador
                        i++;
                        arbolSec.Insertar(factor(arrayValores, i).Dato, nuevo); //Hijo como identificador
                        i++;
                        temp = nuevo;
                    }
                    else
                    {
                        NodoArbol Aux = new NodoArbol();
                        Aux.Dato = arrayValores[i];
                        Aux.Hijo = arbolSec.raiz;
                        arbolSec.raiz = Aux;
                        nuevo = Aux;
                        i++;
                        temp = factor(arrayValores, i);
                        i++;
                        arbolSec.Insertar(temp.Dato, nuevo);
                        temp = nuevo;
                    }
                }
            }
            else
            {
                arbolSec.Insertar(temp.Dato, null);
                i++;
            }


            temp.AuxIteracion = i;
            return temp;
        }
        private NodoArbol factor(List<string> arrayValores, int i)
        {
            NodoArbol temp = new NodoArbol();////Variables temporales
            if (arrayValores[i] == "(")
            {

            }
            else
            {
                temp.Dato = arrayValores[i];////Insertamos valor en nodo
                i++;
            }
            return temp;
        }
        private int asignacion(Arbol arbol, NodoArbol nodo, List<string> arrayValores, int i)
        {
            NodoArbol nodoAux = new NodoArbol(), trabajo;
            i++;
            bool errorFlag = false;
            if (colaEntrada[i] == "=")
            {
                nodo = arbol.Insertar(arrayValores[i], nodo);
                arbol.Insertar(arrayValores[i - 1], nodo);
                i++;
                ///Leer B-Expresion
                while (colaEntrada[i] != ";")
                {
                    if (colaEntrada[i] == "{")
                    {
                        banderaErrores = true;
                        escribeErrores.WriteLine("Simbolo '{' irreconocible cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i]);
                    }
                    else if (colaEntrada[i] == "}")
                    {
                        banderaErrores = true;
                        escribeErrores.WriteLine("Simbolo '}' irreconocible cerca de " + colaEntrada[i - 1] + " en la linea " + lineaToken[i]);
                    }
                    if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);
                    }
                    else
                    {
                        ///Llamamos expresion
                        nodoAux = expresion(arbol, nodo, arrayValores, i);
                        if (nodo.Hijo == null)
                            nodo.Hijo = nodoAux;
                        else
                        {
                            trabajo = new NodoArbol();
                            trabajo = nodo.Hijo;
                            //Avanzamos hasta el ultimo hermano
                            while (trabajo.Hermano != null)
                            {
                                trabajo = trabajo.Hermano;
                            }
                            //Unimos el temporal al ultimo hermano
                            trabajo.Hermano = nodoAux;
                        }
                        i = nodoAux.AuxIteracion;
                    }
                }
            }
            else
            {
                errorFlag = true;
            }

            return i;
        }


    }
}
