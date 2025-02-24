using Application.Diaries.DTOs;
using FluentValidation;

namespace Application.Diaries.Vlidators;

public class DiaryValidator : AbstractValidator<CreateDiaryDto>
{
    public DiaryValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}