using BoardGameLibrary.Data.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Utility
{
    public class FileUploader
    {
        private ApplicationDbContext _db;

        public FileUploader(ApplicationDbContext context)
        {
            _db = context;
        }

        public IList<string> UploadCopies(HttpPostedFileBase file)
        {
            IList<string> errors = new List<string>();
            var rows = GetCopyImportRows(file, ref errors);

            // Extract games and copies from the import rows into the database.
            var newGames = new List<Game>();
            var newCopies = new List<Copy>();
            
            foreach (var row in rows)
            {
                bool newGameInserted = false;
                Game game;
                // See if the game is already in the list of new games to be added or in the database.
                game = newGames.SingleOrDefault(g => g.Title.Trim().ToLower() == row.GameTitle.Trim().ToLower());
                if(game == null)
                    game = _db.Games.SingleOrDefault(g => g.Title.Trim().ToLower() == row.GameTitle.Trim().ToLower());

                // If the game wasn't found in either the list of new games or the database, add it to the list of new games to be added.
                if (game == null)
                {
                    game = new Game
                    {
                        Title = row.GameTitle
                    };
                    newGames.Add(game);
                    newGameInserted = true;
                }

                // Check to see if a copy exists already with the library ID provided.  
                // If it does, remove any games that would be inserted into the new games list.  Otherwise, add the copy to the new copies list.
                Copy copy;
                copy = _db.Copies.SingleOrDefault(c => c.LibraryID == row.LibraryID);
                if (copy != null)
                {
                    if(newGameInserted)
                        newGames.Remove(game);

                    errors.Add(string.Format("Copy with library ID {0} already exists.  Please check row {1} of the file.", row.LibraryID, row.FileRowNumber));
                    continue;
                }
                copy = new Copy
                {
                    Game = game,
                    LibraryID = row.LibraryID,
                    OwnerName = row.OwnerName
                };
                newCopies.Add(copy);
            }

            // Try to save the games and copies to the database.
            try
            {
                _db.Games.AddRange(newGames);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                errors.Add("An error occurred while saving the new games (step 1 of saving) to the database.");
            }

            try
            {
                _db.Copies.AddRange(newCopies);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                errors.Add("An error occurred while saving the new copies (step 2 of saving) to the database.");
            }

            return errors;
        }

        private IList<CopyImportRow> GetCopyImportRows(HttpPostedFileBase file, ref IList<string> errors)
        {
            ICsvParser csvParser = new CsvParser(new StreamReader(file.InputStream));
            CsvReader csvReader = new CsvReader(csvParser);
            string[] headers = { };
            var rows = new List<CopyImportRow>();
            int rowNum = 1;
            while (csvReader.Read())
            {
                // Gets Headers if they exist
                if (csvReader.Configuration.HasHeaderRecord && !headers.Any())
                    headers = csvReader.FieldHeaders;

                var row = new CopyImportRow { FileRowNumber = rowNum };

                for (int j = 0; j < headers.Count(); j++)
                {
                    try
                    {
                        switch (j)
                        {
                            case 0:
                                row.GameTitle = csvReader.GetField(j);
                                break;
                            case 1:
                                row.LibraryID = Convert.ToInt32(csvReader.GetField(j));
                                break;
                            case 2:
                                row.OwnerName = csvReader.GetField(j);
                                break;
                            default:
                                continue;
                        }
                    }
                    catch (Exception)
                    {
                        errors.Add(string.Format("Processing of data in file row # {0} failed.  Make sure the library ID contains only numbers and that everything is filled in.", j+1));
                    }
                }

                if (!string.IsNullOrWhiteSpace(row.GameTitle) && !string.IsNullOrWhiteSpace(row.OwnerName) && row.LibraryID != null && row.LibraryID != 0)
                    rows.Add(row);
                else
                    errors.Add(string.Format("Processing of data in file row # {0} failed.  Make sure the library ID contains only numbers and that everything is filled in.", row.FileRowNumber));

                rowNum++;
            }

            return rows;
        }

        public IList<string> UploadAttendees(HttpPostedFileBase file)
        {
            IList<string> errors = new List<string>();
            var rows = GetAttendeeImportRows(file, ref errors);

            // Extract new attendees from the import rows.
            var newAttendees = new List<Attendee>();
            foreach (var row in rows)
            {
                Attendee attendee;
                attendee = _db.Attendees.SingleOrDefault(a => a.BadgeID.Trim().ToLower() == row.BadgeID.Trim().ToLower());
                if (attendee != null)
                {
                    // Attendee exists in the system already.
                    errors.Add(string.Format("An attendee with badge ID {0} exists already.  Check row {1} of the file.", row.BadgeID, row.FileRowNumber));
                    continue;
                }

                attendee = new Attendee
                {
                    BadgeID = row.BadgeID,
                    Name = row.Name
                };

                newAttendees.Add(attendee);
            }

            // Save any new attendees to the database.
            if (newAttendees.Any())
            {
                _db.Attendees.AddRange(newAttendees);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    errors.Add("An error occurred while saving the attendees to the database.");
                }
            }

            return errors;
        }

        private IList<AttendeeImportRow> GetAttendeeImportRows(HttpPostedFileBase file, ref IList<string> errors)
        {
            ICsvParser csvParser = new CsvParser(new StreamReader(file.InputStream));
            CsvReader csvReader = new CsvReader(csvParser);
            string[] headers = { };
            var rows = new List<AttendeeImportRow>();
            int rowNum = 1;
            while (csvReader.Read())
            {
                // Gets Headers if they exist
                if (csvReader.Configuration.HasHeaderRecord && !headers.Any())
                    headers = csvReader.FieldHeaders;

                var row = new AttendeeImportRow { FileRowNumber = rowNum };

                for (int j = 0; j < headers.Count(); j++)
                {
                    try
                    {
                        switch (j)
                        {
                            case 0:
                                row.Name = csvReader.GetField(j);
                                break;
                            case 1:
                                row.BadgeID = csvReader.GetField(j);
                                break;
                            default:
                                continue;
                        }

                        
                    }
                    catch (Exception)
                    {
                        errors.Add(string.Format("Processing of data in file row # {0} failed.  Make sure both the name and badge ID are filled in.", j + 1));
                    }
                }
                if (!string.IsNullOrWhiteSpace(row.Name) && !string.IsNullOrWhiteSpace(row.BadgeID))
                    rows.Add(row);
                else
                    errors.Add(string.Format("Processing of data in file row # {0} failed.  Make sure both the name and badge ID are filled in.", row.FileRowNumber));

                rowNum++;
            }

            return rows;
        }
    }
}