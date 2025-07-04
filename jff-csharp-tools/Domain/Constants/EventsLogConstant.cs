namespace JffCsharpTools.Domain.Constants
{
    /// <summary>
    /// Constants defining event log types and their corresponding numeric identifiers.
    /// These constants are used for categorizing and identifying different types of 
    /// system events, exceptions, and user actions in logging systems.
    /// </summary>
    public static class EventsLogConstant
    {
        /// <summary>
        /// Generic system exception event identifier
        /// </summary>
        public static int Generic_Exception_System = 1;

        /// <summary>
        /// Unauthorized access system event identifier
        /// </summary>
        public static int Unauthorized_System = 2;

        /// <summary>
        /// SMTP/Email system exception event identifier
        /// </summary>
        public static int Smtp_Exception_System = 3;

        /// <summary>
        /// File not found system exception event identifier
        /// </summary>
        public static int File_NotFound_System = 4;

        /// <summary>
        /// Database system exception event identifier
        /// </summary>
        public static int DB_Exception_System = 5;

        /// <summary>
        /// Unmapped identity system exception event identifier
        /// </summary>
        public static int Unmapped_Identity_Exception_System = 6;

        /// <summary>
        /// User login event identifier
        /// </summary>
        public static int Login_User = 7;

        /// <summary>
        /// User registration event identifier
        /// </summary>
        public static int Register_User = 8;

        /// <summary>
        /// User forgotten password event identifier
        /// </summary>
        public static int Forgotten_Password_User = 9;

        /// <summary>
        /// Null reference system exception event identifier
        /// </summary>
        public static int NullReference_Exception_System = 10;

        /// <summary>
        /// Request return error system event identifier
        /// </summary>
        public static int RequestReturn_Error_System = 11;

        /// <summary>
        /// Identity error system event identifier
        /// </summary>
        public static int Identity_Error_System = 12;
    }
}