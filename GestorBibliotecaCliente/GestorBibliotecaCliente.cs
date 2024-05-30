using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace GestorBibliotecaComun
{
    class GestorBibliotecaCliente
    {
        static GestorBibliotecaComun llamadaServidor;
        static System.IO.TextReader scInt = Console.In;
        static System.IO.TextReader scString = Console.In;
        static System.IO.TextReader scChar = Console.In;

        static int MenuPrincipal()
        {
            int opcion;
            Console.WriteLine("Menu Principal:");
            Console.WriteLine("1. Entrar como Administrador");
            Console.WriteLine("2. Consultar Libros");
            Console.WriteLine("3. Pedir prestado Libros");
            Console.WriteLine("4. Devolver Libros");
            Console.WriteLine("0. Salir");
            Console.Write("Elija una opcion: ");
            opcion = int.Parse(scInt.ReadLine());
            return opcion;
        }

        static int MenuAdministracion()
        {
            int opcionElegida = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("GESTOR BIBLIOTECARIO 1.0 (M. ADMINISTRACION)");
                Console.WriteLine("********************************************");
                Console.WriteLine("\t1.- Cargar Repositorio");
                Console.WriteLine("\t2.- Guardar Repositorio");
                Console.WriteLine("\t3.- Nuevo libro");
                Console.WriteLine("\t4.- Comprar libros");
                Console.WriteLine("\t5.- Retirar libros");
                Console.WriteLine("\t6.- Ordenar libros");
                Console.WriteLine("\t7.- Buscar libros");
                Console.WriteLine("\t8.- Listar libros");
                Console.WriteLine("\t0.- Salir");
                Console.Write("  Elige opcion:\n");
                opcionElegida = int.Parse(Console.ReadLine());
            } while (opcionElegida < 0 || opcionElegida > 8);
            return opcionElegida;
        }

        static void EsperarEntradaPorConsola()
        {
            Console.WriteLine("Pulse Enter para continuar...");
            scInt.ReadLine();
        }

        static void Main(string[] args)
        {
            int opcionElegida = -1;
            int opcionElegida2 = -1;
            string contrasenha = "";
            int idAdministrador = 0;
            string nombreFichero = "";
            int NumLibros = 0;
            TLibro libro = null;
            string isbn = "";
            string autor = "";
            string titulo = "";
            int anio = 0;
            string pais = "";
            string idioma = "";
            TLibro nuevoLibro = null;//En C lo declarabámos como TNuevo, pero ya no existen esas estructuras en la versión de Java.
            int campoElegido = 0;
            string textoABuscar = "";
            char codigoBusqueda = '\0';
            int posicion = 0;
            string textoCampo = "";
            string isbnCompra = "";
            char confirmacionCompra = '\0';
            int numeroLibrosComprados = 0;
            int numeroLibrosRetirados = 0;
            int numeroRepositorios = 0;
            TDatosRepositorio repositorio = null;
            int repositorioElegido = 0;
            int numeroLibrosInicial = 0;
            bool[] punteroAlgunaCoincidencia = new bool[5]; // Array de booleanos para la búsqueda en todos los campos (*).
            string isbnDevolucion = "";

            //Declaramos variables result como en la práctica de C:
            int result_1 = -1;
            bool result_2 = false;
            int result_3 = -1;
            int result_4 = -1;
            int result_5 = -1;
            int result_6 = -1;
            int result_7 = -1;
            bool result_8 = false;
            int result_9 = -1;
            int result_10 = -1;
            TLibro result_11 = null;
            int result_12 = -1;
            int result_13 = -1;
            int result_14 = -1;
            int result_15 = -1;

            ChannelServices.RegisterChannel(new TcpChannel(), false);
            llamadaServidor = (GestorBibliotecaComun)Activator.GetObject(typeof(GestorBibliotecaComun), "tcp://localhost:12345/GestorBibliotecaComun");
            Console.WriteLine("Conectado al servidor");

            do
            {
                opcionElegida = MenuPrincipal();
                switch (opcionElegida)
                {
                    case 0: //Salir
                        {
                            Console.WriteLine("Cerrando programa...");
                            EsperarEntradaPorConsola();
                            break;
                        }
                    case 1: //Menu Administracion
                        {
                            Console.Write("Por favor inserte la contrasenha de Administracion:\n");
                            contrasenha = Console.ReadLine();
                            Console.WriteLine("Conectando con el sistema...");
                            result_1 = llamadaServidor.Conexion(contrasenha);
                            if (result_1 == -2)
                            {
                                Console.Write("ERROR: la contrasenha introducida es incorrecta\n");
                            }
                            else if (result_1 == -1)
                            {
                                Console.Write("ERROR: ya hay un administrador logueado\n");
                            }
                            else
                            {
                                idAdministrador = result_1;
                                Console.Write("*** Contraseña correcta, puede acceder al menu de Administracion.**\n");
                                EsperarEntradaPorConsola(); // Esperamos a que el usuario pulse cualquier tecla.
                                do
                                {
                                    opcionElegida2 = MenuAdministracion();
                                    switch (opcionElegida2)
                                    {
                                        case 0:
                                            { //Desconexion
                                                result_2 = llamadaServidor.Desconexion(idAdministrador);
                                                if (result_2 == false)
                                                {
                                                    Console.Write("ERROR: el id administrador no coincide con el del servidor\n");
                                                }
                                                else
                                                {
                                                    Console.Write("Ha cerrado sesion con exito\n");
                                                }
                                                break;
                                            }
                                        case 1:
                                            { //Abrir repositorio
                                                Console.Write("Introduce el nombre del fichero de datos:\n");
                                                nombreFichero = Console.ReadLine();
                                                result_3 = llamadaServidor.AbrirRepositorio(idAdministrador, nombreFichero);
                                                if (result_3 == -1)
                                                {
                                                    Console.Write("ERROR: ya hay un administrador logueado\n");
                                                }
                                                else if (result_3 == -2)
                                                {
                                                    Console.Write("ERROR: ya hay un fichero cargado con el mismo nombre\n");
                                                }
                                                else if (result_3 == 0)
                                                {
                                                    Console.Write("ERROR: error al cargar los datos\n");
                                                }
                                                else if (result_3 == 1)
                                                {
                                                    Console.Write("Datos cargados y ordenados correctamente\n");
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 2:
                                            { // Guardar repositorio
                                                numeroRepositorios = llamadaServidor.NRepositorios(idAdministrador);
                                                Console.WriteLine("El numero de repositorios es " + numeroRepositorios);
                                                Console.WriteLine("POS\tNOMBRE\tDIRECCION\tNº LIBROS");
                                                Console.WriteLine("*********************************");
                                                for (int i = 1; i <= numeroRepositorios; i++)
                                                {
                                                    repositorio = llamadaServidor.DatosRepositorio(idAdministrador, i);
                                                    if (repositorio != null)
                                                    {
                                                        Console.WriteLine(i + "\t" + repositorio.Nombre + "\t" + repositorio.Direccion + "\t" + repositorio.NumLibros);
                                                    }
                                                }
                                                Console.WriteLine("Elige repositorio:");
                                                repositorioElegido = Convert.ToInt32(Console.ReadLine());
                                                result_4 = llamadaServidor.GuardarRepositorio(idAdministrador, repositorioElegido);
                                                if (result_4 == -1)
                                                {
                                                    Console.Write("ERROR: ya hay un administrador logueado\n");
                                                }
                                                else if (result_4 == -2)
                                                {
                                                    Console.Write("ERROR: el repositorio no existe\n");
                                                }
                                                else if (result_4 == 0)
                                                {
                                                    Console.Write("ERROR: no se ha podido guardar a fichero el/los repositorios\n");
                                                }
                                                else if (result_4 == 1)
                                                {
                                                    Console.Write("Datos guardados correctamente\n");
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 3:
                                            { //Nuevo libro
                                              // Pedimos los datos del nuevo libro:
                                                Console.Write("Introduce el Isbn:\n");
                                                isbn = Console.ReadLine();
                                                Console.Write("Introduce el Autor:\n");
                                                autor = Console.ReadLine();
                                                Console.Write("Introduce el Titulo:\n");
                                                titulo = Console.ReadLine();
                                                Console.Write("Introduce el anio:\n");
                                                anio = Convert.ToInt32(Console.ReadLine());
                                                Console.Write("Introduce el Pais:\n");
                                                pais = Console.ReadLine();
                                                Console.Write("Introduce el Idioma:\n");
                                                idioma = Console.ReadLine();
                                                Console.Write("Introduce Numero de Libros inicial:\n");
                                                numeroLibrosInicial = Convert.ToInt32(Console.ReadLine());
                                                numeroRepositorios = llamadaServidor.NRepositorios(idAdministrador);
                                                Console.WriteLine("POS\tNOMBRE\tDIRECCION\tNº LIBROS");
                                                Console.WriteLine("*********************************");
                                                for (int i = 1; i <= numeroRepositorios; i++)
                                                {
                                                    repositorio = llamadaServidor.DatosRepositorio(idAdministrador, i);
                                                    if (repositorio != null)
                                                    {
                                                        Console.WriteLine(i + "\t" + repositorio.Nombre + "\t" + repositorio.Direccion + "\t" + repositorio.NumLibros);
                                                    }
                                                }
                                                Console.WriteLine("Elige repositorio:");
                                                repositorioElegido = Convert.ToInt32(Console.ReadLine());
                                                // Reservamos memroia y llenamos la variable libro:
                                                libro = new TLibro();
                                                libro.Isbn = isbn;
                                                libro.Autor = autor;
                                                libro.Titulo = titulo;
                                                libro.Anio = anio;
                                                libro.Pais = pais;
                                                libro.Idioma = idioma;

                                                // Inicializamos los valores no pedidos por consola:
                                                libro.NoLibros = numeroLibrosInicial;
                                                libro.NoListaEspera = 0;
                                                libro.NoPrestados = 0;

                                                result_5 = llamadaServidor.NuevoLibro(idAdministrador, libro, repositorioElegido);

                                                if (result_5 == -1)
                                                {
                                                    Console.Write("ERROR: ya hay un administrador logueado\n");
                                                }
                                                else if (result_5 == 0)
                                                {
                                                    Console.Write("ERROR: ya hay un libro registrado con el ISBN dado\n");
                                                }
                                                else if (result_5 == -2)
                                                {
                                                    Console.Write("ERROR: el repositorio no existe\n");
                                                }
                                                else if (result_5 == 1)
                                                {
                                                    Console.Write("Se ha anhadido el nuevo libro correctamente\n");
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 4:
                                            { //Comprar libro
                                                Console.Write("Introduce Isbn a Buscar:\n");
                                                isbnCompra = Console.ReadLine();
                                                // Por ISBN.
                                                result_10 = llamadaServidor.Buscar(idAdministrador, isbnCompra);
                                                if (result_10 == -1)
                                                {
                                                    Console.Write("ERROR: no se ha encontrado ningun libro\n");
                                                }
                                                else if (result_10 == -2)
                                                {
                                                    Console.Write("ERROR: ya hay un administrador logueado\n");
                                                }
                                                else
                                                {
                                                    // Tenemos la posición del libro buscado en result_10.
                                                    result_11 = llamadaServidor.Descargar(idAdministrador, -1, result_10 + 1);
                                                    if (result_11 == null)
                                                    {
                                                        Console.Write("ERROR: no se ha podido descargar el libro\n");
                                                    }
                                                    else
                                                    {
                                                        libro = result_11;
                                                        Console.Write(libro.Titulo + "\t" + libro.Isbn + "\t\t" + libro.NoLibros + "\t" + libro.NoPrestados + "\t" + libro.NoListaEspera + "\n");
                                                        Console.Write(libro.Autor + "\t" + libro.Pais + "\t" + libro.Idioma + "\t" + libro.Anio + "\n");
                                                        Console.Write("¿ Es este el libro al que desea comprar más unidades (s/n) ?\n");
                                                        confirmacionCompra = Convert.ToChar(Console.ReadLine());
                                                        if (confirmacionCompra != 's') // Si el usuario no ha confirmado con s:
                                                        {
                                                            Console.Write("*** Compra abortada ***\n");
                                                        }
                                                        else // Si el usuario ha confirmado con s:
                                                        {
                                                            Console.Write("Introduce Numero de Libros comprados:\n");
                                                            numeroLibrosComprados = Convert.ToInt32(Console.ReadLine());
                                                            // Pasamos los parámetros:
                                                            result_6 = llamadaServidor.Comprar(idAdministrador, isbnCompra, numeroLibrosComprados);
                                                            if (result_6 == -1)
                                                            {
                                                                Console.Write("ERROR: ya hay un administrador logueado\n");
                                                            }
                                                            else if (result_6 == 0)
                                                            {
                                                                Console.Write("ERROR: no se ha encontrado el libro\n");
                                                            }
                                                            else if (result_6 == 1)
                                                            {
                                                                Console.Write("*** Se han agregado los nuevos ejemplares del libro y los datos están ordenados ***\n");
                                                            }
                                                        }
                                                    }
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 5:
                                            { //Retirar libro.
                                                Console.Write("Introduce Isbn a Buscar:\n");
                                                isbnCompra = Console.ReadLine();
                                                // Por ISBN.
                                                result_10 = llamadaServidor.Buscar(idAdministrador, isbnCompra);
                                                if (result_10 == -2)
                                                {
                                                    Console.Write("ERROR: ya hay un administrador logueado");
                                                }
                                                else if (result_10 == -1)
                                                {
                                                    Console.WriteLine("No se ha encontrado ningun libro");
                                                }
                                                else //mostrar libro
                                                {
                                                    result_11 = llamadaServidor.Descargar(idAdministrador, -1, result_10 + 1);
                                                    if (result_11 != null)
                                                    {
                                                        // Hemos recibido el resultado bien, podemos guardarlo en libro y escribir por pantalla.
                                                        libro = result_11;
                                                        Console.Write((result_10 + 1) + "\t" + libro.Titulo + "\t" + libro.Isbn + "\t\t" + libro.NoLibros + "\t" + libro.NoPrestados + "\t" + libro.NoListaEspera + "\n");
                                                        Console.Write(libro.Autor + "\t" + libro.Pais + "\t" + libro.Idioma + "\t" + libro.Anio + "\n");
                                                    }
                                                }
                                                Console.WriteLine("Es este el libro que quieres retirar(s/n)?");
                                                confirmacionCompra = Convert.ToChar(Console.ReadLine());
                                                if (confirmacionCompra == 's')
                                                {
                                                    Console.WriteLine("Introduce el numero de unidades a retirar:");
                                                    numeroLibrosRetirados = Convert.ToInt32(Console.ReadLine());
                                                    result_7 = llamadaServidor.Retirar(idAdministrador, isbnCompra, numeroLibrosRetirados);
                                                    if (result_7 == -1)
                                                    {
                                                        Console.Write("ERROR: ya hay un administrador logueado");
                                                    }
                                                    else if (result_7 == 0)
                                                    {
                                                        Console.Write("ERROR: no se ha encontrado ningun libro");
                                                    }
                                                    else if (result_7 == 2)
                                                    {
                                                        Console.WriteLine("No hay suficientes ejemplares para retirar");
                                                    }
                                                    else if (result_7 == 1)
                                                    {
                                                        Console.WriteLine("Se han reducido el número de ejemplares disponibles y se han ordenado los datos");
                                                    }
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 6:
                                            { //Ordenar.
                                                Console.WriteLine("0.- Por Isbn");
                                                Console.WriteLine("1.- Por Titulo");
                                                Console.WriteLine("2.- Por Autor");
                                                Console.WriteLine("3.- Por Anho");
                                                Console.WriteLine("4.- Por Pais");
                                                Console.WriteLine("5.- Por Idioma");
                                                Console.WriteLine("6.- Por nº de libros Disponibles");
                                                Console.WriteLine("7.- Por nº de libros Prestados");
                                                Console.WriteLine("8.- Por nº de libros en Espera");
                                                Console.WriteLine("Elige el campo para ordenar los libros:");
                                                campoElegido = Convert.ToInt32(Console.ReadLine());
                                                // Llamamos a ordenar en el servidor:
                                                result_8 = llamadaServidor.Ordenar(idAdministrador, campoElegido);
                                                if (campoElegido >= 0 && campoElegido <= 8)
                                                {
                                                    if (!result_8)
                                                    {
                                                        Console.WriteLine("ERROR: el id administrador no coincide con el del servidor\n");
                                                    }
                                                    else if (result_8)
                                                    {
                                                        Console.WriteLine("Se ha ordenado correctamente el vector\n");
                                                    }
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        case 7:
                                            { //Buscar.
                                                Console.WriteLine("Introduce el texto a buscar:");
                                                textoABuscar = Console.ReadLine();
                                                Console.WriteLine("I.- Por Isbn");
                                                Console.WriteLine("T.- Por Titulo");
                                                Console.WriteLine("A.- Por Autor");
                                                Console.WriteLine("P.- Por Pais");
                                                Console.WriteLine("D.- Por Idioma");
                                                Console.WriteLine("*.- Por todos los campos");
                                                Console.WriteLine("Introduce el codigo de busqueda:");
                                                string input = Console.ReadLine();
                                                codigoBusqueda = input[0];

                                                numeroRepositorios = llamadaServidor.NRepositorios(idAdministrador);
                                                for (int i = 1; i <= numeroRepositorios; i++)
                                                {
                                                    repositorio = llamadaServidor.DatosRepositorio(idAdministrador, i);
                                                    if (repositorio != null)
                                                    {
                                                        Console.WriteLine(i + "\t" + repositorio.Nombre + "\t" + repositorio.Direccion + "\t" + repositorio.NumLibros);
                                                    }
                                                }

                                                result_9 = llamadaServidor.NLibros(-1); // Recogemos el nº de libros del servidor para todos los repos.
                                                Console.WriteLine("0\tTodos los repositorios\t" + result_9);
                                                Console.WriteLine("Elige repositorio:");
                                                repositorioElegido = int.Parse(Console.ReadLine());
                                                if (repositorioElegido == 0)
                                                {
                                                    repositorioElegido = -1; // El servidor entiende todos los repos como -1, el cliente lo ve como 0. De ahí esta condición y asignación.
                                                }
                                                result_9 = llamadaServidor.NLibros(repositorioElegido); // Recogemos el nº de libros del servidor.
                                                if (result_9 == -1)
                                                {
                                                    Console.WriteLine("ERROR: el repositorio no existe");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("POS\tTITULO\tISBN\tDIS\tPRE\tESP");
                                                    Console.WriteLine("\tAUTOR\tPAIS (IDIOMA)\tANIO");
                                                    Console.WriteLine("*********************************************************************************************");

                                                    NumLibros = result_9;

                                                    // Descargaremos cada libro del servidor. Si pasa el filtrado, lo mostraremos por pantalla:
                                                    for (int i = 0; i <= NumLibros; i++)
                                                    {
                                                        libro = llamadaServidor.Descargar(idAdministrador, repositorioElegido, i);
                                                        if (libro != null)
                                                        {
                                                            // Solo mostraremos el libro si aparece la cadena buscada en los campos deseados.
                                                            switch (codigoBusqueda)
                                                            {
                                                                case 'I':
                                                                    punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar); // Isbn:0.
                                                                    punteroAlgunaCoincidencia[1] = false;
                                                                    punteroAlgunaCoincidencia[2] = false;
                                                                    punteroAlgunaCoincidencia[3] = false;
                                                                    punteroAlgunaCoincidencia[4] = false;
                                                                    break;
                                                                case 'T':
                                                                    punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar); // Titulo:1.
                                                                    punteroAlgunaCoincidencia[0] = false;
                                                                    punteroAlgunaCoincidencia[2] = false;
                                                                    punteroAlgunaCoincidencia[3] = false;
                                                                    punteroAlgunaCoincidencia[4] = false;
                                                                    break;
                                                                case 'A':
                                                                    punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar); // Autor:2.
                                                                    punteroAlgunaCoincidencia[0] = false;
                                                                    punteroAlgunaCoincidencia[1] = false;
                                                                    punteroAlgunaCoincidencia[3] = false;
                                                                    punteroAlgunaCoincidencia[4] = false;
                                                                    break;
                                                                case 'P':
                                                                    punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar); // Pais:3.
                                                                    punteroAlgunaCoincidencia[0] = false;
                                                                    punteroAlgunaCoincidencia[1] = false;
                                                                    punteroAlgunaCoincidencia[2] = false;
                                                                    punteroAlgunaCoincidencia[4] = false;
                                                                    break;
                                                                case 'D':
                                                                    punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar); // Idioma:4.
                                                                    punteroAlgunaCoincidencia[0] = false;
                                                                    punteroAlgunaCoincidencia[1] = false;
                                                                    punteroAlgunaCoincidencia[2] = false;
                                                                    punteroAlgunaCoincidencia[3] = false;
                                                                    break;
                                                                case '*':
                                                                    punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar); // Isbn:1.
                                                                    punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar); // Titulo:1.
                                                                    punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar); // Autor:2.
                                                                    punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar); // Pais:3.
                                                                    punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar); // Idioma:4.
                                                                    break;
                                                            }
                                                            if (punteroAlgunaCoincidencia[0] || punteroAlgunaCoincidencia[1] || punteroAlgunaCoincidencia[2] || punteroAlgunaCoincidencia[3] || punteroAlgunaCoincidencia[4])
                                                            {
                                                                Console.Write(libro.Titulo + "\t" + libro.Isbn + "\t\t" + libro.NoLibros + "\t" + libro.NoPrestados + "\t" + libro.NoListaEspera + "\n");
                                                                Console.Write(libro.Autor + "\t" + libro.Pais + "\t" + libro.Idioma + "\t" + libro.Anio + "\n\n");
                                                            }
                                                        }
                                                    }
                                                    EsperarEntradaPorConsola();
                                                }
                                                break;
                                            }
                                        case 8:
                                            { //Listar libro
                                              // Recogemos del servidor el numero de libros:
                                                result_9 = llamadaServidor.NLibros(-1); // Recogemos el nº de libros del servidor (todos los repositorios con *).
                                                if (result_9 == -1)
                                                {
                                                    Console.WriteLine("ERROR: el repositorio no existe\n");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("POS\tTITULO\tISBN\tDIS\tPRE\tESP\n");
                                                    Console.WriteLine("\tAUTOR\tPAIS (IDIOMA)\tANIO\n");
                                                    Console.WriteLine("*********************************************************************************************\n");

                                                    NumLibros = result_9;
                                                    //MODIFICADO TEMPORALMENTE PARA ACCEDER AL PRIMER REPOSITORIO
                                                    for (int i = 1; i <= NumLibros; i++)
                                                    {
                                                        result_11 = llamadaServidor.Descargar(idAdministrador, -1, i);
                                                        if (result_11 != null)
                                                        {
                                                            // Hemos recibido el resultado bien, podemos guardarlo en libro y escribir por pantalla.
                                                            libro = result_11;
                                                            Console.Write(libro.Titulo + "\t" + libro.Isbn + "\t\t" + libro.NoLibros + "\t" + libro.NoPrestados + "\t" + libro.NoListaEspera + "\n");
                                                            Console.Write(libro.Autor + "\t" + libro.Pais + "\t" + libro.Idioma + "\t" + libro.Anio + "\n\n");
                                                        }
                                                    }
                                                }
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                        default:
                                            {
                                                Console.WriteLine("ERROR: Opcion incorrecta");
                                                EsperarEntradaPorConsola();
                                                break;
                                            }
                                    }
                                } while (opcionElegida2 != 0);
                            }
                            break;
                            EsperarEntradaPorConsola();
                        }
                    case 2: //Consulta
                        {
                            Console.Write("Introduce el texto a buscar:\n");
                            textoABuscar = Console.ReadLine();
                            Console.Write("I.- Por Isbn\n");
                            Console.Write("T.- Por Titulo\n");
                            Console.Write("A.- Por Autor\n");
                            Console.Write("P.- Por Pais\n");
                            Console.Write("D.- Por Idioma\n");
                            Console.Write("*.- Por todos los campos\n");
                            Console.Write("Introduce el codigo de busqueda\n");
                            string input = Console.ReadLine();
                            codigoBusqueda = input[0];

                            result_9 = llamadaServidor.NLibros(-1); // Recogemos el nº de libros del servidor para todos los repos.
                            repositorioElegido = -1;
                            result_9 = llamadaServidor.NLibros(repositorioElegido); // Recogemos el nº de libros del servidor.

                            if (result_9 == -1)
                            {
                                Console.WriteLine("ERROR: el repositorio no existe");
                            }
                            else
                            {
                                Console.WriteLine("POS\tTITULO\tISBN\tDIS\tPRE\tESP");
                                Console.WriteLine("\tAUTOR\tPAIS (IDIOMA)\tANIO");
                                Console.WriteLine("*********************************************************************************************");

                                NumLibros = result_9;

                                for (int i = 0; i < NumLibros; i++)
                                {
                                    libro = llamadaServidor.Descargar(idAdministrador, repositorioElegido, i);
                                    if (libro != null)
                                    {
                                        punteroAlgunaCoincidencia = new bool[5];
                                        switch (codigoBusqueda)
                                        {
                                            case 'I':
                                                punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'T':
                                                punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'A':
                                                punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'P':
                                                punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'D':
                                                punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                break;
                                            case '*':
                                                punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar);
                                                break;
                                        }
                                        if (punteroAlgunaCoincidencia[0] || punteroAlgunaCoincidencia[1] || punteroAlgunaCoincidencia[2] || punteroAlgunaCoincidencia[3] || punteroAlgunaCoincidencia[4])
                                        {
                                            Console.WriteLine($"{i}\t{libro.Titulo}\t{libro.Isbn}\t\t{libro.NoLibros}\t{libro.NoPrestados}\t{libro.NoListaEspera}");
                                            Console.WriteLine($"{libro.Autor}\t{libro.Pais}\t{libro.Idioma}\t{libro.Anio}");
                                        }
                                    }
                                }
                            }
                            break;
                            EsperarEntradaPorConsola();
                        }

                    case 3: //Prestamo
                        {
                            Console.Write("Introduce el texto a buscar:\n");
                            textoABuscar = Console.ReadLine();
                            Console.Write("I.- Por Isbn\n");
                            Console.Write("T.- Por Titulo\n");
                            Console.Write("A.- Por Autor\n");
                            Console.Write("P.- Por Pais\n");
                            Console.Write("D.- Por Idioma\n");
                            Console.Write("*.- Por todos los campos\n");
                            Console.Write("Introduce el codigo de busqueda\n");
                            codigoBusqueda = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            result_9 = llamadaServidor.NLibros(-1); // Recogemos el nº de libros del servidor para todos los repos.
                            repositorioElegido = -1;
                            result_9 = llamadaServidor.NLibros(repositorioElegido); // Recogemos el nº de libros del servidor.

                            if (result_9 == -1)
                            {
                                Console.WriteLine("ERROR: el repositorio no existe");
                            }
                            else
                            {
                                Console.WriteLine("POS\tTITULO\tISBN\tDIS\tPRE\tESP");
                                Console.WriteLine("\tAUTOR\tPAIS (IDIOMA)\tANIO");
                                Console.WriteLine("*********************************************************************************************");

                                NumLibros = result_9;

                                for (int i = 0; i < NumLibros; i++)
                                {
                                    libro = llamadaServidor.Descargar(idAdministrador, repositorioElegido, i);
                                    if (libro != null)
                                    {
                                        punteroAlgunaCoincidencia = new bool[5];
                                        switch (codigoBusqueda)
                                        {
                                            case 'I':
                                                punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'T':
                                                punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'A':
                                                punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'P':
                                                punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[4] = false;
                                                break;
                                            case 'D':
                                                punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[0] = false;
                                                punteroAlgunaCoincidencia[1] = false;
                                                punteroAlgunaCoincidencia[2] = false;
                                                punteroAlgunaCoincidencia[3] = false;
                                                break;
                                            case '*':
                                                punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[1] = libro.Titulo.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[2] = libro.Autor.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[3] = libro.Pais.Contains(textoABuscar);
                                                punteroAlgunaCoincidencia[4] = libro.Idioma.Contains(textoABuscar);
                                                break;
                                        }
                                        if (punteroAlgunaCoincidencia[0] || punteroAlgunaCoincidencia[1] || punteroAlgunaCoincidencia[2] || punteroAlgunaCoincidencia[3] || punteroAlgunaCoincidencia[4])
                                        {
                                            Console.WriteLine($"{i}\t{libro.Titulo}\t{libro.Isbn}\t\t{libro.NoLibros}\t{libro.NoPrestados}\t{libro.NoListaEspera}");
                                            Console.WriteLine($"{libro.Autor}\t{libro.Pais}\t{libro.Idioma}\t{libro.Anio}");
                                        }
                                    }
                                }
                            }
                            Console.Write("¿ Quieres sacar algún libro de la biblioteca ? (s/n) ?\n");
                            confirmacionCompra = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            if (confirmacionCompra != 's') // Si el usuario no ha confirmado con s:
                            {
                                Console.WriteLine("*** Prestamo abortado ***");
                            }
                            else // Si el usuario ha confirmado con s:
                            {
                                Console.WriteLine("Introduce la Posicion del libro a solicitar su prestamo:");
                                int posPrestar = int.Parse(Console.ReadLine());
                                result_12 = llamadaServidor.Prestar(posPrestar);
                                if (result_12 == -1)
                                {
                                    Console.WriteLine("ERROR: La posicion introducida no es correcta");
                                }
                                else if (result_12 == 0)
                                {
                                    Console.WriteLine("No hay suficientes libros disponibles, entrando en lista de espera");
                                }
                                else if (result_12 == 1)
                                {
                                    Console.WriteLine("*** Se ha recogido el libro de forma exitosa ***");
                                }
                            }
                            break;
                        }
                    case 4: //Devolucion
                        {
                            Console.Write("Introduce Isbn a Buscar:\n");
                            isbnDevolucion = Console.ReadLine();
                            repositorioElegido = -1;
                            result_9 = llamadaServidor.NLibros(-1); // Recogemos el nº de libros del servidor para todos los repos.
                            NumLibros = result_9;

                            for (int i = 0; i < NumLibros; i++)
                            {
                                libro = llamadaServidor.Descargar(idAdministrador, repositorioElegido, i);
                                if (libro != null)
                                {
                                    punteroAlgunaCoincidencia[0] = libro.Isbn.Contains(isbnDevolucion); // Isbn:0.
                                    if (punteroAlgunaCoincidencia[0])
                                    {
                                        Console.WriteLine($"{i}\t{libro.Titulo}\t{libro.Isbn}\t\t{libro.NoLibros}\t{libro.NoPrestados}\t{libro.NoListaEspera}");
                                        Console.WriteLine($"{libro.Autor}\t{libro.Pais}\t{libro.Idioma}\t{libro.Anio}");
                                    }
                                }
                            }
                            Console.Write("Introduce la posicion del libro a devolver:");
                            int posDevolver = int.Parse(Console.ReadLine());
                            result_13 = llamadaServidor.Devolver(posDevolver);
                            if (result_13 == -1)
                            {
                                Console.WriteLine("ERROR: La posicion introducida no es correcta");
                            }
                            else if (result_13 == 0)
                            {
                                Console.WriteLine("*** Se ha devuelto el libro y se le ha dado a alguien que estaba esperando ***");
                            }
                            else if (result_13 == 1)
                            {
                                Console.WriteLine("*** Se ha devuelto el libro a su estantería ***");
                            }
                            else if (result_13 == 2)
                            {
                                Console.WriteLine("ERROR: no se ha podido devolver el libro porque no hay ni usuarios en lista de espera ni libros prestados");
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR: Opcion incorrecta");
                            break;
                        }
                        EsperarEntradaPorConsola();
                }

            } while (opcionElegida != 0);
        }
    }
}