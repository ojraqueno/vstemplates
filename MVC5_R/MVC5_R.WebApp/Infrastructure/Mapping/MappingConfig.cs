using AutoMapper;

namespace MVC5_R.WebApp.Infrastructure.Mapping
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