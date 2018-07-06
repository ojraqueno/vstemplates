namespace Core2.Web.Features.Home

open Microsoft.AspNetCore.Mvc

type HomeController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        base.View()