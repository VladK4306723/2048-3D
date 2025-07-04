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
        CubeView view = _cubePool.GetCubeView();
        int po2Value = Random.value < 0.75f ? 2 : 4;

        CubeModel model = new CubeModel(po2Value);
        GameObject cubeGO = view.gameObject;
        CubeController controller = cubeGO.GetComponent<CubeController>();
        if (controller == null)
        {
            controller = cubeGO.AddComponent<CubeController>();
        }
        controller.Init(model, view, _cubePool);

        controller.transform.position = spawnPoint.position;
        controller.SetForLaunch(true);

        return controller;
    }
}