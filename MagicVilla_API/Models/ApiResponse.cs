using System.Net;

namespace MagicVilla_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessed { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
