namespace Core;


public  class Values
{
    public const  string LOCAL_POSTGRES =
        "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";


    public const string JWT_KEY =
        "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";

    public  class Environments
    {
        public const string Production = nameof(Production);
        public const  string Development = nameof(Development);
        public const string Testing = nameof(Testing);
    }
}