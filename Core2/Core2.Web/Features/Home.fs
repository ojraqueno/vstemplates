namespace Core2.WebF.Features.Home

open Microsoft.AspNetCore.Mvc

type HomeController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        base.View()