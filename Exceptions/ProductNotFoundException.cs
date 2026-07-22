namespace Exceptions;

class ProductNotFoundException : Exception
{
    public int ProductId;

    public ProductNotFoundException(int productId) : base($"Id : {productId} bulgan mahsulot topilmadi!")
    {
        ProductId = productId;
    }
}