open FS
open LSB
open System
open System.IO
[<EntryPoint>]
let main args =
    let getPath path = Directory.GetCurrentDirectory() + @"\" + path
    let arg = args.[0]
    match arg with
    |"-encode" ->
        let file = readFile args.[1] |> List.ofArray
        if (checkPNG file) then
            let data = readFile args.[3] |> messageBits
            let (png, w, h) = decodePNG (getPath args.[1])
            let b = hide1Bit data png 
            encodePNG args.[2] b w h
        else
            System.Console.Write("Error! The file isn't a PNG file")
    |"-decode" ->
        let file = readFile args.[1] |> List.ofArray
        if (checkPNG file) then
            let (png, w, h) = decodePNG (getPath args.[1])
            let msgSize = args.[2] |> int |> (*) 8
            let b = extract1Bit msgSize png
            writeFile args.[3] b
        else
            System.Console.Write("Error! The file isn't a PNG file")
    |_ ->
        raise(System.ArgumentException("Incorrect argument!"))
    0