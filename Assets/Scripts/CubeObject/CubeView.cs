using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> CubeAmountText;
    [SerializeField] private AudioSource HitSound;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetText(string text)
    {
        CubeAmountText.ForEach(x => x.text = text);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void PlayHit()
    {
        HitSound.Play();
    }
}