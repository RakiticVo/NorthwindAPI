namespace NorthwindApi.Infrastructure.Cache;

public class CacheKeys
{
    public static string Product(int id) => $"product:{id}";
    public static string Products => "products:all";
    public static string ProductsByCategory(string categoryId) => $"products:category:{categoryId}";
    public static string Category(string id) => $"category:{id}";
    public static string Categories => "categories:all";
    public static string User(string id) => $"user:{id}";
}