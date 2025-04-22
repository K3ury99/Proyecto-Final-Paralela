// Program.cs (versión corregida)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Se crea un token para cancelación, útil en tareas asincrónicas que podrían ser canceladas si es necesario
var cts = new CancellationTokenSource();
var token = cts.Token;

try
{
    // Se instancia el servicio que obtiene precios desde Yahoo Finance
    var servicioYahoo = new YahooFinanceService();

    // Se obtienen los precios iniciales de tres acciones específicas: AAPL, MSFT y GOOG
    var accionesIniciales = await servicioYahoo.ObtenerPreciosAsync(new[] { "AAPL", "MSFT", "GOOG" });

    // Si no se pudieron obtener los precios, se informa al usuario y termina la ejecución
    if (accionesIniciales.Count == 0)
    {
        Console.WriteLine(" No se pudieron obtener precios.");
        return;
    }

    // Se crea una instancia del mercado, que manejará las acciones en la simulación
    var mercado = new Mercado();

    // Se agregan las acciones obtenidas al mercado
    foreach (var accion in accionesIniciales)
    {
        mercado.AgregarAccion(accion);
    }

    // Se crea e inicia un cliente simulado que actuará como bróker o intermediario de compra/venta
    var cliente = new SimulatedBrokerClient();
    cliente.Connect();

    // Se crean x cantidad de agentes simulados (pueden ser bots de trading, por ejemplo)
    var agentes = new List<Agente>();
    for (int i = 1; i <= 200; i++) 
    {
        agentes.Add(new Agente($"Agente{i}", mercado, cliente));
    }

    // Se crea el simulador de mercado, que administrará las operaciones entre agentes y el mercado
    var simulador = new Simulador(mercado, cliente);

    // Se agregan los agentes al simulador para que participen en la simulación
    foreach (var agente in agentes)
    {
        simulador.AgregarAgente(agente);
    }

    // Se muestra la cantidad de agentes creados
    Console.WriteLine($"\n Agentes creados: {agentes.Count}");

    // Se ejecuta la simulación de mercado (probablemente contenga lógica asincrónica de compra/venta)
    await simulador.Ejecutar();

    // Se desconecta el cliente del bróker después de finalizar la simulación
    cliente.Disconnect();
}
catch (Exception ex)
{
    // En caso de error durante la simulación, se muestra el mensaje de excepción
    Console.WriteLine($" Ocurrió un error: {ex.Message}");
}
finally
{
    // Al finalizar (éxito o fallo), se cancela cualquier operación pendiente relacionada al token
    cts.Cancel();
}
