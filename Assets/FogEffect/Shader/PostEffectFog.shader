Shader "Unlit/PostEffectFog"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogColor("Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
		_FogStart("Fog Start", float) = 0
		_Density("Density", float) = 0

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
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 interpolatedRay : TEXCOORD1;
			};

			float _FogStart;
			float _Density;
			fixed4 _FogColor;
			float4 _MainTex_ST;
			float4 _CameraDepthTexture_ST;
			float4x4 _FrustumCornersWS;
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			
			/*v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}*/

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;

				int xx = (int)v.vertex.x;
				int yy = (int)v.vertex.y;
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

				return lerp (tex2D (_MainTex, i.uv), _FogColor, saturate(exp(worldPos.y - _FogStart) * _Density));
			}
			ENDCG
		}
	}
}
