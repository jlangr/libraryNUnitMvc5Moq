using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace LibraryTests.LibraryTest.Util
{
    public class DictionarySvc
    {
        public Auditor EventAuditor { get; set; }
        public IEmail Emailer { get; internal set; }

        public virtual string LookUp(string key)
        {
            throw new Exception();
        }
        public virtual string StringStuff(string input)
        {
            throw new Exception();
        }
        public virtual void Add(string word, string definition)
        {
            EventAuditor.Initialize();
            EventAuditor.Log($"adding {word}:{definition}");
        }
    }

    public interface IEmail
    {
        void Send(string recipient, string sender, string title, string content);
    }

    public class Auditor
    {
        public virtual void Initialize() { }
        public virtual void Log(string message) { throw new Exception("LOGGER DOWN"); }
    }

    public class Client
    {
        public string GetDefinition(string word) { return CreateDict().LookUp(word); }
        public virtual DictionarySvc CreateDict() { return new DictionarySvc(); }
    }

    [TestFixture]
    public class ScratchMoqTests
    {
        private DictionarySvc dictionaryService;

        class TestClient : Client
        {
            public DictionarySvc Dict { get; set; }
            public override DictionarySvc CreateDict() { return Dict; }
        }

        [SetUp]
        public void Create()
        {
            dictionaryService = new DictionarySvc();
        }

        [Test]
        public void LogsAuditRecordOnAdd()
        {
            var auditorSpy = new Mock<Auditor>();
            dictionaryService.EventAuditor = auditorSpy.Object;

            dictionaryService.Add("dog", "a canine");

            auditorSpy.Verify(auditor => auditor.Log("adding dog:a canine"));
        }
        [Test]
        public void LogsAuditRecordOnAdd_Strict()
        {
            var auditorSpy = new Mock<Auditor>(MockBehavior.Strict);
            dictionaryService.EventAuditor = auditorSpy.Object;
            auditorSpy.Setup(auditor => auditor.Initialize());
            auditorSpy.Setup(auditor => auditor.Log("adding dog:a canine"));

            dictionaryService.Add("dog", "a canine");

            //auditorSpy.Verify(auditor => auditor.Log("adding dog:a canine"));
        }
        [Test]
        public void PropertyOverride()
        {

            TestClient c = new TestClient();
            var mock = new Mock<DictionarySvc>();
            mock.Setup(d => d.LookUp("smelt")).Returns("hooo");
            c.Dict = mock.Object;
            Assert.That(c.GetDefinition("smelt"), Is.EqualTo("hooo"));

        }
        [Test]
        public void X()
        {
            var mock = new Moq.Mock<IList<string>>();
            mock.Setup(l => l.Count).Returns(42);
            IList<string> list = mock.Object;
            Assert.That(list.Count, Is.EqualTo(42));
        }

        [Test]
        public void Args()
        {
            var dictionary = Mock.Of<DictionarySvc>(s => s.LookUp("smelt") == "a fish");

            Assert.That(dictionary.LookUp("smelt"), Is.EqualTo("a fish"));
        }

        [Test]
        public void Args2()
        {
            var dictionary = Mock.Of<DictionarySvc>();
            Mock.Get(dictionary).Setup(d => d.LookUp(It.IsAny<string>())).Returns("a fish");

            Assert.That(dictionary.LookUp("smelt"), Is.EqualTo("a fish"));
        }

        [Test]
        public void ArgsWithPredicate()
        {
            var dictionary = Mock.Of<DictionarySvc>();
            Mock.Get(dictionary).Setup(
                d => d.LookUp(It.Is<string>(s => s.Last() == 's'))).Returns("plural");

            Assert.That(dictionary.LookUp("smelts"), Is.EqualTo("plural"));
            Assert.That(dictionary.LookUp("smelt"), Is.Null);
        }

        [Test]
        public void Args3()
        {
            var mock = new Moq.Mock<DictionarySvc>();
            mock.Setup(x => x.StringStuff(It.IsAny<string>()))
                .Returns((string s) => s.ToLower());

            Assert.That(mock.Object.StringStuff("BOZO"), Is.EqualTo("bozo"));
        }

        [Test]
        public void Args4()
        {
            // returning different values on each invocation
            var mock = new Mock<DictionarySvc>();
            var i = 0;
            var definitions = new string[] { "a fish", "did smell" };
            mock.Setup(d => d.LookUp("smelt"))
                .Returns(() => definitions[i])
                .Callback(() => i++);
            var dict = mock.Object;
            Assert.That(dict.LookUp("smelt"), Is.EqualTo("a fish"));
            Assert.That(dict.LookUp("smelt"), Is.EqualTo("did smell"));
        }
    }
}