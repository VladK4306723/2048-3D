using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> CubeAmountText;

    public void SetText(string text)
    {
        CubeAmountText.ForEach(x => x.text = text);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}