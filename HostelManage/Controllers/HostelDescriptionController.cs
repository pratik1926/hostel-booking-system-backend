//using HostelManage.Data;
//using HostelManage.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;

//namespace HostelManage.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class HostelDescriptionController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public HostelDescriptionController(AppDbContext context)
//        {
//            _context = context;
//        }

//        // POST: api/HostelDescription
//        [HttpPost]
//        public async Task<IActionResult> AddHostelDescription([FromBody] HostelDescription model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            // Optional: check if HostelID exists in Hostel table before inserting
//            var hostelExists = await _context.Hostel.FindAsync(model.HostelID);
//            if (hostelExists == null)
//            {
//                return NotFound($"Hostel with ID {model.HostelID} does not exist.");
//            }

//            _context.HostelDescription.Add(model);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetHostelDescriptionByHostelId), new { hostelId = model.HostelID }, model);
//        }

//        // GET method to retrieve a description by HostelID
//        [HttpGet("by-hostel/{hostelId}")]
//        public async Task<IActionResult> GetHostelDescriptionByHostelId(int hostelId)
//        {
//            var description = await _context.HostelDescription
//                .FirstOrDefaultAsync(h => h.HostelID == hostelId);

//            if (description == null)
//                return NotFound($"No description found for HostelID {hostelId}.");

//            return Ok(description);
//        }

//        // PUT: api/HostelDescription/{hostelId}
//        [HttpPut("{hostelId}")]
//        public async Task<IActionResult> UpdateHostelDescription(int hostelId, [FromBody] HostelDescription updatedDescription)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (hostelId != updatedDescription.HostelID)
//            {
//                return BadRequest("HostelID in URL and body do not match.");
//            }

//            var existingDescription = await _context.HostelDescription
//                .FirstOrDefaultAsync(h => h.HostelID == hostelId);

//            if (existingDescription == null)
//            {
//                return NotFound($"No description found for HostelID {hostelId}.");
//            }

//            // Update all fields
//            existingDescription.Location = updatedDescription.Location;
//            existingDescription.Description = updatedDescription.Description;
//            existingDescription.RoomType1Count = updatedDescription.RoomType1Count;
//            existingDescription.RoomType2Count = updatedDescription.RoomType2Count;
//            existingDescription.RoomType3Count = updatedDescription.RoomType3Count;
//            existingDescription.RoomType4Count = updatedDescription.RoomType4Count;
//            existingDescription.CreatedDate = updatedDescription.CreatedDate;

//            _context.HostelDescription.Update(existingDescription);
//            await _context.SaveChangesAsync();

//            return Ok(new { message = "Hostel description updated successfully", updatedDescription = existingDescription });
//        }


//    }
//}


using Microsoft.AspNetCore.Mvc;
using HostelManage.Application.Interfaces;
using HostelManage.Application.DTOs.Hostel;

namespace HostelManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostelDescriptionController : ControllerBase
    {
        private readonly IHostelDescriptionService _service;

        public HostelDescriptionController(IHostelDescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddHostelDescription([FromBody] HostelDescriptionCreateDTO dto)
        {
            var result = await _service.AddDescription(dto);
            return Ok(result);
        }

        [HttpGet("by-hostel/{hostelId}")]
        public async Task<IActionResult> GetHostelDescriptionByHostelId(int hostelId)
        {
            var result = await _service.GetByHostelId(hostelId);
            return Ok(result);
        }

        [HttpPut("{hostelId}")]
        public async Task<IActionResult> UpdateHostelDescription(int hostelId, [FromBody] HostelDescriptionUpdateDTO dto)
        {
            var result = await _service.UpdateDescription(hostelId, dto);
            return Ok(new { message = result });
        }
    }
}