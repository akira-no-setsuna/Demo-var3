using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
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
            catch
            {
                MessageBox.Show("Ошибка загрузки фото!", "Ошибка");
                return null;
                }
        }

        public static string Save(this ImageSource photo)
        {
            int maxW = 300;
            int maxH = 200;

            try
            {
                if (photo is BitmapImage image)
                {

                    if (image.PixelWidth > maxW || image.PixelHeight > maxH)
                    {
                        MessageBox.Show("Фото слишком большое!", "Ошибка");
                        return null;
                    }
                    string name = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    string destPath = Path.Combine(dir, name);
                    File.Copy(image.UriSource.LocalPath, destPath);

                    return name;

                }
                else 
                {
                    MessageBox.Show("Ошибка cохранения фото!", "Ошибка");
                    return null;
                }

            }
            catch 
            { 
                MessageBox.Show("Ошибка cохранения фото!", "Ошибка");
                    return null;
            }
        }

        public static void Delete(this string photo)
        {
            try
            {
                File.Delete(Path.Combine(dir, photo));   
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка удаления фото!", "Ошибка");
            }
        }

    }
}
