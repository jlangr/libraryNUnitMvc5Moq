using NUnit.Framework;
using Library.Models;

namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class MasterClassificationServiceTest
    {
        private IClassificationService service;

        [SetUp]
        public void Initialize()
        {
            service = new MasterClassificationService();
            service.DeleteAllBooks();
        }

        [Test]
        public void AddBook()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
        }

        [Test]
        public void ReturnsNullWhenBookNotFound()
        {
            Assert.That(service.Retrieve("QA123"), Is.Null);
        }

        [Test]
        public void Persists()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");

            service = new MasterClassificationService();
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
        }

        [Test]
        public void MultipleBooks()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");
            service.AddBook("PS334", "Agile Java", "Langr, Jeff", "2005");
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
            AssertBook(service.Retrieve("PS334"), "PS334", "Agile Java", "Langr, Jeff", "2005");
        }

        [Test]
        public void AddMovie()
        {
            service.AddMovie("FF223", "Fight Club", "Fincher, David", "1999");
            var material = service.Retrieve("FF223");
            Assert.That(material.Classification, Is.EqualTo("FF223"));
            Assert.That(material.Title, Is.EqualTo("Fight Club"));
            Assert.That(material.Director, Is.EqualTo("Fincher, David"));
            Assert.That(material.Year, Is.EqualTo("1999"));
        }

        private static void AssertBook(Material material, string classification, string title, string author, string year)
        {
            Assert.That(material.Title, Is.EqualTo(title));
            Assert.That(material.Author, Is.EqualTo(author));
            Assert.That(material.Year, Is.EqualTo(year));
            Assert.That(material.Classification, Is.EqualTo(classification));

            Assert.That(material.CheckoutPolicy.GetType(), Is.EqualTo(typeof(BookCheckoutPolicy)));
        }
    }
}
