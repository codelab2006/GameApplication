sampler2D TextureSampler : register(s0);
float4 MainPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, texCoord);
    color.rgb *= 1.5;
    color.rgb = color.rgb / (color.rgb + 1.0);
    color.rgb = pow(abs(color.rgb), 1.0 / 2.2);
    return color;
}

technique ToneMapping
{
    pass P0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
}
