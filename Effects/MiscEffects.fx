sampler uImage0 : register(s0); //input image
sampler uImage1 : register(s1); //extra stuff
sampler uImage2 : register(s2);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
float4 uShaderSpecificData;

float4 EclipxieSun(float2 coords : TEXCOORD0) : COLOR0
{
    float2 normalizedCoords = coords * 2 - 1;
    float theta = atan2(normalizedCoords.y, normalizedCoords.x);
    float rMultiplier = tex2D(uImage1, float2(uShaderSpecificData.x, theta / 6.28313853071)).x;
    float r = max(length(normalizedCoords) - uShaderSpecificData.z, 0) / (1 - uShaderSpecificData.y * rMultiplier) / (1 - uShaderSpecificData.z);
    if (r <= 1) {
        return tex2D(uImage0, float2(r, 0));
    }
    else {
        return float4(0, 0, 0, 0);
    }
}

float4 TriangleFade(float2 coords : TEXCOORD0) : COLOR0
{
    float4 baseColor = tex2D(uImage0, coords);
    if (abs(coords.y * 2 - 1) <= coords.x) {
        return float4(baseColor.xyz, baseColor.w * (1 - coords.x));
    }
    else {
        return float4(0, 0, 0, 0);
    }
}

float4 WarpZoomRipple(float2 coords : TEXCOORD0) : COLOR0
{
    float2 normalizedCoords = coords * 2 - 1;
    float r = (length(normalizedCoords) - uShaderSpecificData.x) / (1 - uShaderSpecificData.x);
    if (r <= 1 && r >= 0) {
        float theta = atan2(normalizedCoords.y, normalizedCoords.x);
        return float4(0.5 - 0.5 * cos(theta), 0.5 - 0.5 * sin(theta), 0.5, 1) * (4 * r * (1 - r) * uShaderSpecificData.y);
    }
    else {
        return float4(0, 0, 0, 0);
    }
}

technique Technique1
{
    pass TriangleFadePass
    {
        PixelShader = compile ps_2_0 TriangleFade();
    }
    pass EclipxieSunPass
    {
        PixelShader = compile ps_2_0 EclipxieSun();
    }
    pass WarpZoomRipplePass
    {
        PixelShader = compile ps_2_0 WarpZoomRipple();
    }
}