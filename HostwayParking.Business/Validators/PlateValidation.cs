using System.Text.RegularExpressions;

namespace HostwayParking.Business.Validators
{
    public static partial class PlateValidation
    {
        // Brasil Mercosul: ABC1D23
        // Brasil antigo:   ABC1234
        // Argentina Mercosul: AB123CD
        // Argentina antigo:   ABC123
        [GeneratedRegex(@"^([A-Z]{3}[0-9][A-Z][0-9]{2}|[A-Z]{3}[0-9]{4}|[A-Z]{2}[0-9]{3}[A-Z]{2}|[A-Z]{3}[0-9]{3})$")]
        private static partial Regex PlateRegex();

        public static bool IsValidPlate(string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return false;

            return PlateRegex().IsMatch(plate.ToUpperInvariant().Trim());
        }
    }
}
