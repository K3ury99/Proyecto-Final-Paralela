using System;
using System.Text;

// Esta clase simula un cliente de broker que recibe y muestra órdenes de compra/venta de acciones
public class SimulatedBrokerClient
{
    // Campo estático que representa un contador único para identificar cada orden generada
    private static int orderId = 1;

    // Simula la conexión al sistema de broker (aquí solo imprime en consola)
    public void Connect()
    {
        Console.WriteLine("Conectado al broker");
        ImprimirCabeceraTabla(); // Imprime la tabla donde se mostrarán las órdenes
    }

    // Simula la desconexión del sistema de broker
    public void Disconnect()
    {
        Console.WriteLine("Desconectado del broker");
    }

    // Imprime la cabecera de la tabla donde se verán las órdenes (formato tipo consola con bordes)
    private void ImprimirCabeceraTabla()
    {
        Console.WriteLine();
        Console.WriteLine("╔═══════════╦══════╦════╦═══════╦══════════╦════════════╦══════════════╗");
        Console.WriteLine("║   Hora    ║ Tipo ║ Qty║ Ticker║  Precio  ║  Agente    ║   ID Orden   ║");
        Console.WriteLine("╠═══════════╬══════╬════╬═══════╬══════════╬════════════╬══════════════╣");
    }

    // Método público que representa el envío de una orden de trading al broker
    public void PlaceOrder(string simbolo, string accion, int cantidad, double precio, string agenteNombre)
    {
        // Se crea la orden en formato de texto
        var ordenFormateada = CrearOrden(simbolo, accion, cantidad, precio, agenteNombre);

        // Se muestra por consola
        MostrarOrden(ordenFormateada);
    }

    // Crea el string formateado de la orden, incluyendo hora, tipo de operación, etc.
    private string CrearOrden(string simbolo, string accion, int cantidad, double precio, string agenteNombre)
    {
        var sb = new StringBuilder();

        sb.AppendFormat(
            "| {0:HH:mm:ss} | {1,-4} | {2,2} | {3,-5} | ${4,8:F2} | {5,9} | [Orden #{6}]",
            DateTime.Now,          // Hora actual de la orden
            accion,                // "BUY" o "SELL"
            cantidad,              // Cantidad de acciones
            simbolo,               // Ticker (ej: AAPL)
            precio,                // Precio formateado con dos decimales
            agenteNombre,          // Nombre del agente que emitió la orden
            orderId++              // ID de orden incremental
        );

        return sb.ToString();
    }

    // Muestra la orden en consola
    private void MostrarOrden(string orden)
    {
        Console.WriteLine(orden);
    }
}
