sampler TextureSampler : register(s0);

float2 Offsets[15];
float Weights[15];

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 output = 0;

	for(int i=0; i < 15; i++)
		output += tex2D(TextureSampler, texCoord + Offsets[i]) * Weights[i];
	
	return output;
}

technique Technique1
{
	pass p0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
