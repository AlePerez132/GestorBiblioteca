using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaComun
{
    public class GestorBibliotecaComun : MarshalByRefObject, IGestorBiblioteca
    {
        private int nRepo = 0;
        private int idAdmin = -1;
        private int campoOrdenacion = 0;
        private List<Repositorio> repositorios = new List<Repositorio>();
        private List<TLibro> Libros = new List<TLibro>();
        private string nombreFichero = "";
        private readonly Comparison<TLibro> comparadorLibros;

        public GestorBibliotecaComun()
        {
            comparadorLibros = (o1, o2) =>
            {
                int C = 0;
                switch (campoOrdenacion)
                {
                    case 0:
                        C = string.Compare(o1.Isbn, o2.Isbn);
                        break;
                    case 1:
                        C = string.Compare(o1.Titulo, o2.Titulo);
                        break;
                    case 2:
                        C = string.Compare(o1.Autor, o2.Autor);
                        break;
                    case 3:
                        C = o1.Anio.CompareTo(o2.Anio);
                        break;
                    case 4:
                        C = string.Compare(o1.Pais, o2.Pais);
                        break;
                    case 5:
                        C = string.Compare(o1.Idioma, o2.Idioma);
                        break;
                    case 6:
                        C = o1.NoLibros.CompareTo(o2.NoLibros);
                        break;
                    case 7:
                        C = o1.NoPrestados.CompareTo(o2.NoPrestados);
                        break;
                    case 8:
                        C = o1.NoListaEspera.CompareTo(o2.NoListaEspera);
                        break;
                }
                return C;
            };
        }

        public int AbrirRepositorio(int pIda, string pNomFichero)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(pNomFichero, FileMode.Open)))
                    {
                        nombreFichero = pNomFichero;
                        int numLibros = br.ReadInt32();
                        string nombreRepositorio = br.ReadString();
                        string direccionRepositorio = br.ReadString();

                        foreach (var repo in repositorios)
                        {
                            if (repo.Datos.Nombre == nombreRepositorio)
                            {
                                return -2;
                            }
                        }

                        List<TLibro> libros = new List<TLibro>();
                        for (int i = 0; i < numLibros; i++)
                        {
                            string isbn = br.ReadString();
                            string titulo = br.ReadString();
                            string autor = br.ReadString();
                            int anio = br.ReadInt32();
                            string pais = br.ReadString();
                            string idioma = br.ReadString();
                            int noLibros = br.ReadInt32();
                            int noPrestados = br.ReadInt32();
                            int noListaEspera = br.ReadInt32();

                            TLibro libro = new TLibro(isbn, titulo, autor, anio, pais, idioma, noLibros, noPrestados, noListaEspera);
                            libros.Add(libro);
                            Libros.Add(libro);
                        }

                        TDatosRepositorio datos = new TDatosRepositorio(nombreRepositorio, direccionRepositorio, numLibros);
                        Repositorio repositorio = new Repositorio(libros, datos);
                        repositorios.Add(repositorio);
                        nRepo++;
                        Ordenar(idAdmin, campoOrdenacion);
                        return 1;
                    }
                }
                catch (IOException)
                {
                    return 0;
                }
            }
        }

        public int Buscar(int pIda, string pIsbn)
        {
            if (pIda != idAdmin)
            {
                return -2;
            }
            else
            {
                for (int i = 0; i < Libros.Count; i++)
                {
                    if (Libros[i].Isbn == pIsbn)
                    {
                        return i; // Devuelve en un rango de 0 a N-1
                    }
                }
                return -1;
            }
        }

        public int Comprar(int pIda, string pIsbn, int pNoLibros)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else
            {
                int i = 0;
                bool encontrado = false;
                while (!encontrado && i < Libros.Count)
                {
                    if (Libros[i].Isbn == pIsbn)
                    {
                        encontrado = true;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (!encontrado)
                {
                    return 0;
                }
                else
                {
                    while (pNoLibros > 0 && Libros[i].NoListaEspera > 0)
                    {
                        Libros[i].NoListaEspera--; // Libros[i].NoListaEspera -= 1;
                        pNoLibros--;
                    }
                    Libros[i].NoLibros += pNoLibros; // Libros[i].NoLibros += pNoLibros;
                    Ordenar(idAdmin, campoOrdenacion);
                    return 1;
                }
            }
        }

        public int Conexion(string pPasswd)
        {
            if (idAdmin != -1)
            {
                return -1;
            }
            else if (pPasswd != "1234")
            {
                return -2;
            }
            else
            {
                Random r = new Random();
                idAdmin = r.Next(1, 1000001);
                return idAdmin;
            }
        }

        public TDatosRepositorio DatosRepositorio(int pIda, int pRepo)
        {
            if (pIda != idAdmin || pRepo < 1 || pRepo > nRepo)
            {
                return null;
            }
            else
            {
                return repositorios[pRepo - 1].Datos;
            }
        }

        public TLibro Descargar(int pIda, int pRepo, int pPos)
        {
            if (pRepo == -1)
            {
                if (pPos < 1 || pPos > Libros.Count)
                {
                    return null;
                }
                else
                {
                    return Libros[pPos - 1];
                }
            }
            else if (pRepo < 1 || pRepo > nRepo)
            {
                return null;
            }
            else
            {
                var repo = repositorios[pRepo - 1];
                if (pPos < 1 || pPos > repo.Libros.Count)
                {
                    return null;
                }
                else
                {
                    return repo.Libros[pPos - 1];
                }
            }
        }

        public bool Desconexion(int pIda)
        {
            if (pIda != idAdmin)
            {
                return false;
            }
            else
            {
                idAdmin = -1;
                return true;
            }
        }

        public int Devolver(int pPos)
        {
            throw new NotImplementedException();
        }

        public int GuardarRepositorio(int pIda, int pRepo)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else if (string.IsNullOrEmpty(nombreFichero))
            {
                return 0;
            }
            else if (pRepo == -1)
            {
                foreach (var repo in repositorios)
                {
                    if (!GuardarRepositorioHelper(repo))
                    {
                        return 0;
                    }
                }
                return 1;
            }
            else if (pRepo < 1 || pRepo > repositorios.Count)
            {
                return -2;
            }
            else
            {
                if (!GuardarRepositorioHelper(repositorios[pRepo - 1]))
                {
                    return 0;
                }
                return 1;
            }
        }

        private bool GuardarRepositorioHelper(Repositorio repo)
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(nombreFichero, FileMode.Create)))
                {
                    bw.Write(repo.Datos.NumLibros);
                    bw.Write(repo.Datos.Nombre);
                    bw.Write(repo.Datos.Direccion);

                    foreach (var libro in repo.Libros)
                    {
                        bw.Write(libro.Isbn);
                        bw.Write(libro.Titulo);
                        bw.Write(libro.Autor);
                        bw.Write(libro.Anio);
                        bw.Write(libro.Pais);
                        bw.Write(libro.Idioma);
                        bw.Write(libro.NoLibros);
                        bw.Write(libro.NoPrestados);
                        bw.Write(libro.NoListaEspera);
                    }
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        public int NLibros(int pRepo)
        {
            if (pRepo == -1)
            {
                int numLibrosTotal = 0;
                foreach (var repo in repositorios)
                {
                    numLibrosTotal += repo.Datos.NumLibros;
                }
                return numLibrosTotal;
            }
            else
            {
                if (pRepo > repositorios.Count)
                {
                    return -1;
                }
                else
                {
                    return repositorios[pRepo - 1].Datos.NumLibros;
                }
            }
        }

        public int NRepositorios(int pIda)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else
            {
                return nRepo;
            }
        }

        public int NuevoLibro(int pIda, TLibro L, int pRepo)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else if (pRepo < 1 || pRepo > repositorios.Count)
            {
                return -2;
            }
            else
            {
                foreach (var libro in Libros)
                {
                    if (libro.Isbn == L.Isbn)
                    {
                        return 0;
                    }
                }

                repositorios[pRepo - 1].Libros.Add(L);
                Ordenar(idAdmin, campoOrdenacion);
                return 1;
            }
        }

        public bool Ordenar(int pIda, int pCampo)
        {
            if (pIda != idAdmin)
            {
                return false;
            }
            else
            {
                campoOrdenacion = pCampo;
                Libros.Sort(comparadorLibros);
                foreach (var repo in repositorios)
                {
                    repo.Libros.Sort(comparadorLibros);
                }
                return true;
            }
        }

        public int Prestar(int pPos)
        {
            if (pPos < 1 || pPos > Libros.Count)
            {
                return -1;
            }
            else if (Libros[pPos - 1].NoLibros <= 0) // si no hay libros disponibles
            {
                Libros[pPos - 1].NoListaEspera++; // Libros[pPos - 1].NoListaEspera += 1;
                return 0;
            }
            else
            {
                Libros[pPos - 1].NoLibros--; // Libros[pPos - 1].NoLibros -= 1;
                Libros[pPos - 1].NoPrestados++; // Libros[pPos - 1].NoPrestados += 1;
                Ordenar(idAdmin, campoOrdenacion);
                return 1;
            }
        }

        public int Retirar(int pIda, string pIsbn, int pNoLibros)
        {
            if (pIda != idAdmin)
            {
                return -1;
            }
            else
            {
                int i = 0;
                bool encontrado = false;
                while (!encontrado && i < Libros.Count)
                {
                    if (Libros[i].Isbn.Equals(pIsbn))
                    {
                        encontrado = true;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (!encontrado)
                {
                    return 0;
                }
                else
                {
                    if (pNoLibros > Libros[i].NoLibros)
                    {
                        return -2;
                    }
                    else
                    {
                        Libros[i].NoLibros -= pNoLibros; // Libros[i].NoLibros -= pNoLibros;
                        Ordenar(idAdmin, campoOrdenacion);
                        return 1;
                    }
                }
            }
        }
    }

}
