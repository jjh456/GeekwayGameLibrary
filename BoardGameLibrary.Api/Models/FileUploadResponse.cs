using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class FileUploadResponse
    {
        public int NumberOfLinesProcessed { get; set; }
        public int NumberOfSuccesses { get; set; }
        public int NumberOfFailures { get; set; }
        public IList<string> Errors { get; set; }

        public FileUploadResponse()
        {
            NumberOfLinesProcessed = 0;
            NumberOfSuccesses = 0;
            NumberOfFailures = 0;
            Errors = new List<string>();
        }

        public void Success()
        {
            NumberOfSuccesses++;
            NumberOfLinesProcessed++;
        }

        public void Failure(string error)
        {
            NumberOfFailures++;
            NumberOfLinesProcessed++;
            Errors.Add(error);
        }
    }
}