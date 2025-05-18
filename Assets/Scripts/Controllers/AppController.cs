using Mirror;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public static AppController instance;

    private SolarSystemData data;
    private List<CuerpoCeleste> cuerposCelestes = new List<CuerpoCeleste>();
    public Vector3 currentCameraRotation { get; set; }

    public CuerpoCeleste cuerpoActual { set; get; }


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        data = LoadData();

        cuerposCelestes.Add(data.sol);
        foreach (Planeta planeta in data.planetas)
        {
            cuerposCelestes.Add(planeta);
        }
    }

    public List<CuerpoCeleste> GetCuerposCelestes()
    {
        return cuerposCelestes;
    }    

    private SolarSystemData LoadData()
    {
        TextAsset asset = Resources.Load<TextAsset>("data");
        return JsonConvert.DeserializeObject<SolarSystemData>(asset.text);
    }
}
