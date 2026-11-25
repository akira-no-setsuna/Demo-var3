using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Demo_var3
{
    public static class Img
    {
        static string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        public static ImageSource Get(this string photo)
        {

            try
            {

                var path = Path.Combine(dir, photo ?? "");

                if (!File.Exists(path))
                    path = Path.Combine(dir, "picture.png");

                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path);
                image.EndInit();

                return image;
            }
            catch { return null; }

        }
    }
}
