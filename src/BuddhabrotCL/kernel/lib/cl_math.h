#ifndef __MATH_ALREADY_PRESENT
#define __MATH_ALREADY_PRESENT

float cl_signum(float value)
{
	if (value > 0) return 1;
	if (value < 0) return -1;
	return 0;
}

#endif