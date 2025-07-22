using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.DataAccessLayer;
using StudentApp.DTOs.CourseDTOs;
using StudentApp.DTOs.StudentDTOs;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkCourseController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public BulkCourseController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [Authorize]
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBulkCourses([FromBody] List<AddCourseRequestDto> courseDtos)
        {
            if (courseDtos == null || courseDtos.Count == 0)
                return BadRequest("Add all required fields!");

            var courses = courseDtos.Select(dto => new Courses
            {
                CourseTitle = dto.CourseTitle,
                Fee = dto.Fee,
                Credit = dto.Credit
            }).ToList();

            await _appDbContext.Courses.AddRangeAsync(courses);
            await _appDbContext.SaveChangesAsync();

            return Ok("Courses added successfully!");
        }

        [Authorize]
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBulkCourses([FromBody] List<UpdateBulkCourseDto> updateDtos)
        {
            if (updateDtos == null || updateDtos.Count == 0)
                return BadRequest("Add all required fields!");

            var dtoDict = updateDtos.ToDictionary(x => x.Id);

            var courses = await _appDbContext.Courses
                .Where(x => dtoDict.Keys.Contains(x.Id))
                .ToListAsync();

            courses.ForEach(course =>
            {
                var dto = dtoDict[course.Id];
                course.CourseTitle = dto.CourseTitle;
                course.Fee = dto.Fee;
                course.Credit = dto.Credit;
            });

            await _appDbContext.SaveChangesAsync();

            return Ok("Courses updated successfully!");
        }

        [Authorize]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBulkCourses([FromBody] CourseIdRequestDto idRequestDto)
        {
            if (idRequestDto?.Ids == null || !idRequestDto.Ids.Any())
                return BadRequest("No course IDs provided!");

            var courses = await _appDbContext.Courses
                .Where(c => idRequestDto.Ids.Contains(c.Id))
                .ToListAsync();

            if (!courses.Any())
                return NotFound("No matching courses found to delete.");

            _appDbContext.Courses.RemoveRange(courses);
            await _appDbContext.SaveChangesAsync();

            return Ok("Courses deleted successfully!");
        }
    }
}
