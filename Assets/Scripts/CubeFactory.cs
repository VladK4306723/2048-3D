using UnityEngine;

public interface ICubeFactory
{
    void Init(ICubePool cubePool);
    CubeController Create(Transform spawnPoint);
}

public class CubeFactory : ICubeFactory
{
    private ICubePool _cubePool;

    public void Init(ICubePool cubePool)
    {
        _cubePool = cubePool;
    }

    public CubeController Create(Transform spawnPoint)
    {
        int po2Value = Random.value < 0.75 ? 2 : 4;

        CubeView view = _cubePool.GetCubeView();
        view.SetPosition(spawnPoint.position);

        CubeModel model = new CubeModel(po2Value);

        GameObject cubeGO = view.gameObject;
        CubeController controller = cubeGO.AddComponent<CubeController>();
        controller.Initialize(model, view);

        return controller;
    }
}