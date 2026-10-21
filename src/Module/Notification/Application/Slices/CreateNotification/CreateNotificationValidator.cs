using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.CreateNotification
{
    public sealed class CreateNotificationValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.RecipientUserId).NotEmpty(); 
            RuleFor(x => x.Type).NotEmpty().MaximumLength(100); 
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(4000); 
            RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(200); 
            RuleFor(x => x.Channels).NotEmpty(); RuleForEach(x => x.Channels).IsInEnum(); 
            RuleFor(x => x.PayloadJson).Must(IsObjectOrNull).WithMessage("Payload must be a JSON object."); 
        }
        private static bool IsObjectOrNull(string? x) 
        { 
            if (x is null) return true; 
            try 
            { 
                using var d = System.Text.Json.JsonDocument.Parse(x); 
                return d.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object; 
            } 
            catch 
            { 
                return false; 
            }
        }
    }
}
