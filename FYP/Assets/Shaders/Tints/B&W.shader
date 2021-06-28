Shader "Unlit/B&W"
{
    Properties
    {
        Desaturation("Desaturation", Range(0, 1)) = 1
        [Header(Invert)]
        Invert("1 on | 0 off", Int) = 0

        [Header(BW Offsets)]
        AvgROffset("R", Range(0, 2)) = 1
        AvgGOffset("G", Range(0, 2)) = 1
        AvgBOffset("B", Range(0, 2)) = 1

        [Header(Color Shift)]
        ShiftRGB("1 True | 0 False", Int) = 1

        [Header(Color Offsets)]
        FinalROffset("R", Range(0, 2)) = 1
        FinalGOffset("G", Range(0, 2)) = 1
        FinalBOffset("B", Range(0, 2)) = 1

        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float Desaturation;
            float AvgROffset;
            float AvgGOffset;
            float AvgBOffset;
            float ShiftRGB;
            float FinalROffset;
            float FinalGOffset;
            float FinalBOffset;
            int Invert;


            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float averageValue = (col.r * AvgROffset + col.g * AvgGOffset + col.b * AvgBOffset) / 3;

                float rDelta = averageValue - col.r;
                float gDelta = averageValue - col.g;
                float bDelta = averageValue - col.b;

                col.r += rDelta * Desaturation;
                col.g += gDelta * Desaturation;
                col.b += bDelta * Desaturation;

                if (ShiftRGB == 1)
                {
                    col.r *= FinalROffset;
                    col.g *= FinalGOffset;
                    col.b *= FinalBOffset;
                }

                if (Invert == 1)
                {
                    col.r = 1 - col.r;
                    col.g = 1 - col.g;
                    col.b = 1 - col.b;
                }

                return col;
            }
            ENDCG
        }
    }
}