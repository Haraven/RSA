using System.Collections.Generic;
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

        public static List<BigInteger> PrimeSieve(BigInteger min, BigInteger max)
        {
            List<BigInteger> sieve = new List<BigInteger>();
            List<BigInteger> primes = new List<BigInteger>();
            if (max == 0)
                return primes;
            for (BigInteger i = 1; i < max + 1; ++i)
                sieve.Add(i);
            sieve[0] = 0;
            for (BigInteger i = 2; i < max + 1; ++i)
            {
                int index = (int)i;
                if (sieve[index - 1] != 0)
                {
                    primes.Add(sieve[index - 1]);
                    for (BigInteger j = 2 * sieve[index - 1]; j < max + 1; j += sieve[index - 1])
                        sieve[(int)j - 1] = 0;
                }
            }

            for (int i = 0; i < primes.Count; ++i)
                if (primes[i] < min)
                    primes.RemoveAt(i--);
                else break;

            return primes;
        }
    }
}
