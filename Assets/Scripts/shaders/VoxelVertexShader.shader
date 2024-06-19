Shader"Custom/WireframeShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _WireColor ("Wireframe Color", Color) = (1,0,0,1)
        _WireThickness ("Wireframe Thickness", Range(0.1, 10.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _WireColor;
float _WireThickness;

struct Input
{
    float2 uv_MainTex;
};

void surf(Input IN, inout SurfaceOutput o)
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;

            // Wireframe calculation
    float edge = fwidth(IN.uv_MainTex.x) * _WireThickness;
    float2 grid = fract(IN.uv_MainTex * 10.0) / edge;
    if (grid.x < 1.0 || grid.y < 1.0)
        o.Albedo = _WireColor.rgb;
}
        ENDCG
    }
FallBack"Diffuse"
}
