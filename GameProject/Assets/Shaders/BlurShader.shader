Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0.0, 50.0)) = 1.0
        _SampleRange ("Sample Range", Range(1.0, 50.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_TexelSize;
            float _BlurSize;
            float _SampleRange;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // 1.77777... = 16:9 aspect ratio
                float2 texel = _MainTex_TexelSize.xy; // float2(1.777778, 1.0);

                float4 color = tex2D(_MainTex, i.uv); 
                int samples = 1;

                for (int x = -floor(_BlurSize * 0.5); x <= floor(_BlurSize * 0.5); x++) {
                    for (int y = -floor(_BlurSize * 0.5); y <= floor(_BlurSize * 0.5); y++) {
                        color += tex2D(_MainTex, i.uv + float2(x, y) * texel * _SampleRange);
                        samples++;
                    }
                }

                return color / samples;
            }
            ENDCG
        }
    }
}
