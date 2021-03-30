// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RGB_LMS"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}

		[Header(RGB to LSM ConversionMatrix)]
		L("L", Vector) = (17.8824, 43.5161, 4.11935, 1)
		M("M", Vector) = (3.45565, 27.1554, 3.86714, 1)
		S("S", Vector) = (0.0299566, 0.184309, 1.46709, 1)

		[Header(LSM to RGB ConversionMatrix)]
		R("R", Vector) = (0.0809444479, -0.130504409, 0.116721066, 1)
		G("G", Vector) = (0.113614708, -0.0102485335, 0.0540193266, 1)
		B("B", Vector) = (-0.000365296938, -0.00412161469, 0.693511405, 1)

		[Header(LSM shift Colourblind Matrix)] 
		LChange("LChange", Vector) = (1, 1, 1, 1)
		MChange("MChange", Vector) = (1, 1, 1, 1)
		SChange("SChange", Vector) = (1, 1, 1, 1)

		[Header(RGB shift Colourblind Matrix)]
		RChange("RChange", Vector) = (1, 1, 1, 1)
		GChange("GChange", Vector) = (1, 1, 1, 1)
		BChange("BChange", Vector) = (1, 1, 1, 1)
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

		float4 L;
		float4 M;
		float4 S;

		float4 R;
		float4 G;
		float4 B;

		float4 LChange;
		float4 MChange;
		float4 SChange;

		float4 RChange;
		float4 GChange;
		float4 BChange;

		float4 frag(v2f i) : SV_Target
		{
			/*float2 out1Pos = float2(0, _PaletteChoice);
			out1 = tex2D(_PaletteTex, out1Pos);

			float2 out2Pos = float2(0.26, _PaletteChoice);
			out2 = tex2D(_PaletteTex, out2Pos);

			float2 out3Pos = (0, float2(0.51, _PaletteChoice));
			out3 = tex2D(_PaletteTex, out3Pos);

			float2 out4Pos = (0, float2(0.76, _PaletteChoice));
			out4 = tex2D(_PaletteTex, out4Pos);*/

			//float2 out5Pos = (0, 0.5);
			//out5 = tex2D(_PaletteTex, out5Pos);

			float4 color = tex2D(_MainTex, i.uv);

			float4 lsmColor = color;

			lsmColor.r = (color.r * L.x) + (color.g * L.y) + (color.b * L.z);
			lsmColor.g = (color.r * M.x) + (color.g * M.y) + (color.b * M.z);
			lsmColor.b = (color.r * S.x) + (color.g * S.y) + (color.b * S.z);

			float4 cBlindLSMColor = lsmColor;

			cBlindLSMColor.r = (lsmColor.r * LChange.x) + (lsmColor.g * LChange.y) + (lsmColor.b * LChange.z);
			cBlindLSMColor.g = (lsmColor.r * MChange.x) + (lsmColor.g * MChange.y) + (lsmColor.b * MChange.z);
			cBlindLSMColor.b = (lsmColor.r * SChange.x) + (lsmColor.g * SChange.y) + (lsmColor.b * SChange.z);

			float4 cBlindRGBColor = cBlindLSMColor;

			cBlindRGBColor.r = (cBlindLSMColor.r * R.x) + (cBlindLSMColor.g * R.y) + (cBlindLSMColor.b * R.z);
			cBlindRGBColor.g = (cBlindLSMColor.r * G.x) + (cBlindLSMColor.g * G.y) + (cBlindLSMColor.b * G.z);
			cBlindRGBColor.b = (cBlindLSMColor.r * B.x) + (cBlindLSMColor.g * B.y) + (cBlindLSMColor.b * B.z);

			float DR = color.r - cBlindRGBColor.r;
			float DG = color.g - cBlindRGBColor.g;
			float DB = color.b - cBlindRGBColor.b;

			float RMap = (DR * RChange.x) + (DG * RChange.y) + (DB * RChange.z);
			float GMap = (DR * GChange.x) + (DG * GChange.y) + (DB * GChange.z);
			float BMap = (DR * BChange.x) + (DG * BChange.y) + (DB * BChange.z);

			float4 output = float4(color.r + RMap, color.g + GMap, color.b + BMap, color.a);

			return output;
		}
		ENDCG
	}
	}
}
