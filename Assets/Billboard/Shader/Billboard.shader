// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Billborad"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VerticalBillboarding("VerticalBillboarding", Range(0, 1)) = 0.1
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
				UNITY_FOG_COORDS(1)
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _VerticalBillboarding;
			
			v2f vert (appdata v)
			{
				v2f o;
				/*float3 wpos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
				float4 worldCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23, 1);
				float4 viewPos = mul(UNITY_MATRIX_V, worldCoord) + float4(wpos, 0);
				o.pos = mul(UNITY_MATRIX_P, viewPos);*/


				/*float4 vert = v.vertex;
				// 不管模型本身的顶点位置，统一重置为 0
				vert.xyz = 0;
				float4 wPos = mul(unity_ObjectToWorld, v.vertex);
				float3 wCamPos = _WorldSpaceCameraPos;
				// 前方向的基轴
				float3 forward = normalize(wCamPos - mul(unity_ObjectToWorld, v.vertex).xyz);
				// 上方向的基轴
				float3 up = normalize(UNITY_MATRIX_V[1].xyz);
				// 右方向的基轴
				float3 right = cross(forward, up);
				wPos.xyz += normalize(UNITY_MATRIX_V[0].xyz) * wPos.x + normalize(UNITY_MATRIX_V[1].xyz) * wPos.y;
				o.pos = mul(UNITY_MATRIX_VP, wPos);*/

				float4 ori = mul(UNITY_MATRIX_MV, float4(0,0,0,1));
				float4 vt = v.vertex;
				vt.y = vt.z;//这个平面是沿xz平面 展开的
				vt.z = 0;//所以只关心其平面上的信息

				//通过加上Object Space的原点在ViewSpace的信息，保持其透视大小
				vt.xyz += ori.xyz;//result is vt.z==ori.z ,so the distance to camera keeped ,and screen size keeped
				o.pos = mul(UNITY_MATRIX_P, vt);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
