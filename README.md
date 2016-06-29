# BuddhabrotCL

- Multicore OpenCL rendering with fast heuristic kernel.
- Tested on Intel OpenCL 1.2, NVidia OpenCL 1.2 CUDA 8.
- Open source kernels: buddhabrot, anti-brot, mandelbrot, julia, newton, kaliset. You can write your own and run it.
- Metropolis-Hastings algorithm.
- Rendering huge images, up to 120Mpx, about 11000x11000px image size.
- A lot of parameters.
- Selectable regions and zoom.
- RNG inside OpenCL kernel. Tausworthe random number generator by L'Ecuyer.
- Image filters: linear, sqrt, log, ln, exp.
- Image tint, exposure, visual parameters.
- Interpolation of the orbits (just for fun): linear, cosine, bicubic.


## Buddhabrot

Some screenshots are outdated except renders, that still magnificent.

![Buddhabrot](/doc/003.jpg)

![Zoom to region](/doc/002.jpg)

![Cubic Anti-buddhabrot](/doc/004.jpg)

![Bicubic interpolation](/doc/001.jpg)


## Classic figures

Why not to draw classic figures?

### Julia

![Julia](https://img-fotki.yandex.ru/get/26439/97637398.e/0_e1f20_104ef6b_orig.jpg)

### Mandelbrot

![Mandelbrot](https://img-fotki.yandex.ru/get/60682/97637398.e/0_e1f22_9597831a_orig.jpg)

### Newton

![Newton](https://img-fotki.yandex.ru/get/120031/97637398.e/0_e1f23_4e45caa_orig.jpg)

### Modern Kaliset

...but still 2D

![Kaliset](https://img-fotki.yandex.ru/get/96932/97637398.e/0_e1f21_f8931d10_orig.jpg)


## Display driver stopped responding and has recovered

https://support.microsoft.com/en-us/kb/2665946


### Short answer
1. regedit
2. HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers
3. Create: QWORD (64-bit) or DWORD (32-bit) TdrDelay = 8
