using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JffCsharpTools.Domain.Model
{
    public class DefaultResponseModel<T>
    {
        public string Error { get; set; }
        public string BaseException { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public List<string> MessageList { get; set; } = new List<string>();
        public T Result { get; set; }
        public bool Success
        {
            get
            {
                var checkStatus = StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent;
                var checkMessage = string.IsNullOrEmpty(Message) && MessageList?.Any() != true;
                var checkResult = checkStatus && checkMessage;
                return checkResult;
            }
        }
    }
}