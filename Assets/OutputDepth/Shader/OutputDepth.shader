Shader "Custom/OutputDepth"
{
    Properties
    {
    }
   
    SubShader
    {
        Cull Off ZWrite On ZTest Always
        Pass
        {
            Tags { "RenderType"="Opaque" }
           
            CGPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
               
            uniform sampler2D _CameraDepthTexture;

           
            struct v2f
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
           
                   
            v2f vert(appdata_base v)
            {
                v2f o;
               
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.projPos = ComputeScreenPos(o.position);
                return o;
            }
   
           
            fixed4 frag (v2f i) : SV_Target
            {
                float depth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);//tex2D(_CameraDepthTexture, i.uv).r;
                float4 col = float4(depth, depth, depth, 1);
                return col;
            }
            ENDCG
        }
    }
   
    FallBack "Diffuse"
}