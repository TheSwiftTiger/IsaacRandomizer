using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using static IsaacRandomizer.RandomNumberGenerator;
using static IsaacRandomizer.MainWindow;
using System.Drawing.Imaging;

namespace IsaacRandomizer
{
    public static class ImageSplit
    {
        private static Image imgPicture;
        private static Bitmap bmp;

        public static Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(image);

                    //update the size of the final bitmap
                    width = 320;
                    height = 384;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.Transparent);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    int yOffset = 0;
                    for(int i = 1; i < images.Count; i++)
                    {
                        g.DrawImage(images[i],
                        new Rectangle(offset, yOffset, images[i].Width, images[i].Height));
                        offset += images[i].Width;
                        if(i % 20 == 0)
                        {
                            yOffset += 16;
                            offset = 0;
                        }
                        
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                    
                }
            }
        }
        private static void RotateIcon(string p, int i)
        {
            
            byte[] bytes = System.IO.File.ReadAllBytes(p);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            imgPicture = Image.FromStream(ms);

            bmp = new Bitmap(imgPicture);

            if (UniversalList.ElementAtOrDefault(i)!=null)
            {
                if (UniversalList[i].Rotation1)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

                }

                if (UniversalList[i].Rotation2)
                {
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);

                }

                if (UniversalList[i].Rotation3)
                {
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                }

                var bmp2 = new Bitmap(bmp);
                //}

                using (ms = new MemoryStream())
                {
                    Directory.CreateDirectory(@"resources\edited_icons");
                    var FileToSave = @"resources\edited_icons\" + UniversalList[i].ItemID + ".png";
                    Debug.WriteLine(FileToSave);
                    bmp2.Save(FileToSave, ImageFormat.Png);
                }
                bmp2.Dispose();
            }

        }
        public static void SplitImage()
        {
            var counter = 0;
            var file = @"resources\packed\afterbirth_unpack\resources\gfx\ui\death items.png";
            var sheetPath = @"resources\gfx\ui\death items.png";
            Directory.CreateDirectory(@"resources\icons");
            var output = @"resources\icons";

            using (Stream imageStreamSource = new FileStream(
                file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(
                    imageStreamSource,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];
                for (int j = 0; j < bitmapSource.PixelHeight / 16; j++)
                {
                    for (int i = 0; i < bitmapSource.PixelWidth / 16; i++)
                    {
                        CroppedBitmap croppedBitmap = new CroppedBitmap(
                            bitmapSource,
                            new Int32Rect(i * 16, j * 16, 16, 16));

                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        var frame = BitmapFrame.Create(croppedBitmap);
                        encoder.Frames.Add(frame);
                        counter++;
                        var fileName = Path.Combine(output, counter.ToString() + ".png");
                        using (var stream = new FileStream(fileName, FileMode.Create))
                        {
                            encoder.Save(stream);
                        }
                    }
                }
            }

            counter = 1;

            var files = new DirectoryInfo(@"resources\icons").GetFiles()
                                                  .OrderBy(f => f.LastWriteTime)
                                                  .ToList();
            var filePictures = files.Select(f => f.FullName).ToArray();




            var fileNames = files.Select(f => f.Name).ToArray();

            Debug.WriteLine(UniversalList[20].PictureID);
            for (int f = 1; f < fileNames.Count(); f++)
            {

                foreach (var f2 in MainWindow.UniversalList)
                {

                    if (fileNames[f] == (f2.ItemID.ToString() + ".png"))
                    {
                        RotateIcon(@"resources\icons\" + f + ".png", f2.PictureID);
                    }
                }
                counter++;
            }



            var bmp = CombineBitmap(fileNames);
            Directory.CreateDirectory(@"resources\gfx\ui");
            bmp.Save(@"resources\gfx\ui\death items.png");

            foreach (var icon in files)
            {
                using (Stream imageStreamSource = new FileStream(
                    icon.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (Stream sheetStreamSource = new FileStream(
                     sheetPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (Graphics grfx = Graphics.FromImage(Image.FromStream(imageStreamSource)))
                        {
                            grfx.DrawImage(Image.FromStream(sheetStreamSource), counter, 0);

                        }
                        counter += 16;
                    }
                }
            }
            
        }
    }
}