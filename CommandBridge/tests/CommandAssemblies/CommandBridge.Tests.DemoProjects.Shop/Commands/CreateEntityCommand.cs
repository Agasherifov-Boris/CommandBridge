using CommandBridge.Attributes;
using CommandBridge.Interfaces;
using CommandBridge.Tests.DemoProjects.Shop.Abstractions;
using CommandBridge.Tests.DemoProjects.Shop.Interceptors;
using System.ComponentModel.DataAnnotations;

namespace CommandBridge.Tests.DemoProjects.Shop.Commands
{
    //[UseInterceptor(typeof(ValidationInterceptor<>))]
    public class CreateEntityCommand<TEntity> : ICommand<TEntity>, IValidatableObject
        where TEntity : Entity
    {
        public required TEntity Entity { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var context = new ValidationContext(Entity);
            Validator.ValidateObject(Entity, context, validateAllProperties: true);

            return [];
        }
    }
}