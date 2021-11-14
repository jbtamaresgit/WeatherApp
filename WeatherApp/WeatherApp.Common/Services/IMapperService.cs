namespace WeatherApp.Common.Services
{
    public interface IMapperService<T, K> where T : class
                                         where K : class
    {
        K Map(T contract);
    }
}
