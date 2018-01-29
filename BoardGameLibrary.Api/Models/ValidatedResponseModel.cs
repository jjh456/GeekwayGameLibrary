using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class ValidatedResponseModel
    {
        public IList<string> errors { get; set; }
        public object result { get; set; }

        public ValidatedResponseModel(object result, List<string> errors)
        {
            this.errors = errors;
            this.result = result;
        }
    }
}