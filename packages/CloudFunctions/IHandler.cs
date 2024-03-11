namespace CloudFunctions;

public interface IHandler<in TRequest>
{
    Task FunctionHandler(TRequest request);
}