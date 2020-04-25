using Amazon.Lambda.Core;
using Amazon.S3;
using ImageMagick;

namespace Project2
{
    public class ImageService
    {
        private readonly string _source;
        private readonly string _destination;

        public ImageService(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        public void Resize(int width, int height, bool expand = true, Gravity position = Gravity.Center)
        {
            using (MagickImage image = new MagickImage(_source))
            {
                image.Resize(width, height);

                if (expand)
                {
                    image.Extent(
                        width,
                        height,
                        position,
                        MagickColor.FromRgba(
                            image.BackgroundColor.R,
                            image.BackgroundColor.G,
                            image.BackgroundColor.B,
                            image.BackgroundColor.A
                        )
                    );
                }

                image.Write(_destination);
            }
        }
    }
}