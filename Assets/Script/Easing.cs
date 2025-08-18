using UnityEngine;

static public class Easing {

    static public float SmoothStart(float t, int power=2) {
        return Pow(t, power);
    }

    static public float SmoothStop(float t, int power=2) {
        return Flip(Pow(Flip(t), power));
    }

    static public float SmoothStep(float t, int power=2) {
        return Crossfade(SmoothStart(t, power), SmoothStop(t, power), t);
    }

    static public float Crossfade(float a, float b, float t) {
        return a + t * (b - a);
    }

    static public float Flip(float t) {
        return 1 - t;
    }

    static public float Pow(float t, int power) {
        for (int i = 1; i < power; ++i) t *= t;
        return t;
    }

    // Cubic (3rd) Bezier through A, B, C, D,
    // where A (start) and D (end) are assumed to be 0 and 1
    static public float NormalizedBezier3(float b, float c, float t) {
        float s = 1 - t;
        float t2 = t * t;
        float s2 = s * s;
        float t3 = t2 * t;
        return (3 * b * s2 * t) + (3 * c * s * t2) + (t3);
    }
}
