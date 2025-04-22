using System;
using System.Threading;
using System.Threading.Tasks;

// Clase que representa un "agente" o bot simulando operaciones en el mercado
public class Agente
{
    // Referencia al mercado donde se operan las acciones
    private readonly Mercado mercado;

    // Referencia al cliente simulado que gestiona las órdenes de compra/venta
    private readonly SimulatedBrokerClient cliente;

    // Generador de números aleatorios para las decisiones del agente
    private readonly Random random = new();

    // Número de iteraciones que realizará este agente (puede ser aumentado para simulaciones más largas)
    public const int Iteraciones = 3;

    // Nombre identificador del agente (por ejemplo, "Agente1", "Agente2", etc.)
    public string Nombre { get; }

    // Constructor: recibe el nombre del agente, el mercado donde operará y el cliente simulador de bróker
    public Agente(string nombre, Mercado mercado, SimulatedBrokerClient cliente)
    {
        Nombre = nombre;
        this.mercado = mercado;
        this.cliente = cliente;
    }

    // Método principal que ejecuta el comportamiento del agente de forma asíncrona
    public async Task EjecutarAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Itera la cantidad de veces especificada (puedes cambiar Iteraciones para simular más acciones)
            for (int i = 0; i < Iteraciones; i++)
            {
                // Si se ha solicitado cancelación, se lanza una excepción para detener la tarea
                cancellationToken.ThrowIfCancellationRequested();

                // Se obtiene una lista de todas las acciones disponibles en el mercado
                var acciones = mercado.ObtenerTodas();

                // Se elige aleatoriamente una acción para operar
                var accion = acciones[random.Next(acciones.Count)];

                // Se genera una variación aleatoria entre -1.0 y +1.0 (precio nuevo puede subir o bajar)
                double variacion = (random.NextDouble() - 0.5) * 2;

                // Se calcula el nuevo precio asegurando que no sea menor que 1
                double nuevoPrecio = Math.Max(1, accion.Precio + variacion);

                // Se actualiza el precio en el mercado
                mercado.ModificarPrecio(accion.Simbolo, nuevoPrecio);

                // Se decide aleatoriamente si se hace una orden de compra o venta
                string accionOrden = random.Next(2) == 0 ? "BUY" : "SELL";

                // Se decide una cantidad aleatoria entre 1 y 5 para la orden
                int cantidad = random.Next(1, 6);

                // Se envía la orden al cliente simulado (como si se tratara de un bróker)
                cliente.PlaceOrder(accion.Simbolo, accionOrden, cantidad, nuevoPrecio, Nombre);

                // Se espera un breve tiempo antes de la siguiente iteración para simular el paso del tiempo
                await Task.Delay(100, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Mensaje que se muestra si la operación fue cancelada desde fuera (por ejemplo, desde el `finally`)
            Console.WriteLine($"[{Nombre}] Operación cancelada.");
        }
    }
}
