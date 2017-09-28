using AutoMapper;

namespace MVC5_R.WebAPI.Infrastructure.Mapping
{
    public class MappingConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(typeof(MappingConfig));
            });
        }
    }
}