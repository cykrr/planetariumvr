using System;
using System.Collections.Generic;

[Serializable]
public class SolarSystemData
{
    public Estrella sol;
    public List<Planeta> planetas;
}

public class CuerpoCeleste
{
    public string nombre;
    public string modelo;
    public string descripcion;
    public string radio;
    public string masa;
    public string gravedad;
    public Dictionary<string, string> composicion;
}

[Serializable]
public class Estrella : CuerpoCeleste
{
    public string tipoEstrella;
    public string temperaturaNucleo;
    public string temperaturaSuperficial;
}

[Serializable]
public class Planeta : CuerpoCeleste
{
    public string periodoRotacion;
    public string temperaturaMedia;
    public string tipoPlaneta;
    public int cantidadLunas;
}