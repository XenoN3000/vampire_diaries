using Application.Tasks.DTOs;
using FluentValidation;

namespace Application.Diaries.Vlidators;

public class TaskValidator : AbstractValidator<CreateTaskDto>
{
    public TaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}