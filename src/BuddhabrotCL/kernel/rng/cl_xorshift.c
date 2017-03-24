#include "rng/cl_xorshift.h"

__kernel void xorshift(
	const float reMin,
	const float reMax,
	const float imMin,
	const float imMax,
	const uint  minIter,
	const uint  maxIter,
	const uint  width,
	const uint  height,
	const float escapeOrbit,
	const float2 cc,
	const uint4 limColor,
	__global uint4* rngBuffer,
	__global uint4*  outputBuffer)
{
	uint id = get_global_id(0);
	float2 rand = cl_frand2(id, rngBuffer);

	float2 c = (float2)(mix(0.0f, width - 1.0f, rand.x), mix(0.0f, height - 1.0f, rand.y));
	uint i = (uint)(c.x + c.y * width);

	outputBuffer[i].x += 1;
	outputBuffer[i].y += 1;
	outputBuffer[i].z += 1;
	outputBuffer[i].w += 1;
}