using System.Web;

namespace BoardGameLibrary.Api.Models
{
    public class FileUploadRequestModel
    {
        public HttpPostedFileBase File { get; set; }
    }
}