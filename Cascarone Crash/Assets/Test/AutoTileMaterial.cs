using UnityEngine;

[ExecuteAlways]
public class AutoTileMaterial : MonoBehaviour
{
    public Renderer targetRenderer;
    public Vector2 tileMultiplier = Vector2.one;

    void Update()
    {
        if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            Vector3 scale = transform.lossyScale;
            Vector2 newTiling = new Vector2(scale.x * tileMultiplier.x, scale.y * tileMultiplier.y);
            targetRenderer.sharedMaterial.mainTextureScale = newTiling;
        }
    }
}