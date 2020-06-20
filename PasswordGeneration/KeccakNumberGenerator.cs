using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PasswordGeneration
{
    public class KeccakNumberGenerator
    {
        private static readonly int LaneDimension = 5;
        private static readonly int BytesInLong = 8;
        private static readonly int BytesInInt = 8;
        private static readonly int BitsInByte = 8;
        private static readonly int BitsInLong = BytesInLong * BitsInByte;
        private static readonly int BlockSize = LaneDimension * LaneDimension * BytesInLong;
        private static readonly int NumberOfRounds = 24;
        private static readonly int RoundIncrement = 1;
        private static readonly int[,] RotationOffsets =
        {
            {  0,  1, 62, 28, 27 },
            { 36, 44,  6, 55, 20 },
            {  3, 10, 43, 25, 39 },
            { 41, 45, 15, 21,  8 },
            { 18,  2, 61, 56, 14 }
        };
        private static readonly ulong[] RoundConstants =
        {
            0x0000_0000_0000_0001UL,
            0x0000_0000_0000_8082UL,
            0x8000_0000_0000_808AUL,
            0x8000_0000_8000_8000UL,
            0x0000_0000_0000_808BUL,
            0x0000_0000_8000_0001UL,
            0x8000_0000_8000_8081UL,
            0x8000_0000_0000_8009UL,
            0x0000_0000_0000_008AUL,
            0x0000_0000_0000_0088UL,
            0x0000_0000_8000_8009UL,
            0x0000_0000_8000_000AUL,
            0x0000_0000_8000_808BUL,
            0x8000_0000_0000_008BUL,
            0x8000_0000_0000_8089UL,
            0x8000_0000_0000_8003UL,
            0x8000_0000_0000_8002UL,
            0x8000_0000_0000_0080UL,
            0x0000_0000_0000_800AUL,
            0x8000_0000_8000_000AUL,
            0x8000_0000_8000_8081UL,
            0x8000_0000_0000_8080UL,
            0x0000_0000_8000_0001UL,
            0x8000_0000_8000_8008UL,
        };

        private ulong[] _state = new ulong[LaneDimension * LaneDimension];

        private byte this[int i]
        {
            get
            {
                ulong longValue = _state[i / BytesInLong];
                unchecked
                {
                    return (byte)(longValue >> (BitsInByte * (i % BytesInLong)));
                }
            }
            set
            {
                int longIndex = i / BytesInLong;
                int shift = BitsInByte * (i % BytesInLong);
                _state[longIndex] &= ~(0xFFUL << shift);
                _state[longIndex] |= (ulong)value << shift;
            }
        }
        private ulong this[int i, int j]
        {
            get { return _state[i * LaneDimension + j]; }
            set { _state[i * LaneDimension + j] = value; }
        }
        
        public KeccakNumberGenerator(string seed)
        {
            AbsorbInput(seed);
        }

        public byte NextByte()
        {
            byte random = this[0];

            RunKeccak();

            return random;
        }
        public int NextInt()
        {
            int random;
            unchecked
            {
                random = (int)(this[0, 0] >> BitsInLong / 2);
            }

            RunKeccak();

            return random;
        }
        public uint NextUInt()
        {
            uint random;
            unchecked
            {
                random = (uint)(this[0, 0] >> BitsInLong / 2);
            }

            RunKeccak();

            return random;
        }
        public int NextInt(int min, int max)
        {
            uint range = (uint)((long)max - (long)min);
            uint maxUnbiased = (UInt32.MaxValue / range) * range - 1;

            uint random;
            do
                random = NextUInt();
            while (random > maxUnbiased);

            unchecked
            {
                return (int)(min + (random % range));
            }
        }

        private void AbsorbInput(string input)
        {
            int byteCount = Encoding.UTF8.GetByteCount(input);
            int blockCount = byteCount / BlockSize + 1;

            int remainingBytes = byteCount;
            for (int blockIndex = 0; blockIndex < blockCount; ++blockIndex)
            {
                byte[] bytes = new byte[BlockSize];
                Encoding.UTF8.GetBytes(input, blockIndex * BlockSize,
                    remainingBytes < BlockSize ? remainingBytes : BlockSize, bytes, 0);

                for (int longIndex = 0; longIndex < LaneDimension * LaneDimension; ++longIndex)
                {
                    ulong l = 0;
                    for (int byteIndex = 0; byteIndex < BytesInLong; ++byteIndex, --remainingBytes)
                    {
                        l <<= BitsInByte;
                        l |= bytes[blockIndex * BlockSize + longIndex * BytesInLong + byteIndex];
                    }
                    _state[longIndex] ^= l;

                    if (remainingBytes <= 0)
                        break;
                }

                RunKeccak();
            }
        }

        private void RunKeccak()
        {
            for (int i = 0; i < NumberOfRounds; i += RoundIncrement)
            {
                RunTheta();
                RunRhoPiChi();
                RunIota(i);
            }
        }
        private void RunTheta()
        {
            var c = new ulong[LaneDimension];
            for (int i = 0; i < LaneDimension; ++i)
                c[i] = this[i, 0] ^ this[i, 1] ^ this[i, 2] ^ this[i, 3] ^ this[i, 4];

            var d = new ulong[LaneDimension];
            for (int i = 0; i < LaneDimension; ++i)
                d[i] = c[(i + 4) % 5] + RotateLeft(c[(i + 1) % 5], 1);

            for (int i = 0; i < LaneDimension; ++i)
                for (int j = 0; j < LaneDimension; ++j)
                    this[i, j] ^= d[i];
        }
        private void RunRhoPiChi()
        {
            var b = new ulong[LaneDimension, LaneDimension];

            // rho and pi
            (int y, int x) = (1, 0);
            for (int t = 0; t < LaneDimension * LaneDimension - 1; ++t)
            {
                (int newY, int newX) = (x, (2 * y + 3 * x) % 5);
                b[newY, newX] = RotateLeft(this[y, x], RotationOffsets[y, x]);
                (y, x) = (newY, newX);
            }

            // chi
            for (int i = 0; i < LaneDimension; ++i)
                for (int j = 0; j < LaneDimension; ++j)
                    this[i, j] ^= ~b[(i + 1) % 5, j] & b[(i + 2) % 5, j];
        }
        private void RunIota(int round)
        {
            this[0, 0] ^= RoundConstants[round];
        }

        private ulong RotateLeft(ulong x, int distance)
            => x << distance | x >> BitsInLong - distance;
    }
}
