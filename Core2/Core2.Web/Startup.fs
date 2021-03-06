namespace Core2.Web

open System.IO;
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.FileProviders

type RouteDefaults =
    { Controller : string
      Action : string }

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddMvc() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app.UseStaticFiles() |> ignore

        if env.IsDevelopment() then
            app.UseStaticFiles(StaticFileOptions ( RequestPath = PathString("/lib"), FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules/")))) |> ignore
        
        app.UseMvc(fun routes -> 
            routes.MapRoute(name = "Home", template = "", defaults = { Controller = "Home"; Action = "Index" }) |> ignore
        ) |> ignore

    member val Configuration : IConfiguration = null with get, set