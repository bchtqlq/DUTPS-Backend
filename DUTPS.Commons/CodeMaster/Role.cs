namespace DUTPS.Commons.CodeMaster
{
    /// <summary>
    /// S002 Role Class.<br/>
    /// !!! DO NOT MODIFY !!!
    /// <list type="bullet">
    /// <item>
    /// <term>ADMIN</term>
    /// <description>10 : Admin</description>
    /// </item>
    /// <item>
    /// <term>STAFF</term>
    /// <description>20 : Staff</description>
    /// </item>
    /// <item>
    /// <term>CUSTOMER</term>
    /// <description>30 : Customer</description>
    /// </item>
    /// </list>
    /// </summary>
    public static class Role
    {
        /// <summary>
        /// 10 : Admin
        /// </summary>
        public static class Admin
        {
            public static readonly int CODE = 10;
            public static readonly string NAME = "Admin";
        }

        /// <summary>
        /// 20 : Staff<br/>
        /// </summary>
        public static class Staff
        {
            public static readonly int CODE = 20;
            public static readonly string NAME = "Staff";
        }

        /// <summary>
        /// 30 : Customer <br/>
        /// </summary>
        public static class Customer
        {
            public static readonly int CODE = 30;
            public static readonly string NAME = "Customer";
        }
    }
}