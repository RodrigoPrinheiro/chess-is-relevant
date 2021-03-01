using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelEffect : MonoBehaviour
{
    private Material pixelMaterial = null;
    private Camera _mainCamera;

    [SerializeField] private int pixelDensity = 80;

    void SetMaterial()
    {
        pixelMaterial = new Material(Shader.Find("Hidden/PixelShader"));
    }

    void OnEnable()
    {
        SetMaterial();
    }

    void OnDisable()
    {
        pixelMaterial = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (pixelMaterial == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        Vector2 aspectRatioData;
        if (Screen.height > Screen.width)
            aspectRatioData = new Vector2((float)Screen.width / Screen.height, 1);
        else
            aspectRatioData = new Vector2(1, (float)Screen.height / Screen.width);

        pixelMaterial.SetVector("_AspectRatioMultiplier", aspectRatioData);
        pixelMaterial.SetInt("_PixelDensity", pixelDensity);

        Graphics.Blit(source, destination, pixelMaterial);
    }
}
