using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    void OnInteract(CubeController otherCube);
}

public class CubeController : MonoBehaviour
{
    private CubeModel _model;
    private CubeView _view;
    public bool IsForLaunch { get; set; }
    public event Action<CubeController> OnCubeDestroyed;
    public event Action<int> OnIntChanged;

    private HashSet<CubeController> _interactedCubes = new HashSet<CubeController>();

    public void Initialize(CubeModel model, CubeView view)
    {
        _model = model;
        _view = view;
        UpdateView();
    }

    public void SetForLaunch(bool value)
    {
        IsForLaunch = value;
    }

    public void UpdateView()
    {
        _view.SetText(_model.Po2Value.ToString());
        OnIntChanged?.Invoke(_model.Po2Value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CubeController otherCube = collision.gameObject.GetComponent<CubeController>();
        if (otherCube != null && !_interactedCubes.Contains(otherCube) && _model.Po2Value == otherCube._model.Po2Value)
        {
            _model.Po2Value *= 2;
            UpdateView();
            otherCube.DestroyCube();
            _interactedCubes.Add(otherCube);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        CubeController otherCube = collision.gameObject.GetComponent<CubeController>();
        if (otherCube != null && !_interactedCubes.Contains(otherCube) && _model.Po2Value == otherCube._model.Po2Value)
        {
            _model.Po2Value *= 2;
            UpdateView();
            otherCube.DestroyCube();
            _interactedCubes.Add(otherCube);
        }
    }

    public void OnInteract(CubeController otherCube)
    {
        if (otherCube != null && !_interactedCubes.Contains(otherCube) && _model.Po2Value == otherCube._model.Po2Value)
        {
            _model.Po2Value *= 2;
            UpdateView();
            otherCube.DestroyCube();
            _interactedCubes.Add(otherCube);
        }
    }

    private void DestroyCube()
    {
        OnCubeDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactedCubes.Clear();
    }
}