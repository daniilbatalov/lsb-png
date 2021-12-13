module FS
open System.IO
open System.Windows.Media.Imaging
open System.Windows.Media

let readFile filePath = File.ReadAllBytes (filePath)
let writeFile filePath contents = File.WriteAllBytes (filePath, contents)

let rec checkFile (file : byte list) (signature : byte list) =
    if (file.Length <= 8) then false else
        match signature with
        |[] -> true
        |h::t ->
            let fh::ft = file
            if h = fh then checkFile ft t else false
        
let checkPNG file = checkFile file ([0x89; 0x50; 0x4E; 0x47; 0x0D; 0x0A; 0x1A; 0x0A] |> List.map byte)

let decodePNG (path : string) =
     let s = new BitmapImage (new System.Uri(new System.Uri("file://"), path))
     let source =
        if s.Format <> PixelFormats.Bgra32
        then new FormatConvertedBitmap (s, PixelFormats.Bgra32, null, 0.)
            :> BitmapSource
        else s :> BitmapSource
     let width = source.PixelWidth
     let height = source.PixelHeight
     let pixels = Array.create (width * height * 4) 0uy
     source.CopyPixels(pixels, width * 4, 0)
     (pixels, width, height)

let encodePNG path (lst : byte array) width height =
    use imageStreamSource = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write)
    let encoder = new PngBitmapEncoder()
    let stride = ((width * 32 + 31) &&& ~~~31) / 8
    encoder.Frames.Add(BitmapFrame.Create(BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, lst, stride)))
    encoder.Save(imageStreamSource)