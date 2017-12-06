using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectFog : MonoBehaviour {

    public Material material;
    private Transform m_camTrans;
    private float camFov;
    private float camNear;
    private float camFar;
    private float camAspect;
    // Use this for initialization
    void Awake () {
        m_camTrans = gameObject.transform;
        var camCO = gameObject.GetComponent<Camera>();
        camCO.depthTextureMode |= DepthTextureMode.DepthNormals;
        camFov = camCO.fieldOfView;
        camNear = camCO.nearClipPlane;
        camFar = camCO.farClipPlane;
        camAspect = camCO.aspect;
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (material == null)
        {
            Graphics.Blit(src, dst);
            return;
        }
        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = m_camTrans.right * camNear * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = m_camTrans.up * camNear * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (m_camTrans.forward * camNear - toRight + toTop);
        float camScale = topLeft.magnitude * camFar / camNear;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (m_camTrans.forward * camNear + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (m_camTrans.forward * camNear + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (m_camTrans.forward * camNear - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        material.SetMatrix("_FrustumCornersWS", frustumCorners);
        Graphics.Blit(src, dst, material);
    }
}
