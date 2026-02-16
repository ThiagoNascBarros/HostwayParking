namespace HostwayParking.Business.Exceptions
{
    public class ValidationErrorsException : Exception
    {
        public List<string> Errors { get; }

        public ValidationErrorsException(List<string> errors)
            : base(string.Join("; ", errors))
        {
            Errors = errors;
        }
    }
}
