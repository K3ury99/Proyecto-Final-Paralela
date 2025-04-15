using IBApi;
using System;
using System.Threading;

public class Agente
{
    private readonly Mercado mercado;
    private readonly IBKRClient cliente;
    private readonly Random random = new();
    private static int ordenId = 1;

    public string Nombre { get; }

    public Agente(string nombre, Mercado mercado, IBKRClient cliente)
    {
        Nombre = nombre;
        this.mercado = mercado;
        this.cliente = cliente;
    }

    public void Ejecutar()
    {
        for (int i = 0; i < 10; i++)
        {
            var acciones = mercado.ObtenerTodas();
            var accion = acciones[random.Next(acciones.Count)];

            // Variación aleatoria del precio de la acción
            double variacion = (random.NextDouble() - 0.5) * 2;
            double nuevoPrecio = Math.Max(1, accion.Precio + variacion);
            mercado.ModificarPrecio(accion.Simbolo, nuevoPrecio);

            // Acción aleatoria: BUY o SELL
            string accionOrden = random.Next(2) == 0 ? "BUY" : "SELL";

            // Cantidad aleatoria entre 1 y 5 acciones
            int cantidad = random.Next(1, 6);

            var contrato = new Contract
            {
                Symbol = accion.Simbolo,
                SecType = "STK",
                Currency = "USD",
                Exchange = "SMART"
            };

            var orden = new Order
            {
                Action = accionOrden,
                OrderType = "MKT",
                TotalQuantity = cantidad
            };

            int id = Interlocked.Increment(ref ordenId);
            cliente.PlaceOrder(id, contrato, orden);

            // Mostrar en consola si fue compra o venta
            string verbo = accionOrden == "BUY" ? "COMPRÓ" : "VENDIÓ";
            Console.WriteLine($"[{Nombre}] {verbo} {cantidad} acción(es) de {accion.Simbolo} a ${nuevoPrecio:F2}");

            Thread.Sleep(random.Next(100, 500));
        }
    }
}
