using System.Globalization;
using System.Windows.Controls;

namespace Palettetitizer.Rules
{

    public class ValidateColumns : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string s && int.TryParse(s, out var i))
                if (i < MainWindow.MaxColumns && i >= MainWindow.MinColumns)
                    if (i % 2 == 0)
                        return ValidationResult.ValidResult;
                    else
                        return new ValidationResult(false, "Value must be divisible by two.");
                else
                    return new ValidationResult(false, $"Value must be between {MainWindow.MinColumns} and {MainWindow.MaxColumns}.");
            else
                return new ValidationResult(false, "Value is not an integer.");
        }

    }

}
