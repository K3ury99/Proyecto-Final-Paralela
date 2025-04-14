using System;
using IBApi;

namespace IBKRMarketConnection
{
    public class IBKRWrapper : EWrapper
    {
        private EClientSocket clientSocket;
        private int nextOrderId;

        public void Connect()
        {
            clientSocket = new EClientSocket(this);
            clientSocket.eConnect("127.0.0.1", 7497, 0);

            // Esperar conexión
            while (!clientSocket.IsConnected())
            {
                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine("Conectado a TWS API");
        }

        // Implementación de métodos de EWrapper
        public override void nextValidId(int orderId)
        {
            nextOrderId = orderId;
            Console.WriteLine("Next Valid Order ID: " + orderId);
        }

        public override void error(int id, int errorCode, string errorMsg, string advancedOrderRejectJson)
        {
            Console.WriteLine($"Error: ID={id}, Code={errorCode}, Msg={errorMsg}");
        }

        // Otros métodos de EWrapper que necesites implementar...

        public void RequestMarketData(string symbol, string secType = "STK", string exchange = "SMART", string currency = "USD")
        {
            Contract contract = new Contract()
            {
                Symbol = symbol,
                SecType = secType,
                Exchange = exchange,
                Currency = currency
            };

            clientSocket.reqMarketDataType(3); // Delayed data (1 para live, 3 para delayed)
            clientSocket.reqMktData(1, contract, "", false, false, null);
        }

        public override void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            Console.WriteLine($"Tick Price. Ticker Id: {tickerId}, Field: {field}, Price: {price}");
        }

        public override void tickSize(int tickerId, int field, int size)
        {
            Console.WriteLine($"Tick Size. Ticker Id: {tickerId}, Field: {field}, Size: {size}");
        }

        public void Disconnect()
        {
            if (clientSocket != null && clientSocket.IsConnected())
            {
                clientSocket.eDisconnect();
                Console.WriteLine("Desconectado de TWS API");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IBKRWrapper wrapper = new IBKRWrapper();

            try
            {
                wrapper.Connect();
                wrapper.RequestMarketData("AAPL");

                Console.WriteLine("Presiona cualquier tecla para salir...");
                Console.ReadKey();
            }
            finally
            {
                wrapper.Disconnect();
            }
        }
    }
}