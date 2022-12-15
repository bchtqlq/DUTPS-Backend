using DUTPS.Commons.Enums;

namespace DUTPS.Commons.Schemas
{
    public class ResponseInfo
    {
        /// <summary>
        /// Return Code
        /// <para>200: Success</para>
        /// <para>201: Error validate of input</para>
        /// <para>202: Have error in code</para>
        /// <para>403: Not allows</para>
        /// <para>500: Server error</para>
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// Id of message will be display
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// List of error
        /// </summary>
        public Dictionary<string, string> ListError { set; get; }

        /// <summary>
        /// Additional data
        /// </summary>
        public Dictionary<string, object> Data { set; get; }

        public ResponseInfo()
        {
            Code = CodeResponse.OK;
            Message = "";
            Data = new Dictionary<string, object>();
        }
    }
}