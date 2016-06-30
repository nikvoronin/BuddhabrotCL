# BuddhabrotCL

- Multicore OpenCL rendering with Metropolis-Hastings kernel.
- Tested on Intel OpenCL 1.2, NVidia OpenCL 1.2 CUDA 8.
- Open source kernels: buddhabrot, anti-brot, mandelbrot, julia, newton, kaliset. You can write your own and run it.
- Rendering huge images, up to 120Mpx, about 11000x11000px image size.
- A lot of parameters.
- Selectable regions and zoom.
- RNG inside OpenCL kernel. Tausworthe random number generator by L'Ecuyer.
- Image filters: linear, sqrt, log, ln, exp.
- Image tint, exposure, visual parameters.
- Interpolation of the orbits (just for fun): linear, cosine, bicubic.


## Display driver stopped responding and has recovered

https://support.microsoft.com/en-us/kb/2665946<br/>
https://msdn.microsoft.com/en-us/library/windows/hardware/ff569918(v=vs.85).aspx

### Fast answer

You can download and activate the "tool\geforce-timeout-patch.reg".


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
