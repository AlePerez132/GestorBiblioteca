namespace GestorBibliotecaComun
{
    public class TLibro
    {
        public string Isbn { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Anio { get; set; }
        public string Pais { get; set; }
        public string Idioma { get; set; }
        public int NoLibros { get; set; }
        public int NoPrestados { get; set; }
        public int NoListaEspera { get; set; }

        public TLibro() { }

        public TLibro(string isbn, string titulo, string autor, int anio, string pais, string idioma, int noLibros, int noPrestados, int noListaEspera)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            Anio = anio;
            Pais = pais;
            Idioma = idioma;
            NoLibros = noLibros;
            NoPrestados = noPrestados;
            NoListaEspera = noListaEspera;
        }
    }
}