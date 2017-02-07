using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Reporting
{
    public class InventoryReport
    {
        private const int Spacing = 2;
        private const int TitleLength = 20;
        private const int BranchLength = 20;
        private const int AuthorLength = 30;
        private const int YearLength = 6;
        private const int IsbnLength = 10;
        private readonly IRepository<Holding> holdingRepo;
        private readonly IRepository<Branch> branchRepo;
        private readonly IClassificationService classificationService;
        private readonly LibraryOfCongress congress;
        private readonly IDictionary<int, string> branchNames = new Dictionary<int, string>();

        public InventoryReport(IRepository<Holding> holdingRepo, IClassificationService classificationService, IRepository<Branch> branchRepo)
        {
            this.holdingRepo = holdingRepo;
            this.branchRepo = branchRepo;
            this.classificationService = classificationService;
            congress = new LibraryOfCongress();
        }

        public string AllBooks()
        {
            var records = new List<Record>();
            var holdings = holdingRepo.GetAll();
            foreach (var holding in holdings)
                if (holding.CheckoutPolicy is BookCheckoutPolicy)
                {
                    var material = classificationService.Retrieve(holding.Classification);
                    var isbn = congress.RetrieveIsbn(holding.Classification);
                    records.Add(new Record(material.Title, BranchName(holding), material.Author, material.Year, isbn));
                }

            records.Sort();

            var buffer = new StringBuilder();
            AppendHeader(buffer);
            AppendColumnHeaders(buffer);

            foreach (var record in records)
                Append(buffer, record);

            return buffer.ToString();
        }

        private string BranchName(Holding holding)
        {
            if (branchNames.ContainsKey(holding.BranchId))
                return branchNames[holding.BranchId];

            var branchName = branchRepo.GetByID(holding.BranchId).Name;
            branchNames[holding.BranchId] = branchName;
            return branchName;
        }

        private void AppendColumnHeaders(StringBuilder buffer)
        {
            buffer.Append(Pad("Title", TitleLength + Spacing));
            buffer.Append(Pad("Branch", BranchLength + Spacing));
            buffer.Append(Pad("Author", AuthorLength + Spacing));
            buffer.Append(Pad("Year", YearLength + Spacing));
            buffer.Append(Pad("ISBN", IsbnLength));
            buffer.Append(Environment.NewLine);

            buffer.Append(Underlines(TitleLength, Spacing));
            buffer.Append(Underlines(BranchLength, Spacing));
            buffer.Append(Underlines(AuthorLength, Spacing));
            buffer.Append(Underlines(YearLength, Spacing));
            buffer.Append(Underlines(IsbnLength, Spacing));
            buffer.Append(Environment.NewLine);
        }

        private void Append(StringBuilder buffer, Record record)
        {
            buffer.Append(Pad(record.Title, TitleLength));
            buffer.Append(Pad(Spacing));
            buffer.Append(Pad(record.Branch, BranchLength));
            buffer.Append(Pad(Spacing));
            buffer.Append(Pad(record.Author, AuthorLength));
            buffer.Append(Pad(Spacing));
            buffer.Append(Pad(record.Year, YearLength));
            buffer.Append(Pad(Spacing));
            buffer.Append(Pad(record.Isbn, IsbnLength));
            buffer.Append(Environment.NewLine);
        }

        private string Pad(int totalLength)
        {
            return Pad("", totalLength);
        }

        private static string Pad(string text, int totalLength)
        {
            var buffer = new StringBuilder(text);
            for (var i = 0; i < totalLength - text.Length; i++)
                buffer.Append(' ');
            return buffer.ToString();
        }

        private static string Underlines(int count, int spacing)
        {
            var buffer = new StringBuilder();
            for (var i = 0; i < count; i++)
                buffer.Append('-');
            return Pad(buffer.ToString(), count + spacing);
        }

        private static void AppendHeader(StringBuilder buffer)
        {
            buffer.Append("Inventory" + Environment.NewLine);
            buffer.Append(Environment.NewLine);
        }

        class Record : IComparable
        {
            public Record(string title, string branch, string author, string year, string isbn)
            {
                Title = title;
                Branch = branch;
                Author = author;
                Year = year;
                Isbn = isbn;
            }

            public string Title { get; private set; }

            public string Branch { get; private set; }

            public string Author { get; private set; }

            public string Year { get; private set; }

            public string Isbn { get; private set; }

            public int CompareTo(Object o)
            {
                var that = (Record)o;
                var authorCompare = Author.CompareTo(that.Author);
                if (authorCompare != 0)
                    return authorCompare;
                return Title.CompareTo(that.Title);
            }
        }
    }
}