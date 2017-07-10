using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{

    [Serializable]
    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException() { }
        public InvalidTransactionException(string message) : base(message) { }
        public InvalidTransactionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidTransactionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}