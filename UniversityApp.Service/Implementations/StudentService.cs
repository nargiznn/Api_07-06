using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using University.Service.Interfaces;
using UniversityApi.Data.Entities;
using UniversityApi.Models;
using UniversityApp.Service.Dtos.StudentDtos;
using UniversityApp.Service.Exceptions;

namespace UniversityApp.Service.Implementations
{
	public class StudentService: IStudentService
    {
        private readonly UniversityDbContext _context;
        public StudentService(UniversityDbContext context)
		{
			_context = context;
		}
		public int Create(StudentCreateDto createDto)
		{
            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == createDto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new RestException(StatusCodes.Status404NotFound, "GroupId", "Group not found by given GroupId");

            if (group.Limit <= group.Students.Count)
                throw new RestException(StatusCodes.Status400BadRequest, "Group is full");

            if (_context.Students.Any(x => x.Email.ToUpper() == createDto.Email.ToUpper() && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "Email", "Student already exists by given Email");
            Student entity = new Student
            {
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDate = createDto.BirthDate,
                GroupId = createDto.GroupId,
            };

            _context.Students.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }
    }
}

