using Mirror;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance { get; private set; }
    public string serverAddress = "localhost";

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager manager = NetworkManager.singleton;

        if (NetworkServer.active || NetworkClient.isConnected)
        {
            Debug.Log("Ya estás conectado. No se iniciará el host nuevamente.");
            return;
        }

        manager.networkAddress = PlayerPrefs.GetString("hostIp");
        Debug.Log("Iniciando como host (servidor + cliente local)...");
        manager.StartHost();

        NetworkClient.OnConnectedEvent += OnConnected;
        NetworkClient.OnDisconnectedEvent += OnDisconnected;
    }

    public void ConnectClient()
    {
        NetworkManager manager = NetworkManager.singleton;

        if (NetworkClient.isConnected)
        {
            Debug.Log("Ya estás conectado como cliente.");
            return;
        }

        manager.networkAddress = PlayerPrefs.GetString("hostIp");
        Debug.Log($"Conectando al servidor en {serverAddress}...");
        manager.StartClient();

        NetworkClient.OnConnectedEvent += OnConnected;
        NetworkClient.OnDisconnectedEvent += OnDisconnected;
    }

    public void Disconnect()
    {
        NetworkManager manager = NetworkManager.singleton;

        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Debug.Log("Deteniendo Host (Servidor + Cliente)");
            manager.StopHost();
        }
        else if (NetworkServer.active)
        {
            Debug.Log("Deteniendo Servidor");
            manager.StopServer();
        }
        else if (NetworkClient.isConnected)
        {
            Debug.Log("Desconectando Cliente");
            manager.StopClient();
        }
        else
        {
            Debug.Log("No hay conexión activa.");
        }
    }

    public static bool IsConnectedToServer()
    {
        return NetworkClient.isConnected || NetworkServer.active;
    }

    private void OnConnected()
    {
        Debug.Log("¡Conectado al servidor!");
    }

    private void OnDisconnected()
    {
        Debug.LogWarning("Desconectado del servidor.");
    }

    private void OnDestroy()
    {
        // Evitar fugas de eventos si se destruye el objeto
        NetworkClient.OnConnectedEvent -= OnConnected;
        NetworkClient.OnDisconnectedEvent -= OnDisconnected;
    }
}
