using System.Numerics;

namespace Lab_5
{
    public class MathHelper
    {
        public static BigInteger Euler(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }

        public static BigInteger GCDBinary(BigInteger a, BigInteger b)
        {
            if (a == 0) return b;
            if (b == 0) return a;

            int shift;
            for (shift = 0; ((a | b) & 1) == 0; ++shift)
            {
                a >>= 1;
                b >>= 1;
            }

            while ((a & 1) == 0)
                a >>= 1;

            do
            {
                while ((b & 1) == 0)
                    b >>= 1;

                if (a > b)
                    Utils.Swap(ref a, ref b);

                b -= a;
            } while (b != 0);

            return a << shift;
        }

        public static BigInteger Mod(BigInteger val, BigInteger n)
        {
            BigInteger r = val % n;
            if (((n > 0) && (r < 0)) || ((n < 0) && (r > 0)))
                r += n;
            return r;
        }

        public static BigInteger MultiplicativeInverse(BigInteger a, BigInteger b)
        {
            BigInteger b0 = b, t, q;
            BigInteger x0 = 0, x1 = 1;

            if (b == 1) return 1;
            while (a > 1)
            {
                q = a / b;
                t = b;
                b = Mod(a, b);
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
                x1 += b0;

            return x1;
        }
    }
}
