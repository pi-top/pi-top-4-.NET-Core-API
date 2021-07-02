using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Formatting;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTop.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public async Task OnLoadAsync(Kernel kernel)
        {
            Formatter.Register<Sh1106Display>((d, writer) =>
            {
                var id = Guid.NewGuid().ToString("N");
                using var image = d.Capture();
                var format = image.Frames.Count > 1 ? (IImageFormat)GifFormat.Instance : PngFormat.Instance;
                var imageSource = image.ToBase64String(format);
                PocketView imgTag = img[id: id, src: imageSource]();
                writer.Write(imgTag);
            }, "text/html");

            await kernel.SendAsync(
                new DisplayValue(new FormattedValue(
                    "text/markdown",
                    "Added support for pi-top[4] module.")));
        }
    }
}