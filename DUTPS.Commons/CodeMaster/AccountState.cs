namespace DUTPS.Commons.CodeMaster
{
    /// <summary>
    /// S002 Account Status Class.<br/>
    /// !!! DO NOT MODIFY !!!
    /// <list type="bullet">
    /// <item>
    /// <term>NORMAL</term>
    /// <description>10 : Account is using</description>
    /// </item>
    /// <item>
    /// <term>LOCK</term>
    /// <description>20 : Account is locking</description>
    /// </item>
    /// </list>
    /// </summary>
    public static class AccountState
    {
        /// <summary>
        /// 10 : Account is using
        /// </summary>
        public static class Normal
        {
            public static readonly int CODE = 10;
            public static readonly string NAME = "Using";
        }

        /// <summary>
        /// 20 : Account is locking<br/>
        /// </summary>
        public static class Lock
        {
            public static readonly int CODE = 20;
            public static readonly string NAME = "Locking";
        }
    }
}