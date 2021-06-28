// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RGB_HSL"
{
	
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}

	/*[Header(RGB to LSM ConversionMatrix)]
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
	BChange("BChange", Vector) = (1, 1, 1, 1)*/
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

			/*float4 L;
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
			float4 BChange;*/

			float Epsilon = 1e-10;



			float4 frag(v2f i) : SV_Target
			{
			

				float4 color = tex2D(_MainTex, i.uv);

				//1- Convert the RGB values to the range 0-1
				float r1 = color.r;
				float g1 = color.g;
				float b1 = color.b;

				float minrgb;
				float maxrgb;

				float4 output;
				float temporary_1;
				float temporary_2;
				float temporary_R;
				float temporary_G;
				float temporary_B;
				float final_R;
				float final_G;
				float final_B;

				//2- Find the minimum and maximum values of R, G and B
				maxrgb = max(max(r1, g1), b1);
				minrgb = min(min(r1, g1), b1);

				float delta = maxrgb - minrgb;

				//3- Luminace
				float L = (maxrgb + minrgb) / (float)2;

				//4- Saturation
				float S;
				if (minrgb == maxrgb) {
					S = 0;
				}
				else {
					if (L <= 0.5) {
						S = (delta) / (maxrgb + minrgb);
					}
					else {
						S = (delta) / (2 - delta);
					}
					//S = delta / (1 - (float)abs((float)2 * L - (float)1));
				}

				//5- Hue
				float H;
				if (maxrgb == r1) {
					H = (float)60 * ((g1 - b1) / delta);
				}
				else if (maxrgb == g1) {
					H = (float)60 * ((float)2 + (b1 - r1)) / (delta);
				}
				else {
					H = (float)60 * ((float)4 + (r1 - g1)) / (delta);
				}

				H = abs( H*60);

				//float3 HSL = float3(H, S, L);

				/*if (maxrgb == r1 || maxrgb == g1) {
					H *= 0.7;
					S *= 0.9;
					L *= 1.25;
				}
				else {
					S *= 1.1;
					L *= 0.9;
				}*/


				//------------ORIGINAL HLS -> RGB------------//
				if (S == 0) {
					output.r = L;
					output.g = L;
					output.b = L;
				}
				else {
					if (L < 0.5) {
						temporary_1 = L * ((float)1 + S);
					}
					else {
						temporary_1 = (L + S) - (L * S);
					}
				
					temporary_2 = 2 * L - temporary_1;
				
					H = H / (float)360;
				
					temporary_R = H + (float)0.333;
					temporary_G = H;
					temporary_B = H - (float)0.333;
				
					temporary_R -= trunc(temporary_R);
					temporary_G -= trunc(temporary_G);
					temporary_B -= trunc(temporary_B);
				
					if (temporary_R > 1) temporary_R -= 1;
					else if (temporary_R < 0) temporary_R += 1;
				
					if (temporary_G> 1) temporary_G -= 1;
					else if (temporary_G < 0) temporary_G += 1;
				
					if (temporary_B > 1) temporary_B -= 1;
					else if (temporary_B < 0) temporary_B += 1;
				
				
				
				
				
				
					if ((float)6 * temporary_R < 1) {
						final_R = temporary_2 + (temporary_1 - temporary_2) * 6 * temporary_R;
					}
					else if ((float)2 * temporary_R < 1) {
						final_R = temporary_1;
					}
					else if ((float)3 * temporary_R < 2) {
						final_R = temporary_2 + (temporary_1 - temporary_2) * ((float)0.666 - temporary_R) * (float)6;
					}
					else {
						final_R = temporary_2;
					}
				
				
					if ((float)6 * temporary_G < 1) {
						final_G = temporary_2 + (temporary_1 - temporary_2) * (float)6 * temporary_G;
					}
					else if ((float)2 * temporary_G < 1) {
						final_G = temporary_1;
					}
					else if ((float)3 * temporary_G < 2) {
						final_G = temporary_2 + (temporary_1 - temporary_2) * ((float)0.666 - temporary_G) * (float)6;
					}
					else {
						final_G = temporary_2;
					}
				
				
					if ((float)6 * temporary_B < 1) {
						final_B = temporary_2 + (temporary_1 - temporary_2) * (float)6 * temporary_B;
					}
					else if ((float)2 * temporary_B < 1) {
						final_B = temporary_1;
					}
					else if ((float)3 * temporary_B < 2) {
						final_B = temporary_2 + (temporary_1 - temporary_2) * ((float)0.666 - temporary_B) * (float)6;
					}
					else {
						final_B = temporary_2;
					}
				
					output.r = final_R;
					output.g = final_G;
					output.b = final_B;
				
					/*output.r = color.r;
					output.g = color.g;
					output.b = color.b;*/
				}


				//float C = (1 - abs((2 * L) - 1)) * S;

				//float X = C * (1 - abs(((H / 60) % 2) - 1));

				//float m = L - (C / 2);

				//float3 tempRGB;

				//if (0 <= H < 60) {
				//	//tempRGB = float3(C + m, X + m, 0 + m);
				//	output.r = C + m;
				//	output.g = X + m;
				//	output.b = m;
				//}
				//else if (60 <= H < 120) {
				//	//tempRGB = float3(X + m, C + m, 0 + m);
				//	output.r = X + m;
				//	output.g = C + m;
				//	output.b = m;
				//}
				//else if (120 <= H < 180) {
				//	//tempRGB = float3(0 + m, C + m, X + m);
				//	output.r = m;
				//	output.g = C + m;
				//	output.b = X + m;
				//}
				//else if (180 <= H < 240) {
				//	//tempRGB = float3(0 + m, X + m, C + m);
				//	output.r = m;
				//	output.g = X + m;
				//	output.b = C + m;
				//}
				//else if (240 <= H < 300) {
				//	//tempRGB = float3(X + m, 0 + m, C + m);
				//	output.r = X + m;
				//	output.g = m;
				//	output.b = C + m;
				//}
				//else if (300 <= H <= 360) {
				//	//tempRGB = float3(C + m, 0 + m, X + m);
				//	output.r = C + m;
				//	output.g = m;
				//	output.b = X + m;
				//}


				/*output.r = tempRGB.x;
				output.g = tempRGB.y;
				output.b = tempRGB.z;*/

	
				// if(color.r == color.g && color.g == color.b)
				// 	return color;

				// Based on work by Sam Hocevar and Emil Persson
				// float4 P = (color.g < color.b) ? float4(color.bg, -1.0, 0.666) : float4(color.gb, 0.0, -0.333);
				// float4 Q = (color.r < P.x) ? float4(P.xyw, color.r) : float4(color.r, P.yzx);
				// float C1 = Q.x - min(Q.w, Q.y);
				// float H = abs((Q.w - Q.y) / (6 * C1 + Epsilon) + Q.z);
				// float3 HCV = float3(H, C1, Q.x);
				//
				// float L = HCV.z - HCV.y * 0.5;
				// float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);

				// if (max(max(color.r, color.g), color.b) == color.r || max(max(color.r, color.g), color.b) == color.g) {
				// 	H *= 0.7;
				// 	S *= 0.9;
				// 	L *= 1.25;
				// }
				// else {
				// 	S *= 1.1;
				// 	L *= 0.9;
				// }


				// float R = abs(H * 6 - 3) - 1;
				// float G = 2 - abs(H * 6 - 2);
				// float B = 2 - abs(H * 6 - 4);
				// float3 RGB = saturate(float3(R, G, B));
				//
				// float C2 = (1 - abs(2 * L - 1)) * S;
				// float3 FRGB = (RGB - 0.5) * C2 + L;
				//
				//
				// float4 output;
				//
				// output.r = FRGB.x;
				// output.g = FRGB.y;
				// output.b = FRGB.z;
				// output.a = color.a;

				return output;
			}
			ENDCG
		}
	}

}
