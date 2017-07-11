using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTests.LibraryTest.Util
{
    public class ARomanConverter
    {
        public string Convert(int arabic)
        {
            var roman = string.Empty;
            int[] arabics = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] romans = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            for (int i = 0; i < arabics.Length; i++)
            {
                while (arabic >= arabics[i])
                {
                    roman += romans[i];
                    arabic -= arabics[i];
                }
            }
            return roman;
        }

        [Test]
        [TestCase(1, "I")]
        [TestCase(2, "II")]
        [TestCase(3, "III")]
        [TestCase(4, "IV")]
        [TestCase(5, "V")]
        [TestCase(9, "IX")]
        [TestCase(10, "X")]
        [TestCase(11, "XI")]
        [TestCase(20, "XX")]
        [TestCase(40, "XL")]
        [TestCase(50, "L")]
        [TestCase(90, "XC")]
        [TestCase(100, "C")]
        [TestCase(400, "CD")]
        [TestCase(500, "D")]
        [TestCase(900, "CM")]
        [TestCase(1000, "M")]
        [TestCase(1986, "MCMLXXXVI")]
        public void ConvertStuff(int arabic, string expectedRoman)
        {
            Assert.That(Convert(arabic), Is.EqualTo(expectedRoman));
        }
    }
}
