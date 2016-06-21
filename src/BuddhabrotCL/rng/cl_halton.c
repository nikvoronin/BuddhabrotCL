__kernel void rng(
    uint s1,
    uint s2,
    const int bufferSize,
    __global float2* rngBuffer
)
{
    uint mBase2 = s1;
    uint mBase3 = s2;
    float mX;
    float mY;

    const float onbythree = 1.0f / 3.0f;

    for(int i = 0; i < bufferSize; i++) {
        uint oldBase2 = mBase2;
        mBase2++;
        uint diff = mBase2 ^ oldBase2;

        float s = 0.5f;

        do {
            if (oldBase2 & 1)
                mX -= s;
            else
                mX += s;
        
            s *= 0.5f;
        
            diff = diff >> 1;
            oldBase2 = oldBase2 >> 1;
        }
        while (diff);

        uint mask = 0x3;  // also the max base 3 digit
        uint add  = 0x1;  // amount to add to force carry once digit==3
        s = onbythree;

        mBase3++;

        // expected iterations: 1.5
        while (1) {
            if ((mBase3 & mask) == mask) {
                mBase3 += add;          // force carry into next 2-bit digit
                mY -= 2 * s;
            
                mask = mask << 2;
                add  = add  << 2;
            
                s *=  onbythree;
            }
            else {
                mY += s;     // we know digit n has gone from a to a + 1
                break;
            }
        }; // while

		rngBuffer[i] = (float2)(mX, mY);
    } // for
}