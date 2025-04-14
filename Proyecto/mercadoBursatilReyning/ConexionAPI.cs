using System;
using QuickFix;
using static System.Net.Mime.MediaTypeNames;

namespace IBKRFIXConnection
{
    public class FIXApplication : MessageCracker, IApplication
    {
        public void OnCreate(SessionID sessionId)
        {
            Console.WriteLine($"Sesión creada: {sessionId}");
        }

        public void OnLogon(SessionID sessionId)
        {
            Console.WriteLine($"Sesión iniciada: {sessionId}");
        }

        public void OnLogout(SessionID sessionId)
        {
            Console.WriteLine($"Sesión cerrada: {sessionId}");
        }

        public void FromAdmin(Message message, SessionID sessionId)
        {
            Console.WriteLine($"Mensaje Admin recibido: {message}");
        }

        public void ToAdmin(Message message, SessionID sessionId)
        {
            Console.WriteLine($"Mensaje Admin enviado: {message}");
        }

        public void FromApp(Message message, SessionID sessionId)
        {
            Console.WriteLine($"Mensaje App recibido: {message}");
            Crack(message, sessionId);
        }

        public void ToApp(Message message, SessionID sessionId)
        {
            Console.WriteLine($"Mensaje App enviado: {message}");
        }

        public void OnMessage(QuickFix.FIX44.MarketDataSnapshotFullRefresh msg, SessionID sessionID)
        {
            Console.WriteLine("Datos de mercado recibidos:");
            // Procesar los datos de mercado
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SessionSettings settings = new SessionSettings("fixconfig.cfg");
                IApplication app = new FIXApplication();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);
                IAcceptor acceptor = new ThreadedSocketAcceptor(
                    app, storeFactory, settings, logFactory);

                acceptor.Start();
                Console.WriteLine("Conectado a FIX API. Presiona Q para salir.");

                while (Console.ReadKey().Key != ConsoleKey.Q) { }

                acceptor.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}