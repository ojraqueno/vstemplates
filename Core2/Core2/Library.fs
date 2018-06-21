namespace Core2F

module Say =
    let hello name =
        printfn "Hello %s" name

type Test = {
    Message : string }