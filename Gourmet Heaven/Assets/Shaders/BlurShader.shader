Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // 1.77777... = 16:9 aspect ratio
                float2 texel = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y); // float2(1.777778, 1.0);

                // Centre
                half4 color = tex2D(_MainTex, i.uv) * 100;
                int scale = 2;

                // Inner
                color += tex2D(_MainTex, i.uv + float2(0, scale) * texel) * 85;
                color += tex2D(_MainTex, i.uv + float2(0, -scale) * texel) * 85;
                color += tex2D(_MainTex, i.uv + float2(scale, 0) * texel) * 85;
                color += tex2D(_MainTex, i.uv + float2(-scale, 0) * texel) * 85;

                // Outer
                color += tex2D(_MainTex, i.uv + float2(scale, scale) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(scale, -scale) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(-scale, scale) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(-scale, -scale) * texel) * 76;

                color += tex2D(_MainTex, i.uv + float2(0, 2 * scale) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(0, -2 * scale) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(2 * scale, 0) * texel) * 76;
                color += tex2D(_MainTex, i.uv + float2(-2 * scale, 0) * texel) * 76;

                // More Outer
                color += tex2D(_MainTex, i.uv + float2(2 * scale, 1 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(1 * scale, 2 * scale) * texel) * 70;

                color += tex2D(_MainTex, i.uv + float2(2 * scale, -1 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(1 * scale, -2 * scale) * texel) * 70;

                color += tex2D(_MainTex, i.uv + float2(1 * scale, 2 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(2 * scale, 1 * scale) * texel) * 70;

                color += tex2D(_MainTex, i.uv + float2(1 * scale, -2 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(2 * scale, -1 * scale) * texel) * 70;

                color += tex2D(_MainTex, i.uv + float2(0, 3 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(0, -3 * scale) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(3 * scale, 0) * texel) * 70;
                color += tex2D(_MainTex, i.uv + float2(-3 * scale, 0) * texel) * 70;

                return color / 1888;
            }
            ENDCG
        }
    }
}
