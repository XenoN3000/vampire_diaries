using Application.Tasks.DTOs;
using FluentValidation;

namespace Application.Tasks.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}