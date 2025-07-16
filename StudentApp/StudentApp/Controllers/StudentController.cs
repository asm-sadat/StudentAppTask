using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.DataAccessLayer;
using StudentApp.DTOs.StudentDTOs;
using StudentApp.Models;
using System.Threading.Tasks;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public StudentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var studentsModel = await _dbContext.Students.ToListAsync();

            var studentDto = new List<StudentDTO>();
            foreach (var studentModel in studentsModel)
            {
                studentDto.Add(new StudentDTO
                {
                    Id = studentModel.Id,
                    Name = studentModel.Name,
                    Age = studentModel.Age,
                    Phone = studentModel.Phone,
                    Email = studentModel.Email,
                });
            }

            return Ok(studentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsById(int id)
        {
            var studentModel = await _dbContext.Students.FindAsync(id);
            if(studentModel == null)
            {
                return NotFound("Student ID is not found!");
            }

            var studentDto = new StudentDTO
            {
                Id = studentModel.Id,
                Name = studentModel.Name,
                Age = studentModel.Age,
                Phone = studentModel.Phone,
                Email = studentModel.Email,
            };

            return Ok(studentDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentRequestDto addStudentRequestDto)
        {
            if (addStudentRequestDto == null)
            {
                return BadRequest("Add all required field!");
            }

            var studentModel = new Student
            {
                Name = addStudentRequestDto.Name,
                Age = addStudentRequestDto.Age,
                Phone = addStudentRequestDto.Phone,
                Email = addStudentRequestDto.Email,
            };

            await _dbContext.Students.AddAsync(studentModel);
            await _dbContext.SaveChangesAsync();

            var studentDto = new StudentDTO
            {
                Id = studentModel.Id,
                Name = studentModel.Name,
                Age = studentModel.Age,
                Phone = studentModel.Phone,
                Email = studentModel.Email,
            };

            return CreatedAtAction(nameof(GetStudentsById),new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequestDto updatedStudentRequestDto)
        {
            if (updatedStudentRequestDto == null)
            {
                return BadRequest("Add all required field!");
            }
            var studentModel = await _dbContext.Students.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound("Student ID is not found!");
            }

            studentModel.Name = updatedStudentRequestDto.Name;
            studentModel.Age = updatedStudentRequestDto.Age;
            studentModel.Phone = updatedStudentRequestDto.Phone;
            studentModel.Email = updatedStudentRequestDto.Email;

            _dbContext.Students.Update(studentModel);
            await _dbContext.SaveChangesAsync();

            var studentDto = new StudentDTO
            {
                Id = studentModel.Id,
                Name = studentModel.Name,
                Age = studentModel.Age,
                Phone = studentModel.Phone,
                Email = studentModel.Email,
            };

            return CreatedAtAction(nameof(GetStudentsById), new { id = studentDto.Id }, studentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student ID is not found!");
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return Ok($"Student deleted successfully!");
        }
    }
}
