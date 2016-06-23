//Check if choosen point is in MSet
bool isInMSet(
    const float2 c,
    const uint minIter,
    const uint maxIter,
    const float escapeOrbit)
{
    int iter = 0;
    float2 z = 0.0;

    if( !(((c.x-0.25)*(c.x-0.25) + (c.y * c.y))*(((c.x-0.25)*(c.x-0.25) + (c.y * c.y))+(c.x-0.25)) < 0.25* c.y * c.y))  //main cardioid
    {
        if( !((c.x+1.0) * (c.x+1.0) + (c.y * c.y) < 0.0625))            //2nd order period bulb
        {
            if (!(( ((c.x+1.309)*(c.x+1.309)) + c.y*c.y) < 0.00345))    //smaller bulb left of the period-2 bulb
            {
                if (!((((c.x+0.125)*(c.x+0.125)) + (c.y-0.744)*(c.y-0.744)) < 0.0088))      // smaller bulb bottom of the main cardioid
                {
                    if (!((((c.x+0.125)*(c.x+0.125)) + (c.y+0.744)*(c.y+0.744)) < 0.0088))  //smaller bulb top of the main cardioid
                    {
                        while( (iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit) )       //Bruteforce check  
                        {
                            z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
                            iter++;
                        }

						if( (iter > minIter) && (iter < maxIter))
                            return false;
                    }
                }
            }
        }
    }
    return true;
}

float signum(float value)
{
	if (value > 0) return 1;
	if (value < 0) return -1;
	return 0;
}

__kernel void buddhabrot(
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

	float shre = deltaRe / width / 2;
	float shim = deltaIm / height / 2;

	float2 c = (float2)(mix(-2, 2, rand.x), mix(-2, 2, rand.y));

	uint jMax = 100;
	float alpha = 0;
	uint j = 0;
	uint atscr, lastatscr = 0;
	while((j < jMax) && (j < 500))
	{
		lastatscr = atscr;
		atscr = 0;

		if (isInMSet(c, minIter, maxIter, escapeOrbit))
			break;
		else
		{
			int x1, y1;
			int x0 = -1;
			int y0 = -1;
			int iter = 0;
			float2 z = 0.0;
			int i;

			while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
			{
				z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
				x1 = (width * (z.x - reMin) / deltaRe);
				y1 = height - (height * (z.y - imMin) / deltaIm);

				if ((iter > minIter) && (x1 > 0) && (y1 > 0) && (x1 < width) && (y1 < height))
				{
					atscr++;

					i = x1 + (y1 * width);

					if (isgrayscale)
						outputBuffer[i].x++;
					else
						if ((iter > minColor.x) && (iter < maxColor.x))
							outputBuffer[i].x++;
						else
							if ((iter > minColor.y) && (iter < maxColor.y))
								outputBuffer[i].y++;
							else
								if ((iter > minColor.z) && (iter < maxColor.z))
									outputBuffer[i].z++;
				} // if

				iter++;
			} // while
		} // if
		
		// heuristics
		if (!atscr)
			break;
		else {
			if (lastatscr != atscr)
				if (atscr > lastatscr)
					jMax += 10;
				else
				{
					alpha += 1.047197551196597;
					shre *= signum(half_cos(alpha));
					shim *= signum(half_sin(alpha));
					jMax -= 10;
				}

			c.x += shre;
			c.y += shim;
		}

		j++;
	} // while j
}