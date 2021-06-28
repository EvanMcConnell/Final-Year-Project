Shader "Unlit/PIckup"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _cutoff ("Cutoff", Range(0, 1)) = 1
        _target ("Target Colour", Color) = (1,1,1,1)
        _brightness ("Brightness", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float _cutoff;
            float4 _target;
            float _brightness;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float rDelta = _target.r - col.r;
                float gDelta = _target.g - col.g;
                float bDelta = _target.b - col.b;
                
                col.r += rDelta * ( _brightness * _cutoff );
                col.g += gDelta * ( _brightness * _cutoff );
                col.b += bDelta * ( _brightness * _cutoff );
                
                return col;
            }
            ENDCG
        }
    }
}
