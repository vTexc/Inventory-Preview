using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Net;

namespace Inventory_Preview
{
    public class ImageCache
    {
        public bool bIsDownloaded;
        public string Url;
        public string FilePath;

        public void OnGetDownloadedStringCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var contentType = ((WebClient)sender).ResponseHeaders[HttpResponseHeader.ContentType];
            if (e.Error == null && contentType == "image/png")
            {
                Bitmap flaskImg;
                using (var ms = new MemoryStream(e.Result))
                {
                    flaskImg = new Bitmap(ms);
                }

                if (FilePath.Contains("Flasks"))//Cut 1/3 of flask image
                {
                    flaskImg = CropImage(flaskImg, new System.Drawing.Rectangle(0, 0, flaskImg.Width / 3, flaskImg.Height));
                }

                flaskImg.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);

                bIsDownloaded = true;//Due to async processing this must be in the last line
            }
            else
            {
                PoeHUD.Plugins.BasePlugin.LogError("InventoryPreviewPlugin Warning: Invalid Url, ask Admin to fix plugin URL.", 10);
            }
        }

        /**
         * Images download and management
         * 
         * credits: StriderMann
         */
        private static Dictionary<string, ImageCache> ImagesCache = new Dictionary<string, ImageCache>();

        public static ImageCache GetImage(string metadata)
        {
            ImageCache result;

            if (!ImagesCache.TryGetValue(metadata, out result))
            {
                result = DownloadImage(metadata);
                ImagesCache.Add(metadata, result);
            }

            return result;
        }

        //from http://stackoverflow.com/questions/9484935/how-to-cut-a-part-of-image-in-c-sharp
        private Bitmap CropImage(Bitmap source, System.Drawing.Rectangle section)
        {
            return source.Clone(section, source.PixelFormat);
        }

        //Images from site:
        //http://webcdn.pathofexile.com/image/Art/2DItems/Currency/CurrencyRerollRare.png
        private static ImageCache DownloadImage(string metadata)
        {
            //Metadata will be always contains (ends with) ".dds" keyword. Check AddItemToCells.
            metadata = metadata.Replace(".dds", ".png");
            var url = "http://webcdn.pathofexile.com/image/" + metadata;

            var filePath = Info.pluginDirectory + "/resources/" + metadata;

            ImageCache img = new ImageCache()
            {
                FilePath = filePath,
                Url = url
            };

            try
            {
                if (File.Exists(img.FilePath))
                {
                    img.bIsDownloaded = true;
                    return img;
                }

                var settingsDirName = Path.GetDirectoryName(img.FilePath);
                if (!Directory.Exists(settingsDirName))
                    Directory.CreateDirectory(settingsDirName);

                WebClient webClient = new WebClient();
                webClient.DownloadDataAsync(new Uri(img.Url), img.FilePath);
                webClient.DownloadDataCompleted += img.OnGetDownloadedStringCompleted;
            }
            catch
            {
                Core.LogError("Error processing: Url: " + img.Url + ", Path: " + img.FilePath, 4);
            }
            return img;
        }
    }
}
