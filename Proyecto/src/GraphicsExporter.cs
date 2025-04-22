// Importación de librerías necesarias
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // Para serializar a JSON
using ScottPlot; // Para crear gráficos
using ScottPlot.Plottable; // Para los objetos ploteables como barras

// Clase responsable de exportar métricas en formato gráfico y JSON
public class GraphicsExporter
{
    // Método principal que exporta las métricas en JSON y como gráficos
    public static void ExportMetrics(Dictionary<string, double> metrics, string directory = "MetricsExport")
    {
        // Verifica si el directorio existe, y si no, lo crea
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Exporta las métricas como archivo JSON con formato legible
        string jsonPath = Path.Combine(directory, "metrics.json");
        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(metrics, Formatting.Indented));

        // Crea los gráficos basados en las métricas
        CreateComparisonChart(metrics, directory);  // Gráfico de tiempos
        CreateEfficiencyChart(metrics, directory);  // Gráfico de eficiencia
        CreateProcessorChart(metrics, directory);   // Gráfico de recursos
    }

    // Crea un gráfico de barras comparando tiempo secuencial vs paralelo
    private static void CreateComparisonChart(Dictionary<string, double> metrics, string directory)
    {
        var plt = new Plot(800, 600); // Define dimensiones del gráfico

        // Datos para el gráfico
        double[] values = { metrics["Tiempo secuencial"], metrics["Tiempo paralelo"] };
        string[] labels = { "Secuencial", "Paralelo" };

        var barPlot = plt.AddBar(values); // Agrega las barras
        barPlot.ShowValuesAboveBars = true; // Muestra valores encima de las barras
        plt.XTicks(labels); // Etiquetas del eje X

        // Configura título y etiquetas
        plt.Title("Comparación de Tiempos de Ejecución");
        plt.YLabel("Tiempo (ms)");
        plt.XLabel("Modo de ejecución");

        // Guarda el gráfico como imagen
        string filePath = Path.Combine(directory, "time_comparison.png");
        plt.SaveFig(filePath);
    }

    // Crea un gráfico que muestra el speedup y la eficiencia
    private static void CreateEfficiencyChart(Dictionary<string, double> metrics, string directory)
    {
        var plt = new Plot(800, 600);

        // Datos para el gráfico
        double[] values = { metrics["Speedup"], metrics["Eficiencia"] * 100 }; // Eficiencia se convierte a porcentaje
        string[] labels = { "Speedup", "Eficiencia (%)" };

        var barPlot = plt.AddBar(values);
        barPlot.ShowValuesAboveBars = true;
        plt.XTicks(labels);

        // Configura título y etiquetas
        plt.Title("Speedup y Eficiencia");
        plt.YLabel("Valor");

        // Guarda el gráfico como imagen
        string filePath = Path.Combine(directory, "efficiency.png");
        plt.SaveFig(filePath);
    }

    // Crea un gráfico que muestra la cantidad de procesadores disponibles
    private static void CreateProcessorChart(Dictionary<string, double> metrics, string directory)
    {
        var plt = new Plot(800, 600);

        // Solo un dato: cantidad de procesadores
        double[] values = { metrics["Procesadores"] };
        string[] labels = { "Procesadores" };

        var barPlot = plt.AddBar(values);
        barPlot.ShowValuesAboveBars = true;
        plt.XTicks(labels);

        // Configura título y etiquetas
        plt.Title("Recursos del Sistema");
        plt.YLabel("Cantidad");

        // Guarda el gráfico como imagen
        string filePath = Path.Combine(directory, "processors.png");
        plt.SaveFig(filePath);
    }
}
