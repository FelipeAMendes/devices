using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Devices.Shared.Validations.Customs;

public class RegexValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
    private readonly string _pattern;

    public RegexValidator(string pattern)
    {
        _pattern = pattern;
    }

    public override string Name => nameof(RegexValidator<T, TProperty>);

    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        if (value is not string valuePattern)
            return false;

        return Regex.IsMatch(valuePattern, _pattern);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} invalid";
    }
}
