using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NorthwindApi.Infrastructure.Middlewares;

public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly string _logDirectory;

    public ApiLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        
        var baseDir = Environment.GetEnvironmentVariable("LOG_DIR") 
                      ?? AppContext.BaseDirectory;

        _logDirectory = Path.Combine(baseDir, "Logs");

        if (!Directory.Exists(_logDirectory))
            Directory.CreateDirectory(_logDirectory);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string fileName = $"log_{DateTime.Now:dd_MM_yyyy}.txt";
        string filePath = Path.Combine(_logDirectory, fileName);

        // --- LOG REQUEST ---
        var requestInfo = new StringBuilder();
        requestInfo.AppendLine("===== REQUEST =====");
        requestInfo.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        requestInfo.AppendLine($"Method: {context.Request.Method}");
        requestInfo.AppendLine($"Path: {context.Request.Path}");
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            string body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            requestInfo.AppendLine($"Body: {body}");
        }
        requestInfo.AppendLine($"Query: {context.Request.QueryString}");
        requestInfo.AppendLine("===================");
        await File.AppendAllTextAsync(filePath, requestInfo.ToString());

        // --- CAPTURE RESPONSE ---
        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context); // gọi tiếp middleware/controller

            // Đọc status code
            int statusCode = context.Response.StatusCode;

            // Đọc response body
            memoryStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            // --- LOG RESPONSE ---
            var responseInfo = new StringBuilder();
            responseInfo.AppendLine("===== RESPONSE =====");
            responseInfo.AppendLine($"Status Code: {statusCode}");
            // responseInfo.AppendLine($"Body: {responseBody}");
            responseInfo.AppendLine("====================");
            await File.AppendAllTextAsync(filePath, responseInfo.ToString());

            // Copy dữ liệu lại để trả về client
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            // --- LOG ERROR ---
            var errorLog = new StringBuilder();
            errorLog.AppendLine("***** ERROR *****");
            errorLog.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            errorLog.AppendLine($"Message: {ex.Message}");
            errorLog.AppendLine($"StackTrace: {ex.StackTrace}");
            errorLog.AppendLine("*****************");
            await File.AppendAllTextAsync(filePath, errorLog.ToString());

            throw; // vẫn để exception bubble lên
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}