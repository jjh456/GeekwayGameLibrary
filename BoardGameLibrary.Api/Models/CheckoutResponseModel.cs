namespace BoardGameLibrary.Api.Models
{
    public class CheckoutResponseModel
    {
        public int ID { get; set; }
        public CopyResponseModel Copy { get; set; }
        public AttendeeApiModel Attendee { get; set; }
    }
}