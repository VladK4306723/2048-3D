using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ICubePool
{
    void Init(int poolSize, Transform parent);
    CubeView Pull(Transform parent = null);
    void Push(CubeView view);
}

public class CubePool : ICubePool
{
    private DiContainer _container;
    private GameObject _cubePrefab;
    private Queue<CubeView> _pool = new Queue<CubeView>();
    private Transform _parent;

    [Inject]
    public CubePool(DiContainer container, GameObject cubePrefab)
    {
        _container = container;
        _cubePrefab = cubePrefab;
    }

    public void Init(int poolSize, Transform parent)
    {
        _parent = parent;
        for (int i = 0; i < poolSize; i++)
        {
            Create(_parent);
        }
    }

    private CubeView Create(Transform parent)
    {
        GameObject cubeGO = _container.InstantiatePrefab(_cubePrefab, parent);
        CubeView view = cubeGO.GetComponent<CubeView>();
        if (view == null)
        {
            Object.Destroy(cubeGO);
            return null;
        }
        view.SetActive(false);
        _pool.Enqueue(view);
        return view;
    }

    public CubeView Pull(Transform parent = null)
    {
        if (_pool.Count == 0)
        {
            Create(_parent);
        }
        CubeView view = _pool.Dequeue();
        view.SetActive(true);
        return view;
    }

    public void Push(CubeView view)
    {
        view.SetActive(false);
        _pool.Enqueue(view);
    }
}