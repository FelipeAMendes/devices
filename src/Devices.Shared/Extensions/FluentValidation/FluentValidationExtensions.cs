using Devices.Shared.Validations.Customs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Devices.Shared.Extensions.FluentValidation;

public static partial class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> OrderBy<T>(this IRuleBuilder<T, string> ruleBuilder, string allowsProperties, bool allowNull)
    {
        return ruleBuilder.Must(value =>
        {
            switch (value)
            {
                case null when allowNull:
                    return true;
                case null:
                    return false;
            }

            if (allowsProperties is null)
                return false;

            var allowsPropertiesArray = allowsProperties.Split(',').Select(y => y.Trim().ToLower());
            var regexExtractProperty =
                RegexExtractProperty().Match(value.ToLower());

            if (!regexExtractProperty.Success)
                return false;

            var propertyToValid = regexExtractProperty.Groups[2].Value.Trim();
            var propertyAllow = allowsPropertiesArray.Contains(propertyToValid);
            return propertyAllow;
        }).WithMessage("'{PropertyValue}' cannot be used to sort");
    }

    public static IRuleBuilderOptions<T, IList<TElement>> UniqueBy<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, Func<TElement, object> keyExtractor)
    {
        var keyEqualityComparer = new KeyEqualityComparerValidator<TElement>(keyExtractor);
        return ruleBuilder.Must(list => list == null || list.Distinct(keyEqualityComparer).Count() == list.Count)
            .WithMessage("{PropertyName} cannot have duplicate values");
    }

    public static IRuleBuilderOptions<T, string> RegexPattern<T>(this IRuleBuilder<T, string> ruleBuilder,
        string pattern)
    {
        return ruleBuilder.SetValidator(new RegexValidator<T, string>(pattern));
    }

    [GeneratedRegex("^(\\s+)?(.*)\\s+(asc|desc)(\\s+)?$", RegexOptions.Compiled)]
    private static partial Regex RegexExtractProperty();
}