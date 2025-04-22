using System.Diagnostics;
using System.Threading.Tasks;

// Clase encargada de coordinar la simulación entre agentes y el mercado
public class Simulador
{
    // Lista de agentes que participarán en la simulación
    private readonly List<Agente> agentes = new();

    // Referencias al mercado y al cliente simulado (broker)
    private readonly Mercado mercado;
    private readonly SimulatedBrokerClient cliente;

    // Constructor del simulador, recibe el mercado y cliente como dependencias
    public Simulador(Mercado mercado, SimulatedBrokerClient cliente)
    {
        this.mercado = mercado;
        this.cliente = cliente;
    }

    // Método para agregar agentes al simulador
    public void AgregarAgente(Agente agente)
    {
        agentes.Add(agente);
    }

    // Método principal que ejecuta la simulación
    public async Task Ejecutar()
    {
        // Validación: si no hay agentes, se cancela la simulación
        if (agentes.Count == 0)
        {
            Console.WriteLine(" No hay agentes para ejecutar");
            return;
        }

        // Mostrar información inicial
        Console.WriteLine($"\n Procesadores disponibles: {Environment.ProcessorCount}");
        Console.WriteLine($" Agentes a ejecutar: {agentes.Count}");

        // 🟡 EJECUCIÓN SECUENCIAL
        Console.WriteLine("\n Iniciando ejecución SECUENCIAL...");
        Stopwatch swSecuencial = Stopwatch.StartNew(); // Inicia el cronómetro
        await EjecutarSecuencial(); // Ejecuta todos los agentes uno por uno
        swSecuencial.Stop(); // Detiene el cronómetro
        long tiempoSecuencial = swSecuencial.ElapsedMilliseconds;

        // 🟢 EJECUCIÓN PARALELA
        Console.WriteLine("\n Iniciando ejecución PARALELA...");
        Stopwatch swParalela = Stopwatch.StartNew(); // Inicia el cronómetro
        await EjecutarParalela(); // Ejecuta todos los agentes en paralelo
        swParalela.Stop(); // Detiene el cronómetro
        long tiempoParalela = swParalela.ElapsedMilliseconds;

        // 📊 CÁLCULO DE MÉTRICAS DE RENDIMIENTO
        double speedup = (double)tiempoSecuencial / tiempoParalela; // Aceleración obtenida
        double eficiencia = speedup / Environment.ProcessorCount; // Eficiencia según los núcleos

        // Mostrar los resultados por consola
        Console.WriteLine("\n RESULTADOS:");
        Console.WriteLine($"Tiempo secuencial: {tiempoSecuencial} ms");
        Console.WriteLine($"Tiempo paralelo: {tiempoParalela} ms");
        Console.WriteLine($"Speedup: {speedup:F2}x"); // Redondeado a 2 decimales
        Console.WriteLine($"Eficiencia: {eficiencia:P2}"); // En formato porcentaje
        Console.WriteLine($"Operaciones totales: {agentes.Count * 100}"); // Asumiendo 100 operaciones por agente

        // Mostrar información del sistema
        Console.WriteLine("\nINFORMACIÓN DEL SISTEMA:");
        Console.WriteLine($"Procesadores lógicos: {Environment.ProcessorCount}");
        Console.WriteLine($"Hilos usados: {Process.GetCurrentProcess().Threads.Count}");

        // Diccionario con métricas para exportar
        var metrics = new Dictionary<string, double>
        {
            { "Tiempo secuencial", tiempoSecuencial },
            { "Tiempo paralelo", tiempoParalela },
            { "Speedup", speedup },
            { "Eficiencia", eficiencia },
            { "Procesadores", Environment.ProcessorCount }
        };

        // Exportar los gráficos de métricas
        GraphicsExporter.ExportMetrics(metrics);
        Console.WriteLine("\n Gráficos exportados en la carpeta 'MetricsExport'");
    }

    // Método privado que ejecuta los agentes secuencialmente
    private async Task EjecutarSecuencial()
    {
        foreach (var agente in agentes)
        {
            await agente.EjecutarAsync(CancellationToken.None); // Ejecuta cada agente individualmente
        }
    }

    // Método privado que ejecuta los agentes en paralelo
    private async Task EjecutarParalela()
    {
        // Configuración para la ejecución paralela, usando la cantidad de núcleos disponibles
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        // Ejecutar todos los agentes en paralelo
        await Parallel.ForEachAsync(agentes, options, async (agente, token) =>
        {
            await agente.EjecutarAsync(token); // Cada agente se ejecuta de forma asíncrona
        });
    }
}
