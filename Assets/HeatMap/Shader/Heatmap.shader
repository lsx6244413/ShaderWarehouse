// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Credits: Alan Zucconi www.alanzucconi.com
Shader "Ellioman/Heatmap"
{
	Properties
	{
		_HeatTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vertInput
			{
				float4 pos : POSITION;
			};

			struct vertOutput
			{
				float4 pos : POSITION;
				fixed3 worldPos : TEXCOORD1;
			};

			//自定义变量
			uniform int _Points_Length = 0;
			uniform float3 _Points[100];		// (x, y, z) = 位置
			uniform float2 _Properties[100];	// x = 半径, y = 透明度
			sampler2D _HeatTex;

			vertOutput vert(vertInput input)
			{
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
				return o;
			}

			half4 frag(vertOutput output) : COLOR
			{
				half h = 0;
				for (int i = 0; i < _Points_Length; i++)
				{
					// 计算每一个分布点位置的距离
					half di = distance(output.worldPos, _Points[i].xyz);

					half ri = _Properties[i].x;
					half hi = 1 - saturate(di / ri);

					h += hi * _Properties[i].y;
				}

				
				h = saturate(h);
				half4 col = tex2D(_HeatTex, fixed2(h, 0.5));
				return col;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}