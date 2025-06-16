using Servicios;
using System;
using System.ServiceModel;


namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(ServicioPrincipal)))
            {
                try
                {
                    serviceHost.Open();

                    DateTime currentDateTime = DateTime.Now;
                    Console.WriteLine($"Servidor del Juego Ahorcado esta funcionando - Tiempo Inicial: [{currentDateTime}]");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting services: {ex.Message}");
                }
                finally
                {
                    serviceHost.Abort();
                }
            }
        }
    }
}
