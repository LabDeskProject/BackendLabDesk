using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.RecordAuditEntry
{
    public sealed class RecordAuditEntryValidator : AbstractValidator<RecordAuditEntryCommand>
    {
        public RecordAuditEntryValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.ActorUserId).Must(x => x is null || x != Guid.Empty); 
            RuleFor(x => x.Action).NotEmpty().MaximumLength(100); 
            RuleFor(x => x.EntityType).NotEmpty().MaximumLength(150);
            RuleFor(x => x.EntityId).NotEmpty().MaximumLength(100); 
            RuleFor(x => x.CorrelationId).MaximumLength(100); 
            RuleFor(x => x.DetailsJson).Must(IsObjectOrNull).WithMessage("Details must be a JSON object."); 
        }
        private static bool IsObjectOrNull(string? value) 
        { 
            if (string.IsNullOrWhiteSpace(value)) return true; 
            try 
            { 
                using var doc = System.Text.Json.JsonDocument.Parse(value); 
                return doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object; 
            } 
            catch 
            { 
                return false; 
            }
        }
    }
}
