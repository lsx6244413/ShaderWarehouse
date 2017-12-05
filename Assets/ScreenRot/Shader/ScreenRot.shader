﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ScreenRot"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
	_Rot("Rotation", float) = 0
	}

		SubShader
	{
		Tags{ "Queue" = "Geometry" }
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"
			#define PI 3.14159265358979  

			sampler2D _MainTex;
			float _Rot;
			struct a2v
			{
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//与中心点(0.5,0.5)的距离
				float distance = sqrt((i.uv.x - 0.5)*(i.uv.x - 0.5) + (i.uv.y - 0.5)*(i.uv.y - 0.5));
				//距离越大，旋转角度越大
				_Rot *= distance;
				//计算旋转角度
		
				//计算坐标
				float angle = 0;
				if (i.uv.x != 0.5)
				{
					angle = step(i.uv.x, 0.5) * PI + atan((i.uv.y - 0.5) / (i.uv.x - 0.5)) + _Rot;
				}
				else
				{
					angle = step(i.uv.y, 0.5) * PI + PI * 0.5 + _Rot;
				}
				i.uv.x = 0.5 + distance * cos(angle);
				i.uv.y = 0.5 + distance * sin(angle);
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
	FallBack "Specular"
}