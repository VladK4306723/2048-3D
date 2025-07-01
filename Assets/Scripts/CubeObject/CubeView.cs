using TMPro;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    public void SetText(string text)
    {
        if (textMesh != null)
        {
            textMesh.text = text;
        }
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