using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Api.Services
{
    public interface IFileUploadService
    {
        FileUploadResponse UploadCopiesFile(int collectionId, HttpPostedFile file);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICopiesRepository copiesRepository;

        public FileUploadService(ApplicationDbContext db, ICopiesRepository copiesRepository)
        {
            _db = db;
            this.copiesRepository = copiesRepository;
        }

        public bool ValidateCopyRow(CopyUploadRow row, int rowNum, FileUploadResponse response)
        {
            var valid = true;
            if (row.OwnerName == null)
            {
                row.OwnerName = "";
            }
            if (string.IsNullOrWhiteSpace(row.GameTitle))
            {
                response.Failure($"Row {rowNum}: No game title was found");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(row.LibraryID))
            {
                response.Failure($"Row {rowNum}: No id was found for the copy");
                valid = false;
            }
            if (_db.Copies.Any(c => c.LibraryID == row.LibraryID))
            {
                response.Failure($"Row {rowNum}: A copy exists with the ID {row.LibraryID} already.");
                valid = false;
            }

            return valid;
        }

        public void ProcessCopyRow(CopyUploadRow row, int rowNumber, FileUploadResponse response, int collectionId)
        {
            if (!ValidateCopyRow(row, rowNumber, response))
                return;

            try
            {
                copiesRepository.AddCopy(row.LibraryID, collectionId, row.GameTitle, row.OwnerName);
            }
            catch (Exception e)
            {
                response.Failure($"Row {rowNumber}: {e.Message}");
            }
        }

        public FileUploadResponse UploadCopiesFile(int collectionId, HttpPostedFile file)
        {
            var response = new FileUploadResponse();
            using (var reader = new StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader, new Configuration { HasHeaderRecord = false }))
            {
                var rows = csv.GetRecords<CopyUploadRow>();
                var rowIdx = 0;
                foreach(var row in rows)
                {
                    ProcessCopyRow(row, rowIdx+1, response, collectionId);
                    rowIdx++;
                }

                return response;
            }
        }
    }
}