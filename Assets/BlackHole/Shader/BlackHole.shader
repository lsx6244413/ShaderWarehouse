// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BlackHole"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Range("Born Control", Range(0, 6)) = 0
		_BlackHolePos("Black Hole Position", Vector) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Range;
			float4 _BlackHolePos;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 oriLocalPos = v.vertex;
				float4 localBlackHolePos = mul(unity_WorldToObject, _BlackHolePos.xyz);
				float4 oriWorldPos = mul(unity_ObjectToWorld, v.vertex);
				float4 worldToBlackHole = _BlackHolePos - oriWorldPos;
				float3 localToBlackHole = mul(unity_WorldToObject, (worldToBlackHole).xyz);
				float dis = length(worldToBlackHole);
				float val = max(0, _Range - dis);
				
				v.vertex.xyz += localToBlackHole * val;

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				if (dot((oriWorldPos - _BlackHolePos), (_BlackHolePos - worldPos)) >= 0)
				{
					v.vertex = localBlackHolePos;
				}

				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
