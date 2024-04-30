using Application.StudentCRUD;
using Domain.StudentCRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.StudentCRUD.Controllers
{
    public class StudentController : Controller
    {
        public readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost, Route("AddStudent")]
        public async Task<IActionResult> AddStudent(Student student)
        {
            var addStudent = await _studentService.AddStudent(student);
            return Ok(addStudent);
        }


        [Authorize(Roles ="User")]
        [HttpGet, Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            return Ok(await _studentService.GetAllStudents());
        }

        [HttpGet, Route("GetStudentByID")]
        public async Task<IActionResult> GetStudentByID(string id)
        {
            var result = await _studentService.GetStudentsbyId(id);
            return Ok(result);
        }

        [HttpDelete, Route("DeleteStudentByID")]
        public async Task<IActionResult> DeleteStudentByID(string id)
        {
            await _studentService.DeleteStudent(id);
            return Ok();
        }

        [HttpPut, Route("UpdateStudentByID")]
        public async Task<IActionResult> UpdateStudentByID(Student student)
        {
            var updatedStudent = await _studentService.UpdateStudent(student);
            if (updatedStudent != null)
            {
                return Ok(updatedStudent);
            }
            else
            {
                return NotFound("Student not found.");
            }
        }

    }
}