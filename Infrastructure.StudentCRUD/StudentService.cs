using Application.StudentCRUD;
using Domain.StudentCRUD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StudentCRUD
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDBContext _dbContext;

        public StudentService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> AddStudent(Student student)
        {
            var result = await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteStudent(string id)
        {
            var student = await _dbContext.Students.FindAsync(Guid.Parse(id));
            if (student != null)
            {
                _dbContext.Students.Remove(student);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsbyId(string id)
        {
            var result = await _dbContext.Students.Where(s => s.Id.ToString() == id).ToListAsync();
            return result;
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            var existingStudent = await _dbContext.Students.FindAsync(student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Gender = student.Gender;
                existingStudent.Phone = student.Phone;
                await _dbContext.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
    }
}