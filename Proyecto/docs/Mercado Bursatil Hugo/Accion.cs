// Models/Accion.cs
public class Accion
{
    public string Simbolo { get; set; }
    public double Precio { get; set; }

    public Accion(string simbolo, double precio)
    {
        Simbolo = simbolo;
        Precio = precio;
    }
}
