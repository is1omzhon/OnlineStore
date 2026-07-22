namespace Exceptions;

class InvalidPriceException : Exception
{
    public decimal Price;

    public InvalidPriceException (decimal price)
        : base($"Narx notug'ri '{price}' - 0 dan katta bulish kk. " )
    {
        Price = price;
    }
}