using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Meeting_Organizer.Models
{
    public class MeetingRecords
    {
        public string Id { get; set; } // Otomatik oluşturduk
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string> Participants { get; set; } // birden fazla katılımcıyı saklamak için list kullandım.
    }
}
