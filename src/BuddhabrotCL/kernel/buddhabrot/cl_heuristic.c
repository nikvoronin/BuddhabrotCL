uint isInMSet(
    const float2 c,
    const uint minIter,
    const uint maxIter,
    const float escapeOrbit)
{
    int iter = 0;
    float2 z = 0.0f;

    if( !(((c.x-0.25f)*(c.x-0.25f) + (c.y * c.y))*(((c.x-0.25f)*(c.x-0.25f) + (c.y * c.y))+(c.x-0.25f)) < 0.25f* c.y * c.y))  //main cardioid
    {
        if( !((c.x+1.0f) * (c.x+1.0f) + (c.y * c.y) < 0.0625f))            //2nd order period bulb
        {
            if (!(( ((c.x+1.309f)*(c.x+1.309f)) + c.y*c.y) < 0.00345f))    //smaller bulb left of the period-2 bulb
            {
                if (!((((c.x+0.125f)*(c.x+0.125f)) + (c.y-0.744f)*(c.y-0.744f)) < 0.0088f))      // smaller bulb bottom of the main cardioid
                {
                    if (!((((c.x+0.125f)*(c.x+0.125f)) + (c.y+0.744f)*(c.y+0.744f)) < 0.0088f))  //smaller bulb top of the main cardioid
                    {
                        while( (iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit) )       //Bruteforce check  
                        {
                            z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;
                            iter++;
                        }

						if( (iter > minIter) && (iter < maxIter))
                            return 0;
                    }
                }
            }
        }
    }
    return 1;
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
	const float2 cc,
    const uint4 minColor,
    const uint4 maxColor,
    const uint isgrayscale,
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

	float rew = reMax - reMin;
	float imh = imMax - imMin;

	rew = 1.0f / rew;
	imh = 1.0f / imh;

	float2 c = (float2)(mix(-2.0f, 2.0f, rand.x), mix(-2.0f, 2.0f, rand.y));

	uint jMax = 100;
	float alpha = 0.0f;
	uint j = 0;
	uint atscr, lastatscr = 0;
	float rr = rew / width * 0.5f;
	float ri = imh / height * 0.5f;
	while((j < jMax) && (j < 500))
	{
		lastatscr = atscr;
		atscr = 0;

		if (isInMSet(c, minIter, maxIter, escapeOrbit))
			break;
		else
		{
			int x, y;
			int iter = 0;
			float2 z = 0.0f;
			int i;

			while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
			{
				z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;

				x = (z.x - reMin) * rew * width;
				y = height - (z.y - imMin) * imh * height;

				if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
				{
					atscr++;

					i = x + (y * width);

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
			if (atscr != lastatscr)
				if (atscr > lastatscr)
					jMax += 1;
				else
					jMax -= 3;

			c.x += rr * half_cos(alpha);
			c.y += ri * half_sin(alpha);
			rr *= 1.25f;
			ri *= 1.25f;
			alpha += 0.785398163397458f;
		}

		j++;
	} // while j
}