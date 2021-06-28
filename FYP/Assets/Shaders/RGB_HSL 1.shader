// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RGB_HSL1"
{
	
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
	}
		SubShader
	{
		Tags {
		"RenderType" = "Opaque"
		//"Queue" = "Transparent"

		}
		LOD 100

		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha

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
			};

			sampler2D _MainTex;
			sampler2D _PaletteTex;
			float _Magnitude;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


			float output;

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);

				if(color.r == color.g && color.g == color.b)
				{
					return color;
				}
				
				//RGB -> HSL
				float Cmax = max(max(color.r, color.g), color.b);
				float Cmin = min(min(color.r, color.g), color.b);
				float delta = Cmax - Cmin;

				float H = 
					Cmax == color.r ? (color.g - color.b / delta) % 6 :
					Cmax == color.g ? (color.b - color.r / delta) + 2 :
					(color.r - color.g / delta) + 4;

				float L = (Cmax + Cmin) / 2;

				float S =
					delta == 0 ? 0 :
					delta / (1 - abs((2 * L) - 1));

				
				// if (Cmax == color.r || Cmax == color.g) {
				// 	H *= 0.7;
				// 	S *= 0.9;
				// 	L *= 1.25;
				// }
				// else {
				// 	S *= 1.1;
				// 	L *= 0.9;
				// }


				//HSL -> RBG
				float C = (1 - abs((2 * L) - 1)) * S;

				float X = C * (1 - abs((H % 2) - 1));

				float m = L - (C / 2);

				float HDeg = H * 60;
				float3 rgbTemp =
					0 <= HDeg < 60 ? float3 (C, X, 0) :
					60 <= HDeg < 120 ? float3 (X, C, 0) :
					120 <= HDeg < 180 ? float3 (0, C, X) :
					180 <= HDeg < 240 ? float3 (0, X, C) :
					240 <= HDeg < 300 ? float3 (X, 0, C) :
					float3 (C, 0, X);

				float3 RGB = float3(rgbTemp.xyz + m);

				return float4(RGB.xyz, color.a);
			}
			ENDCG
		}
	}

}
