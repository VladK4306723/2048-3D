using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private CubeModel _model;
    private CubeView _view;
    public bool IsForLaunch { get; set; }
    public event Action<CubeController> OnCubeDestroyed;
    public event Action<int> OnIntChanged;

    private HashSet<CubeController> _interactedCubes = new();
    private ICubePool _cubePool;

    public void Init(CubeModel model, CubeView view, ICubePool cubePool)
    {
        _model = model;
        _view = view;
        _cubePool = cubePool;
        IsForLaunch = false;
        _interactedCubes.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.layer = LayerMask.NameToLayer("CubeLaunch");
        UpdateView();
    }

    private void Clear()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _interactedCubes.Clear();
    }

    public void SetForLaunch(bool value)
    {
        IsForLaunch = value;
    }

    public void UpdateView()
    {
        _view.SetText(_model.Po2Value.ToString());
        SetColorBasedOnValue();
        OnIntChanged?.Invoke(_model.Po2Value);
    }

    private void SetColorBasedOnValue()
    {
        float hue = (float)Mathf.Log(_model.Po2Value, 2) / 10f;
        Color color = Color.HSVToRGB(hue, 1f, 1f);
        _view.SetColor(color);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CubeController other = collision.gameObject.GetComponent<CubeController>();
        if (IsValidMerge(other, collision))
        {
            StartCoroutine(PerformMerge(other));
        }
        else
        {
            float impulseMag = collision.impulse.magnitude;
            if (impulseMag > 1.5f)
            {
                TryMergeWithNearbyCubes();
            }
        }
    }

    private bool IsValidMerge(CubeController other, Collision collision)
    {
        if (other == null || _interactedCubes.Contains(other)) return false;
        if (_model.Po2Value != other._model.Po2Value || other.IsForLaunch) return false;

        float minImpulseThreshold = 1.5f;
        Vector3 impulse = collision.impulse;
        float impulseMag = impulse.magnitude;
        if (impulseMag < minImpulseThreshold) return false;

        Vector3 dirToOther = (other.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(impulse.normalized, dirToOther);

        return dot > 0.5f;
    }

    private IEnumerator PerformMerge(CubeController other)
    {
        yield return new WaitForFixedUpdate();

        if (other == null || _interactedCubes.Contains(other)) yield break;
        if (_model.Po2Value != other._model.Po2Value || other.IsForLaunch) yield break;

        _view.PlayHit();
        _model.Po2Value *= 2;
        UpdateView();
        _interactedCubes.Add(other);
        other.ReturnToPool();
    }

    private void TryMergeWithNearbyCubes()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.6f);
        foreach (Collider col in hits)
        {
            if (col.gameObject == gameObject) continue;

            CubeController other = col.GetComponent<CubeController>();
            if (other != null && !_interactedCubes.Contains(other) && _model.Po2Value == other._model.Po2Value && !other.IsForLaunch)
            {
                StartCoroutine(PerformMerge(other));
                break;
            }
        }
    }

    public void ReturnToPool()
    {
        OnCubeDestroyed?.Invoke(this);
        StartCoroutine(ReturnToPoolDelayed());
    }

    private IEnumerator ReturnToPoolDelayed()
    {
        yield return new WaitForEndOfFrame();
        Clear();
        _cubePool.Push(_view);
    }
}
