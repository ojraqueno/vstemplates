namespace Core2.WebF.Features.Values

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc

module Add =
    type User = 
        { FirstName: string
          LastName: string }

    let handle (user : User) =
        OkResult()

module Search =
    type SearchQuery =
        { SearchTerm : string }

    let handle (searchQuery : SearchQuery) =
        [|"value1"; "value2"|]

[<Route("api/[controller]")>]
type ValuesController () =
    inherit Controller()

    [<HttpPost>]
    [<Route("add")>]
    member this.Add([<FromBody>]user) =
        Add.handle user :> IActionResult

    [<HttpGet>]
    [<Route("search")>]
    member this.Search(searchTerm) =
        Search.handle { SearchTerm = searchTerm }