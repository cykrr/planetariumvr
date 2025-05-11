using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public GameObject itemPrefab;

    public void AddItem(string name, string description)
    {
        GameObject item = Instantiate(itemPrefab, gameObject.transform);
        TextMeshPro[] texts = item.GetComponentsInChildren<TextMeshPro>();

        texts[0].text = Util.CapitalizeFirstLetter(name) + ":";
        texts[1].text = description;
    }
}
