//Conectar a TWS a través de API
using System;
using IBApi;

namespace IBKR_TWS_Example
{
    class Program : EWrapper
    {
        private EClientSocket clientSocket;

        public Program()
        {
            var signal = new EReaderMonitorSignal();
            clientSocket = new EClientSocket(this, signal);
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.ConnectToTWS();
            Console.ReadLine();
        }

        public void ConnectToTWS()
        {
            // TWS debe estar abierto y con API habilitada
            clientSocket.eConnect("127.0.0.1", 7497, 0); // puerto 7496 para live, 7497 para paper
            Console.WriteLine("Conectado a TWS...");
        }

        // Métodos requeridos del EWrapper interface (puedes dejar vacíos los que no uses)
        public void nextValidId(int orderId)
        {
            Console.WriteLine("Next valid order ID: " + orderId);
        }

        public void error(Exception e) => Console.WriteLine("Error: " + e.Message);
        public void error(string str) => Console.WriteLine("Error: " + str);
        public void error(int id, int errorCode, string errorMsg) => Console.WriteLine($"Error {errorCode}: {errorMsg}");

    }
}
