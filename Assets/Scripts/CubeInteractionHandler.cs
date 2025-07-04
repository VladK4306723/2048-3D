using UnityEngine;
using UnityEngine.InputSystem;

public class CubeInteractionHandler : MonoBehaviour
{
    [Header("Movement Boundaries")]
    public float minX;
    public float maxX;

    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private bool _isDragging = false;
    private CubeController _currentCube;
    private ILevelManager _levelManager;

    public void Initialize(ILevelManager levelManager)
    {
        Debug.Log("CubeInteractionHandler: Initialize called");
        _mainCamera = Camera.main;
        _levelManager = levelManager;
    }

    public void SetCube(CubeController currentCube)
    {
        Debug.Log("CubeInteractionHandler: SetCube called");
        _currentCube = currentCube;
        if (_currentCube != null)
        {
            _rigidbody = _currentCube.GetComponent<Rigidbody>();
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
                Debug.Log("CubeInteractionHandler: Cube set with Rigidbody");
            }
            else
            {
                Debug.LogError("CubeInteractionHandler: Rigidbody not found on the cube");
            }
        }
        else
        {
            _rigidbody = null;
        }
    }

    private void Update()
    {
        if (_currentCube == null || _rigidbody == null || !_currentCube.IsForLaunch)
        {
            return;
        }

        Pointer pointer = Pointer.current;
        if (pointer == null)
        {
            return;
        }

        if (pointer.press.wasPressedThisFrame)
        {
            OnDragStart();
        }
        else if (pointer.press.isPressed)
        {

            OnDrag(pointer.position.ReadValue());
        }
        else if (pointer.press.wasReleasedThisFrame)
        {
            OnDragEnd();
        }
    }

    private void OnDragStart()
    {
        _isDragging = true;
        Debug.Log("Drag Started");
    }

    private void OnDrag(Vector2 screenPosition)
    {
        if (_isDragging)
        {
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.WorldToScreenPoint(_currentCube.transform.position).z));

            float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);

            _currentCube.transform.position = new Vector3(clampedX, _currentCube.transform.position.y, _currentCube.transform.position.z);
        }
    }

    private void OnDragEnd()
    {
        if (_isDragging)
        {
            _isDragging = false;

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(Vector3.forward * 30f, ForceMode.Impulse);
                Debug.Log("CubeInteractionHandler: Force applied to cube");
            }

            if (_levelManager != null)
            {
                _levelManager.OnCubeLaunched();
                Debug.Log("CubeInteractionHandler: OnCubeLaunched called");
            }

            _currentCube = null;
            _rigidbody = null;
        }
    }
}