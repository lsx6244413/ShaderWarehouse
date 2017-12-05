using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class RenderDepthMap : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private Shader shader;

    private Material mat = null;

    private void OnEnable()
    {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        Graphics.Blit(source, destination, mat);
    }

    void Awake()
    {
        if (mat == null)
        {
            mat = new Material(shader);
        }
        var camCO = GetComponent<Camera>();
        camCO.depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
