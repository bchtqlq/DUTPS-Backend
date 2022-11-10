namespace DUTPS.Commons.CodeMaster
{
    /// <summary>
    /// G001 Gender Class.<br/>
    /// !!! DO NOT MODIFY !!!
    /// <list type="bullet">
    /// <item>
    /// <term>MALE</term>
    /// <description>1 : Male</description>
    /// </item>
    /// <item>
    /// <term>FEMALE</term>
    /// <description>2 : Female</description>
    /// </item>
    /// <item>
    /// <term>SECRET</term>
    /// <description>3 : Secret</description>
    /// </item>
    /// </list>
    /// </summary>
    public static class Gender
    {
        /// <summary>
        /// 1 : Male
        /// </summary>
        public static class Male
        {
            public static readonly int CODE = 1;
            public static readonly string NAME = "Male";
        }

        /// <summary>
        /// 2 : Female
        /// </summary>
        public static class Female
        {
            public static readonly int CODE = 2;
            public static readonly string NAME = "Female";
        }

        /// <summary>
        /// 3 : Secret
        /// </summary>
        public static class Secret
        {
            public static readonly int CODE = 3;
            public static readonly string NAME = "Secret";
        }
    }
}