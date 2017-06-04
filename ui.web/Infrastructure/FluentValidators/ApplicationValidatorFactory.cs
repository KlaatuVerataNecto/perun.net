﻿using System;
using FluentValidation;
using SimpleInjector;


namespace ui.web.Infrastructure.FluentValidators
{
    public class ApplicationValidatorFactory : IValidatorFactory
    {
        private readonly Container _container;

        /// <summary>The constructor of the factory.</summary>
        /// <param name="container">The Simple Injector Container</param>
        public ApplicationValidatorFactory(Container container)
        {
            _container = container;
        }

        /// <summary>Gets the validator for the specified type.</summary>
        public IValidator<T> GetValidator<T>()
        {
            return _container.GetInstance<IValidator<T>>();
        }

        /// <summary>Gets the validator for the specified type.</summary>
        public IValidator GetValidator(Type type)
        {
            var validator = typeof(IValidator<>).MakeGenericType(type);
            return (IValidator)_container.GetInstance(validator);
        }
    }

    internal sealed class ValidateNothingDecorator<T> : AbstractValidator<T>
    {
        // I do nothing :-)
    }
}