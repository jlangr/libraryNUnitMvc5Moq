/*
using System;
using NUnit.Framework;
using Library.Util;
*/

// 0. Remove the multiline comment and make sure things compile.
// 1. Un-ignore the next commented-out test method.
// 2. Run all tests in the project.
// 3. Did the current test fail? If not: You built too much code in a prior step. Undo work for prior tests and try again.
// 4. Make sure you are clear on why the test failed.
// 5. Write only enough code to make that failing test pass (and not break any other tests).
//    Did you write too much? Is there a simple way to get that test to pass???
// 6. If there is a commented-out assertion, uncomment it. It should fail. If not, return to step 5.
// 7. Return to step 1.

namespace LibraryTests.Library.Util
{
    public class NameNormalizerTest
    {
        /*
        private NameNormalizer normalizer;

        [SetUp]
        public void Create()
        {
            normalizer = new NameNormalizer();
        }

        [Ignore("")]
        [Test]
        public void ReturnsEmptyStringWhenEmpty()
        {
            Assert.That(normalizer.Normalize(""), Is.EqualTo(""));
        }

        [Ignore("")]
        [Test]
        public void ReturnsLastFirstWhenFirstLastProvided()
        {
            Assert.That(normalizer.Normalize("Joseph Heller"), Is.EqualTo("Heller, Joseph"));
            Assert.That(normalizer.Normalize("Haruki Murakami"), Is.EqualTo("Murakami, Haruki"));
        }

        [Ignore("")]
        [Test]
        public void ReturnsSingleWordName()
        {
            Assert.That(normalizer.Normalize("Plato"), Is.EqualTo("Plato"));
        }

        [Ignore("")]
        [Test]
        public void TrimsWhitespace()
        {
            Assert.That(normalizer.Normalize("  Big Boi   "), Is.EqualTo("Boi, Big"));
        }

        [Ignore("")]
        [Test]
        public void InitializesMiddleName()
        {
            Assert.That(normalizer.Normalize("Henry David Thoreau"), Is.EqualTo("Thoreau, Henry D."));
        }

        [Ignore("")]
        [Test]
        public void DoesNotInitializeOneLetterMiddleName()
        {
            Assert.That(normalizer.Normalize("Harry S Truman"), Is.EqualTo("Truman, Harry S"));
        }

        [Ignore("")]
        [Test]
        public void InitializesEachOfMultipleMiddleNames()
        {
            Assert.That(normalizer.Normalize("Julia Scarlett Elizabeth Louis-Dreyfus"), Is.EqualTo("Louis-Dreyfus, Julia S. E."));
        }

        [Ignore("")]
        [Test]
        public void AppendsSuffixesToEnd()
        {
            Assert.That(normalizer.Normalize("Martin Luther King, Jr."), Is.EqualTo("King, Martin L., Jr."));
        }

        [Ignore("")]
        [Test]
        public void ThrowsWhenNameContainsTwoCommas()
        {
            var exception = Assert.Throws<ArgumentException>(() => normalizer.Normalize("Thurston, Howell, III"));
            Assert.That(exception.Message, Is.EqualTo("name can have at most one comma"));
        }

        [Ignore("")]
        [Test]
        public void OneWordNamesCanHaveSuffixes()
        {
            Assert.That(normalizer.Normalize("Madonna, Jr."), Is.EqualTo("Madonna, Jr."));
        }
        */
    }
}