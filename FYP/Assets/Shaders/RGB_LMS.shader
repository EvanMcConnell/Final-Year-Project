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

        [Header(ConversionType)]
        Protanopia("Protanopia", Int) = 0
        Duteranopia("Duteranopia", Int) = 0
        Tritanopia("Tritanopia", Int) = 0

    }
    SubShader
    {
        Tags
        {
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

            // float4x4 LSMRGBconvert =
            // 	float4x4{
            // 		L.x, L.y, L.z, L.w,
            // 		S.x, S.y, S.z, S.w,
            // 		M.x, M.y, M.z, M.w
            // 	} ;


            float4 R;
            float4 G;
            float4 B;

            // float4x4 RGBLSMconvert =
            // 	float4x4{
            // 		R.x, R.y, R.z, R.w,
            // 		G.x, G.y, G.z, G.w,
            // 		B.x, B.y, B.z, B.w
            // 	} ;

            float4 LChange;
            float4 MChange;
            float4 SChange;

            float4 RChange;
            float4 GChange;
            float4 BChange;

            int Protanopia;
            int Duteranopia;
            int Tritanopia;

            float4 output;

            float4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);

                // float4 lsmColor = color;
                //
                // lsmColor.r = (color.r * L.x) + (color.g * L.y) + (color.b * L.z);
                // lsmColor.g = (color.r * M.x) + (color.g * M.y) + (color.b * M.z);
                // lsmColor.b = (color.r * S.x) + (color.g * S.y) + (color.b * S.z);
                //
                // float4 cBlindLSMColor = lsmColor;
                //
                // cBlindLSMColor.r = (lsmColor.r * LChange.x) + (lsmColor.g * LChange.y) + (lsmColor.b * LChange.z);
                // cBlindLSMColor.g = (lsmColor.r * MChange.x) + (lsmColor.g * MChange.y) + (lsmColor.b * MChange.z);
                // cBlindLSMColor.b = (lsmColor.r * SChange.x) + (lsmColor.g * SChange.y) + (lsmColor.b * SChange.z);
                //
                // float4 cBlindRGBColor = cBlindLSMColor;
                //
                // cBlindRGBColor.r = (cBlindLSMColor.r * R.x) + (cBlindLSMColor.g * R.y) + (cBlindLSMColor.b * R.z);
                // cBlindRGBColor.g = (cBlindLSMColor.r * G.x) + (cBlindLSMColor.g * G.y) + (cBlindLSMColor.b * G.z);
                // cBlindRGBColor.b = (cBlindLSMColor.r * B.x) + (cBlindLSMColor.g * B.y) + (cBlindLSMColor.b * B.z);
                //
                // float DR = color.r - cBlindRGBColor.r;
                // float DG = color.g - cBlindRGBColor.g;
                // float DB = color.b - cBlindRGBColor.b;
                //
                // float RMap = (DR * RChange.x) + (DG * RChange.y) + (DB * RChange.z);
                // float GMap = (DR * GChange.x) + (DG * GChange.y) + (DB * GChange.z);
                // float BMap = (DR * BChange.x) + (DG * BChange.y) + (DB * BChange.z);
                //
                // float4 output = float4(color.r + RMap, color.g + GMap, color.b + BMap, color.a);

                int conversionType;

                if (Protanopia == 1)
                    conversionType = 1;
                else if (Duteranopia == 1)
                    conversionType = 2;
                else if (Tritanopia == 1)
                    conversionType = 3;
                else
                    conversionType = 0;

                if (conversionType != 0)
                {
                    float3x3 RGBtoLMS =
                    {
                        17.8824f, 43.5161f, 4.11935f,
                        3.45565f, 27.1554f, 3.86714f,
                        0.0299566f, 0.184309f, 1.46709f
                    };

                    float1x3 RGB = {
                        color.r,
                        color.g,
                        color.b
                    };

                    float1x3 LMS = RGBtoLMS * RGB;

                    float3x3 sim;

                    if (conversionType == 1)
                    {
                        float3x3 temp = {
                            0, 2.02344, -2.52581,
                            0, 1, 0,
                            0, 0, 1
                        };

                        sim._m00_m22 = temp._m00_m22; 
                    }
                    else if (conversionType == 2)
                    {
                        float3x3 temp = {
                            1, 0, 0,
                            0.49421, 0, 1.24827,
                            0, 0, 1
                        };

                        sim._m00_m22 = temp._m00_m22;
                    }
                    else
                    {
                        float3x3 temp = {
                            1, 0, 0,
                            0, 1, 0,
                            -0.395913, 0.8011, 0
                        };

                        sim._m00_m22 = temp._m00_m22;
                    }


                    float1x3 LMSSim = LMS * sim;

                    float3x3 LMStoRGB = {
                        0.08094445, -0.1305044, 0.1167211,
                        0.1136147, -0.01024853, 0.05401933,
                        -0.0003652969, -0.004121615, 0.6935114
                    };

                    float1x3 RGBSim = LMSSim * LMStoRGB;

                    float1x3 RGBdiff = {
                        color.r - RGBSim._11,
                        color.g - RGBSim._12,
                        color.b - RGBSim._13
                    };

                    float3x3 map;

                    if (conversionType == 1)
                    {
                        float3x3 temp = {
                            0, 0, 0,
                            0.7, 1, 0,
                            0.7, 0, 1
                        };
                        
                        map._m00_m22 = temp._m00_m22;
                    }
                    else if (conversionType == 2)
                    {
                        float3x3 temp = {
                            1, 0.7, 0,
                            0, 0, 0,
                            0, 0.7, 1
                        };

                        map._m00_m22 = temp._m00_m22;
                    }
                    else
                    {
                        float3x3 temp = {
                            1, 0, 0.7,
                            0, 1, 0.7,
                            0, 0, 0
                        };

                        map._m00_m22 = temp._m00_m22;
                    }

                    float1x3 RGBMap = RGBdiff * map;


                    float1x3 RGBFinal = {
                        color.r + RGBMap._11,
                        color.g - RGBMap._12,
                        color.b - RGBMap._13
                    };

                    output = float4(RGBFinal._11, RGBFinal._12, RGBFinal._13, color.a);
                }
                else
                {
                    return color;
                }

                return output;
            }
            ENDCG
        }
    }
}