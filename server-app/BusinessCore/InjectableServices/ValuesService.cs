namespace IndependentHelpers.InjectableServices;


public  class ValuesService
{
    public   string LOCAL_POSTGRES =
        "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";


    public string JWT_KEY =
        "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";
    
    
        public  string Production = nameof(Production);
        public   string Development = nameof(Development);
        public  string Testing = nameof(Testing);
    
}