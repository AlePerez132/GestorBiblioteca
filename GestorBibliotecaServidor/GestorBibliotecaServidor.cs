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
    class GestorBibliotecaServidor
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(12345), false);
            Console.WriteLine("Registrando el servicio del Gestor biblioteca remoto...");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(GestorBibliotecaComun), "GestorBibliotecaComun",
            WellKnownObjectMode.Singleton);
            Console.WriteLine("Esperando llamadas Remotas...");
            Console.WriteLine("Pulsa Enter para Salir..");
            Console.ReadLine();
        }
    }
}
