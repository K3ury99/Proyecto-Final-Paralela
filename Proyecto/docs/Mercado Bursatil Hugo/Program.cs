// Program.cs
using System;

class Program
{
    static void Main()
    {
        var mercado = new Mercado();
        mercado.AgregarAccion(new Accion("AAPL", 175.50));
        mercado.AgregarAccion(new Accion("MSFT", 310.75));
        mercado.AgregarAccion(new Accion("GOOG", 132.30));

        var cliente = new IBKRClient();
        cliente.Connect("127.0.0.1", 7497, 0); // TWS por defecto

        var simulador = new Simulador(mercado, cliente);

        for (int i = 1; i <= 5; i++)
        {
            simulador.AgregarAgente(new Agente($"Agente-{i}", mercado, cliente));
        }

        simulador.Ejecutar();

        cliente.Disconnect();
    }
}
