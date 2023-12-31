﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface ICoursesService :
    ICrudService<Course, int, CoursePreviewDto, CourseViewDto, CourseCreateDto, CourseCreateDto>
{
}
