#ifndef __MSET_ALREADY_PRESENT
#define __MSET_ALREADY_PRESENT

uint cl_inside_mandelbrot(float2 c, uint minIter, uint maxIter, float escapeOrbit)
{
	uint iter = 0;
	float2 z = 0.0f;
	if (!(((c.x - 0.25f)*(c.x - 0.25f) + (c.y * c.y))*(((c.x - 0.25f)*(c.x - 0.25f) + (c.y * c.y)) + (c.x - 0.25f)) < 0.25f* c.y * c.y))  //main cardioid
	{
		if (!((c.x + 1.0f) * (c.x + 1.0f) + (c.y * c.y) < 0.0625f))            //2nd order period bulb
		{
			if (!((((c.x + 1.309f)*(c.x + 1.309f)) + c.y*c.y) < 0.00345f))    //smaller bulb left of the period-2 bulb
			{
				if (!((((c.x + 0.125f)*(c.x + 0.125f)) + (c.y - 0.744f)*(c.y - 0.744f)) < 0.0088f))      // smaller bulb bottom of the main cardioid
				{
					if (!((((c.x + 0.125f)*(c.x + 0.125f)) + (c.y + 0.744f)*(c.y + 0.744f)) < 0.0088f))  //smaller bulb top of the main cardioid
					{
						while ((iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit))       //Bruteforce check  
						{
							z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;
							iter++;
						}

						if ((iter >= minIter) && (iter < maxIter))
							return iter;
					}
				}
			}
		}
	}

	return 0;
}

uint cl_iterate_julia(float2* z, float2 c, uint minIter, uint maxIter, float escapeOrbit)
{
	uint iter = 0;
	while ((iter < maxIter) && (z->x * z->x + z->y * z->y < escapeOrbit))
	{
		*z = (float2)(z->x * z->x - z->y * z->y, (z->x * z->y * 2.0f)) + c;
		iter++;
	}

	return iter;
}

#endif