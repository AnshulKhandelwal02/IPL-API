namespace DataFeed.Utilities
{
    public class ResponseEnvelope<T>
    {
        public string MessageKey { get; set; }
        public string ErrorCode { get; set; }
        public ResponseType ResponseType { get; set; }
        public T Data { get; set; }

        protected ResponseEnvelope(T data, ResponseType responseType, string messageKey, string errorCode = null)
        {
            Data = data;
            ResponseType = responseType;
            MessageKey = messageKey;
            ErrorCode = errorCode;
        }

        public static ResponseEnvelope<T> Error(T data, string messageKey = MessageKeys.General.Error)
        {
            return new ResponseEnvelope<T>(data, ResponseType.Error, messageKey);
        }

        public static ResponseEnvelope<T> Error(string messageKey = MessageKeys.General.Error)
        {
            return new ResponseEnvelope<T>(default(T), ResponseType.Error, messageKey);
        }

        public static ResponseEnvelope<T> Success(T data, string messageKey = MessageKeys.General.Success)
        {
            return new ResponseEnvelope<T>(data, ResponseType.Success, messageKey);
        }
    }

    public class ResponseEnvelope : ResponseEnvelope<object>
    {
        private ResponseEnvelope(object data, ResponseType responseType, string messageKey, string errorCode = null) :
            base(data, responseType, messageKey, errorCode)
        { }

        public static ResponseEnvelope Success(string messageKey = MessageKeys.General.Success)
        {
            return new ResponseEnvelope(default(object), ResponseType.Success, messageKey);
        }

        public static ResponseEnvelope Success<T>(T data, string messageKey = MessageKeys.General.Success) where T : class
        {
            return new ResponseEnvelope(data, ResponseType.Success, messageKey);
        }

        public static ResponseEnvelope SuccessWithWarning(string messageKey = MessageKeys.General.SuccessWithWarning)
        {
            return new ResponseEnvelope(default(object), ResponseType.SuccessWithWarning, messageKey);
        }

        public static ResponseEnvelope SuccessWithWarning<T>(T data, string messageKey = MessageKeys.General.SuccessWithWarning) where T : class
        {
            return new ResponseEnvelope(data, ResponseType.SuccessWithWarning, messageKey);
        }

        public static ResponseEnvelope Error(string messageKey = MessageKeys.General.Error, string errorCode = null)
        {
            return new ResponseEnvelope(default(object), ResponseType.Error, messageKey, errorCode);
        }
        public static ResponseEnvelope SqlError(string messageKey = MessageKeys.General.SqlError, string errorCode = null)
        {
            return new ResponseEnvelope(default(object), ResponseType.Error, messageKey, errorCode);
        }

        public static ResponseEnvelope Fatal(string messageKey = MessageKeys.General.Error, string errorCode = null)
        {
            return new ResponseEnvelope(default(object), ResponseType.Fatal, messageKey, errorCode);
        }
    }
}
