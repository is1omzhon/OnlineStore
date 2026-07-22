using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Exceptions;
using Models;

Dictionary<int, Product> products = new Dictionary<int, Product>();
HashSet<string> categories = new HashSet<string>();
Queue<string> orderQueue = new Queue<string>();

ProductAddedHandler? productNotifier = null;
StockChangedHandler? stockNotifier = null;



// obuna bulish
productNotifier += OnProductAdded;
productNotifier += OnProductCategory;

stockNotifier += OnLowStock;
stockNotifier += OnLowStockEmail;

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
    Console.Clear();
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
    Console.WriteLine("11. Tekshirish mahsulot bor yuqligini!");
    Console.WriteLine("12. Mahsulotlarni saqlash");
    Console.WriteLine("13. Mahsulotlarni yuklash");
    Console.WriteLine("14. API dan mahsulotlar olish");
    Console.WriteLine("15. Backup saqlash");

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
            try
            {
                GetProductById(products, id);
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Xato: {ex.Message}");
                Console.WriteLine($"Qidirilgan Id: {ex.ProductId}");

            }
            break;
        case "3":
            try
            {
                AddProduct(products, categories);
            }
            catch (InvalidPriceException ex)
            {
                Console.WriteLine($"Xato: {ex.Message}");
                Console.WriteLine($"Kititilingan narx: {ex.Message}");
            }
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
            try
            {
                await AddOrder(products, orderQueue);
            }
            catch (ProductOutOfStockException ex)
            {
                Console.WriteLine($"Xato: {ex.Message}");
                Console.WriteLine($"Mahsulot: {ex.ProductName}");

            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Xato: {ex.Message}");
            }
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
        case "12":
            await SaveProductsAsync(products);
            break;
        case "13":
            await LoadProductsAsync(products);
            break;
        case "14":
            await GetProductsFromApiAsync(products);
            break;
        case "15":
            await BackupProductsAsync(products);
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

    Console.Write("Narxi: ");
    decimal price = decimal.Parse(Console.ReadLine());

    if (price <= 0)
    {
        throw new InvalidPriceException(price);
    }

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

    productNotifier?.Invoke(newProduct);
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

        if (qty < 5)
        {
            stockNotifier?.Invoke(foundProduct.Name, qty);
        }
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

async Task AddOrder(Dictionary<int, Product> products, Queue<string> orderQueue)
{
    Console.WriteLine("Mahsulotni Id sini kiriting: ");
    int id = int.Parse(Console.ReadLine());

    if (products.TryGetValue(id, out Product foundProduct))
    {
        if (foundProduct.StockQuantity == 0)
        {
            throw new ProductOutOfStockException(foundProduct.Name);
        }

        orderQueue.Enqueue(foundProduct.Name);
        foundProduct.StockQuantity--;
        Console.WriteLine($"{foundProduct.Name} buyurtma navbatiga qo'shildi!");

        // ← SHU QATORLARNI QO'SHING
        Console.Write("Emailingizni kiriting: ");
        string email = Console.ReadLine();
        await SendEmailAsync(foundProduct.Name, email);
    }
    else
    {
        throw new ProductNotFoundException(id);
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
        throw new ProductNotFoundException(id);
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
    foreach (var p in res)
    {
        Console.WriteLine($"{p.Name} - stock: {p.StockQuantity} ta");
    }
    Console.WriteLine($"Stocki 5 va undan kickik bulgan mahsulotlar: {res}");
}

void GetProductCountByCategory(List<Product> products)
{
    var resCat = products.GroupBy(p => p.Category);
    foreach (var item in resCat)
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

    foreach (var item in sortPrice)
    {
        Console.WriteLine($"{item.Name} - {item.Price}  {item.Category}");
    }
}

void OnProductAdded(Product product)
{
    Console.WriteLine($"✅ Yangi mahsulot qo'shildi: {product.Name}");
}

void OnProductCategory(Product product)
{
    Console.WriteLine($"📋 Kategoriya: {product.Category}");
}

void OnLowStock(string productName, int stock)
{
    Console.WriteLine($"⚠️ OGOHLANTIRISH: {productName} — {stock} ta qoldi!");
}

void OnLowStockEmail(string productName, int stock)
{
    Console.WriteLine($"📧 Email yuborildi: {productName} kam qoldi!");
}

async Task SaveProductsAsync(Dictionary<int, Product> products)
{
    List<string> lines = new List<string>();

    foreach (var item in products)
    {
        Product p = item.Value;
        lines.Add($"{p.Id} , {p.Name}, {p.Price}, {p.StockQuantity}, {p.Category}");
    }

    await File.WriteAllLinesAsync("products.txt", lines);
    Console.WriteLine("✅ Mahsulotlar saqlandi!");
}

async Task LoadProductsAsync(Dictionary<int, Product> products)
{
    if (!File.Exists("products.txt"))
    {
        Console.WriteLine("Fayl topilmadi, boshlang'ich malu'motlar ishlatilinadi!");
        return;
    }

    string[] lines = await File.ReadAllLinesAsync("products.txt");

    products.Clear();

    foreach (var line in lines)
    {
        string[] parts = line.Split(',');

        Product p = new Product
        {
            Id = int.Parse(parts[0]),
            Name = parts[1],
            Price = decimal.Parse(parts[2]),
            StockQuantity = int.Parse(parts[3]),
            Category = parts[4]
        };

        products.Add(p.Id, p);
    }

    Console.WriteLine($"✅ {products.Count} ta mahsulot yuklandi!");

}

async Task SendEmailAsync(string productName, string customerEmail)
{
    Console.WriteLine($"📧 Email yuborilmoqda: {customerEmail} ga...");

    // Real hayotda bu yerda email xizmatiga so'rov ketadi
    // Biz simulyatsiya qilamiz — 1 soniya kutish
    await Task.Delay(1000);

    Console.WriteLine($"✅ Email yuborildi!");
    Console.WriteLine($"   Kimga: {customerEmail}");
    Console.WriteLine($"   Xabar: '{productName}' buyurtmangiz qabul qilindi!");
}

async Task GetProductsFromApiAsync(Dictionary<int, Product> products)
{
    Console.WriteLine("🌐 API dan mahsulot olinmoqda");

    using HttpClient client = new HttpClient();

    try
    {
        string json = await client.GetStringAsync(
            "https://jsonplaceholder.typicode.com/posts");

        Console.WriteLine("API dan javob keldi");

        int startId = products.Count + 1;

        products.Add(startId, new Product
        {
            Id = startId,
            Name = "API Mahsulot 1",
            Price = 999000,
            StockQuantity = 15,
            Category = "API"
        });

        products.Add(startId + 1, new Product
        {
            Id = startId + 1,
            Name = "API Mahsulot 2",
            Price = 1499000,
            StockQuantity = 8,
            Category = "API"
        });

        products.Add(startId + 2, new Product
        {
            Id = startId + 2,
            Name = "API Mahsulot 3",
            Price = 2999000,
            StockQuantity = 3,
            Category = "API"
        });

        Console.WriteLine($"✅ 3 ta yangi mahsulot qo'shildi!");
        Console.WriteLine($"Jami mahsulotlar: {products.Count} ta");

    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("Internet xatosi");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Xato: {ex.Message}");
    }
}

async Task BackupProductsAsync(Dictionary<int, Product> products)
{
    Console.WriteLine("💾 Backup qilinmoqda...");

    try
    {
        // Parallel — asosiy va backup faylga bir vaqtda saqlash
        List<string> lines = new List<string>();

        foreach (var item in products)
        {
            Product p = item.Value;
            lines.Add($"{p.Id},{p.Name},{p.Price},{p.StockQuantity},{p.Category}");
        }

        // Ikki faylga PARALLEL saqlash — Task.WhenAll!
        Task saveMain = File.WriteAllLinesAsync("products.txt", lines);
        Task saveBackup = File.WriteAllLinesAsync(
            $"backup_{DateTime.Now:yyyy-MM-dd}.txt", lines);

        await Task.WhenAll(saveMain, saveBackup);

        Console.WriteLine($"✅ Asosiy fayl saqlandi: products.txt");
        Console.WriteLine($"✅ Backup saqlandi: backup_{DateTime.Now:yyyy-MM-dd}.txt");
        Console.WriteLine($"💾 Jami {products.Count} ta mahsulot saqlandi!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Backup xatosi: {ex.Message}");
    }
}



