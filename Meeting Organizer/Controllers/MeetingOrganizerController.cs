using Meeting_Organizer.Business;
using Meeting_Organizer.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Meeting_Organizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingService _meetingService;

        public MeetingsController(MeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet] //t�m toplant�lar� getirir.
        public async Task<List<MeetingRecords>> Get() =>
            await _meetingService.GetAsync();

        [HttpGet("{id}")] //belirli id ile getirir.
        public async Task<ActionResult<MeetingRecords>> Get(string id)
        {
            var meeting = await _meetingService.GetAsync(id);

            if (meeting is null) //toplant� yoksa eror d�n
            {
                return NotFound();
            }

            return meeting; //toplant� bilgileirni d�n
        }

        [HttpPost] // yeni toplant� kayd� olu�tur
        public async Task<IActionResult> Post(MeetingRequestModel newMeeting)
        {
            var result = await _meetingService.CreateAsync(newMeeting);
            return CreatedAtAction(nameof(Get), new { id = result }, newMeeting);
        }

        [HttpPut("{id}")] //g�ncelleme
        public async Task<IActionResult> Update(string id, MeetingRequestModel updatedMeeting)
        {
            var meeting = await _meetingService.GetAsync(id);

            if (meeting is null)
            {
                return NotFound();
            }

            var model = new MeetingRecords()
            {
                Date = updatedMeeting.Date,
                EndTime = updatedMeeting.EndTime,
                Participants = updatedMeeting.Participants,
                StartTime = updatedMeeting.StartTime,
                Topic = updatedMeeting.Topic,
                Id = id, 
            };

            await _meetingService.UpdateAsync(id, model); //toplant�y� g�ncelledi.
             
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var meeting = await _meetingService.GetAsync(id);

            if (meeting is null)
            {
                return NotFound();
            }

            await _meetingService.RemoveAsync(id);

            return NoContent();
        }
    }
}
