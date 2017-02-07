using System;

namespace Library.Reporting
{
    public class LibraryOfCongress
    {
        virtual public string RetrieveIsbn(string classification)
        {
            throw new Exception("connection currently unavailable, please try later");
        }
    }
}