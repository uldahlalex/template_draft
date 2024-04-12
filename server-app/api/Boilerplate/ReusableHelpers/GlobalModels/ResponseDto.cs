namespace api.Boilerplate.ReusableHelpers.GlobalModels;

public class ResponseDto<T>(T data, object? metadata = null)
{
    public object? Metadata = metadata;
    public T ResponseData = data;
}