using Application.Projects.DTOs;
using FluentValidation;

namespace Application.Projects.Validators;

 public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}