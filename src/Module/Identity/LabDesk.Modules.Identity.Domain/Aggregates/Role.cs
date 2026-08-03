using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.Aggregates
{
    public enum Role
    {
        Requester = 1,          // Tạo và xem request của chính mình
        Agent = 2,              // Xử lý request thuộc Team mình
        Approver = 3,           // Phê duyệt các bước workflow
        ServiceAdmin = 4,       // Quản lý catalog service & request types
        OrganizationAdmin = 5,  // Quản trị viên toàn bộ tổ chức
        Auditor = 6             // Xem log compliance và audit
    }
}
