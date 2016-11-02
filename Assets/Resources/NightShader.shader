Shader "Custom/NightShader" {
  Properties{
    _ColorTint("Tint", Color) = (0.3, 0.3, 0.5, 1.0)
    _Color("Color", Color) = (.3,.3,1,1)
    _MainTex("Albedo (RGB)", 2D) = "blue" {}
    _Glossiness("Smoothness", Range(0,1)) = 0.3
    _Metallic("Metallic", Range(0,1)) = 0.2
  }
        SubShader{
          Tags { "RenderType" = "Opaque" }
          LOD 200

          CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows finalcolor:mycolor

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input {
          float2 uv_MainTex;
        };

        fixed4 _ColorTint;
        void mycolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
        {
          color *= _ColorTint;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutputStandard o) {
          // Albedo comes from a texture tinted by color
          fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
          o.Albedo = c.rgb * 0.5;
          // Metallic and smoothness come from slider variables
          o.Metallic = _Metallic;
          o.Smoothness = _Glossiness;
          o.Alpha = c.a;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
