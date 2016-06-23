# BuddhabrotCL

The Buddhabrot renderer in C# with help of OpenCL and with fast heuristic kernel. Based on Laurent (ker2x) Laborde's project [WinBuddhaOpenCL](https://github.com/ker2x/WinBuddhaOpenCL).

- Multicore OpenCL rendering.
- Image filters: linear, square root, logarithmic (e, 10), exponential.
- Image tint: red, blue, green;
- Overexposure.
- A lot of parameters.
- Open source kernels. You can write your own and run it.
- Selectable regions.


## Kernels

- Improved heuristic.
- Classic.
- Linear interpolation.
- Cosine interppolation.
- Anti-buddhabrot.
- Anti-buddhabrot w/ bicubic interpolation.
- Heuristic anti-buddhabrot.

![Buddhabrot](/doc/003.jpg)

![Zoom to region](/doc/002.jpg)

![Bicubic interpolation](/doc/001.jpg)
