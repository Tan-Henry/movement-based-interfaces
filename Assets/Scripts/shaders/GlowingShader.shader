Shader"Custom/GlowingShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0,1,0,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _GlowColor;
float _GlowIntensity;

struct Input
{
    float2 uv_MainTex;
};

void surf(Input IN, inout SurfaceOutput o)
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;

            // Glow effect
    float glow = sin(_Time.y * 5.0) * _GlowIntensity;
    o.Emission = _GlowColor.rgb * glow;
}
        ENDCG
    }
FallBack"Diffuse"
}

