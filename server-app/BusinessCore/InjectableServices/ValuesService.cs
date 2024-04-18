namespace IndependentHelpers.InjectableServices;

public class ValuesService
{
    public string Development = nameof(Development);


    public string JWT_KEY =
        "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";

    public string LOCAL_POSTGRES =
        "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";


    public string Production = nameof(Production);
    public string Testing = nameof(Testing);
}