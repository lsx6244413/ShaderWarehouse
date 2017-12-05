using UnityEngine;
using System.Collections;

public class ScreenRot : MonoBehaviour
{
    public Material mtl;
    public float rot;

    // Update is called once per frame
    void Update()
    {
        rot += 0.1f;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (rot == 0.0)
            return;
        mtl.SetFloat("_Rot", rot);
        Graphics.Blit(src, dest, mtl);
    }
}