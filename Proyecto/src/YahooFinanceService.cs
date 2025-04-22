using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YahooFinanceApi;

// Servicio que se encarga de obtener precios de acciones desde la API de Yahoo Finance
public class YahooFinanceService
{
    // Método asíncrono que recibe una lista de símbolos y devuelve una lista de objetos Accion
    public async Task<List<Accion>> ObtenerPreciosAsync(IEnumerable<string> simbolos)
    {
        var acciones = new List<Accion>(); // Lista que almacenará los resultados

        try
        {
            // Solicita los datos a la API de Yahoo Finance, solicitando solo el símbolo y el precio actual
            var resultados = await Yahoo.Symbols(simbolos.ToArray())
                                         .Fields(Field.Symbol, Field.RegularMarketPrice)
                                         .QueryAsync()
                                         .ConfigureAwait(false); // Evita la captura del contexto (mejora rendimiento en consola)

            // Itera por cada símbolo solicitado
            foreach (var simbolo in simbolos)
            {
                // Verifica si se obtuvieron datos para el símbolo actual
                if (resultados.TryGetValue(simbolo, out var data))
                {
                    // Verifica si hay un precio válido disponible
                    if (data[Field.RegularMarketPrice] != null)
                    {
                        double precio = (double)data[Field.RegularMarketPrice]; // Convierte el valor a double
                        acciones.Add(new Accion(simbolo, precio)); // Crea y agrega la acción a la lista
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró precio para {simbolo}"); // Si no hay precio, lo informa
                    }
                }
                else
                {
                    Console.WriteLine($"No se encontró información para {simbolo}"); // Si no hay info, lo informa
                }
            }
        }
        catch (Exception ex)
        {
            // Captura cualquier excepción que ocurra durante la consulta
            Console.WriteLine($"Error al obtener precios: {ex.Message}");
        }

        return acciones; // Devuelve la lista final de acciones con sus precios
    }
}
