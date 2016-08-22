sampler TextureSampler : register(s0);
float2 ViewportSize;
float4x4 ScrollMatrix;

struct VertexToPixel
{
	float4 Position : SV_Position0;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

VertexToPixel SpriteVertexShader(float4 color : COLOR0, float2 texCoord : TEXCOORD0, float4 position : POSITION0)
{
	VertexToPixel Output = (VertexToPixel)0;

	// Half pixel offset for correct texel centering. - This is solved by DX10 and half pixel offset would actually mess it up
	//position.xy -= 0.5;

	// Viewport adjustment.
	position.xy = position.xy / ViewportSize;
	position.xy *= float2(2, -2);
	position.xy -= float2(1, -1);

	// Transform our texture coordinates to account for camera
	Output.TexCoord = mul(float4(texCoord.xy, 0, 1), ScrollMatrix).xy;

	//pass position and color to PS
	Output.Color = color;
	Output.Position = position;

	return Output;
}

float4 SpritePixelShader(VertexToPixel PSIn) : COLOR0
{
	float4 diffuse = tex2D(TextureSampler , PSIn.TexCoord);
	return PSIn.Color *diffuse;
}

technique SpriteBatch
{
	pass P0
	{
#if SM4
		PixelShader = compile ps_4_0_level_9_1 SpritePixelShader();
#elif SM3
		PixelShader = compile ps_3_0 SpritePixelShader();
#else
		PixelShader = compile ps_2_0 SpritePixelShader();
#endif
	}
}