Shader "Unlit/Hologram"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}		//法线贴图
		_RimColor ("Rim Color", Color) = (0.26,0.7,1.0,0.0)		//边缘光颜色
		_RimPower ("Rim Power", Range(0.1,8.0)) = 3.0  //边缘光放大倍数
		_ClipPower("Clip Power",Range(0.0,301.0)) = 200.0 //分割强度  参见surfaceshader examples 中Slices via World Space Position例子，值越大条纹越细
		_Brightness("Brightness",Range(0.0,3.0)) = 1.5	//光强
		_DiffuseAmount("Diffuse Amount",Range(0.0,1.0)) = 0.0	//渐变比例值，看后面lerp函数就明白

		
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
		LOD 100

		Pass
		{
			ZWrite On    // 写入深度缓存的作用就是为ZTest的比较做准备
            //ColorMask 0   //关闭所有颜色渲染通道， 默认是渲染RGBA通道
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal :	NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float4 screenUV : TEXCOORD2;
				float4 posWorld : TEXCOORD3;
				float3 normalDir : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
            float4 _RimColor;
            float _RimPower;
            float _ClipPower;
            float _Brightness;
            float _DiffuseAmount;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
				o.screenUV = ComputeScreenPos(o.pos);
				o.posWorld = mul (UNITY_MATRIX_M, v.vertex );
				o.normalDir = UnityObjectToWorldNormal ( v.normal );
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{	
				if(_ClipPower <= 300.0f)
					clip(frac(i.screenUV.y *  _ClipPower) - 0.5);
				half4 basecol = tex2D(_MainTex, i.uv);
				//灰度值
				half3 graycol = dot(basecol.rgb, float3(0.3, 0.59, 0.11));
				//法线贴图
				half3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));
				//边缘光
				half rim = 1.0 - saturate(dot(normalize(i.viewDir), normal));

			    fixed4 col = fixed4(0, 0, 0, 1);
				col.rgb = lerp(_RimColor.rgb  * _Brightness, basecol, _DiffuseAmount);
				return col;
			}
			ENDCG
		}
	}
}
