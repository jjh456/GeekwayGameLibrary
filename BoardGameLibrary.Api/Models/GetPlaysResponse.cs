using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class GetPlaysResponse
    {
        public IList<PlayResponseModel> Plays { get; set; }
    }
}