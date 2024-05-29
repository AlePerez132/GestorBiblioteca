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
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(), false);
            llamadaServidor = (GestorBibliotecaComun)Activator.GetObject(typeof(GestorBibliotecaComun), "tcp://localhost:12345/GestorBibliotecaComun");
            Console.WriteLine("Conectado al servidor");
            Console.ReadLine();
        }
    }
}
