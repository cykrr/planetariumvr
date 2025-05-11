using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VistaDetalladaController : MonoBehaviour
{
    private CuerpoCeleste cuerpoCeleste;

    public TextMeshPro descriptionText, nameText, radiusText, massText, gravityText;
    public MeshRenderer modelRenderer;

    public ItemList compositionItemList, specialPropsList;

    private void Awake()
    {
        cuerpoCeleste = AppController.instance.cuerpoActual;

        Material mat = Resources.Load<Material>("Materials/" + cuerpoCeleste.modelo);
        Material[] mats = modelRenderer.materials;
        mats[0] = mat;
        modelRenderer.materials = mats;

        descriptionText.SetText(cuerpoCeleste.descripcion);
        nameText.SetText(cuerpoCeleste.nombre);
        radiusText.SetText(cuerpoCeleste.radio);
        massText.SetText(cuerpoCeleste.masa);
        gravityText.SetText(cuerpoCeleste.gravedad);

        foreach(var item in cuerpoCeleste.composicion)
        {
            compositionItemList.AddItem(item.Key, item.Value);
        }

        List<Tuple<string, string>> props = new List<Tuple<string, string>>();
        if (cuerpoCeleste is Estrella sol)
        {
            props.Add(Tuple.Create("Tipo de estrella", sol.tipoEstrella));
            props.Add(Tuple.Create("Temperatura núcleo", sol.temperaturaNucleo));
            props.Add(Tuple.Create("Temperatura superficial", sol.temperaturaSuperficial));
        }
        else if (cuerpoCeleste is Planeta planeta)
        {
            props.Add(Tuple.Create("Periodo de rotación", planeta.periodoRotacion));
            props.Add(Tuple.Create("Temperatura media", planeta.temperaturaMedia));
            props.Add(Tuple.Create("Tipo de planeta", planeta.tipoPlaneta));
            props.Add(Tuple.Create("Cantidad de lunas", planeta.cantidadLunas.ToString()));
        }

        foreach (var (key, value) in props)
        {
            specialPropsList.AddItem(key, value);
        }
    }

}
