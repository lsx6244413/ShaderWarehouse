// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShowDots"
{
	Properties
	{
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
				float4  pos     : POSITION;
				float3  normal  : NORMAL;
				float2  tex0    : TEXCOORD0;
			};
			struct FS_INPUT
			{
				float4  pos        : POSITION;
				float2  tex0    : TEXCOORD0;
			};
			//step1
			GS_INPUT VS_Main(appdata_base v)
			{
				GS_INPUT output = (GS_INPUT)0;
				output.pos = v.vertex;
				output.normal = v.normal;
				output.tex0 = float2(0, 0);
				return output;
			}
			//step2
			[maxvertexcount(4)]
			void GS_Main(triangle GS_INPUT p[3], inout PointStream<FS_INPUT> triStream)
			{
				for (int i = 0; i < 3; i++)
				{
					FS_INPUT pIn;
					pIn.pos = UnityObjectToClipPos(p[i].pos);
					pIn.tex0 = float2(0.0f, 0.0f);
					triStream.Append(pIn);
				}
			}

			/*[maxvertexcount(4)]
			void GS_Main(point GS_INPUT p[1], inout PointStream<FS_INPUT> triStream)
			{
				FS_INPUT pIn;
				pIn.pos = UnityObjectToClipPos(p[0].pos);
				pIn.tex0 = float2(0.0f, 0.0f);
				triStream.Append(pIn);
			}*/

			//step3
			float4 FS_Main(FS_INPUT input) : COLOR
			{
				return float4(1,1,1,1);
			}
			ENDCG
		}
	}
}