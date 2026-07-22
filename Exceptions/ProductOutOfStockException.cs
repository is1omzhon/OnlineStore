namespace Exceptions;

class ProductOutOfStockException : Exception
{
    public string ProductName;

    public ProductOutOfStockException(string productName)
        : base($"'{productName}' mahsuloti omborda tugagan!")
    {
        ProductName = productName;
    }
}