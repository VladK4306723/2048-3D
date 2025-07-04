using UnityEngine;

public class BufferWallTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        CubeController cube = other.GetComponent<CubeController>();
        if (cube != null)
        {
            cube.gameObject.layer = LayerMask.NameToLayer("CubeActive");
        }
    }
}
