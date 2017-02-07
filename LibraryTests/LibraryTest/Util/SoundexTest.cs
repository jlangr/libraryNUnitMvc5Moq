using Library.Util;
using NUnit.Framework;

//Implementing the Soundex Algorithm--Test-First
//
//Per Wikipedia, Soundex is a phonetic algorithm for indexing names
//by sound, as pronounced in English. The goal is for homophones to
//be encoded to the same representation so that they can be matched
//despite minor differences in spelling.
//
//Each [Test] method in SoundexTest.java describes a small increment
//of behavior needed to build the Soundex algorithm.
//
// First: Remove the multiline comments around all the tests.
//
//For each test from top to bottom:
//- Remove the [Ignore("")] annotation
//- Run all tests. Ensure the newly-unignored (current) test fails,
//  and that all other previously passing tests still pass.
//- Write code in Soundex.java to get the current test to pass.
//  Build no more implementation than required.
//
//You'll find a couple very useful helper methods in Soundex.java.
//Use these so you can focus on incrementing the core algorithm.
//
//If you un-ignore a test and it immediately passes,
//you wrote too much code to get a prior test to pass.
//Return to the prior step and find a way to get prior tests
//passing with the minimal code needed.
//
//Read additional comments in each test and follow any additional rules specified.
//
//The rules for Soundex, per Wikipedia (http://en.wikipedia.org/wiki/Soundex):
//1. Retain the first letter of the name and drop all other occurrences of a,e,i,o,u,y,h,w.
//2. Replace consonants with digits as follows (after the first letter):
//    b, f, p, v => 1
//    c, g, j, k, q, s, x, z => 2
//    d, t => 3
//    l => 4
//    m, n => 5
//    r => 6
//3. Two adjacent letters (in the original name) with the same number are coded as a single
//   number; also two letters with the same number separated by 'h' or 'w' are coded as a
//   single number, whereas such letters separated by a vowel are coded twice. This rule
//   also applies to the first letter.
//4. Continue until you have one letter and three numbers. If you run out of letters, fill
//   in 0s until there are three numbers.

namespace LibraryTests.LibraryTest.Util
{
    public class SoundexTest
    {
        private Soundex soundex;

        [SetUp]
        public void Create()
        {
            soundex = new Soundex();
        }

        /*
        [Test]
        [Ignore("until you're ready to uncomment this test and get it to pass!")]
        public void RetainsSoleLetterOfOneLetterWord()
        {
            Assert.That(soundex.Encode("A"), Is.EqualTo("A000"));
        }

        [Test]
        [Ignore("")]
        public void ReplacesConsonantsWithAppropriateDigits()
        {
            Assert.That(soundex.Encode("Ab"), Is.EqualTo("A100"));
            // Uncomment each subsequent assertion, and get to pass, one at a time:
            // Assert.That(soundex.encode("Bcdl"), Is.EqualTo("B234"));
            // Assert.That(soundex.encode("Ajmr"), Is.EqualTo("A256"));

            // Prepare to discuss: Should we have multiple assertions in one test?
        }

        [Test, Ignore("")]
        public void LimitsLengthToFourCharacters()
        {
            Assert.That(soundex.Encode("Dbcdlmr"), Is.EqualTo("D123"));
        }

        [Test, Ignore("")]
        public void IgnoresVowelLikeLetters()
        {
            Assert.That(soundex.Encode("Faeiouhycts"), Is.EqualTo("F232"));
        }

        [Test, Ignore("")]
        public void CombinesDuplicateEncodingsAsSingleNumber()
        {
            // Prepare to discuss:
            // - What is the value of these three preconditions?
            // - What is the cost (tradeoff)? How do you feel about the design choice?
            //Assert.That(soundex.toDigit('b'), Is.EqualTo(soundex.toDigit('f')));
            //Assert.That(soundex.toDigit('c'), Is.EqualTo(soundex.toDigit('g')));
            //Assert.That(soundex.toDigit('d'), Is.EqualTo(soundex.toDigit('t')));

            Assert.That(soundex.Encode("Gbfcgdt"), Is.EqualTo("G123"));
        }

        [Test, Ignore("")]
        public void UppercasesFirstLetter()
        {
            Assert.That(soundex.Encode("abcd"), Is.EqualTo("A123"));
        }

        [Test, Ignore("")]
        public void IgnoresVowelLikeLettersRegardlessOfCase()
        {
            Assert.That(soundex.Encode("FcAEIOUHYts"), Is.EqualTo("F232"));
        }

        [Test, Ignore("")]
        public void ReplacesConsonantsWithAppropriateDigitsRegardlessOfCase()
        {
            Assert.That(soundex.Encode("BCDL"), Is.EqualTo("B234"));
        }

        [Test, Ignore("")]
        public void CombinesDuplicateEncodingsWhenSecondLetterDuplicatesFirst()
        {
            Assert.That(soundex.Encode("Bbcd"), Is.EqualTo("B230"));
        }

        [Test, Ignore("")]
        public void DoesNotCombineDuplicateEncodingsSeparatedByVowels()
        {
            Assert.That(soundex.Encode("Jbobby"), Is.EqualTo("J110"));
        }
        // Congratulations if you made it this far!
        // Prepare to discuss:
        // - What other tests are missing?
        // - What were the costs and benefits of building Soundex incrementally?
        */
    }
}
