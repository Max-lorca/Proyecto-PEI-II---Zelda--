using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerInvisibility : MonoBehaviour
{
    Renderer[] rends;
    List<RendererState> renderers;

    public float invisibilityValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();
        renderers = new List<RendererState>();

        for (int i = 0; i < rends.Length; i++)
        {
            RendererState state = new RendererState();
            state.renderer = rends[i];

            Material[] mat = rends[i].materials;
            state.originalColors = new Color[mat.Length];

            for(int j = 0; j < mat.Length; j++)
            {
                state.originalColors[j] = mat[j].color;
            }

            renderers.Add(state);
        }

    }
    public void SetInvisibility(bool invisible)
    {
        foreach (RendererState state in renderers)
        {
            Material[] mats = state.renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (invisible) SetMaterialTransparent(mats[i]);
                else SetMaterialOpaque(mats[i]);
                
                Color c = mats[i].color;
                c.a = invisible ? invisibilityValue : state.originalColors[i].a;
                mats[i].color = c;
            }
        }
    }

    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 2); // Fade
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
    private void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Mode", 0); // Opaque
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
    }
}
