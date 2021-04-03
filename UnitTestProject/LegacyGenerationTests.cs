using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PasswordGeneration;
using PasswordGeneration.Legacy;

using PasswordRegenerator.Models;

namespace UnitTestProject
{
    [TestClass]
    public class LegacyGenerationTests
    {
        [TestMethod]
        public void BasicTest()
        {
            string master = "a";
            string keyword = "a";
            string optionalKeyword = null;
            string modifier = null;
            int length = 10;
            var unitSets = new List<UnitSelection>()
            {
                new UnitSelection(PasswordUnitSet.Lowercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Uppercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Numbers, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.AtLeastOne),
            };

            string result = PasswordGeneratorLegacy.Generate(
                master, keyword, optionalKeyword, modifier, length, unitSets);

            Assert.AreEqual(result, @";SY-jzX67a");
        }
        [TestMethod]
        public void OptionalKeywordTest()
        {
            string master = "b";
            string keyword = "b";
            string optionalKeyword = "b";
            string modifier = null;
            int length = 10;
            var unitSets = new List<UnitSelection>()
            {
                new UnitSelection(PasswordUnitSet.Lowercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Uppercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Numbers, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.AtLeastOne),
            };

            string result = PasswordGeneratorLegacy.Generate(
                master, keyword, optionalKeyword, modifier, length, unitSets);

            Assert.AreEqual(result, @"X8{x&0E[_~");
        }
        [TestMethod]
        public void ModifierTest()
        {
            string master = "c";
            string keyword = "c";
            string optionalKeyword = null;
            string modifier = "c";
            int length = 10;
            var unitSets = new List<UnitSelection>()
            {
                new UnitSelection(PasswordUnitSet.Lowercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Uppercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Numbers, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.AtLeastOne),
            };

            string result = PasswordGeneratorLegacy.Generate(
                master, keyword, optionalKeyword, modifier, length, unitSets);

            Assert.AreEqual(result, @"dKYq!6n#mY");
        }
        [TestMethod]
        public void LengthTest()
        {
            string master = "d";
            string keyword = "d";
            string optionalKeyword = null;
            string modifier = null;
            int length = 11;
            var unitSets = new List<UnitSelection>()
            {
                new UnitSelection(PasswordUnitSet.Lowercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Uppercase, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.Numbers, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.AtLeastOne),
            };

            string result = PasswordGeneratorLegacy.Generate(
                master, keyword, optionalKeyword, modifier, length, unitSets);

            Assert.AreEqual(result, @"fSQ!JBSZS8|");
        }
        [TestMethod]
        public void BoundsTest()
        {
            string master = "e";
            string keyword = "e";
            string optionalKeyword = null;
            string modifier = null;
            int length = 10;
            var unitSets = new List<UnitSelection>()
            {
                new UnitSelection(PasswordUnitSet.Lowercase, Bounds.None),
                new UnitSelection(PasswordUnitSet.Uppercase, Bounds.One),
                new UnitSelection(PasswordUnitSet.Numbers, Bounds.AtLeastOne),
                new UnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.Any),
            };

            string result = PasswordGeneratorLegacy.Generate(
                master, keyword, optionalKeyword, modifier, length, unitSets);

            Assert.AreEqual(result, @"4<^F,4|+4:");
        }
    }
}
