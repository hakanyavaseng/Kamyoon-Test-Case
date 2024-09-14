using AutoMapper;

namespace ProductManagement.Persistence.Mapper;

public class ObjectMapper
{
    private static readonly Lazy<IMapper> mapper = new(() =>
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<ProductManagementMapperProfile>(); });

        return config.CreateMapper();
    });

    public static IMapper Mapper => mapper.Value;
}