using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.DataAccessLayer;
using StudentApp.DTOs.CourseDTOs;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var coursesModel = await _dbContext.Courses.ToListAsync();

            var courseDto = new List<CourseDTO>();
            foreach (var courseModel in coursesModel)
            {
                courseDto.Add(new CourseDTO
                {
                    Id = courseModel.Id,
                    CourseTitle = courseModel.CourseTitle,
                    Credit = courseModel.Credit,
                });
            }

            return Ok(courseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var courseModel = await _dbContext.Courses.FindAsync(id);
            if(courseModel == null)
            {
                return BadRequest("Course ID is not found!");
            }

            var courseDto = new CourseDTO
            {
                Id = courseModel.Id,
                CourseTitle = courseModel.CourseTitle,
                Credit = courseModel.Credit,
            };

            return Ok(courseDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseRequestDto addCourseRequestDto)
        {
            if(addCourseRequestDto == null)
            {
                return BadRequest("Add all required field!");
            }

            var courseModel = new Courses
            {
                CourseTitle = addCourseRequestDto.CourseTitle,
                Fee = addCourseRequestDto.Fee,
                Credit = addCourseRequestDto.Credit,
            };

            await _dbContext.Courses.AddAsync(courseModel);
            await _dbContext.SaveChangesAsync();

            var courseDto = new CourseDTO
            {
                Id = courseModel.Id,
                CourseTitle = courseModel.CourseTitle,
                Fee = courseModel.Credit,
                Credit = courseModel.Credit,
            };

            return CreatedAtAction(nameof(GetCourseById), new {id = courseDto.Id}, courseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequestDto updatedCourseRequestDto)
        {
            if (updatedCourseRequestDto == null)
            {
                return BadRequest("Add all required field!");
            }
            var courseModel = await _dbContext.Courses.FindAsync(id);
            if (courseModel == null)
            {
                return BadRequest("Course ID is not found!");
            }

            courseModel.CourseTitle = updatedCourseRequestDto.CourseTitle;
            courseModel.Fee = updatedCourseRequestDto.Fee;
            courseModel.Credit = updatedCourseRequestDto.Credit;

            _dbContext.Courses.Update(courseModel);
            await _dbContext.SaveChangesAsync();

            var courseDto = new CourseDTO
            {
                Id = courseModel.Id,
                CourseTitle = courseModel.CourseTitle,
                Fee = courseModel.Credit,
                Credit = courseModel.Credit,
            };

            return CreatedAtAction(nameof(GetCourseById), new {id = courseDto.Id}, courseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            if(course == null)
            {
                return BadRequest("Course ID is not found!");
            }
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
            return Ok($"Course deleted successfully!");
        }
    }
}
