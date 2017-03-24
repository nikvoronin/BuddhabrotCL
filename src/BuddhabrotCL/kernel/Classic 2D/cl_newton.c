#include "rng/cl_taus88.h"

#define EPSILON 0.0000001

struct complex {
	float im;
	float re;
	float r;
	float phi;
};

struct complex createComplexFromPolar(float _r, float _phi) {
	struct complex t;
	t.r = _r;
	t.phi = _phi;

	t.re = _r*half_cos(_phi);
	t.im = _r*half_sin(_phi);

	return t;
}

struct complex kart2comp(float real, float imag) {
	struct complex t;
	t.re = real;
	t.im = imag;

	t.phi = atan2(imag, real);
	t.r = sqrt(t.re*t.re + t.im*t.im);

	return t;
}

struct complex rekart2comp(struct complex t) {
	return kart2comp(t.re, t.im);
}

struct complex recreateComplexFromPolar(struct complex t) {
	return createComplexFromPolar(t.r, t.phi);
}

struct complex addComplex(const struct complex z, const struct complex c) {
	return kart2comp(c.re + z.re, c.im + z.im);
}

struct complex subComplex(const struct complex z, const struct complex c) {
	return kart2comp(z.re - c.re, z.im - c.im);
}

struct complex addComplexScalar(const struct complex z, const float n) {
	return kart2comp(z.re + n, z.im);
}

struct complex subComplexScalar(const struct complex z, const float n) {
	return kart2comp(z.re - n, z.im);
}

struct complex multComplexScalar(const struct complex z, const float n) {
	return kart2comp(z.re * n, z.im * n);
}

struct complex multComplex(const struct complex z, const struct complex c) {
	return kart2comp(z.re*c.re - z.im*c.im, z.re*c.im + z.im*c.re);
}

struct complex powComplex(const struct complex z, int i) {
	struct complex t = z;
	for (int j = 0; j < i - 1; j++)
		t = multComplex(t, z);
	return t;
}

struct complex divComplex(const struct complex z, const struct complex c) {
	return createComplexFromPolar(z.r / c.r, z.phi - c.phi);
}

bool compComplex(const struct complex z, const struct complex c, float comp) {
	if (fabs(z.re - c.re) <= comp && fabs(z.im - c.im) <= comp)
		return true;
	return false;
}

__kernel void newton(
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

	float2 c = (float2)(mix(reMin, reMax, rand.x), mix(imMin, imMax, rand.y));

	struct complex z = kart2comp(c.x, c.y);

	struct complex x1 = kart2comp(0.7071068, 0.7071068);
	struct complex x2 = kart2comp(0.7071068, -0.7071068);
	struct complex x3 = kart2comp(-0.7071068, 0.7071068);
	struct complex x4 = kart2comp(-0.7071068, -0.7071068);

	struct complex f, d;

	uint i = 0;
	uint p = 0;
	while (i < maxIter && fabs(z.r) < escapeOrbit) {
		f = addComplexScalar(powComplex(z, 4), 1);
		d = multComplexScalar(powComplex(z, 3), 3);

		z = subComplex(z, divComplex(f, d));

		i++;
		if (compComplex(z, x1, EPSILON)) {
			p = 1;
			break;
		}
		else if (compComplex(z, x2, EPSILON)) {
			p = 2;
			break;
		}
		else if (compComplex(z, x3, EPSILON)) {
			p = 3;
			break;
		}
		else if (compComplex(z, x4, EPSILON)) {
			p = 4;
			break;
		}
	}


	int x = (c.x - reMin) / (reMax - reMin) * width;
	int y = height - (c.y - imMin) / (imMax - imMin) * height;

	if ((x > 0) && (y > 0) && (x < width) && (y < height))
	{
		outputBuffer[x + y * width].w += i;
		switch (p)
		{
		case 1:
			outputBuffer[x + y * width].x += i;
			outputBuffer[x + y * width].y += i;
			outputBuffer[x + y * width].z = 0;
			break;
		case 2: outputBuffer[x + y * width].x += i; break;
		case 3: outputBuffer[x + y * width].y += i; break;
		case 4: outputBuffer[x + y * width].z += i; break;
		} // switch
	} // if
}