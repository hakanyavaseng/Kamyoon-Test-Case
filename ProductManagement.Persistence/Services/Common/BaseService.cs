using AutoMapper;
using ProductManagement.Core.Helpers;
using ProductManagement.Core.Interfaces.Services;

namespace ProductManagement.Persistence.Services.Common;

public class BaseService : IBaseService
{
    private readonly Lazy<IMapper> _lazyObjectMapper;

    public BaseService()
    {
        _lazyObjectMapper = new Lazy<IMapper>(() => ServiceLocator.GetService<IMapper>());
    }

    // Public properties
    public IMapper ObjectMapper => _lazyObjectMapper.Value;
}