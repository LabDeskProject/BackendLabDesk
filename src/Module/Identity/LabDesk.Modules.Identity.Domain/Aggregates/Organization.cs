using LabDesk.SeedWork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.Aggregates
{
    public class Organization : AggregateRoot<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string? Description { get; private set; }

        private Organization() { } //EF Core

        public Organization(Guid id, string name, string slug, string? description = null) : base(id) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name không được để trống.", nameof(name));

            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug không được để trống.", nameof(slug));

            Name = name.Trim();
            Slug = slug.ToLowerInvariant().Trim();
            Description = description?.Trim();
            CreatedAt = DateTime.UtcNow;
        }
        
        public void UpdateInfo(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name không được để trống.", nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
