#if defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL)
#define UNITY_CAN_COMPILE_TESSELLATION 1
#   define UNITY_domain                 domain
#   define UNITY_partitioning           partitioning
#   define UNITY_outputtopology         outputtopology
#   define UNITY_patchconstantfunc      patchconstantfunc
#   define UNITY_outputcontrolpoints    outputcontrolpoints
#endif

struct TessellationFactors
{
	float edge[3] : SV_TessFactor;
	float inside : SV_InsideTessFactor;
};

TessellationFactors patchConstantFunction(InputPatch<Varyings, 3> patch)
{
	TessellationFactors f;

	float2 uv = patch[0].texcoord;
	float factor = tex2Dlod(_TessellationMap, float4(uv, 0, 0)).r;

	float value = lerp(0, 10.0, factor);

	f.edge[0] = value;
	f.edge[1] = value;
	f.edge[2] = value;
	f.inside = value;
	return f;
}

[UNITY_domain("tri")]
[UNITY_outputcontrolpoints(3)]
[UNITY_outputtopology("triangle_cw")]
[UNITY_partitioning("integer")]
[UNITY_patchconstantfunc("patchConstantFunction")]
Varyings hull(InputPatch<Varyings, 3> patch, uint id : SV_OutputControlPointID)
{
	return patch[id];
}

[UNITY_domain("tri")]
Varyings domain(TessellationFactors factors, OutputPatch<Varyings, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
{
	Varyings v;

#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) v.fieldName = \
		patch[0].fieldName * barycentricCoordinates.x + \
		patch[1].fieldName * barycentricCoordinates.y + \
		patch[2].fieldName * barycentricCoordinates.z;

	// These should match Varyings struct, in Grass.hlsl

	//MY_DOMAIN_PROGRAM_INTERPOLATE(positionOS)
	MY_DOMAIN_PROGRAM_INTERPOLATE(positionWS)
		MY_DOMAIN_PROGRAM_INTERPOLATE(positionVS)
		MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
		MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)
		MY_DOMAIN_PROGRAM_INTERPOLATE(texcoord)

		return v;
}