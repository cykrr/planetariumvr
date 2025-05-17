using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class SharedObject : NetworkBehaviour
{
    // Posición sincronizada
    [SyncVar]
    public Vector3 position;
    [SyncVar(hook = nameof(OnScaleChanged))]
    public Vector3 scale;
    [SyncVar]
    public string materialName;
    [SyncVar]
    public string tagName;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Cargar y asignar el material en cliente
        Material mat = Resources.Load<Material>("Materials/" + materialName);
        if (mat != null)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            Material[] mats = renderer.materials;
            mats[0] = mat;
            renderer.materials = mats;

            tag = tagName;
        }
    }

    private void Update()
    {
        transform.position = position;
    }

    void OnScaleChanged(Vector3 oldScale, Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    // Comando llamado por el cliente para pedir mover el objeto
    [Command(requiresAuthority = false)]
    public void CmdMoveObject(Vector3 newPosition)
    {
        position = newPosition;
    }

    [Command(requiresAuthority = false)]
    public void SetScale(Vector3 newScale)
    {
        scale = newScale;
    }
}