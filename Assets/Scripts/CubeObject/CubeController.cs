using System;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        CubeController otherCube = collision.gameObject.GetComponent<CubeController>();
        if (otherCube != null)
        {
            float thisSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            float otherSpeed = otherCube.GetComponent<Rigidbody>().velocity.magnitude;

            if (thisSpeed > otherSpeed)
            {
                otherCube.OnInteract(this);
            }
        }
    }

    public void OnInteract(CubeController otherCube)
    {
        if (otherCube != null && _model.Po2Value == otherCube._model.Po2Value)
        {
            _model.Po2Value *= 2;
            UpdateView();
            otherCube.DestroyCube();
        }
    }

    private void DestroyCube()
    {
        OnCubeDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}