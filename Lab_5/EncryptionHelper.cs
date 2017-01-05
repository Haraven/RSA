using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab_5
{
    public class EncryptionHelper
    {
        private static int KEYCOUNT = 27;
        private static int RAND_POS_OFFSET = 5;

        private BigInteger GenerateE(BigInteger phi)
        {
            Random rand = new Random();
            BigInteger e;
            do
            {
                e = new BigInteger(rand.Next(1, (int)phi));
                if (e == 1)
                    ++e;

            } while (!(MathHelper.GCDBinary(e, phi) == 1));

            return e;
        }

        public EncryptionHelper InitRSA()
        {
            List<BigInteger> prime_list = MathHelper.PrimeSieve((int)Math.Sqrt((int)Math.Pow(26, _k)), (int)Math.Sqrt((int)Math.Pow(27, _l)));
            Random rand = new Random();

            int rand_pos = rand.Next(0, prime_list.Count);
            _p = prime_list[rand_pos];
            int offset;
            do
            {
                offset = rand.Next(-RAND_POS_OFFSET, RAND_POS_OFFSET);
            } while (offset == 0);
            if (rand_pos + offset < 0 || rand_pos + offset >= prime_list.Count)
                offset = -offset;
            _q = prime_list[rand_pos + offset];

            BigInteger n = _p * _q;
            BigInteger phi = MathHelper.Euler(_p, _q);
            BigInteger e = GenerateE(phi);

            _ke = new Tuple<BigInteger, BigInteger>(n, e);

            _kd = MathHelper.MultiplicativeInverse(e, phi);

            return this;
        }

        private static BigInteger GetKeycodeFor(char letter)
        {
            if (letter == ' ')
                return 0;
            return Convert.ToInt32(letter) - 64;
        }

        private static BigInteger GetKeycodeFor(string msg)
        {
            BigInteger _keycode = new BigInteger(0);
            BigInteger my_pow = (BigInteger)Math.Pow(KEYCOUNT, msg.Length - 1);

            foreach (char letter in msg)
            {
                _keycode += my_pow * GetKeycodeFor(letter);
                my_pow /= KEYCOUNT;
            }
            return _keycode;
        }

        private static char GetCharFor(BigInteger code)
        {
            if (code == 0)
                return ' ';
            return (char)(code + 64);
        }

        private static BigInteger GetHighestPowOfNFrom(BigInteger val, BigInteger n)
        {
            BigInteger myval = 1;
            BigInteger mypow = 0;
            while (myval <= val)
            {
                myval *= n;
                ++mypow;
            }

            return --mypow;
        }

        private string KeycodeToString(BigInteger _keycode, int blocksize)
        {
            string res = "";

            --blocksize;
            while (_keycode != 0 && blocksize >= 0)
            {
                int mypow = blocksize--;
                BigInteger pow27 = (BigInteger)Math.Pow(27, mypow);
                BigInteger letter = _keycode / pow27;
                res += GetCharFor(letter);
                _keycode -= letter * pow27;
            }

            return res;
        }

        private static IList<int> GetAsPowIndicesOfN(BigInteger val, int n)
        {
            IList<int> res = new List<int>();

            while (val != 0)
            {
                res.Add((int)GetHighestPowOfNFrom(val, n));
                val -= (BigInteger)Math.Pow(n, res[res.Count - 1]);
            }

            return res;
        }

        private static BigInteger RepeatedSquaringModularExponentiation(BigInteger a, BigInteger pow, BigInteger mod)
        {
            BigInteger res = a;
            BigInteger last_pow = GetHighestPowOfNFrom(pow, 2);
            BigInteger crt_pow = 0;
            IList<BigInteger> results = new List<BigInteger>();
            while (crt_pow <= last_pow)
            {
                results.Add(res);
                res *= res;
                res = MathHelper.Mod(res, mod);
                crt_pow++;
            }

            IList<int> pows = GetAsPowIndicesOfN(pow, 2);
            res = 1;
            foreach (int mypow in pows)
                res *= results[mypow];

            res = MathHelper.Mod(res, mod);

            return res;
        }

        private string InternalEncryptRSA(string msg)
        {
            BigInteger _keycode = RepeatedSquaringModularExponentiation(GetKeycodeFor(msg), _ke.Item2, _ke.Item1);

            return KeycodeToString(_keycode, _l);
        }

        private string InternalDecryptRSA(string msg)
        {
            BigInteger _keycode = RepeatedSquaringModularExponentiation(GetKeycodeFor(msg), _kd, _ke.Item1);

            return KeycodeToString(_keycode, _k);
        }

        private string PadWithBlanks(string msg)
        {
            while (msg.Length < _k)
                msg += " ";

            return msg;
        }

        public string EncryptRSA(string msg)
        {
            string encrypted_msg = "";
            int i = 0;
            for (; i < msg.Length; i += _k)
            {
                string block = PadWithBlanks((i + _k < msg.Length) ? msg.Substring(i, _k) : msg.Substring(i, msg.Length - i));
                encrypted_msg += InternalEncryptRSA(block);
            }

            return encrypted_msg;
        }

        public string DecryptRSA(string msg)
        {
            string encrypted_msg = "";
            int i = 0;
            for (; i < msg.Length; i += _l)
            {
                string block = PadWithBlanks((i + _l < msg.Length) ? msg.Substring(i, _l) : msg.Substring(i, msg.Length - i));
                encrypted_msg += InternalDecryptRSA(block);
            }

            return encrypted_msg;
        }

        public Tuple<BigInteger, BigInteger> Ke
        {
            get
            {
                return _ke;
            }
            set
            {
                _ke = value;
            }
        }

        public BigInteger Kd
        {
            get
            {
                return _kd;
            }
            set
            {
                _kd = value;
            }
        }

        public int K
        {
            get
            {
                return _k;
            }
            set
            {
                _k = value;
            }
        }

        public int L
        {
            get
            {
                return _l;
            }
            set
            {
                _l = value;
            }
        }

        public BigInteger P
        {
            get
            {
                return _p;
            }
        }

        public BigInteger Q
        {
            get
            {
                return _q;
            }
        }

        private int _k; // encryption blocksize
        private int _l; // decryption blocksize

        private BigInteger _p;
        private BigInteger _q;
        private Tuple<BigInteger, BigInteger> _ke; // encryption key Ke = (n, e)
        private BigInteger _kd; // decryption key Kd = d
    }
}