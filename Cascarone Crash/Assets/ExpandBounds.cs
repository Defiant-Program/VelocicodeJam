using UnityEngine;

[ExecuteAlways]
public class ExpandBounds : MonoBehaviour
{
    public float boundsSize = 50f; // Match or exceed your shader's size multiplier

    void Update()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter && meshFilter.sharedMesh)
        {
            meshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * boundsSize);
        }
    }
}