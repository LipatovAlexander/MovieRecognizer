namespace ReceiveVideoHandler;

public class Handler
{
    public async Task<Response> FunctionHandler(Request request)
    {
        Console.WriteLine(request.Body);
        return new Response(200, "Hello, world!", "application/json");
    }
}