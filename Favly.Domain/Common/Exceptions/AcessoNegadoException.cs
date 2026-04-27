namespace Favly.Domain.Common.Exceptions
{
    public class AcessoNegadoException : Exception
    {
        public AcessoNegadoException(string message) : base(message) { }

        public static void When(bool denied, string message)
        {
            if (denied) throw new AcessoNegadoException(message);
        }
    }
}
