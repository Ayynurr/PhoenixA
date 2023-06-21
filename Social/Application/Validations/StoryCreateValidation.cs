using Application.DTOs;
using FluentValidation;

namespace Application.Validations;

public class StoryCreateValidation : AbstractValidator<CreateStoryDto>
{
    public StoryCreateValidation()
    {
        RuleFor(s => s.Content).NotEmpty().NotNull();
    }
}
