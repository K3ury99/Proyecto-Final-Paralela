using System;
using IBApi;
using QuickFix;


namespace IBKR_MarketData
{
    class Program : EWrapper
    {
        private EClientSocket clientSocket;
        private int nextOrderId;

        public Program()
        {
            var signal = new EReaderMonitorSignal();
            clientSocket = new EClientSocket(this, signal);
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.ConnectToTWS();
            Console.ReadLine(); // Mantener consola abierta
        }

        public void ConnectToTWS()
        {
            clientSocket.eConnect("127.0.0.1", 7497, 0); // Paper Trading
            Console.WriteLine("Conectando a TWS...");

            var reader = new EReader(clientSocket, new EReaderMonitorSignal());
            reader.Start();

            new System.Threading.Thread(() =>
            {
                while (clientSocket.IsConnected())
                {
                    reader.signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();

            System.Threading.Thread.Sleep(1000); // Esperar conexión

            // Solicitar datos de mercado
            Contract contract = GetStockContract("AAPL");
            clientSocket.reqMktData(1, contract, "", false, false, null);
        }

        public Contract GetStockContract(string symbol)
        {
            return new Contract()
            {
                Symbol = symbol,
                SecType = "STK",
                Currency = "USD",
                Exchange = "SMART"
            };
        }

        // la parte de Callbacks obligatorios 

        public void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            string priceType = field switch
            {
                1 => "Bid Price",
                2 => "Ask Price",
                4 => "Last Price",
                _ => "Otro"
            };

            Console.WriteLine($"[{tickerId}] {priceType}: {price}");
        }

        public void tickSize(int tickerId, int field, int size)
        {
            Console.WriteLine($"[{tickerId}] Tamaño: {size}");
        }

        public void nextValidId(int orderId)
        {
            nextOrderId = orderId;
            Console.WriteLine($"Next valid order ID: {orderId}");
        }

        public void error(Exception e) => Console.WriteLine("ERROR: " + e.Message);
        public void error(string str) => Console.WriteLine("ERROR: " + str);
        public void error(int id, int errorCode, string errorMsg)
            => Console.WriteLine($"ERROR {errorCode}: {errorMsg}");

        // Métodos no usados, se pueden dejarlos vacíos!!
        public void connectionClosed() { }
        public void tickString(int tickerId, int field, string value) { }
        public void tickGeneric(int tickerId, int field, double value) { }
        public void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double totalDividends, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate) { }
        public void tickSnapshotEnd(int tickerId) { }
        public void marketDataType(int reqId, int marketDataType) { }
        public void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size) { }
        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size, bool isSmartDepth) { }
        public void updateNewsBulletin(int msgId, int msgType, string message, string origExchange) { }
        public void managedAccounts(string accountsList) { }
        public void accountSummary(int reqId, string account, string tag, string value, string currency) { }
        public void accountSummaryEnd(int reqId) { }
        public void position(string account, Contract contract, double pos, double avgCost) { }
        public void positionEnd() { }
        public void accountDownloadEnd(string account) { }
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency) { }
        public void accountUpdateMultiEnd(int requestId) { }
        public void commissionReport(CommissionReport commissionReport) { }
        public void currentTime(long time) { }
        public void deltaNeutralValidation(int reqId, DeltaNeutralContract deltaNeutralContract) { }
        public void execDetails(int reqId, Contract contract, Execution execution) { }
        public void execDetailsEnd(int reqId) { }
        public void fundamentalData(int reqId, string data) { }
        public void historicalData(int reqId, Bar bar) { }
        public void historicalDataEnd(int reqId, string start, string end) { }
        public void marketRule(int marketRuleId, PriceIncrement[] priceIncrements) { }
        public void orderStatus(int orderId, string status, double filled, double remaining,
            double avgFillPrice, int permId, int parentId, double lastFillPrice,
            int clientId, string whyHeld, double mktCapPrice)
        { }
        public void openOrder(int orderId, Contract contract, Order order, OrderState orderState) { }
        public void openOrderEnd() { }
        public void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL) { }
        public void pnlSingle(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value) { }
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost) { }
        public void positionMultiEnd(int requestId) { }
        public void receiveFA(int faDataType, string faXmlData) { }
        public void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr) { }
        public void scannerDataEnd(int reqId) { }
        public void scannerParameters(string xml) { }
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes) { }
        public void securityDefinitionOptionParameterEnd(int reqId) { }
    }
}
