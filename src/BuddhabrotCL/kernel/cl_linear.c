void line(
	int x0,
	int y0,
	int x1,
	int y1,
	uint width,
	uint height,
	__global uint4*  out,
	int p)
{
	int dx = abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
	int dy = abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
	int err = (dx > dy ? dx : -dy) / 2, e2;

	for (;;)
	{
		switch (p)
		{
			case 0:
				out[x0 + (y0 * width)].x++;
				break;
			case 1:
				out[x0 + (y0 * width)].y++;
				break;
			case 2:
				out[x0 + (y0 * width)].z++;
				break;
		}

		if (x0 == x1 && y0 == y1) break;

		e2 = err;

		if (e2 >-dx) { err -= dy; x0 += sx; }
		if (e2 < dy) { err += dx; y0 += sy; }
	}
}

//Check if choosen point is in MSet
bool isInMSet(
	const float2 c,
	const uint minIter,
	const uint maxIter,
	const float escapeOrbit)
{
	int iter = 0;
	float2 z = 0.0;

	if (!(((c.x - 0.25)*(c.x - 0.25) + (c.y * c.y))*(((c.x - 0.25)*(c.x - 0.25) + (c.y * c.y)) + (c.x - 0.25)) < 0.25* c.y * c.y))  //main cardioid
	{
		if (!((c.x + 1.0) * (c.x + 1.0) + (c.y * c.y) < 0.0625))            //2nd order period bulb
		{
			if (!((((c.x + 1.309)*(c.x + 1.309)) + c.y*c.y) < 0.00345))    //smaller bulb left of the period-2 bulb
			{
				if (!((((c.x + 0.125)*(c.x + 0.125)) + (c.y - 0.744)*(c.y - 0.744)) < 0.0088))      // smaller bulb bottom of the main cardioid
				{
					if (!((((c.x + 0.125)*(c.x + 0.125)) + (c.y + 0.744)*(c.y + 0.744)) < 0.0088))  //smaller bulb top of the main cardioid
					{
						while ((iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit))      //Bruteforce check  
						{
							z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
							iter++;
						}

						if ((iter > minIter) && (iter < maxIter))
							return false;
					}
				}
			}
		}
	}
	return true;
}

//Main kernel
__kernel void buddhabrot(
	const float realMin,
	const float realMax,
	const float imaginaryMin,
	const float imaginaryMax,
	const uint  minIter,
	const uint  maxIter,
	const uint  width,
	const uint  height,
	const float escapeOrbit,
	const uint4 minColor,
	const uint4 maxColor,
	const uint isgrayscale,
	const uint hackMode,
	__global float2* rngBuffer,
	__global uint4*  outputBuffer)
{

	float2 rand = rngBuffer[get_global_id(0)];

	const float deltaReal = (realMax - realMin);
	const float deltaImaginary = (imaginaryMax - imaginaryMin);

	//float2 c = (float2)(mix(realMin, realMax, rand.x), mix(imaginaryMin, imaginaryMax, rand.y));
	float2 c = (float2)(mix(-2, 2, rand.x), mix(-2, 2, rand.y));


	if (isInMSet(c, minIter, maxIter, escapeOrbit) == false)
	{
		int x1, y1;
		int x0 = -1;
		int y0 = -1;
		int iter = 0;
		float2 z = 0.0;

		while ((iter < maxIter) && ((z.x*z.x + z.y*z.y) < escapeOrbit))
		{
			z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
			x1 = (width * (z.x - realMin) / deltaReal);
			y1 = height - (height * (z.y - imaginaryMin) / deltaImaginary);

			if ((iter > minIter) && (x1 > 0) && (y1 > 0) && (x1 < width) && (y1 < height))
			{
				int p = -1;
				if ((iter > minColor.x) && (iter < maxColor.x))
					p = 0;
				else
					if ((iter > minColor.y) && (iter < maxColor.y))
						p = 1;
					else
						if ((iter > minColor.z) && (iter < maxColor.z))
							p = 2;

				if (x0 > -1 && y0 > -1 && p > -1)
					line(x0, y0, x1, y1, width, height, outputBuffer, p);

				x0 = x1; y0 = y1;
			}
			iter++;
		}
	}
}
