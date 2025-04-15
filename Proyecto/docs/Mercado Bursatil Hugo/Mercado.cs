// Models/Mercado.cs
using System.Collections.Generic;

public class Mercado
{
    private readonly Dictionary<string, Accion> acciones = new();
    private readonly object locker = new();

    public void AgregarAccion(Accion accion)
    {
        lock (locker)
        {
            acciones[accion.Simbolo] = accion;
        }
    }

    public Accion ObtenerAccion(string simbolo)
    {
        lock (locker)
        {
            return acciones.TryGetValue(simbolo, out var accion) ? accion : null;
        }
    }

    public void ModificarPrecio(string simbolo, double nuevoPrecio)
    {
        lock (locker)
        {
            if (acciones.ContainsKey(simbolo))
            {
                acciones[simbolo].Precio = nuevoPrecio;
            }
        }
    }

    public List<Accion> ObtenerTodas()
    {
        lock (locker)
        {
            return new List<Accion>(acciones.Values);
        }
    }
}
