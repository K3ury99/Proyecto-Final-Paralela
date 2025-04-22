using System.Collections.Concurrent;
using System.Collections.Generic;

// Esta clase representa el mercado donde se gestionan todas las acciones disponibles.
public class Mercado
{
    // Diccionario concurrente para almacenar las acciones por su símbolo (clave).
    // ConcurrentDictionary es ideal para entornos multi-hilo, como este simulador.
    private readonly ConcurrentDictionary<string, Accion> acciones = new();

    // Método para agregar una nueva acción al mercado o actualizarla si ya existe.
    public void AgregarAccion(Accion accion)
    {
        // Asigna o reemplaza la acción directamente. No necesita lock, ya que ConcurrentDictionary lo gestiona internamente.
        acciones[accion.Simbolo] = accion;
    }

    // Método que devuelve una acción específica por su símbolo (ej: "AAPL").
    public Accion ObtenerAccion(string simbolo)
    {
        acciones.TryGetValue(simbolo, out var accion); // Intenta obtenerla de forma segura.
        return accion; // Devuelve la acción o null si no existe.
    }

    // Método para modificar el precio de una acción existente.
    public void ModificarPrecio(string simbolo, double nuevoPrecio)
    {
        if (acciones.ContainsKey(simbolo))
        {
            acciones[simbolo].Precio = nuevoPrecio; // Aunque el acceso es seguro, la propiedad en sí puede no ser atómica.
        }
    }

    // Método que devuelve una lista con todas las acciones actuales en el mercado.
    public List<Accion> ObtenerTodas()
    {
        // Copia los valores actuales del diccionario a una nueva lista.
        return new List<Accion>(acciones.Values);
    }
}
