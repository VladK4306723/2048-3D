using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public interface ILevelManager
{
    void Launch();
    void Init(Transform spawnPoint, ICubePool cubePool, ICubeFactory cubeFactory, CubeInteractionHandler cubeInteractionHandler, int intForWin);
    void Update();
    void Reload();
    void OnCubeLaunched();

    event Action<int> OnScoreChanged;
    event Action<int> OnWinGame;
}

public class LevelManager : ILevelManager
{
    private const float c_stopVelocityThreshold = 0.05f;
    private const float c_stopDelay = 0.25f;
    private int _intForWin;

    private ICubeFactory _cubeFactory;
    private ICubePool _cubePool;
    private Transform _spawnPoint;
    private CubeInteractionHandler _cubeInteractionHandler;

    private CubeController _currentCube;
    private List<CubeController> _activeCubes = new();
    private float _stationaryTimer = 0f;
    private bool _isWaitingForStop = false;
    private int _score = 0;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnWinGame;

    public void Init(Transform spawnPoint, ICubePool cubePool, ICubeFactory cubeFactory, CubeInteractionHandler cubeInteractionHandler, int intForWin)
    {
        _spawnPoint = spawnPoint;
        _cubeFactory = cubeFactory;
        _cubePool = cubePool;
        _cubeInteractionHandler = cubeInteractionHandler;
        _intForWin = intForWin; 

        _cubeFactory.Init(_cubePool);
        _cubeInteractionHandler.Initialize(this);
    }

    public void Launch()
    {
        CubeController newCube = _cubeFactory.Create(_spawnPoint);
        newCube.SetForLaunch(true);
        _currentCube = newCube;
        _cubeInteractionHandler.SetCube(_currentCube);
        newCube.OnCubeDestroyed += HandleCubeDestroyed;
        newCube.OnIntChanged += CheckWinCondition;
        _activeCubes.Add(newCube);
        _stationaryTimer = 0f;
        _isWaitingForStop = false;
    }

    private void HandleCubeDestroyed(CubeController destroyedCube)
    {
        _activeCubes.Remove(destroyedCube);
        destroyedCube.OnCubeDestroyed -= HandleCubeDestroyed;
        destroyedCube.OnIntChanged -= CheckWinCondition;
        if (destroyedCube == _currentCube)
        {
            _currentCube = null;
            Launch();
        }
    }

    public void Update()
    {
        if (_currentCube == null || _currentCube.IsForLaunch) return;

        Rigidbody rb = _currentCube.GetComponent<Rigidbody>();
        if (rb == null) return;

        if (rb.linearVelocity.magnitude > c_stopVelocityThreshold)
        {
            _stationaryTimer = 0f;
            return;
        }

        _stationaryTimer += Time.deltaTime;

        if (_stationaryTimer >= c_stopDelay && !_isWaitingForStop)
        {
            _isWaitingForStop = true;
            Launch();
        }
    }

    public void OnCubeLaunched()
    {
        _currentCube.SetForLaunch(false);
        _stationaryTimer = 0f;
    }

    private void CheckWinCondition(int newPo2Value)
    {
        _score += newPo2Value / 4;
        OnScoreChanged?.Invoke(_score);
        Debug.Log($"Score updated: {_score}");

        if (newPo2Value == _intForWin)
        {
            OnWinGame?.Invoke(_score);
            //Reload();
            Debug.LogError("Win condition met! Game Over!");
        }
    }

    public void Reload()
    {
        _score = 0;
        OnScoreChanged?.Invoke(_score);

        List<CubeController> cubesToProcess = new List<CubeController>(_activeCubes);
        foreach (var cube in cubesToProcess)
        {
            if (cube != null)
            {
                cube.OnCubeDestroyed -= HandleCubeDestroyed;
                cube.OnIntChanged -= CheckWinCondition;
                cube.ReturnToPool();
            }
        }
        _activeCubes.Clear();

        _stationaryTimer = 0f;
        _isWaitingForStop = false;
        _currentCube = null;

        Launch();
    }
}
