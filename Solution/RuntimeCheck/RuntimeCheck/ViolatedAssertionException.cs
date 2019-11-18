using System;
using System.Diagnostics;
using System.Runtime.Serialization;



namespace RuntimeCheck
{
    [Serializable]
    public class ViolatedAssertionException : SystemException
    {
        const string StackTraceSerializationName = "AssertionStackTrace";
        string _stackTrace;
        internal ViolatedAssertionException(string message)
            : base(message) {
            var stack = new StackTrace(2); // i do not want to see the call frame of assert and this constructor!
            _stackTrace = stack.ToString();
        }
        protected ViolatedAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { _stackTrace = info.GetString(StackTraceSerializationName); }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(StackTraceSerializationName, _stackTrace);
        }

        public override string StackTrace => _stackTrace;
        
    }

}
