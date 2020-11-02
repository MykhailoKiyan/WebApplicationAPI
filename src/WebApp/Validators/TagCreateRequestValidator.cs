using FluentValidation;

using WebApplicationAPI.Contracts.V1.Requests;

namespace WebApplicationAPI.Validators {
    public class TagCreateRequestValidator : AbstractValidator<TagCreateRequest> {
        public TagCreateRequestValidator() {
            this.RuleFor(x => x.TagName)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}
