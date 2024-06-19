

Shader"Custom/FresnelShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FresnelColor ("Fresnel Color", Color) = (0,0,1,1)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _FresnelColor;
float _FresnelPower;

struct Input
{
    float2 uv_MainTex;
    float3 viewDir;
};

void surf(Input IN, inout SurfaceOutput o)
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;

            // Fresnel effect
    float fresnel = pow(1.0 - dot(normalize(IN.viewDir), o.Normal), _FresnelPower);
    o.Albedo = lerp(o.Albedo, _FresnelColor.rgb, fresnel);
}
        ENDCG
    }
FallBack"Diffuse"
}

