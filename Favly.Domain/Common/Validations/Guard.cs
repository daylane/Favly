using Favly.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Validations
{
    internal static class Guard
    {
        public static void AgainstEmptyGuid(Guid id, string parameterName)
        {
            if (id == Guid.Empty)
                throw new DomainException($"{parameterName} não pode ser Guid.Empty");
        }
        public static void AgainstNull<T>(T value, string parameterName)
        {
            if (value is null)
                throw new DomainException($"{parameterName} não pode ser nulo.");
        }
        public static void AgainstNullOrWhiteSpace<T>(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{parameterName} não pode ser nulo ou vazio.");
        }
        public static void AgainstInvalidEnum<TEnum>(object value, string parameterName) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new DomainException($"O valor '{value}' não é um {typeof(TEnum).Name} válido para o parâmetro {parameterName}.");
        }
        public static void Against<TException>(bool condition, string message) where TException : Exception
        {
            if (condition)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }
    }
}
