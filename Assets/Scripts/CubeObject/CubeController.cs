using UnityEngine;

public class CubeController : MonoBehaviour
{
    private CubeModel _model;
    private CubeView _view;

    public void Initialize(CubeModel model, CubeView view)
    {
        _model = model;
        _view = view;
        UpdateView();
    }

    public void UpdateView()
    {
        _view.SetText(_model.Po2Value.ToString());
    }
}