using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ICubePool
{
    void Init(int poolSize);
    CubeView GetCubeView();
    void ReturnCubeView(CubeView view);
}

public class CubePool : ICubePool
{
    private DiContainer _container;
    private GameObject _cubePrefab;
    private Queue<CubeView> _pool = new Queue<CubeView>();

    [Inject]
    public CubePool(DiContainer container, GameObject cubePrefab)
    {
        _container = container;
        _cubePrefab = cubePrefab;
    }

    public void Init(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewCubeView();
        }
    }

    private CubeView CreateNewCubeView()
    {
        GameObject cubeGO = _container.InstantiatePrefab(_cubePrefab);
        CubeView view = cubeGO.GetComponent<CubeView>();
        if (view == null)
        {
            Debug.LogError("Cube prefab is missing CubeView component!");
            Object.Destroy(cubeGO);
            return null;
        }
        view.SetActive(false);
        _pool.Enqueue(view);
        return view;
    }

    public CubeView GetCubeView()
    {
        if (_pool.Count == 0)
        {
            CreateNewCubeView();
        }
        CubeView view = _pool.Dequeue();
        view.SetActive(true);
        return view;
    }

    public void ReturnCubeView(CubeView view)
    {
        view.SetActive(false);
        _pool.Enqueue(view);
    }
}