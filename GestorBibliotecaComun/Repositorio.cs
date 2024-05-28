using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaComun
{
    class Repositorio
    {
        private List<TLibro> libros;
        private TDatosRepositorio datos;

        public Repositorio() { }

        public Repositorio(List<TLibro> libros, TDatosRepositorio datos)
        {
            this.libros = libros;
            this.datos = datos;
        }

        public List<TLibro> Libros
        {
            get { return libros; }
            set { libros = value; }
        }

        public TDatosRepositorio Datos
        {
            get { return datos; }
            set { datos = value; }
        }
    }
}
