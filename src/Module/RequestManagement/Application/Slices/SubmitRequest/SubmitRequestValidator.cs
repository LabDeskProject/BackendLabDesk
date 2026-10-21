using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.SubmitRequest
{
    public sealed class SubmitRequestValidator : AbstractValidator<SubmitRequestCommand>
    {
        public SubmitRequestValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty(); RuleFor(x => x.ServiceId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(300); RuleFor(x => x.Description).MaximumLength(4000);
            RuleFor(x => x.FormDataJson).NotEmpty().Must(IsJsonObject).WithMessage("FormData must be a valid JSON object.");
        }
        private static bool IsJsonObject(string json)
        {
            try { using var doc = System.Text.Json.JsonDocument.Parse(json); return doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object; } catch (System.Text.Json.JsonException) { return false; }
        }
    }
}
