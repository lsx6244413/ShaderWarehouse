Shader "Unlit/PostEffectFog"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogColor("Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
		_FogStart("Fog Start", Range(500, 1000)) = 500
		_FogEnd("Fog End", Range(-500, 500)) = 0
		_Density("Density", Range(0, 1)) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work			
			#include "UnityCG.cginc"


			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 interpolatedRay : TEXCOORD1;
			};

			float _FogStart;
			float _FogEnd;
			float _Density;
			fixed4 _FogColor;
			float4 _MainTex_ST;
			float4 _CameraDepthTexture_ST;
			float4x4 _FrustumCornersWS;
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;

				int xx = (int)v.texcoord.x;
				int yy = (int)v.texcoord.y;
				int z = abs (3 - xx - 3 * yy);
				o.interpolatedRay = _FrustumCornersWS[z];

				o.interpolatedRay.w = v.vertex.z ;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				float depth = Linear01Depth ( SAMPLE_DEPTH_TEXTURE ( _CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv, _CameraDepthTexture_ST)));
				float3 worldPos = ( depth * i.interpolatedRay ).xyz + _WorldSpaceCameraPos;

				return lerp (tex2D (_MainTex, i.uv), _FogColor, saturate((_FogStart - worldPos.y) / (_FogStart - _FogEnd) * _Density));
			}
			ENDCG
		}
	}
}
