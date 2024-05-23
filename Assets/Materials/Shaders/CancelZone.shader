Shader "Unlit/CancelZone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Base ("Base", Color) = (1, 1, 1, 1)
        _Stripe ("Stripe", Color) = (1, 1, 1, 1)
        _Repetition ("Repetition", float) = 10
        _Speed ("Stripe Speed", float) = 1
        _StripeWidthPower ("Stripe Width Power", Range(0.1, 10)) = 1
        _Opacity ("Opacity", Range(0, 1)) = 0.5
        _AspectRatio ("Aspect Ratio", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Cull Off
            ZWrite Off
            Blend One One
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Base;
            float4 _Stripe;
            float _Repetition;
            float _Speed;
            float _StripeWidthPower;
            float _Opacity;
            float _AspectRatio;
            
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float InverseLerp( float a, float b, float v )
            {
                return (v-a)/(b-a);
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x *= _AspectRatio;
                
                float xOffset = uv.y;
                float t = cos( (uv.x - xOffset - (_Time.y * _Speed / 100)) * _Repetition * TAU);
                t = (t + 1.0) * 0.5;
                t = pow(t, _StripeWidthPower);
                float4 outColor = lerp ( _Base, _Stripe, t );
                outColor *= 1 - i.uv.y;
                outColor *= _Opacity;
                return outColor;
                
            }
            
            ENDCG
        }
    }
}
