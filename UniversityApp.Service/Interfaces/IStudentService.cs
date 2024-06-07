using System;
using UniversityApp.Service.Dtos.StudentDtos;

namespace University.Service.Interfaces
{
    public interface IStudentService
    {
        int Create(StudentCreateDto createDto);
    }
}

