using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class ValidatedResponseModel
    {
        public IList<string> Errors { get; set; }
        public object Result { get; set; }

        public ValidatedResponseModel(object result, List<string> errors)
        {
            Errors = errors;
            Result = result;
        }
    }
}