// Simulador.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class Simulador
{
    private readonly List<Agente> agentes = new();
    private readonly Mercado mercado;
    private readonly IBKRClient cliente;

    public Simulador(Mercado mercado, IBKRClient cliente)
    {
        this.mercado = mercado;
        this.cliente = cliente;
    }

    public void AgregarAgente(Agente agente)
    {
        agentes.Add(agente);
    }

    public void Ejecutar()
    {
        Stopwatch sw = Stopwatch.StartNew();

        Parallel.ForEach(agentes, agente =>
        {
            agente.Ejecutar();
        });

        sw.Stop();
        Console.WriteLine($"\n🕒 Tiempo total: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"📊 Operaciones totales: {agentes.Count * 10}");
        Console.WriteLine($"⚙️ Rendimiento por agente: {sw.ElapsedMilliseconds / agentes.Count} ms promedio");
    }
}
