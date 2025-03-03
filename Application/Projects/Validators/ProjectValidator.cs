using Application.Projects.DTOs;
using FluentValidation;

namespace Application.Projects.Validators;

public class ProjectValidator : AbstractValidator<ProjectDto>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}