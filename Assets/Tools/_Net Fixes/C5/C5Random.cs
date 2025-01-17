// This file is part of the C5 Generic Collection Library for C# and CLI
// See https://github.com/sestoft/C5/blob/master/LICENSE for licensing details.

using System;
namespace C5
{
    /// <summary>
    /// A modern random number generator based on G. Marsaglia:
    /// Seeds for Random Number Generators, Communications of the
    /// ACM 46, 5 (May 2003) 90-93; and a posting by Marsaglia to
    /// comp.lang.c on 2003-04-03.
    /// </summary>
    [Serializable]
    public class C5Random : Random
    {
        private readonly uint[] Q = new uint[16];

        private uint c = 362436, i = 15;


        private uint Cmwc()
        {
            ulong t, a = 487198574UL;
            uint x, r = 0xfffffffe;

            i = (i + 1) & 15;
            t = a * Q[i] + c;
            c = (uint)(t >> 32);
            x = (uint)(t + c);
            if (x < c)
            {
                x++;
                c++;
            }

            return Q[i] = r - x;
        }


        /// <summary>
        /// Get a new random System.Double value
        /// </summary>
        /// <returns>The random double</returns>
        public override double NextDouble()
        {
            return Cmwc() / 4294967296.0;
        }


        /// <summary>
        /// Get a new random System.Double value
        /// </summary>
        /// <returns>The random double</returns>
        protected override double Sample()
        {
            return NextDouble();
        }


        /// <summary>
        /// Get a new random System.Int32 value
        /// </summary>
        /// <returns>The random int</returns>
        public override int Next()
        {
            return (int)Cmwc();
        }


        /// <summary>
        /// Get a random non-negative integer less than a given upper bound
        /// </summary>
        /// <exception cref="ArgumentException">If max is negative</exception>
        /// <param name="max">The upper bound (exclusive)</param>
        /// <returns></returns>
        public override int Next(int max)
        {
            if (max < 0)
            {
                throw new ArgumentException("max must be non-negative");
            }

            return (int)(Cmwc() / 4294967296.0 * max);
        }


        /// <summary>
        /// Get a random integer between two given bounds
        /// </summary>
        /// <exception cref="ArgumentException">If max is less than min</exception>
        /// <param name="min">The lower bound (inclusive)</param>
        /// <param name="max">The upper bound (exclusive)</param>
        /// <returns></returns>
        public override int Next(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max");
            }

            return min + (int)(Cmwc() / 4294967296.0 * (max - min));
        }

        /// <summary>
        /// Fill a array of byte with random bytes
        /// </summary>
        /// <param name="buffer">The array to fill</param>
        public override void NextBytes(byte[] buffer)
        {
            for (int i = 0, length = buffer.Length; i < length; i++)
            {
                buffer[i] = (byte)Cmwc();
            }
        }


        /// <summary>
        /// Create a random number generator seed by system time.
        /// </summary>
        public C5Random()
            : this(DateTime.Now.Ticks)
        {
        }


        /// <summary>
        /// Create a random number generator with a given seed
        /// </summary>
        /// <exception cref="ArgumentException">If seed is zero</exception>
        /// <param name="seed">The seed</param>
        public C5Random(long seed)
        {
            if (seed == 0)
            {
                throw new ArgumentException("Seed must be non-zero");
            }

            uint j = (uint)(seed & 0xFFFFFFFF);

            for (int i = 0; i < 16; i++)
            {
                j ^= j << 13;
                j ^= j >> 17;
                j ^= j << 5;
                Q[i] = j;
            }

            Q[15] = (uint)(seed ^ (seed >> 32));
        }

        /// <summary>
        /// Create a random number generator with a specified internal start state.
        /// </summary>
        /// <exception cref="ArgumentException">If Q is not of length exactly 16</exception>
        /// <param name="Q">The start state. Must be a collection of random bits given by an array of exactly 16 uints.</param>
        public C5Random(uint[] Q)
        {
            if (Q.Length != 16)
            {
                throw new ArgumentException("Q must have length 16, was " + Q.Length);
            }

            Array.Copy(Q, this.Q, 16);
        }
    }
}