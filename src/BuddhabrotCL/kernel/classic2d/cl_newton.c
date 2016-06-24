#define pi 3.14159265359

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
	const uint4 minColor,
	const uint4 maxColor,
	const uint isgrayscale,
	const uint hackMode,
	__global uint4* rngBuffer,
	__global uint4*  outputBuffer)
{
	int id = get_global_id(0);

	// taus88
	uint s1 = rngBuffer[id].x;
	uint s2 = rngBuffer[id].y;
	uint s3 = rngBuffer[id].z;

	float2 rand;

	uint b;
	b = (((s1 << 13) ^ s1) >> 19);
	s1 = (((s1 & 4294967294) << 12) ^ b);
	b = (((s2 << 2) ^ s2) >> 25);
	s2 = (((s2 & 4294967288) << 4) ^ b);
	b = (((s3 << 3) ^ s3) >> 11);
	s3 = (((s3 & 4294967280) << 17) ^ b);

	rand.x = (float)((s1 ^ s2 ^ s3) * 2.3283064365e-10);

	b = (((s1 << 13) ^ s1) >> 19);
	s1 = (((s1 & 4294967294) << 12) ^ b);
	b = (((s2 << 2) ^ s2) >> 25);
	s2 = (((s2 & 4294967288) << 4) ^ b);
	b = (((s3 << 3) ^ s3) >> 11);
	s3 = (((s3 & 4294967280) << 17) ^ b);

	rand.y = (float)((s1 ^ s2 ^ s3) * 2.3283064365e-10);

	rngBuffer[id] = (uint4)(s1, s2, s3, b);

	const float deltaRe = (reMax - reMin);
	const float deltaIm = (imMax - imMin);

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
		if (compComplex(z, x1, 0.0000001)) {
			p = 1;
			break;
		}
		else if (compComplex(z, x2, 0.0000001)) {
			p = 2;
			break;
		}
		else if (compComplex(z, x3, 0.0000001)) {
			p = 3;
			break;
		}
		else if (compComplex(z, x4, 0.0000001)) {
			p = 4;
			break;
		}
	}
	int x = (width * (c.x - reMin) / deltaRe);
	int y = height - (height * (c.y - imMin) / deltaIm);
	if ((x > 0) && (y > 0) && (x < width) && (y < height))
		switch (p)
		{
		case 0:
			outputBuffer[x + y * width].x = 0;
			outputBuffer[x + y * width].y = 0;
			outputBuffer[x + y * width].z = 0;
			break;
		case 1:
			outputBuffer[x + y * width].x += i;
			outputBuffer[x + y * width].y += i;
			outputBuffer[x + y * width].z = 0;
			break;
		case 2: outputBuffer[x + y * width].x += i; break;
		case 3: outputBuffer[x + y * width].y += i; break;
		case 4: outputBuffer[x + y * width].z += i; break;
		}

}