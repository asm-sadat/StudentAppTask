using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.DataAccessLayer;
using StudentApp.DTOs.StudentDTOs;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkStudentController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public BulkStudentController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [Authorize]
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBulkStudent([FromBody] List<AddStudentRequestDto> studentDtos)
        {
            if (studentDtos == null || studentDtos.Count == 0)
                return BadRequest("Add all required field!");

            var students = studentDtos.Select(dto => new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                Email = dto.Email,
                Phone = dto.Phone,
            }).ToList();

            await _appDbContext.AddRangeAsync(students);
            await _appDbContext.SaveChangesAsync();

            return Ok("Students added successfully!");
        }

        [Authorize]
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBulkStudent([FromBody] List<UpdateBulkStudentDto> updateStudentDto)
        {
            if (updateStudentDto == null || updateStudentDto.Count == 0)
                return BadRequest("Add all required field!");

            var dtoDict = updateStudentDto.ToDictionary(x => x.Id);

            var students = await _appDbContext.Students
                .Where(x => dtoDict.Keys.Contains(x.Id))
                .ToListAsync();

            students.ForEach(student =>
            {
                var dto = dtoDict[student.Id];
                student.Name = dto.Name;
                student.Age = dto.Age;
                student.Email = dto.Email;
                student.Phone = dto.Phone;
            });

            await _appDbContext.SaveChangesAsync();

            return Ok("Students updated successfully!");
        }

        [Authorize]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBulkStudent([FromBody] IdRequestDto idRequestDto)
        {
            if(idRequestDto?.Ids == null || !idRequestDto.Ids.Any())
            {
                return BadRequest("No student found!");
            }

            var students = await _appDbContext.Students
        .Where(s => idRequestDto.Ids.Contains(s.Id))
        .ToListAsync();

            if (!students.Any())
                return NotFound("No matching students found to delete.");

            _appDbContext.Students.RemoveRange(students);
            await _appDbContext.SaveChangesAsync();

            return Ok("Students deleted successfully!");
        }
    }
}
