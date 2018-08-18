
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DebugOverDraw : MonoBehaviour
{
    public Shader m_OverdrawShader;

    private Camera m_Camera;
    private bool m_SceneFogSettings = false;
    private CameraClearFlags m_ClearFlagSetting;
    private Color m_BackGroundColor;

    void Awake()
    {
        m_Camera = GetComponent<Camera>();
        StoreParam();
    }

    //void OnLevelWasLoaded()
    //{
    //    //每次场景加载取消雾效，缓存并在OnDisable后恢复
    //    m_SceneFogSettings = RenderSettings.fog;
    //    RenderSettings.fog = false;
    //}

    void StoreParam()
    {
        m_SceneFogSettings = RenderSettings.fog;
        RenderSettings.fog = false;

        m_ClearFlagSetting = m_Camera.clearFlags;
        m_BackGroundColor = m_Camera.backgroundColor;
    }

    void OnEnable()
    {
        if (m_OverdrawShader == null)
        {
            m_OverdrawShader = Shader.Find("Tools/DebugOverDraw");
            //m_OverdrawShader = UnityEditor.EditorGUIUtility.LoadRequired("SceneView/SceneViewShowOverdraw.shader") as Shader; //应用unity自带shader即可达到相同效果
        }

        if (m_OverdrawShader != null && m_Camera != null)
        {
            RenderSettings.fog = false;
            m_Camera.clearFlags = CameraClearFlags.Color;
            m_Camera.backgroundColor = Color.black;
            m_Camera.SetReplacementShader(m_OverdrawShader, "");
            bChanged = true;
        }
    }

    void OnDisable()
    {
        if (m_Camera != null)
        {
            RestoreParam();
        }
    }

    void RestoreParam()
    {
        RenderSettings.fog = m_SceneFogSettings;
        //m_Camera.SetReplacementShader(null, ""); //和下面效果相同
        m_Camera.ResetReplacementShader();
        m_Camera.backgroundColor = m_BackGroundColor;
        m_Camera.clearFlags = m_ClearFlagSetting;
    }

    //测试方法 为了方便切换  可在非运行模式下测试
    bool bChanged;
    bool bInited;
    Material mat;
    [ContextMenu("ChangeMode")]
    public void ChangeMode()
    {
        if (bChanged)
        {
            RestoreParam();
        }
        else
        {
            if (!bInited)
            {
                m_Camera = GetComponent<Camera>();
                StoreParam();
                m_OverdrawShader = Shader.Find("Tools/DebugOverDraw");
                bInited = true;
            }

            RenderSettings.fog = false;
            m_Camera.clearFlags = CameraClearFlags.Color;
            m_Camera.backgroundColor = Color.black;
            m_Camera.SetReplacementShader(m_OverdrawShader, "");
        }
        bChanged = !bChanged;
    }
}