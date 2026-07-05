using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Models;

Dictionary<int, Product> products = new Dictionary<int, Product>();
HashSet<string> categories = new HashSet<string>();
Queue<string> orderQueue = new Queue<string>();

products.Add(1, new Product { Id = 1, Name = "Telefon", Price = 1500000, StockQuantity = 10, Category = "Elektronika" });
products.Add(2, new Product { Id = 2, Name = "Noutbook", Price = 8000000, StockQuantity = 5, Category = "Elektronika" });
products.Add(3, new Product { Id = 3, Name = "Miw", Price = 500000, StockQuantity = 30, Category = "Elektronika" });
products.Add(4, new Product { Id = 4, Name = "T-Shirt", Price = 1200000, StockQuantity = 15, Category = "Kiyim" });
products.Add(5, new Product { Id = 5, Name = "Bryuk", Price = 2500000, StockQuantity = 8, Category = "Kiyim" });


categories.Add("Elektronika");
categories.Add("Kiyim");

string userChoice = string.Empty;
do
{
    Console.WriteLine("\n===== ONLINE STORE =====");
    Console.WriteLine("1. Barcha mahsulotlarni ko'rish");
    Console.WriteLine("2. Mahsulot qidirish (Id bo'yicha)");
    Console.WriteLine("3. Mahsulot qo'shish");
    Console.WriteLine("4. Stock yangilash");
    Console.WriteLine("5. Kategoriyalarini ko'rish");
    Console.WriteLine("6. Buyurtma berish");
    Console.WriteLine("7. Navbatdagi buyurtmani qayta ishlash");
    Console.WriteLine("8. Kategoriya bo'yicha ko'rish");
    Console.WriteLine("9. Statistika");
    Console.WriteLine("10. Qidiruv");

    Console.Write("Dasturni ishlatish uchun birini tanleng: ");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowAllProducts(products);
            break;
        case "2":
            Console.Write("Id kiriting: ");
            int id = int.Parse(Console.ReadLine());
            GetProductById(products, id);
            break;
        case "3":
            AddProduct(products, categories);
            break;
        case "4":
            Console.Write("Id kiriting: ");
            int updateId = int.Parse(Console.ReadLine());
            Console.Write("Yangi stock miqdori: ");
            int qty = int.Parse(Console.ReadLine());
            UpdateStock(products, updateId, qty);
            break;
        case "5":
            ShowCategories(categories);
            break;
        case "6":
            AddOrder(products, orderQueue);
            break;
        case "7":
            ProcessOrder(orderQueue);
            break;
        case "8":
            Console.Write("Kategoriyani kiriting: ");
            string categ = Console.ReadLine();
            GetProductsByCategory(products.Values.ToList(), categ);
            break;
        case "9":
            ShowStatictics(products.Values.ToList());
            break;
        case "10":
            Console.Write("Qidiruv so'zi: ");
            string kword = Console.ReadLine();
            SearchProducts(products.Values.ToList(), kword);
            break;
        case "11":
            Console.Write("Idni kiriting: ");
            int idProduct = int.Parse(Console.ReadLine());
            Console.WriteLine(IsProductAvailable(products.Values.ToList(), idProduct));
            break;

        default:
            Console.WriteLine("Noto'gri tanlov!");
            break;
    }

    Console.Write("Dasturni davom ettirishni xoxlaysizmi (y|n): ");
    userChoice = Console.ReadLine();

} while (userChoice.ToLower() == "y");


// Wunchaki productni ekranga chiqoriw
void ShowAllProducts(Dictionary<int, Product> products)
{
    if (products.Count == 0)
    {
        Console.WriteLine("Mahsulotlar yuq!");
        return;
    }

    Console.WriteLine("\n===== MAHSULOTLAR RO'YXATI =====");
    foreach (var item in products)
    {
        Product p = item.Value;
        Console.WriteLine($"Id: {p.Id} | Nomi: {p.Name} | Narxi: {p.Price} so'm | Stock: {p.StockQuantity} ta | Kategoriya: {p.Category}");
    }
}

void AddProduct(Dictionary<int, Product> products, HashSet<string> categories)
{
    Console.Write("Yangi Id kiriting: ");
    int id = int.Parse(Console.ReadLine());

    if (products.ContainsKey(id))
    {
        Console.WriteLine($"Id: {id} allaqachon band! Boshqa Id kiriting! ");
        return;
    }

    Console.Write("Nomi: ");
    string name = Console.ReadLine();

    Console.WriteLine("Narxi: ");
    decimal price = decimal.Parse(Console.ReadLine());

    Console.Write("Stock miqdori: ");
    int stock = int.Parse(Console.ReadLine());

    Console.Write("Kategoriya: ");
    string category = Console.ReadLine();

    // yangi mahsulot yaratish
    Product newProduct = new Product
    {
        Id = id,
        Name = name,
        Price = price,
        StockQuantity = stock,
        Category = category
    };

    products.Add(id, newProduct);

    categories.Add(category);

    Console.WriteLine($" {name} mahsuloti muvaffaqiyatli qo'shildi! ");
}

void UpdateStock(Dictionary<int, Product> products, int id, int qty)
{
    if (qty < 0)
    {
        Console.WriteLine("Stock miqdori 0 dan kichik bo'lishi mumkin emas!");
        return;
    }

    if (products.TryGetValue(id, out Product foundProduct))
    {
        foundProduct.StockQuantity = qty;
        Console.WriteLine($"{foundProduct.Name} mahsulotining stocki {qty} taga yangilandi!");
    }
    else
    {
        Console.WriteLine($"Id: {id} bo'lgan mahsulot topilmadi!");
    }
}

void ShowCategories(HashSet<string> categories)
{
    Console.WriteLine("\n===== KATEGORIYA =====");
    foreach (var category in categories)
    {
        Console.WriteLine($"- {category}");
    }
}

void AddOrder(Dictionary<int, Product> products, Queue<string> orderQueue)
{
    Console.WriteLine("Mahsulotni Id sini kiriting: ");
    int id = int.Parse(Console.ReadLine());

    if (products.TryGetValue(id, out Product foundProduct))
    {
        if (foundProduct.StockQuantity == 0)
        {
            Console.WriteLine("Bu mahsulot omborda yuq");
            return;
        }

        orderQueue.Enqueue(foundProduct.Name);
        foundProduct.StockQuantity--;
        Console.WriteLine($"{foundProduct.Name} buyurtma navbatiga qushildi! ");
    }
    else
    {
        Console.WriteLine("Mahsulot topilmadi");
    }
}

void ProcessOrder(Queue<string> orderQueue)
{
    if (orderQueue.Count == 0)
    {
        Console.WriteLine("Navbatda buyurtma yo'q!");
        return;
    }

    string order = orderQueue.Dequeue();
    Console.WriteLine($" {order} buyurtmasi qayta ishlandi");
    Console.WriteLine($"Navbatda qolgan buyurtmalar {orderQueue.Count} ta");
}

void GetProductById(Dictionary<int, Product> products, int id)
{
    if (products.TryGetValue(id, out Product foundProduct))
    {
        Console.WriteLine($"Id: {foundProduct.Id}");
        Console.WriteLine($"Nomi: {foundProduct.Name}");
        Console.WriteLine($"Narxi: {foundProduct.Price}");
        Console.WriteLine($"Stock miqdori: {foundProduct.StockQuantity} ta");
        Console.WriteLine($"Kategoriya: {foundProduct.Category}");
    }
    else
    {
        Console.WriteLine($"{id} mahsulot mavjud emas!");
    }
}

void GetProductsByCategory(List<Product> products, string category)
{
    var result = products
        .Where(p => p.Category == category)
        .OrderBy(p => p.Price)
        .ToList();

    if (!result.Any())
    {
        Console.WriteLine($"{category} kategoriyasadagi mahsulot yuq!");
        return;
    }

    Console.WriteLine($"/n==== {category.ToUpper()} =====");
    foreach (var p in result)
    {
        Console.WriteLine($"{p.Name} - {p.Price} so'm (stock: {p.StockQuantity})");
    }
}


void ShowStatictics(List<Product> products)
{
    Console.WriteLine("\n===== STATISTICS =====");
    Console.WriteLine($"Jami mahsulotlar: {products.Count()}");
    Console.WriteLine($"Eng qimmat: {products.Max(p => p.Price)} so'm");
    Console.WriteLine($"Eng arzon: {products.Min(p => p.Price)} so'm");
    Console.WriteLine($"O'rtacha narx: {products.Average(p => p.Price)} so'm");
    Console.WriteLine($"Ombordagi soni: {products.Sum(p => p.StockQuantity)} ta");
}

void SearchProducts(List<Product> products, string keyword)
{
    var result = products
        .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (!result.Any())
    {
        Console.WriteLine("Hech narsa topilmadi!");
    }

    foreach (var item in result)
    {
        Console.WriteLine($"{item.Name} - {item.Price} so'm");
    }
}

void GetMostExpensiveProduct(List<Product> products)
{
    Product mostExpensive = products.MaxBy(p => p.Price);

    Console.WriteLine($"Eng qimmat: {mostExpensive.Name} - {mostExpensive.Price} so'm");       
}
void GetLowStockProducts(List<Product> products)
{
    var res = products.Where(p => p.StockQuantity <= 5).ToList();
    foreach(var p in res)
    {
        Console.WriteLine($"{p.Name} - stock: {p.StockQuantity} ta");
    }
    Console.WriteLine($"Stocki 5 va undan kickik bulgan mahsulotlar: {res}");
}

void GetProductCountByCategory(List<Product> products)
{
    var resCat = products.GroupBy(p => p.Category);
    foreach(var item in resCat)
    {
        Console.WriteLine($"{item.Key} : {item.Count()} ta");
    }
}

bool IsProductAvailable(List<Product> products, int id)
{
    return products.Any(p => p.Id == id && p.StockQuantity > 0);
}

void GetProductsSortedByPrice(List<Product> products, string category)
{
    var sortPrice = products
        .Where(p => p.Category == category)
        .OrderBy(p => p.Price)
        .ToList();
    if (!sortPrice.Any())
    {
        Console.WriteLine($"{category} kategoriyasadagi mahsulot yuq!");
        return;
    }

    foreach(var item in sortPrice)
    {
        Console.WriteLine($"{item.Name} - {item.Price}  {item.Category}");
    }
}
