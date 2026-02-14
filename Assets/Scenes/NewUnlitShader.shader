Shader "UI/AcrylicPanel"
{
    Properties
    {
        _MainTex ("Background Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,0.5)
        _BlurRadius ("Blur Radius", Range(0, 5)) = 2
        _Desaturate ("Desaturate", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Tint;
            float _BlurRadius;
            float _Desaturate;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed3 Desaturate(fixed3 c, float a)
            {
                fixed g = dot(c, fixed3(0.299, 0.587, 0.114));
                return lerp(c, fixed3(g,g,g), a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Kawase/box blur semplificato
                float2 offset = _BlurRadius * _MainTex_TexelSize.xy;
                fixed4 col = tex2D(_MainTex, i.uv);
                col += tex2D(_MainTex, i.uv + offset);
                col += tex2D(_MainTex, i.uv - offset);
                col += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y));
                col += tex2D(_MainTex, i.uv - float2(offset.x, -offset.y));
                col /= 5;

                // Desaturazione
                col.rgb = Desaturate(col.rgb, _Desaturate);

                // Applica tinta
                col = lerp(col, _Tint, _Tint.a);

                return col;
            }
            ENDCG
        }
    }
}
