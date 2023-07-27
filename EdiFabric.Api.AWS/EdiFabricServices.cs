using EdiFabric.Api;
using EdiFabric.Api.AWS;
using Microsoft.Extensions.DependencyInjection;

public static class EdiFabricServices
{
    private static ServiceProvider _serviceProvider;
    static EdiFabricServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEdiFabricApi();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        //  Load all models from the S3 cache
        S3Cache.LoadModels(Get<IModelService>()).Wait();
        Console.WriteLine("Models loaded from cache.");
    }

    public static T Get<T>()
    {
        var service = _serviceProvider.GetService<T>();
        if (service == null)
            throw new Exception($"Can't find service {typeof(T).Name}");

        return service;
    }
}
