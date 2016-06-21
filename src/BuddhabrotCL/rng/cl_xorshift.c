__kernel void rng(
	uint s1,
	uint s2,
	const int bufferSize,
	__global float2* rngBuffer)
{
	uint st;

	for (int i = 0; i < bufferSize; i++) {
		st = s1 ^ (s1 << 11);
		s1 = s2;
		s2 = s2 ^ (s2 >> 19) ^ (st ^ (st >> 18));
		rngBuffer[i] = (float2)((float)st / UINT_MAX, (float)s1 / UINT_MAX);
	}
}