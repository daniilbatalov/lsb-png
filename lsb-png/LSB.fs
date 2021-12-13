module LSB

let byteToEightBits (b : byte) =
    [|(b &&& 1uy) >>> 0; (b &&& 2uy) >>> 1; (b &&& 4uy) >>> 2; (b &&& 8uy) >>> 3; 
    (b &&& 16uy) >>> 4; (b &&& 32uy) >>> 5; (b &&& 64uy) >>> 6; (b &&& 128uy) >>> 7|]
let eightBitsToByte (bits : byte array) =
    bits.[0] + bits.[1] * 2uy + bits.[2] * 4uy + bits.[3] * 8uy + bits.[4] * 16uy + bits.[5] * 32uy + bits.[6] * 64uy + bits.[7] * 128uy
let apply1Bit (bit : byte) (b : byte)  =
    (b &&& 254uy) ||| bit
let get1Bit (b : byte) =
    b &&& 1uy
let messageBits (message : byte array) =
    Array.map byteToEightBits message |> Array.concat
let concatMessage (message : byte array) =
    message |> Array.chunkBySize 8 |> Array.map eightBitsToByte
let hide1Bit (msgBit : byte array) (png : byte array) =
    let size_encoded = msgBit.Length
    let (coded, not_coded) = Array.splitAt size_encoded png
    Array.append (Array.map2 apply1Bit msgBit coded) not_coded
let extract1Bit (msgSize : int) (png : byte array) =
    let (coded, _) = Array.splitAt msgSize png
    Array.map get1Bit coded |> concatMessage
     
    
