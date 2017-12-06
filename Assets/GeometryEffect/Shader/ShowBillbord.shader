// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/2Billboard"
{
	Properties
	{
		_SpriteTex("Base (RGB)", 2D) = "white" {}
		_Size("Size", Range(0, 0.1)) = 0.05
	}

	SubShader
	{
		Pass
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma target 5.0
			#pragma vertex VS_Main
			#pragma fragment FS_Main
			#pragma geometry GS_Main
			#include "UnityCG.cginc" 

			struct GS_INPUT
			{
				float4    pos        : POSITION;
				float3    normal    : NORMAL;
				float2  tex0    : TEXCOORD0;
			};

			struct FS_INPUT
			{
				float4    pos        : POSITION;
				float2  tex0    : TEXCOORD0;
			};

			float _Size;
			float4x4 _VP;
			Texture2D _SpriteTex;
			SamplerState sampler_SpriteTex;

			GS_INPUT VS_Main(appdata_base v)
			{
				GS_INPUT output = (GS_INPUT)0;

				output.pos =  v.vertex;
				output.normal = v.normal;
				output.tex0 = float2(0, 0);

				return output;
			}

			[maxvertexcount(4)]
			void GS_Main(triangle GS_INPUT p[3], inout TriangleStream<FS_INPUT> triStream)
			{
				float3 up = float3(0, 1, 0);
				float3 look = _WorldSpaceCameraPos - p[0].pos;
				look.y = 0;
				look = normalize(look);
				float3 right = cross(up, look);

				float halfS = 0.5f * _Size;

				float4 v[4];
				v[0] = float4(p[0].pos + halfS * right - halfS * up, 1.0f);
				v[1] = float4(p[0].pos + halfS * right + halfS * up, 1.0f);
				v[2] = float4(p[0].pos - halfS * right - halfS * up, 1.0f);
				v[3] = float4(p[0].pos - halfS * right + halfS * up, 1.0f);

				FS_INPUT pIn;
				pIn.pos = UnityObjectToClipPos(v[0]);
				pIn.tex0 = float2(1.0f, 0.0f);
				triStream.Append(pIn);			

				pIn.pos = UnityObjectToClipPos(v[1]);
				pIn.tex0 = float2(1.0f, 1.0f);
				triStream.Append(pIn);

				pIn.pos = UnityObjectToClipPos(v[2]);
				pIn.tex0 = float2(0.0f, 0.0f);
				triStream.Append(pIn);

				pIn.pos = UnityObjectToClipPos(v[3]);
				pIn.tex0 = float2(0.0f, 1.0f);
				triStream.Append(pIn);

				triStream.RestartStrip();
			}

			float4 FS_Main(FS_INPUT input) : COLOR
			{
				return _SpriteTex.Sample(sampler_SpriteTex, input.tex0);
			}

			ENDCG
		}
	}
}