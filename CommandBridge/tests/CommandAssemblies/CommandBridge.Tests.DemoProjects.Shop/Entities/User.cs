using CommandBridge.Tests.DemoProjects.Shop.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace CommandBridge.Tests.DemoProjects.Shop.Entities
{
    public record User : Entity
    {
        [Required, Length(2, 50)]
        public required string Name { get; init; }

        [Required, EmailAddress]
        public required string Email { get; init; }
    }
}