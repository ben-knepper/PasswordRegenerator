using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordGeneration
{
    public class PasswordGenerator
    {
        private static readonly char Delimiter = (char)30; // ASCII Record Separator

        public string MasterPassword { get; set; }
        public string Keyword { get; set; }
        public string Modifier { get; set; }
        public int Length { get; set; }
        public IList<UnitSet> UnitSets { get; set; }

        public PasswordGenerator() { }

        public string Generate()
        {
            var seedBuilder = new StringBuilder();
            AddSeedPart(seedBuilder, MasterPassword);
            AddSeedPart(seedBuilder, Keyword);
            AddSeedPart(seedBuilder, Modifier);
            AddSeedPart(seedBuilder, Length);
            foreach (var unitSet in UnitSets)
                AddSeedPart(seedBuilder, unitSet);

            var rng = new KeccakNumberGenerator(seedBuilder.ToString());

            string unshuffled = PickUnits(rng);
            string password = Shuffle(unshuffled, rng);

            return password;
        }
        private static void AddSeedPart(StringBuilder builder, object part)
        {
            builder.Append(part);
            builder.Append(Delimiter);
        }

        private List<UnitSet> GenerateFormat()
        {
            var format = new List<UnitSet>();

            foreach (var unitSet in UnitSets)
                for (int i = 0; i < unitSet.Bounds.Min; ++i)
                    format.Add(unitSet);

            while (format.Count < Length)
                format.Add(null);

            return format;
        }
        private string PickUnits(KeccakNumberGenerator rng)
        {
            var builder = new StringBuilder();
            var remainingCounts = UnitSets.ToDictionary(u => u, u => u.Bounds.Max);

            // Fill in the minimum amount of each unit set.
            foreach (var unitSet in UnitSets)
                for (int i = 0; i < unitSet.Bounds.Min; ++i)
                    builder.Append(unitSet[rng.NextInt(unitSet.Count)]);

            // Fill in the rest with random characters from the entire superset,
            // removing sets if they reach their maximum
            var availableUnitSets = UnitSets.Where(u => u.Bounds.Max > u.Bounds.Min).ToList();
            int supersetCount = availableUnitSets.Sum(u => u.Count);
            while (builder.Length < Length)
            {
                int supersetIndex = rng.NextInt(supersetCount);
                
                for (int i = 0; i < availableUnitSets.Count; ++i)
                {
                    if (supersetIndex < availableUnitSets[i].Count)
                    {
                        builder.Append(availableUnitSets[i][supersetIndex]);

                        --remainingCounts[availableUnitSets[i]];
                        if (remainingCounts[availableUnitSets[i]] == 0)
                        {
                            availableUnitSets.Remove(availableUnitSets[i]);
                            supersetCount = availableUnitSets.Sum(u => u.Count);
                        }

                        break;
                    }

                    supersetIndex -= availableUnitSets[i].Count;
                }
            }
            return builder.ToString();
        }
        private string Shuffle(string password, KeccakNumberGenerator rng)
        {
            var builder = new StringBuilder(password);
            for (int i = builder.Length; i > 1; --i)
            {
                int swapIndex = rng.NextInt(i);
                var temp = builder[i - 1];
                builder[i - 1] = builder[swapIndex];
                builder[swapIndex] = temp;
            }
            return builder.ToString();
        }
    }
}
