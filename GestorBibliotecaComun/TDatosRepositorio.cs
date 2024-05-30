using System;

namespace GestorBibliotecaComun
{
    [Serializable]
    public class TDatosRepositorio
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int NumLibros { get; set; }

        public TDatosRepositorio(string nombre, string direccion, int numLibros)
        {
            Nombre = nombre;
            Direccion = direccion;
            NumLibros = numLibros;
        }
    }
}