using DomainCore.Teams;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Organizations
{
    public sealed class Organization
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Slug { get; private set; }

        public string? Description { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public ICollection<Team> Teams { get; private set; }= new List<Team>();


        private Organization()
        {
        }


        public Organization(
            Guid id,
            string name,
            string slug,
            string? description,
            DateTime createdAt)
        {
            Id = id;

            Name = name;

            Slug = slug;

            Description = description;

            CreatedAt = createdAt;
        }
    }
}
