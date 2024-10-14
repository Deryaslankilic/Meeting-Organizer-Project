using Meeting_Organizer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Meeting_Organizer.Business
{
    public class MeetingService
    {
        
            private readonly IMongoCollection<MeetingRecords> _meetings; 

            public MeetingService(IOptions<MongoDbSettings> mongoDbSettings)
            {
                var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
                _meetings = mongoDatabase.GetCollection<MeetingRecords>(mongoDbSettings.Value.CollectionName);
            //_meetings değişkeni, MeetingRecords koleksiyonuna bağlandı
            // bu koleksiyona veri ekleyebilir, güncelleyebilir, veya silebiliriz.
        }

           public async Task<List<MeetingRecords>> GetAsync() =>  //Getasync ile tüm meetingrecords kayıtlrını liste olarak döndrebilriz.
                await _meetings.Find(_ => true).ToListAsync();

            public async Task<MeetingRecords> GetAsync(string id) => // getasync burada belirli bir ıd ye sahip olanları liste olarak döndürür.
                await _meetings.Find(x => x.Id == id).FirstOrDefaultAsync(); //Id değeri verilen id'ye eşit olan ilk kaydı bulur.

        public async Task<string> CreateAsync(MeetingRequestModel meeting) {
            var model = new MeetingRecords()
            {
                Date = meeting.Date,
                EndTime = meeting.EndTime,
                Participants = meeting.Participants,
                StartTime = meeting.StartTime,
                Topic = meeting.Topic,
                Id = ObjectId.GenerateNewId().ToString(), 
            };
                        await _meetings.InsertOneAsync(model); 
            return model.Id;
        } 
            


        public async Task UpdateAsync(string id, MeetingRecords updatedMeeting) => // güncelleme
                await _meetings.ReplaceOneAsync(x => x.Id == id, updatedMeeting);

            public async Task RemoveAsync(string id) => //removeasync belirli bir idye sahip kaydı siler.
                await _meetings.DeleteOneAsync(x => x.Id == id);
        
    }
}
