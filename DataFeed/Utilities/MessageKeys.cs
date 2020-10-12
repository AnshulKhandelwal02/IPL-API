namespace DataFeed.Utilities
{
    public static class MessageKeys
    {
        public static class General
        {
            public const string Success = "msg_success"; // "Operation was successful";
            public const string Error = "msg_error"; // "An unspecified error has occurred, please try again";\
            public const string SqlError = "msg_sql_error";
            public const string ErrorNoDataFound = "msg_nodatafound"; // "An unspecified error has occurred, please try again";
            public const string SuccessWithWarning = "msg_success_withwarning"; // "Operation was successful but with warnings";
        }

        public static class ReportRequest
        {
            public const string TrackerCannotBeDeleted = "tracker_cannot_bedeleted";
            public const string TrackerDeleted = "tracker_deleted";
        }
    }
}
