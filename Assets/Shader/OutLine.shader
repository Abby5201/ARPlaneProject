Shader "Custom/Outline" {
    Properties {
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.01
        _MainTex("Texture", 2D) = "white" {}
        _OutlineTex("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
      
        
        Pass {
            // 其他Pass

            // 描边Pass
            Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv:TEXCOORD0;
                float3 uv2:TEXCOOR1;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _OutlineTex;
            fixed4 frag (v2f j) : SV_Target {
                fixed4 col = tex2D(_MainTex, j.uv);
                fixed4 outline = tex2D(_OutlineTex, j.uv); // 描边纹理
                return lerp(col, outline, outline.a);
            }
            ENDCG
        }
        
        
          Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            fixed4 _OutlineColor;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return _OutlineColor;
            }
            ENDCG
        }
        
    }
}