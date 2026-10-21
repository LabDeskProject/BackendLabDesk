using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.ListMyNotifications
{
    public sealed class ListMyNotificationsValidator : AbstractValidator<ListMyNotificationsQuery> 
    { 
        public ListMyNotificationsValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.Page).GreaterThan(0); 
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200); 
        } 
    }

}
