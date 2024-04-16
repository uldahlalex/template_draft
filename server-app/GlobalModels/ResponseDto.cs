namespace Agnostics;

public class ResponseDto<T>(T data, object? metadata = null)
{
    public object? Metadata = metadata;
    public T ResponseData = data;
}