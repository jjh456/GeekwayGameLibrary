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
    public interface IAttendeesFileUploadService
    {
        FileUploadResponse UploadAttendeesFile(int collectionId, HttpPostedFile file);
    }

    public class AttendeesFileUploadService : IAttendeesFileUploadService
    {
        private readonly ApplicationDbContext _db;

        public AttendeesFileUploadService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ValidateAttendeeRow(AttendeeUploadRow row, int rowNum, FileUploadResponse response)
        {
            var valid = true;
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                response.Failure($"Row {rowNum}: No name was found");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(row.BadgeID))
            {
                response.Failure($"Row {rowNum}: No badge id was found");
                valid = false;
            }
            if (_db.Attendees.Any(a => a.BadgeID == row.BadgeID))
            {
                response.Failure($"Row {rowNum}: An attendee exists with the badge ID {row.BadgeID} already.");
                valid = false;
            }

            return valid;
        }

        public void ProcessAttendeeRow(AttendeeUploadRow row, int rowNumber, FileUploadResponse response, int collectionId)
        {
            if (!ValidateAttendeeRow(row, rowNumber, response))
                return;

            var newAttendee = new Attendee { BadgeID = row.BadgeID, Name = row.Name };
            _db.Attendees.Add(newAttendee);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                response.Failure($"Row {rowNumber}: {e.Message}");
            }
        }

        public FileUploadResponse UploadAttendeesFile(int collectionId, HttpPostedFile file)
        {
            var response = new FileUploadResponse();
            using (var reader = new StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader, new Configuration { HasHeaderRecord = false }))
            {
                var rows = csv.GetRecords<AttendeeUploadRow>();
                var rowIdx = 0;
                foreach(var row in rows)
                {
                    ProcessAttendeeRow(row, rowIdx+1, response, collectionId);
                    rowIdx++;
                }

                return response;
            }
        }
    }
}