using Application.Tasks.DTOs;
using FluentValidation;

namespace Application.Tasks.Validators;

public class TaskValidator : AbstractValidator<TaskDto>
{
    public TaskValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}