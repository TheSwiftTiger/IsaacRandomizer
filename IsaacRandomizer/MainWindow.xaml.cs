using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;

using System.Reflection;
using System.Windows.Interop;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using SoundTouch;
using NAudio.Wave;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
using System.IO;
using Path = System.IO.Path;
using static IsaacRandomizer.RandomNumberGenerator;
using static IsaacRandomizer.RandomNameGenerator;
using static IsaacRandomizer.ShuffleClass;
using System.Windows.Input;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Drawing2D;
using NAudio.Vorbis;
using System.Text.RegularExpressions;
using System.Media;

namespace IsaacRandomizer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Image imgPicture;
        Bitmap bmpPicture;
        Bitmap bmp;

        System.Drawing.Imaging.ImageAttributes iaPicture;
        System.Drawing.Imaging.ColorMatrix cmPicture;
        Graphics gfxPicture;
        System.Drawing.Rectangle rctPicture;

        MemoryStream ms, ms2;

        public string Filename;

        public string FileToSave;

        public int CurrentFile;

        private static int tasks = 0;

        const float rwgt = 0.3086f;
        const float gwgt = 0.6094f;
        const float bwgt = 0.0820f;

        private static List<Picture> universalList = new List<Picture>();

        public static List<Picture> UniversalList
        {
            get { return universalList; }
        }

        PrivateFontCollection pfc = new PrivateFontCollection();

        static FontFamily isaacHandwritten;

        static FontFamily readableFont;
        

        public MainWindow()
        {
            InitializeComponent();

            textBox.Text = GetIsaacPath();
            string strWorkingDirectory = Directory.GetCurrentDirectory();
            Debug.WriteLine(strWorkingDirectory);

            UpdatePresets();

            PrivateFontCollection _fonts = new PrivateFontCollection();

            byte[] fontData = Properties.Resources.IsaacHandwritten;

            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);

            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            _fonts.AddMemoryFont(fontPtr, fontData.Length);

            Marshal.FreeCoTaskMem(fontPtr);

            isaacHandwritten = _fonts.Families[0];


            fontData = Properties.Resources.AmaticSC_Bold;

            fontPtr = Marshal.AllocCoTaskMem(fontData.Length);

            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            _fonts.AddMemoryFont(fontPtr, fontData.Length);

            Marshal.FreeCoTaskMem(fontPtr);

            readableFont = _fonts.Families[0];

            LoadPreset(@"Presets\Recommended.boir");
        }

        private void UpdatePresets()
        {
            foreach (var file in new DirectoryInfo(@"Presets").GetFiles())
            {
                if (file.Name.EndsWith(".boir") && !presetList.Items.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                {
                    presetList.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
                }
            }
            List<object> toRemove = new List<object>();
            foreach (var elem in presetList.Items)
            {
                if (!File.Exists(@"Presets\" + elem.ToString() + ".boir"))
                {
                    toRemove.Add(elem);
                }
            }
            foreach (var i in toRemove)
            {
                presetList.Items.Remove(i);
            }
        }

        private bool IsValid(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        private void isInteger_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int outparse;

            if (!int.TryParse(e.Text, out outparse))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private string preset;

        private void isFloat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsValid(e.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private float PositiveOrNegative(float number)
        {
            if (RandomInt(1, 100) > 50)
            {
                return number * (-1);
            }
            else return number;
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            this.StopFlashingWindow();
        }

        public List<System.Drawing.Point> playerOffsets = new List<System.Drawing.Point> { new System.Drawing.Point(0, 288), new System.Drawing.Point(0, 352), new System.Drawing.Point(0, 320), new System.Drawing.Point(0, 384), new System.Drawing.Point(0, 448), new System.Drawing.Point(0, 416), new System.Drawing.Point(0, 480), new System.Drawing.Point(80, 320), new System.Drawing.Point(80, 288), new System.Drawing.Point(80, 352), new System.Drawing.Point(80, 384), new System.Drawing.Point(176, 480), new System.Drawing.Point(176, 512) };

        private void CreateCharacterMenuName(string name, int offsetIndex)
        {
            var rect = new RectangleF(playerOffsets[offsetIndex], new System.Drawing.Size(80, 32));

            using (Bitmap b = new Bitmap(80, 32))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    var fontSize = 100f;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    StringFormat stringFormat = new StringFormat();
                    
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    if (fontReadable.IsChecked ?? false)
                    {
                        while (g.MeasureString(name, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), 80, stringFormat).Height > 32 || g.MeasureString(name, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), new System.Drawing.Size(80, 32), stringFormat).Width > 80)
                        {
                            fontSize -= 0.1f;
                        }
                        g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        g.DrawString(name, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, new Rectangle(0, 0, 80, 32), stringFormat);
                    }
                    else
                    {
                        while (g.MeasureString(name, new Font(isaacHandwritten, fontSize), 80, stringFormat).Height > 32 || g.MeasureString(name, new Font(isaacHandwritten, fontSize), new System.Drawing.Size(80, 32), stringFormat).Width > 80)
                        {
                            fontSize -= 0.1f;
                        }
                        g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        g.DrawString(name, new Font(isaacHandwritten, fontSize), System.Drawing.Brushes.Black, new Rectangle(0, 0, 80, 32), stringFormat);
                    }
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            System.Drawing.Color bitColor = b.GetPixel(x, y);
                            b.SetPixel(x, y, System.Drawing.Color.FromArgb(bitColor.A, 55, 43, 45));
                        }
                    }
                    using (Bitmap charImage = new Bitmap(@"resources\gfx\ui\main menu\charactermenu.png"))
                    {
                        using (Graphics charGraphics = Graphics.FromImage(charImage))
                        {
                            charGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                            charGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            charGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            charGraphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(255, 233, 218, 223)), rect);
                            charGraphics.DrawImage(b, new Rectangle(playerOffsets[offsetIndex], new System.Drawing.Size(80, 32)));
                            charGraphics.Flush();
                        }

                        charImage.Save(@"resources\gfx\ui\main menu\charactermenu.png.bak", ImageFormat.Png);
                    }
                    File.Copy(@"resources\gfx\ui\main menu\charactermenu.png.bak", @"resources\gfx\ui\main menu\charactermenu.png", true);
                    File.Delete(@"resources\gfx\ui\main menu\charactermenu.png.bak");
                    tasks += 10;
                }
            }
        }

        private void CreateBossVsName(string bossname, string path)
        {
            if (bossname.Contains("s's"))
            {
                bossname = bossname.Replace("s's", "s'");
            }
            using (Bitmap b = new Bitmap(192, 64))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    var rectF = new RectangleF(0, 0, 192, 64);
                    var fontSize = 100f;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    if (bossname.Length > 13 && bossname.StartsWith("The "))
                    {
                        bossname.Replace("The ", "");
                    }

                    if (fontReadable.IsChecked ?? false)
                    {
                        while (g.MeasureString(bossname, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), 192, stringFormat).Height > 64 || g.MeasureString(bossname, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), new SizeF(192, 64), stringFormat).Width > 192)
                        {
                            fontSize -= 0.1f;
                        }
                        g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        g.DrawString(bossname, new Font(readableFont, fontSize, System.Drawing.FontStyle.Bold), Brushes.Black, rectF, stringFormat);
                    }
                    else
                    {
                        while (g.MeasureString(bossname, new Font(isaacHandwritten, fontSize), 192, stringFormat).Height > 64 || g.MeasureString(bossname, new Font(isaacHandwritten, fontSize), new SizeF(192, 64), stringFormat).Width > 192)
                        {
                            fontSize -= 0.1f;
                        }
                        g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        g.DrawString(bossname, new Font(isaacHandwritten, fontSize), Brushes.Black, rectF, stringFormat);
                    }
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color bitColor = b.GetPixel(x, y);
                            b.SetPixel(x, y, Color.FromArgb(bitColor.A, 199, 178, 153));
                        }
                    }
                }
                b.Save(path, ImageFormat.Png);
            }
            tasks += 10;
        }

        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }

        /*    private void DisplayBitmap(Bitmap bmp)
            {
                //convert loaded drawing.image to windows.media.imagesource
                ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                image1.Source = bi;
            }*/

        private string GetIsaacPath()
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.OpenSubKey(@"Software\Valve\Steam");
            string isaacpath = @"\steamapps\common\The Binding of Isaac Rebirth\resources";

            string fullPath;

            string result;

            if (regKey != null)
            {
                string path = regKey.GetValue("SteamPath").ToString();
                if (Directory.Exists(Path.GetFullPath(path) + isaacpath))
                {
                    result = "The path was found successfully!";
                    fullPath = Path.GetFullPath(path) + isaacpath;
                    return fullPath;
                }
                else
                {
                    if (!File.Exists(path + @"\config\config.vdf"))
                    {
                        MessageBox.Show("Can't find Steam installation. Please enter the Isaac installation path manually.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                    using (StreamReader sr = File.OpenText(path + @"\config\config.vdf"))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s.ToLower().Contains("baseinstallfolder_"))
                            {
                                if (Directory.Exists(s.Split('"')[3] + isaacpath))
                                {
                                    result = "The path was found successfully!";
                                    fullPath = (s.Split('"')[3] + isaacpath);
                                    return fullPath;
                                }
                            }
                        }
                    }
                    MessageBox.Show("Isaac folder was not found. Please enter the path manually.", "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    return null;
                }
            }
            MessageBox.Show("Isaac folder was not found. Please enter the path manually.", "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            return null;
        }

        private void PreparePicture()
        {
            if (imgPicture != null)
            {
                bmpPicture = new Bitmap(imgPicture.Width, imgPicture.Height);
                iaPicture = new ImageAttributes();
            }
        }

        private void FinalizePicture()
        {
            iaPicture.SetColorMatrix(cmPicture);
            gfxPicture = Graphics.FromImage(bmpPicture);
            rctPicture = new Rectangle(0, 0, imgPicture.Width, imgPicture.Height);
            gfxPicture.DrawImage(imgPicture, rctPicture, 0, 0, imgPicture.Width, imgPicture.Height, GraphicsUnit.Pixel, iaPicture);
            RotatePicture();



            //using (ms = new MemoryStream())
            //{
            var bmp2 = new Bitmap(bmpPicture);
            //}

            using (ms2 = new MemoryStream())
            {
                FileToSave = @"resources\gfx\items\collectibles\" + Path.GetFileName(filenames[CurrentFile]);
                Debug.WriteLine(FileToSave);
                Debug.WriteLine(FileToSave);
                Debug.WriteLine(FileToSave);
                bmp2.Save(FileToSave, ImageFormat.Png);
            }
            tasks += 7;
        }

        private void FinalizeStagePicture(string path, bool newModifiers = true)
        {
            var saturation = 0.3f;

            if (newModifiers)
            {
                if (RandomInt(0, 1) == 1)
                {
                    rModifier = RandomInt(-110, -120) / 100f;
                }
                else
                {
                    rModifier = RandomInt(110, 120) / 100f;
                }
                if (RandomInt(0, 1) == 1)
                {
                    gModifier = RandomInt(-110, -120) / 100f;
                }
                else
                {
                    gModifier = RandomInt(110, 120) / 100f;
                }
                if (RandomInt(0, 1) == 1)
                {
                    bModifier = RandomInt(-110, -120) / 100f;
                }
                else
                {
                    bModifier = RandomInt(110, 120) / 100f;
                }
            }

            var baseSat = 1.0f - saturation;

            cmPicture[0, 0] = (baseSat * cmPicture[0, 0] + saturation);
            cmPicture[0, 1] = baseSat * cmPicture[0, 1] * rModifier;
            cmPicture[0, 2] = baseSat * cmPicture[0, 2] * rModifier;
            cmPicture[1, 0] = baseSat * cmPicture[1, 0] * gModifier;
            cmPicture[1, 1] = (baseSat * cmPicture[1, 1] + saturation);
            cmPicture[1, 2] = baseSat * cmPicture[1, 2] * gModifier;
            cmPicture[2, 0] = baseSat * cmPicture[2, 0] * bModifier;
            cmPicture[2, 1] = baseSat * cmPicture[2, 1] * bModifier;
            cmPicture[2, 2] = (baseSat * cmPicture[2, 2] + saturation);

            iaPicture.SetColorMatrix(cmPicture);
            gfxPicture = Graphics.FromImage(bmpPicture);
            rctPicture = new Rectangle(0, 0, imgPicture.Width, imgPicture.Height);
            gfxPicture.DrawImage(imgPicture, rctPicture, 0, 0, imgPicture.Width, imgPicture.Height, GraphicsUnit.Pixel, iaPicture);

            //using (ms = new MemoryStream())
            //{
            var bmp2 = new Bitmap(bmpPicture);
            //}
            Directory.CreateDirectory(@"resources\gfx\backdrop\");
            Directory.CreateDirectory(@"resources\gfx\grid\");
            using (ms2 = new MemoryStream())
            {
                File.Delete(path);
                FileToSave = path;
                Debug.WriteLine(FileToSave);
                bmp2.Save(FileToSave, ImageFormat.Png);
            }
            tasks += 7;
        }

        float rModifier;
        float gModifier;
        float bModifier;

        private void ColorWithSameMatrix(string path, string destpath)
        {
            var saturation = 0.3f;

            var baseSat = 1.0f - saturation;

            cmPicture[0, 0] = (baseSat * cmPicture[0, 0] + saturation);
            cmPicture[0, 1] = baseSat * cmPicture[0, 1] * rModifier;
            cmPicture[0, 2] = baseSat * cmPicture[0, 2] * rModifier;
            cmPicture[1, 0] = baseSat * cmPicture[1, 0] * gModifier;
            cmPicture[1, 1] = (baseSat * cmPicture[1, 1] + saturation);
            cmPicture[1, 2] = baseSat * cmPicture[1, 2] * gModifier;
            cmPicture[2, 0] = baseSat * cmPicture[2, 0] * bModifier;
            cmPicture[2, 1] = baseSat * cmPicture[2, 1] * bModifier;
            cmPicture[2, 2] = (baseSat * cmPicture[2, 2] + saturation);

            iaPicture.SetColorMatrix(cmPicture);
            gfxPicture = Graphics.FromImage(bmpPicture);
            rctPicture = new Rectangle(0, 0, imgPicture.Width, imgPicture.Height);
            gfxPicture.DrawImage(imgPicture, rctPicture, 0, 0, imgPicture.Width, imgPicture.Height, GraphicsUnit.Pixel, iaPicture);

            //using (ms = new MemoryStream())
            //{
            var bmp2 = new Bitmap(bmpPicture);
            //}
            Directory.CreateDirectory(Path.GetDirectoryName(destpath));
            FileToSave = destpath;

            bmp2.Save(FileToSave, ImageFormat.Png);
            tasks += 7;
        }

        private void FinalizeCharacterPicture(string path, bool newModifiers = true)
        {
            var saturation = 0.3f;

            if (newModifiers)
            {
                if (RandomInt(0, 1) == 1)
                {
                    rModifier = RandomInt(-110, -130) / 100f;
                }
                else
                {
                    rModifier = RandomInt(110, 130) / 100f;
                }
                if (RandomInt(0, 1) == 1)
                {
                    gModifier = RandomInt(-110, -130) / 100f;
                }
                else
                {
                    gModifier = RandomInt(110, 130) / 100f;
                }
                if (RandomInt(0, 1) == 1)
                {
                    bModifier = RandomInt(-110, -130) / 100f;
                }
                else
                {
                    bModifier = RandomInt(110, 130) / 100f;
                }
            }


            var baseSat = 1.0f - saturation;

            cmPicture[0, 0] = (baseSat * cmPicture[0, 0] + saturation);
            cmPicture[0, 1] = baseSat * cmPicture[0, 1] * rModifier;
            cmPicture[0, 2] = baseSat * cmPicture[0, 2] * rModifier;
            cmPicture[1, 0] = baseSat * cmPicture[1, 0] * gModifier;
            cmPicture[1, 1] = (baseSat * cmPicture[1, 1] + saturation);
            cmPicture[1, 2] = baseSat * cmPicture[1, 2] * gModifier;
            cmPicture[2, 0] = baseSat * cmPicture[2, 0] * bModifier;
            cmPicture[2, 1] = baseSat * cmPicture[2, 1] * bModifier;
            cmPicture[2, 2] = (baseSat * cmPicture[2, 2] + saturation);

            iaPicture.SetColorMatrix(cmPicture);
            gfxPicture = Graphics.FromImage(bmpPicture);
            rctPicture = new Rectangle(0, 0, imgPicture.Width, imgPicture.Height);
            gfxPicture.DrawImage(imgPicture, rctPicture, 0, 0, imgPicture.Width, imgPicture.Height, GraphicsUnit.Pixel, iaPicture);

            using (var bmp2 = new Bitmap(bmpPicture))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                FileToSave = path;

                Debug.WriteLine(FileToSave);
                bmp2.Save(FileToSave, ImageFormat.Png);
            }
            tasks += 7;
        }

        private void RotatePicture()
        {
            if (RandomInt(1, 100) > 50)
            {
                bmpPicture.RotateFlip(RotateFlipType.Rotate90FlipNone);
                if (UniversalList.ElementAtOrDefault(CurrentFile) != null)
                {
                    UniversalList[CurrentFile].Rotation1 = true;
                }
            }

            if (RandomInt(1, 100) > 50)
            {
                bmpPicture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                if (UniversalList.ElementAtOrDefault(CurrentFile) != null)
                {
                    UniversalList[CurrentFile].Rotation2 = true;
                }
            }

            if (RandomInt(1, 100) > 50)
            {
                bmpPicture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                if (UniversalList.ElementAtOrDefault(CurrentFile) != null)
                {
                    UniversalList[CurrentFile].Rotation3 = true;
                }
            }
            tasks += 5;
        }

        private void ProcessPicture()
        {
            PreparePicture();
            if (RandomInt(1, 100) < 90)
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] { RandomInt(0, 3), 0, 0, 0, 0},
                new float[] { 0, RandomInt(0, 3), 0, 0, 0},
                new float[] { 0, 0, RandomInt(0, 3), 0, 0},
                new float[] { 0, 0, 0, RandomInt(1, 3), 0},
                new float[] { 0, 0, 0, 0, RandomInt(1, 3)}


                });
            }
            else
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1},
                });
            }

            FinalizePicture();
            tasks += 8;
        }
        private void ProcessStagePicture(string path)
        {
            PreparePicture();
            if (RandomInt(1, 100) < 90)
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] { RandomInt(0, 3), 0, 0, 0, 0},
                new float[] { 0, RandomInt(0, 3), 0, 0, 0},
                new float[] { 0, 0, RandomInt(0, 3), 0, 0},
                new float[] { 0, 0, 0, RandomInt(1, 3), 0},
                new float[] { 0, 0, 0, 0, RandomInt(1, 3)}


                });
            }
            else
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1},
                });
            }
            tasks += 8;
            FinalizeStagePicture(path);
        }

        private void ProcessCharacterPicture(string path)
        {
            PreparePicture();
            if (RandomInt(1, 100) < 90)
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] { RandomInt(0, 3), 0, 0, 0, 0},
                new float[] { 0, RandomInt(0, 3), 0, 0, 0},
                new float[] { 0, 0, RandomInt(0, 3), 0, 0},
                new float[] { 0, 0, 0, RandomInt(1, 3), 0},
                new float[] { 0, 0, 0, 0, RandomInt(1, 3)}


                });
            }
            else
            {
                cmPicture = new ColorMatrix(new float[][]
                {
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0.5f, 0.5f, 0.5f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1},
                });
            }
            tasks += 8;
            FinalizeCharacterPicture(path);
        }

        private Dictionary<string, string> playerPortraitNames = new Dictionary<string, string>
        {
            ["character_001_isaac.png"] = "playerportrait_01_isaac.png",
            ["character_002_magdalene.png"] = "playerportrait_02_magdalene.png",
            ["character_003_cain.png"] = "playerportrait_03_cain.png",
            ["character_004_judas.png"] = "playerportrait_04_judas.png",
            ["character_005_eve.png"] = "playerportrait_05_eve.png",
            ["character_006_bluebaby.png"] = "playerportrait_06_bluebaby.png",
            ["character_007_samson.png"] = "playerportrait_07_samson.png",
            ["character_008_azazel.png"] = "playerportrait_08_azazel.png",
            ["character_009_eden.png"] = "playerportrait_09_eden.png",
            ["character_009_lazarus.png"] = "playerportrait_09_lazarus.png",
            ["character_010_lazarus2.png"] = "playerportrait_10_lazarus2.png",
            ["character_012_thelost.png"] = "playerportrait_12_thelost.png",
            ["character_013_blackjudas.png"] = "playerportrait_blackjudas.png",
            ["character_014_lilith.png"] = "playerportrait_13_lilith.png",
            ["character_015_keeper.png"] = "playerportrait_14_keeper.png"
        };

        private void RandomizeCharacterSprites()
        {
            Directory.CreateDirectory(@"resources\gfx\characters\costumes");
            Directory.CreateDirectory(@"resources\gfx\ui\boss");
            Directory.CreateDirectory(@"resources\gfx\ui\stage");
            foreach (var file in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\characters\costumes").GetFiles())
            {
                file.CopyTo(@"resources\gfx\characters\costumes\" + file.Name, true);
                tasks += 2;
            }
            foreach (var file in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\ui\boss").GetFiles())
            {
                if (file.Name.StartsWith("playerportrait_"))
                {
                    file.CopyTo(@"resources\gfx\ui\boss\" + file.Name, true);
                }
                tasks += 2;
            }
            foreach (var file in new DirectoryInfo(@"resources\packed\graphics_unpack\resources\gfx\ui\boss").GetFiles())
            {
                if (file.Name.StartsWith("playerportrait_"))
                {
                    file.CopyTo(@"resources\gfx\ui\boss\" + file.Name, true);
                }
                tasks += 2;
            }

            foreach (var file in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\ui\stage").GetFiles())
            {
                if (file.Name.StartsWith("playerportraitbig_"))
                {
                    file.CopyTo(@"resources\gfx\ui\stage\" + file.Name, true);
                }
                tasks += 2;
            }
            foreach (var file in new DirectoryInfo(@"resources\packed\graphics_unpack\resources\gfx\ui\stage").GetFiles())
            {
                if (file.Name.StartsWith("playerportraitbig_"))
                {
                    file.CopyTo(@"resources\gfx\ui\stage\" + file.Name, true);
                }
                tasks += 2;
            }

            foreach (var file in new DirectoryInfo(@"resources\gfx\characters\costumes").GetFiles(@"character_*.png"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                imgPicture = Image.FromStream(ms);
                ProcessCharacterPicture(file.FullName);
                if (playerPortraitNames.ContainsKey(file.Name))
                {
                    var playerPortrait = @"resources\gfx\ui\boss\" + playerPortraitNames[file.Name];
                    bytes = System.IO.File.ReadAllBytes(playerPortrait);
                    ms = new System.IO.MemoryStream(bytes);
                    imgPicture = Image.FromStream(ms);
                    bmpPicture = new Bitmap(imgPicture.Width, imgPicture.Height);
                    iaPicture = new ImageAttributes();
                    FinalizeCharacterPicture(playerPortrait, false);

                    if (playerPortraitNames[file.Name].Contains("lilith"))
                    {
                        playerPortrait = @"resources\gfx\ui\stage\" + "playerportraitbig_lilith.png";
                    }
                    else if (playerPortraitNames[file.Name].Contains("keeper"))
                    {
                        continue;
                    }
                    else
                    {
                        playerPortrait = @"resources\gfx\ui\stage\" + playerPortraitNames[file.Name].Replace("playerportrait_", "playerportraitbig_");
                    }
                    
                    
                    bytes = System.IO.File.ReadAllBytes(playerPortrait);
                    ms = new System.IO.MemoryStream(bytes);
                    imgPicture = Image.FromStream(ms);
                    bmpPicture = new Bitmap(imgPicture.Width, imgPicture.Height);
                    iaPicture = new ImageAttributes();
                    FinalizeCharacterPicture(playerPortrait, false);
                }
                tasks += 8;
            }
        }

        private void RandomizeCards()
        {
            if (!File.Exists(@"resources\pocketitems.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\pocketitems.xml", @"resources\pocketitems.xml");
            }
            XmlDocument PocketItemsXml = new XmlDocument();

            PocketItemsXml.Load(@"resources\pocketitems.xml");

            XmlNodeList cardNameList = PocketItemsXml.SelectNodes("//pocketitems//card/@name");
            List<string> cardNumberList = RandomCardName();
            for (int i = 1; i < 23; i++)
            {
                cardNameList[i].Value = cardNumberList[i - 1];
            }
            PocketItemsXml.Save(@"resources\pocketitems.xml");
            tasks += 8;
        }

        private void RandomizePills()
        {
            if (!File.Exists(@"resources\pocketitems.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\pocketitems.xml", @"resources\pocketitems.xml");
            }
            XmlDocument PocketItemsXml = new XmlDocument();
            PocketItemsXml.Load(@"resources\pocketitems.xml");

            XmlNodeList pillNameList = PocketItemsXml.SelectNodes("//pocketitems//pilleffect/@name");
            foreach (XmlAttribute name in pillNameList)
            {
                if (name != null)
                {
                    name.Value = RandomPill(); // Set to new value.
                }
            }
            PocketItemsXml.Save(@"resources\pocketitems.xml");
            tasks += 8;
        }

        public List<String> filenames = new List<String>();

        private void RandomizeItemSprites()
        {
            Directory.CreateDirectory(@"resources\gfx\items\collectibles");

            foreach (var file in Directory.GetFiles(@"resources\packed\graphics_unpack\resources\gfx\items\collectibles", "*.png"))
            {
                File.Copy(file, @"resources\gfx\items\collectibles\" + Path.GetFileName(file), true);
            }
            foreach (var file in Directory.GetFiles(@"resources\packed\afterbirth_unpack\resources\gfx\items\collectibles", "*.png"))
            {
                File.Copy(file, @"resources\gfx\items\collectibles\" + Path.GetFileName(file), true);
            }
            var files = Directory.GetFiles(@"resources\gfx\items\collectibles").ToList();
            var numOfFiles = files.Count;
            files.Shuffle();
            for (int i = 0; i < files.Count - 1; i++)
            {
                tasks += 1;
                Debug.WriteLine(i);
                filenames.Add(files[i]);
                Debug.WriteLine(filenames[i]);
            }

            // OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //  if(openFileDialog1.ShowDialog() == true)
            //  {
            filenames.Shuffle();
            for (int i = 0; i < files.Count - 1; i++)
            {
                if (Path.GetFileNameWithoutExtension(files[i]).Length > 16 && Path.GetFileNameWithoutExtension(filenames[i]).Length > 16)
                {
                    UniversalList.Add(new Picture(false, false, false, Path.GetFileNameWithoutExtension(filenames[i]).Substring(13, 3), Int32.Parse(Path.GetFileNameWithoutExtension(files[i]).Substring(13, 3))));
                }
                if (UniversalList.ElementAtOrDefault(i) != null)
                {
                    Debug.WriteLine(UniversalList[i].ItemID);
                    Debug.WriteLine(UniversalList[i].PictureID.ToString());
                }
                CurrentFile = i;
                Filename = files[i];

                Debug.WriteLine(Filename);
                byte[] bytes = System.IO.File.ReadAllBytes(Filename);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                imgPicture = Image.FromStream(ms);

                bmp = new Bitmap(imgPicture);
                ProcessPicture();
                tasks += 7;

            }
            //ImageSplit.SplitImage();

            Directory.CreateDirectory(@"resources\gfx\characters\costumes\");

            foreach (var file in new DirectoryInfo(@"resources\packed\graphics_unpack\resources\gfx\characters\costumes").GetFiles("costume_*.png"))
            {
                file.CopyTo(@"resources\gfx\characters\costumes\" + file.Name, true);
                tasks += 2;
            }

            foreach (var file in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\characters\costumes").GetFiles("costume_*.png"))
            {
                file.CopyTo(@"resources\gfx\characters\costumes\" + file.Name, true);
                tasks += 2;
            }

            if (!File.Exists(@"resources\costumes2.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\costumes2.xml", @"resources\costumes2.xml");
            }

            XmlDocument xml = new XmlDocument();

            xml.Load(@"resources\costumes2.xml");

            XmlNodeList costumes = xml.SelectNodes("//costumes//costume");

            List<string> nodes = new List<string>();

            int v = 0;

            for (int i = 34; i < costumes.Count - 34; i++)
            {
                nodes.Add(costumes[i].Attributes["id"].Value);
            }

            nodes.Shuffle();

            for (int i = 34; i < costumes.Count - 34; i++)
            {
                costumes[i].Attributes["id"].Value = nodes[i - 34];
            }

            xml.Save(@"resources\costumes2.xml");

            foreach (var file in new DirectoryInfo(@"resources\gfx\characters\costumes").GetFiles())
            {
                byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                imgPicture = Image.FromStream(ms);
                ProcessCharacterPicture(file.FullName);
                tasks += 8;
            }
        }

        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                if (dir.Name != "packed")
                {
                    CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
                }
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        public List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private void RandomizeBossNames(int mode)
        {
            if (!Directory.Exists(@"resources\gfx\ui\boss"))
            {
                Directory.CreateDirectory(@"resources\gfx\ui\boss");
            }
            foreach (var file in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\ui\boss").GetFiles())
            {
                if (file.Name.StartsWith("bossname_"))
                {
                    file.CopyTo(@"resources\gfx\ui\boss\" + file.Name, true);
                }
            }

            foreach (var file in Directory.GetFiles(@"resources\gfx\ui\boss"))
            {
                if (Path.GetFileName(file).StartsWith("bossname_"))
                {
                    var bossname = "";
                    if (mode == 1)
                    {
                        bossname = RandomBossNameConsistent(jokeWords.IsChecked ?? false, edgyWords.IsChecked ?? false, originalWords.IsChecked ?? false);
                    }
                    else
                    {
                        bossname = RandomBossNameRandom();
                    }

                    CreateBossVsName(bossname, file);
                    tasks += 4;
                }
            }
        }

        private List<FileInfo> SwapFiles(List<FileInfo> list)
        {
            List<FileInfo> originalList = new List<FileInfo>();
            for (int i = 0; i < list.Count; i++)
            {
                var file = list[RandomInt(0, list.Count - 1)];
                while (originalList.Contains(file))
                {
                    file = list[RandomInt(0, list.Count - 1)];
                }
                originalList.Add(file);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != originalList[i])
                {
                    list[i].CopyTo(originalList[i].FullName, true);
                }
            }
            return originalList;
            tasks += 15;
        }

        private void RandomizeMusic()
        {
            Directory.CreateDirectory(@"resources\music\afterbirth");
            Directory.CreateDirectory(@"resources\music\fight ogg");
            Directory.CreateDirectory(@"resources\music\jingle ogg");
            var loop = new List<FileInfo>();
            var musicIntro = new List<FileInfo>();
            var layerIntro = new List<FileInfo>();
            var layer = new List<FileInfo>();
            var jingle = new List<FileInfo>();
            var fight = new List<FileInfo>();

            CopyFilesRecursively(new DirectoryInfo(@"resources\packed\music_unpack\resources\music"), new DirectoryInfo(@"resources\music"));
            CopyFilesRecursively(new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\music\afterbirth"), new DirectoryInfo(@"resources\music\afterbirth"));
            foreach (var file in new DirectoryInfo(@"resources\music").GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                if (file.Name.Contains("silence") || file.Name.Contains("silent"))
                {
                    continue;
                }
                if (file.Name.Contains("layer intro"))
                {
                    layerIntro.Add(file);
                }
                else if (file.Name.Contains("intro") && !file.Name.Contains("silent"))
                {
                    musicIntro.Add(file);
                }
                else if (file.Name.Contains("loop"))
                {
                    loop.Add(file);
                }
                else if (file.Name.Contains("layer"))
                {
                    layer.Add(file);
                }
                else
                {
                    loop.Add(file);
                }
            }
            tasks += 20;

            foreach (var file in new DirectoryInfo(@"resources\music\afterbirth").GetFiles())
            {
                if (file.Name.Contains("silence") || file.Name.Contains("silent"))
                {
                    continue;
                }
                if (file.Name.Contains("layer intro"))
                {
                    layerIntro.Add(file);
                }
                else if (file.Name.Contains("intro") && !file.Name.Contains("silent"))
                {
                    musicIntro.Add(file);
                }
                else if (file.Name.Contains("loop"))
                {
                    loop.Add(file);
                }
                else if (file.Name.Contains("layer"))
                {
                    layer.Add(file);
                }
            }
            foreach (var file in new DirectoryInfo(@"resources\music\fight ogg").GetFiles())
            {
                fight.Add(file);
            }
            foreach (var file in new DirectoryInfo(@"resources\music\jingle ogg").GetFiles())
            {
                jingle.Add(file);
            }
            tasks += 20;

            loop = SwapFiles(loop);
            musicIntro = SwapFiles(musicIntro);
            layerIntro = SwapFiles(layerIntro);
            layer = SwapFiles(layer);
            jingle = SwapFiles(jingle);
            fight = SwapFiles(fight);
        
            
        }

        private void RandomizeItemTypes(int mode)
        {
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            var xml = XDocument.Load(@"resources\items.xml");
            if (mode == 1)
            {
                foreach (var child in xml.Root.Elements().Where(child => child.Name != "trinket"))
                {
                    var choice = RandomInt(0, 3);
                    if (choice == 0) child.Name = "passive";
                    else if (choice == 1) child.Name = "active";
                    else if (choice == 2) child.Name = "familiar";

                }
                xml.Save(@"resources\items.xml");
            }
            else
            {
                foreach (var child in xml.Root.Elements().Where(child => child.Name != "trinket"))
                {
                    tasks += 1;
                    if (RandomInt(0, 50) == 50)
                    {
                        var choice = RandomInt(0, 3);
                        if (choice == 0) child.Name = "passive";
                        else if (choice == 1) child.Name = "active";
                        else if (choice == 2) child.Name = "familiar";
                    }
                }
                xml.Save(@"resources\items.xml");
            }
        }

        private void RandomizeSounds(int numberOfChunks)
        {
            string sfxPath = @"sfx\";
            string afterbirthSfx = @"resources\packed\afterbirth_unpack\resources\sfx\";
            string rebirthSfx = @"resources\packed\sfx_unpack\resources\sfx\";
            if (!Directory.Exists(sfxPath))
            {
                Directory.CreateDirectory(sfxPath);
            }

            if (Directory.Exists(afterbirthSfx))
            {
                CopyFilesRecursively(new DirectoryInfo(afterbirthSfx), new DirectoryInfo(sfxPath));
            }

            foreach (var file in new DirectoryInfo(rebirthSfx).GetFiles())
            {
                File.Copy(file.FullName, sfxPath + file.Name, true);
            }

            var d = new DirectoryInfo(sfxPath);
            var durations = new Dictionary<string, double>();
            foreach (var file in d.GetFiles("*.wav", SearchOption.AllDirectories))
            {
                using (var wf = new WaveFileReader(file.FullName))
                {
                    durations.Add(file.FullName, wf.TotalTime.TotalSeconds);
                }
            }
            var list = durations.OrderBy(x => x.Value).Select(x => x.Key).ToList();

            var chunks = ChunkBy(list, numberOfChunks);
            var shuffledChunks = new List<List<string>>();
            foreach (var i in chunks)
            {
                shuffledChunks.Add(RShuffle(i));
            }

            foreach (var x in shuffledChunks)
            {
                tasks += 3;
                string temp = "";
                string tempPath = @"sfx\";
                foreach (var y in x)
                {
                    if (temp != "")
                    {
                        File.Copy(y, temp, true);
                        temp = y;
                    }
                    else
                    {
                        temp = y;
                        File.Copy(y, tempPath + "temp.wav", true);
                    }

                }
                File.Copy(tempPath + "temp.wav", temp, true);
            }

            if (File.Exists(sfxPath + "temp.wav"))
            {
                File.Delete(sfxPath + "temp.wav");
            }

            if (!Directory.Exists(@"resources\sfx\"))
            {
                Directory.CreateDirectory(@"resources\sfx\");
            }
            string resourceSfx = @"resources\sfx\";

            CopyFilesRecursively(new DirectoryInfo(sfxPath), new DirectoryInfo(resourceSfx));

            foreach (var file in Directory.GetFiles(sfxPath))
            {
                File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(sfxPath))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    File.Delete(file);
                }
                Directory.Delete(dir, true);
            }
            Directory.Delete(sfxPath, true);
        }

        private void RandomizeItemPools()
        {
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(@"resources\itempools.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\itempools.xml", @"resources\itempools.xml");
            }

            xml.Load(@"resources\itempools.xml");

            XmlNodeList pools = xml.SelectNodes("//ItemPools//Pool");
            XmlNodeList items = xml.SelectNodes("//ItemPools//Pool//Item");

            foreach (XmlNode i in items)
            {
                var definiteNode = RandomInt(0, 26);
                while (definiteNode > 16 && definiteNode < 25)
                {
                    definiteNode = RandomInt(0, 24);
                }
                var definiteGreedNode = RandomInt(16, 24);
                for (int x = 0; x < pools.Count; x++)
                {
                    if (RandomInt(1, 20) == 5 || (x == definiteNode || x == definiteGreedNode))
                    {
                        XmlNode newNode = i.Clone();
                        pools[x].AppendChild(newNode);
                    }
                }
                i.ParentNode.RemoveChild(i);
            }
            tasks += 30;
            xml.Save(@"resources\itempools.xml");
        }

        private void RandomizeItems(int mode)
        {
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument ItemsXml = new XmlDocument();
            ItemsXml.Load(@"resources\items.xml");

            XmlNodeList nameList = ItemsXml.SelectNodes("//items//passive/@name");
            foreach (XmlAttribute name in nameList)
            {
                if (name != null)
                {
                    if (mode == 1)
                    {
                        name.Value = RandomItemNameConsistent();
                        if (name.Value.Contains("s's"))
                        {
                            name.Value = name.Value.Replace("s's", "s'");
                        }
                        if (name.Value.EndsWith("ss"))
                        {
                            name.Value = name.Value.Substring(0, name.Value.Length - 1);
                        }
                    }
                    else
                    {
                        name.Value = RandomItemName(); // Set to new value.
                    }

                }
            }
            tasks += 30;
            XmlNodeList nameList2 = ItemsXml.SelectNodes("//items//active/@name");
            foreach (XmlAttribute name in nameList2)
            {
                if (name != null)
                {
                    if (mode == 1)
                    {
                        name.Value = RandomItemNameConsistent();
                    }
                    else
                    {
                        name.Value = RandomItemName(); // Set to new value.
                    }
                }
            }
            tasks += 30;
            XmlNodeList nameList3 = ItemsXml.SelectNodes("//items//familiar/@name");
            foreach (XmlAttribute name in nameList3)
            {
                if (name != null)
                {
                    if (mode == 1)
                    {
                        name.Value = RandomFamiliarNameConsistent();
                    }
                    else
                    {
                        name.Value = RandomFamiliarName(); // Set to new value.
                    }
                }
            }
            tasks += 30;
            XmlNodeList descList = ItemsXml.SelectNodes("//items//passive/@description");
            foreach (XmlAttribute desc in descList)
            {
                if (desc != null)
                {
                    if (mode == 1)
                    {
                        desc.Value = RandomItemDescriptionConsistent();
                    }
                    else
                    {
                        desc.Value = RandomItemDescription();
                    }
                }
            }
            tasks += 30;
            XmlNodeList descList2 = ItemsXml.SelectNodes("//items//active/@description");
            foreach (XmlAttribute desc in descList2)
            {
                if (desc != null)
                {
                    if (mode == 1)
                    {
                        desc.Value = RandomActiveDescriptionConsistent();
                    }
                    else
                    {
                        desc.Value = RandomActiveDescription();
                    }
                }
            }
            tasks += 30;
            XmlNodeList descList3 = ItemsXml.SelectNodes("//items//familiar/@description");
            foreach (XmlAttribute desc in descList3)
            {
                if (desc != null)
                {
                    if (mode == 1)
                    {
                        desc.Value = RandomFamiliarDescriptionConsistent();
                    }
                    else
                    {
                        desc.Value = RandomFamiliarDescription();
                    }
                }
            }
            tasks += 30;
            ItemsXml.Save(@"resources\items.xml");
        }

        public List<String> stageFilenames = new List<String>();
        public List<string> floorFilenames = new List<String>();

        private void RandomizeCharacterNames()
        {
            Directory.CreateDirectory(@"resources\gfx\ui\main menu");
            File.Copy(@"resources\packed\afterbirth_unpack\resources\gfx\ui\main menu\charactermenu.png", @"resources\gfx\ui\main menu\charactermenu.png", true);
            if (!File.Exists(@"resources\players.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\players.xml", @"resources\players.xml");
            }
            Directory.CreateDirectory(@"resources\gfx\ui\boss");
            XmlDocument playerXml = new XmlDocument();
            playerXml.Load(@"resources\players.xml");

            XmlNodeList players = playerXml.SelectNodes("//players//player");
            foreach (var dir in new DirectoryInfo(@"resources\packed\afterbirth_unpack\resources\gfx\ui\boss").GetFiles())
            {
                if (dir.FullName.StartsWith("playername_"))
                {
                    dir.CopyTo(@"resources\gfx\ui\boss\" + dir.Name);
                }
            }
            for (int i = 0; i < players.Count; i++)
            {
                XmlNode node = players[i];
                var name = RandomCharacterName();
                while (name.Count() > 16 || names.Contains(name))
                {
                    name = RandomCharacterName();
                }
                node.Attributes["name"].Value = name;

                CreateBossVsName(name, @"resources\gfx\ui\boss\" + node.Attributes["nameimage"].Value.ToLower());
                if (i > 12)
                {
                    CreateCharacterMenuName(name, i - 2);
                }
                else
                {
                    CreateCharacterMenuName(name, i);
                }
                names.Add(name);
                tasks += 8;
            }

            playerXml.Save(@"resources\players.xml");
        }

        Dictionary<string, string[]> additionalStageNames = new Dictionary<string, string[]>
        {
            ["0a_library.png"] = new string[] { "0a_library_nfloor.png", "!door_13_librarydoor.png" },
            ["0b_shop.png"] = new string[] { "0b_shop_nfloor.png" },
            ["0e_diceroom.png"] = new string[] { "0e_diceroom_nfloor.png", "!door_25_diceroomdoor.png" },
            ["0f_secretroom.png"] = new string[] { "0f_secretroom_nfloor.png", "!rocks_secretroom.png" },
            ["01_basement.png"] = new string[] { "01_basement_nfloor.png", "01_lbasementfloor.png", "!door_01_normaldoor.png", "!door_08_holeinwall.png", "!grid_bridge.png", "!grid_pit.png", "!rocks_basement.png", },
            ["02_cellar.png"] = new string[] { "02_cellar_nfloor.png", "02_lcellarfloor.png", "!door_12_cellardoor.png", "!rocks_cellar.png" },
            ["03_caves.png"] = new string[] { "03_caves_nfloor.png", "03_lcavesfloor.png", "!door_08_holeinwall_caves.png", "!grid_pit_water.png", "!rocks_caves.png" },
            ["04_catacombs.png"] = new string[] { "04_catacombs_nfloor.png", "04_lcatacombsfloor.png", "!grid_bridge_catacombs.png", "!grid_pit_catacombs.png", "!grid_pit_water_catacombs.png", "!rocks_catacombs.png" },
            ["05_depths.png"] = new string[] { "05_depths_nfloor.png", "05_ldepthsfloor.png", "!door_08_holeinwall_depths.png", "!door_14_depthsdoor.png", "!grid_bridge_depths.png", "!grid_pit_depths.png", "!rocks_depths.png" },
            ["06_necropolis.png"] = new string[] { "06_necropolis_nfloor.png", "06_lnecropolisfloor.png", "!grid_bridge_necropolis.png", "!grid_pit_necropolis.png" },
            ["07_the womb.png"] = new string[] { "07_lthe wombfloor.png", "07_the womb_nfloor.png", "!door_08_holeinwall_womb.png", "!door_11_wombhole.png", "!door_29_doortobluewomb.png", "!grid_bridge_womb.png", "!grid_pit_blood_womb.png", "!grid_pit_womb.png", "!rocks_womb.png" },
            ["08_utero.png"] = new string[] { "08_luterofloor.png", "08_utero_nfloor.png" },
            ["09_sheol.png"] = new string[] { "09_sheol_nfloor.png", "09_lsheolfloor.png", "!door_19_sheoldoor.png", "!rocks_sheol.png" },
            ["10_cathedral.png"] = new string[] { "10_cathedral_nfloor.png", "10_lcathedralfloor.png", "!door_08_holeinwall_cathedral.png", "!door_22_cathedraldoor.png", "!grid_bridge_cathedral.png", "!grid_pit_cathedral.png", "!rocks_cathedral.png" },
            ["11_chest.png"] = new string[] { "11_chest_nfloor.png", "11_lchestfloor.png", "!door_23_chestdoor.png" },
            ["12_darkroom.png"] = new string[] { "12_darkroom_nfloor.png", "12_ldarkroomfloor.png", "!door_08_holeinwall_darkroom.png", "!door_21_darkroomdoor.png", "!grid_pit_darkroom.png" },
            ["13_the burning basement.png"] = new string[] { "13_the burning basement_nfloor.png", "13_lthe burning basementfloor.png", "!door_01_burningbasement.png", "!rocks_burningbasement.png" },
            ["14_the drowned caves.png"] = new string[] { "14_lthe drowned cavesfloor.png", "14_the drowned caves_nfloor.png", "!door_27_drownedcaves.png", "!grid_bridge_drownedcaves.png", "!grid_pit_water_drownedcaves.png", "!rocks_drownedcaves.png" },
            ["15_the dank depths.png"] = new string[] { "15_the dank depths_nfloor.png", "15_lthe dank depthsfloor.png", "!grid_bridge_dankdepths.png", "!grid_pit_dankdepths.png", "!grid_pit_water_dankdepths.png" },
            ["16_the scarred womb.png"] = new string[] { "16_lthe scarred wombfloor.png", "16_the scarred womb_nfloor.png", "!door_28_scarredroomdoor.png", "!grid_bridge_scarredwomb.png", "!grid_pit_acid_womb.png", "!grid_pit_blood_scarredwomb.png", "!rocks_scarredwomb.png" },
            ["18_blue_womb.png"] = new string[] { "!rocks_bluewomb.png" }
        };
        
        private void RandomizeStages()
        {
            if (!File.Exists(@"resources\stages.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\stages.xml", @"resources\stages.xml");
            }
            if (!File.Exists(@"resources\backdrops.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\backdrops.xml", @"resources\backdrops.xml");
            }
            Directory.CreateDirectory(@"resources\gfx\grid");
            Directory.CreateDirectory(@"resources\gfx\backdrop");
            XmlDocument StagesXml = new XmlDocument();

            StagesXml.Load(@"resources\stages.xml");

            XmlNodeList stages = StagesXml.SelectNodes("//stages//stage");
            XmlNodeList stageNameList = StagesXml.SelectNodes("//stages//stage/@name");
            foreach (XmlAttribute name in stageNameList)
            {
                name.Value = RandomStageName();
            }

            StagesXml.Save(@"resources\stages.xml");

            XmlDocument xml = new XmlDocument();

            xml.Load(@"resources\backdrops.xml");

            XmlNodeList backdrops = xml.SelectNodes("//backdrops//backdrop");

            List<string> nodes = new List<string>();

            for (int i = 0; i < backdrops.Count - 3; i++)
            {
                nodes.Add(i.ToString());
            }

            nodes.Shuffle();

            for (int i = 0; i < backdrops.Count - 3; i++)
            {
                backdrops[i].Attributes["id"].Value = nodes[i];
            }

            xml.Save(@"resources\backdrops.xml");

            var stageFiles = Directory.GetFiles(@"resources\packed\afterbirth_unpack\resources\gfx\backdrop", "*.png").ToList();

            foreach (var file in Directory.GetFiles(@"resources\packed\graphics_unpack\resources\gfx\grid"))
            {
                File.Copy(file, @"resources\gfx\grid\" + Path.GetFileName(file), true);
            }

            foreach (var file in Directory.GetFiles(@"resources\packed\afterbirth_unpack\resources\gfx\grid"))
            {
                File.Copy(file, @"resources\gfx\grid\" + Path.GetFileName(file), true);
            }

            foreach (var file in Directory.GetFiles(@"resources\packed\afterbirth_unpack\resources\gfx\backdrop"))
            {
                File.Copy(file, @"resources\gfx\backdrop\" + Path.GetFileName(file), true);
            }

            foreach (string s in stageFiles.ToArray())
            {
                if (s.Contains("floor"))
                {
                    stageFiles.Remove(s);
                }
                else if (s.Contains("water"))
                {
                    stageFiles.Remove(s);
                }
                if (s.Contains("shading_"))
                {
                    stageFiles.Remove(s);
                }
                if (s.Contains(".anm2"))
                {
                    stageFiles.Remove(s);
                }
                if (s.Contains("overlay"))
                {
                    stageFiles.Remove(s);
                }
                if (s.Contains("clouds"))
                {
                    stageFiles.Remove(s);
                }
                if (s.Contains("mega_satan_land"))
                {
                    stageFiles.Remove(s);
                }
            }
            foreach (string s in stageFiles)
            {
                stageFilenames.Add(s);
            }

            for (int i = 0; i < stageFiles.Count - 1; i++)
            {
                CurrentFile = i;

                byte[] bytes = File.ReadAllBytes(stageFiles[i]);
                MemoryStream ms = new MemoryStream(bytes);
                imgPicture = Image.FromStream(ms);
                bmp = new Bitmap(imgPicture);
                ProcessStagePicture(stageFiles[i]);
                tasks += 15;
                
                if (additionalStageNames.ContainsKey(Path.GetFileName(stageFiles[i])))
                {
                    foreach (var x in additionalStageNames[Path.GetFileName(stageFiles[i])])
                    {
                        if (x.StartsWith("!"))
                        {
                            var v = x.Replace("!", "");
                            bytes = System.IO.File.ReadAllBytes(@"resources\gfx\grid\" + v);
                            ms = new System.IO.MemoryStream(bytes);
                            imgPicture = Image.FromStream(ms);
                            bmpPicture = new Bitmap(imgPicture);
                            iaPicture = new ImageAttributes();

                            FinalizeStagePicture(@"resources\gfx\grid\" + v, false);
                        }
                        else
                        {
                            bytes = System.IO.File.ReadAllBytes(@"resources\gfx\backdrop\" + x);
                            ms = new System.IO.MemoryStream(bytes);
                            imgPicture = Image.FromStream(ms);
                            bmpPicture = new Bitmap(imgPicture);
                            iaPicture = new ImageAttributes();
                            
                            FinalizeStagePicture(@"resources\gfx\backdrop\" + x, false);
                        }
                    }
                }
            }
        }

        private void RandomizeRarities(int mode)
        {
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(@"resources\itempools.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\itempools.xml", @"resources\itempools.xml");
            }
            xml.Load(@"resources\itempools.xml");

            XmlNodeList items = xml.SelectNodes("//ItemPools//Pool//Item");

            foreach (XmlNode i in items)
            {
                if (mode == 1)
                {
                    i.Attributes["Weight"].InnerText = (RandomInt(6, 16) / 10f).ToString();
                }
                else
                {
                    i.Attributes["Weight"].InnerText = (RandomInt(0, 200) / 10f).ToString();
                }
            }
            xml.Save(@"resources\itempools.xml");
        }

        private void RemoveSpecialFlag()
        {
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");
            foreach (XmlElement i in items)
            {
                i.SetAttribute("special", "false");
            }
            tasks += 15;
            xml.Save(@"resources\items.xml");
        }

        const int healthUp = 63;
        const int maxHealthUp = 68;
        const int soulHeartsUp = 44;
        const int blackHeartsUp = 20;
        const int fullHpUp = 6;
        const int keys = 3;
        const int bombs = 15;
        const int coins = 5;

        private void RandomizeKeyGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["keys"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["keys"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("keys", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeBombGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["bombs"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["bombs"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("bombs", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeCoinGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["coins"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["coins"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("coins", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeMaxHpGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["maxhearts"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute / 2; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["maxhearts"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 2).ToString();
                }
                else
                {
                    node.SetAttribute("maxhearts", "2");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeCurses()
        {
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(@"resources\curses.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\curses.xml", @"resources\curses.xml");
            }
            xml.Load(@"resources\curses.xml");

            XmlNodeList curses = xml.SelectNodes("//curses//curse");
            foreach (XmlElement i in curses)
            {
                i.SetAttribute("name", RandomCurseName());
            }
            xml.Save(@"resources\curses.xml");
            tasks += 8;
        }

        private void RandomizeHpGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["hearts"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["hearts"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("hearts", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeBlackGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["blackhearts"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["blackhearts"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("blackhearts", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        int[] activeIds = { 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 44, 45, 47, 49, 56, 58, 65, 66, 77, 78, 83, 84, 85, 86, 93, 97, 102, 105, 107, 111, 123, 124, 126, 127, 130, 133, 135, 136, 137, 145, 146, 147, 158, 160, 164, 166, 171, 175, 177, 181, 186, 192, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 323, 324, 325, 326, 338, 347, 348, 349, 351, 352, 357, 382, 383, 386, 396, 406, 419, 421, 422, 427, 434, 437, 439, 441 };
        int[] familiarIds = { 8, 10, 11, 57, 67, 73, 81, 88, 94, 95, 96, 98, 99, 100, 112, 113, 128, 131, 144, 155, 163, 167, 170, 172, 174, 178, 187, 188, 206, 207, 238, 239, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 318, 319, 320, 321, 322, 360, 361, 362, 363, 364, 365, 372, 384, 385, 387, 388, 389, 390, 403, 404, 405, 417, 426, 430, 431, 435, 436 };
        int[] passiveIds = { 1, 2, 3, 4, 5, 6, 7, 9, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 46, 48, 50, 51, 52, 53, 54, 55, 60, 62, 63, 64, 68, 69, 70, 71, 72, 74, 75, 76, 79, 80, 82, 87, 89, 90, 91, 92, 101, 103, 104, 106, 108, 109, 110, 114, 115, 116, 117, 118, 119, 120, 121, 122, 125, 129, 132, 134, 138, 139, 140, 141, 142, 143, 148, 149, 150, 151, 152, 153, 154, 156, 157, 159, 161, 162, 165, 168, 169, 173, 176, 179, 180, 182, 183, 184, 185, 189, 190, 191, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 236, 237, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 339, 340, 341, 342, 343, 344, 345, 346, 350, 353, 354, 355, 356, 358, 359, 366, 367, 368, 369, 370, 371, 373, 374, 375, 376, 377, 378, 379, 380, 381, 391, 392, 393, 394, 395, 397, 398, 399, 400, 401, 402, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 418, 420, 423, 424, 425, 428, 429, 432, 433, 438, 440 };

        private void RandomizeCharacterItems(bool ensureActive, int specificAmount = 0)
        {
            if (!File.Exists(@"resources\players.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\players.xml", @"resources\players.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\players.xml");

            XmlNodeList players = xml.SelectNodes("//players//player");

            foreach (XmlElement player in players)
            {
                player.SetAttribute("items", "");
                bool givenActive = false;
                List<int> idsToGive = new List<int>();
                int numToGive = 0;
                if (specificAmount != 0)
                {
                    numToGive = specificAmount;
                    for (int i = 0; i < numToGive; i++)
                    {
                        var itemType = RandomInt(0, 2);
                        if (!givenActive && itemType == 0 || (ensureActive && !givenActive))
                        {
                            idsToGive.Add(activeIds[RandomInt(0, activeIds.Length - 1)]);
                            givenActive = true;
                        }
                        else if (itemType == 1)
                        {
                            idsToGive.Add(familiarIds[RandomInt(0, familiarIds.Length - 1)]);
                        }
                        else
                        {
                            idsToGive.Add(passiveIds[RandomInt(0, passiveIds.Length - 1)]);
                        }
                    }
                    player.SetAttribute("items", string.Join(", ", idsToGive));
                }
                else
                {
                    numToGive = RandomInt(1, 3);
                    for (int i = 0; i < numToGive; i++)
                    {
                        var itemType = RandomInt(0, 2);
                        if (!givenActive && itemType == 0 || (ensureActive && !givenActive))
                        {
                            idsToGive.Add(activeIds[RandomInt(0, activeIds.Length - 1)]);
                            givenActive = true;
                        }
                        else if (itemType == 1)
                        {
                            idsToGive.Add(familiarIds[RandomInt(0, familiarIds.Length - 1)]);
                        }
                        else
                        {
                            idsToGive.Add(passiveIds[RandomInt(0, passiveIds.Length - 1)]);
                        }
                    }
                    player.SetAttribute("items", string.Join(", ", idsToGive));
                }
            }
            xml.Save(@"resources\players.xml");
        }

        private void RandomizeCharacterPickups(bool coinsValuedLess, int specificAmount = 0, int totalAmount = 0)
        {
            if (!File.Exists(@"resources\players.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\players.xml", @"resources\players.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\players.xml");

            XmlNodeList players = xml.SelectNodes("//players//player");

            foreach (XmlElement i in players)
            {
                i.SetAttribute("bombs", "0");
                i.SetAttribute("keys", "0");
                i.SetAttribute("coins", "0");
            }

            if (totalAmount > 0)
            {
                for (int i = 0; i < totalAmount; i++)
                {
                    XmlElement element = players[RandomInt(0, players.Count - 1)] as XmlElement;
                    var pickup = RandomInt(0, 2);
                    if (pickup == 0)
                    {
                        if (element.Attributes["bombs"] == null)
                        {
                            element.SetAttribute("bombs", "1");
                        }
                        else
                        {
                            element.SetAttribute("bombs", (int.Parse(element.Attributes["bombs"].Value) + 1).ToString());
                        }
                    }
                    else if (pickup == 1)
                    {
                        if (element.Attributes["keys"] == null)
                        {
                            element.SetAttribute("keys", "1");
                        }
                        else
                        {
                            element.SetAttribute("keys", (int.Parse(element.Attributes["keys"].Value) + 1).ToString());
                        }
                    }
                    else
                    {
                        if (coinsValuedLess)
                        {
                            if (element.Attributes["coins"] == null)
                            {
                                element.SetAttribute("coins", "3");
                            }
                            else
                            {
                                element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                            }
                        }
                        else
                        {
                            if (element.Attributes["coins"] == null)
                            {
                                element.SetAttribute("coins", "1");
                            }
                            else
                            {
                                element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                            }
                        }

                    }
                }
            }
            else if (specificAmount > 0)
            {
                foreach (XmlElement element in players)
                {
                    for (int i = 0; i < specificAmount; i++)
                    {
                        var pickup = RandomInt(0, 3);
                        
                        if (pickup == 0)
                        {
                            if (element.Attributes["bombs"] == null)
                            {
                                element.SetAttribute("bombs", "1");
                            }
                            else
                            {
                                element.SetAttribute("bombs", (int.Parse(element.Attributes["bombs"].Value) + 1).ToString());
                            }
                        }
                        else if (pickup == 1)
                        {
                            if (element.Attributes["keys"] == null)
                            {
                                element.SetAttribute("keys", "1");
                            }
                            else
                            {
                                element.SetAttribute("keys", (int.Parse(element.Attributes["keys"].Value) + 1).ToString());
                            }
                        }
                        else
                        {
                            if (coinsValuedLess)
                            {
                                if (element.Attributes["coins"] == null)
                                {
                                    element.SetAttribute("coins", "3");
                                }
                                else
                                {
                                    element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                                }
                            }
                            else
                            {
                                if (element.Attributes["coins"] == null)
                                {
                                    element.SetAttribute("coins", "1");
                                }
                                else
                                {
                                    element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (XmlElement element in players)
                {
                    for (int i = 0; i < RandomInt(5, 15); i++)
                    {
                        var pickup = RandomInt(0, 3);

                        if (pickup == 0)
                        {
                            if (element.Attributes["bombs"] == null)
                            {
                                element.SetAttribute("bombs", "1");
                            }
                            else
                            {
                                element.SetAttribute("bombs", (int.Parse(element.Attributes["bombs"].Value) + 1).ToString());
                            }
                        }
                        else if (pickup == 1)
                        {
                            if (element.Attributes["keys"] == null)
                            {
                                element.SetAttribute("keys", "1");
                            }
                            else
                            {
                                element.SetAttribute("keys", (int.Parse(element.Attributes["keys"].Value) + 1).ToString());
                            }
                        }
                        else
                        {
                            if (coinsValuedLess)
                            {
                                if (element.Attributes["coins"] == null)
                                {
                                    element.SetAttribute("coins", "3");
                                }
                                else
                                {
                                    element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                                }
                            }
                            else
                            {
                                if (element.Attributes["coins"] == null)
                                {
                                    element.SetAttribute("coins", "1");
                                }
                                else
                                {
                                    element.SetAttribute("coins", (int.Parse(element.Attributes["coins"].Value) + 1).ToString());
                                }
                            }
                        }
                    }
                }
            }
            xml.Save(@"resources\players.xml");
        }

        private void RandomizeSoulGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["soulhearts"];
                if (attr != null)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["soulhearts"];
                if (attr != null)
                {
                    attr.Value = (int.Parse(attr.Value) + 1).ToString();
                }
                else
                {
                    node.SetAttribute("soulhearts", "1");
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeFullGains(float modifier)
        {
            int toDistribute = (int)Math.Round(healthUp * modifier);
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList items = xml.SelectNodes("//items//*");

            foreach (XmlNode i in items)
            {
                var attr = i.Attributes["hearts"];
                if (attr != null && int.Parse(attr.Value) > 24)
                {
                    attr.Value = "0";
                }
            }

            for (int i = 0; i < toDistribute; i++)
            {
                var index = RandomInt(0, items.Count - 1);
                var node = items[index] as XmlElement;
                var attr = items[index].Attributes["hearts"];
                if (attr != null)
                {
                    attr.Value = "99";
                }
                else
                {
                    node.SetAttribute("hearts", "99");
                }
            }

            xml.Save(@"resources\items.xml");
            tasks += 15;
        }

        private void RandomizeActiveCharges(int mode)
        {
            if (!File.Exists(@"resources\items.xml"))
            {
                File.Copy(@"resources\packed\afterbirth_unpack\resources\items.xml", @"resources\items.xml");
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(@"resources\items.xml");

            XmlNodeList actives = xml.SelectNodes("//items//active");

            foreach (XmlElement i in actives)
            {
                var charge = i.Attributes["maxcharges"];
                if (charge == null)
                {
                    if (mode == 1)
                    {
                        i.SetAttribute("maxcharges", RandomInt(1, 6).ToString());
                    }
                    else
                    {
                        i.SetAttribute("maxcharges", RandomInt(0, 12).ToString());
                    }
                }
                else if (int.Parse(charge.Value) > 12)
                {
                    if (mode == 1)
                    {
                        i.SetAttribute("maxcharges", RandomInt(70, 250).ToString());
                    }
                    else
                    {
                        i.SetAttribute("maxcharges", RandomInt(0, 500).ToString());
                    }
                }
                else
                {
                    if (mode == 1)
                    {
                        i.SetAttribute("maxcharges", (int.Parse(charge.Value) - RandomInt(-2, 2)).ToString());
                    }
                    else
                    {
                        i.SetAttribute("maxcharges", RandomInt(0, 12).ToString());
                    }
                }
            }
            xml.Save(@"resources\items.xml");
            tasks += 20;
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                button.Content = "Working...";
                MessageBox.Show("This will take a while. Please wait until the randomize button says \"Done\".");
                string isaacPath = textBox.Text;
                if (!File.Exists(Path.GetDirectoryName(isaacPath) + @"\isaac-ng.exe") || !Directory.Exists(isaacPath))
                {
                    throw new Exception("Wrong Isaac path!");
                }
                var percentage = 0.0f;
                List<string> packedFiles = new List<string>();
                foreach (var file in Directory.GetFiles(GetIsaacPath() + @"\packed", "*.a"))
                {
                    packedFiles.Add(file);
                }

                button.Content = "Working...";

                foreach (var file in packedFiles)
                {
                    tasks += 2;
                    if (!File.Exists(@"resources\packed\" + Path.GetFileName(file)))
                    {
                        File.Copy(file, @"resources\packed\" + Path.GetFileName(file), true);
                    }

                }

                File.Delete(@"resources\packed\afterbirth_jp.a");
                File.Delete(@"resources\packed\afterbirth_kr.a");

                foreach (var file in Directory.GetFiles(@"resources\packed", "*.a"))
                {
                    if (!File.Exists(@"resources\packed\" + Path.GetFileNameWithoutExtension(file) + "_unpack"))
                    {
                        var unpacker = Process.Start(new ProcessStartInfo
                        {
                            FileName = @"RicksUnpacker\bin\Gibbed.Rebirth.Unpack.exe",
                            Arguments = file,
                            WindowStyle = ProcessWindowStyle.Hidden
                        });
                        unpacker.WaitForExit();
                    }
                }

                if (deletesCurrentFiles.IsChecked ?? false)
                {
                    foreach (var dir in new DirectoryInfo(@"resources").GetDirectories())
                    {
                        if (dir.Name != "packed")
                        {
                            RecursiveDelete(dir);
                        }
                    }

                    foreach (var file in new DirectoryInfo(@"resources").GetFiles("*", SearchOption.TopDirectoryOnly))
                    {
                        file.Delete();
                    }
                }

                /*     foreach(var subFolder in Directory.GetDirectories(@"resources\packed\afterbirth_unpack\resources\"))
                     {
                         new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(subFolder, @"resources\" + Path.GetDirectoryName(subFolder), true);
                         File.SetAttributes(@"resources\" + Path.GetDirectoryName(subFolder), FileAttributes.Normal);
                     }
                     foreach (var file in Directory.GetFiles(@"resources\packed\afterbirth_unpack\resources\"))
                     {
                         File.Copy(file, @"resources\" + Path.GetFileName(file), true);
                         File.SetAttributes(@"resources\" + Path.GetFileName(file), FileAttributes.Normal);
                     }

                     foreach (var folder in Directory.GetDirectories(@"resources\packed", "*_unpack"))
                     {
                         foreach (var subFolder in Directory.GetDirectories(folder + @"\resources\"))
                         {
                             Debug.WriteLine(subFolder);
                             new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(subFolder, @"resources\" + Path.GetFileName(subFolder), false);

                             File.SetAttributes(@"resources\" + Path.GetFileName(subFolder), FileAttributes.Normal);

                         }
                         foreach (var file in Directory.GetFiles(folder))
                         {

                             File.Copy(file, @"resources\" + Path.GetFileName(file), false);
                             File.SetAttributes(@"resources\" + Path.GetFileName(file), FileAttributes.Normal);
                         }
                     }
                     */
                if (doesRandomizeCharacterNames.IsChecked ?? false)
                {
                    RandomizeCharacterNames();
                }
                if (checkBox.IsChecked == true)
                {
                    if (radioButton.IsChecked ?? false)
                    {
                        RandomizeItems(1);
                    }
                    else
                    {
                        RandomizeItems(0);
                    }
                }
                if (checkBox3.IsChecked == true)
                {
                    RandomizeItemSprites();
                }
                if (checkBox9.IsChecked == true)
                {
                    RandomizeCards();
                }
                if (checkBox10.IsChecked == true)
                {
                    RandomizePills();
                }
                if (checkBox6.IsChecked ?? false)
                {
                    RandomizeItemPools();
                }
                if (checkBox2.IsChecked == true)
                {
                    RandomizeStages();
                }
                if (doesRandomizeSounds.IsChecked ?? false)
                {
                    RandomizeSounds(int.Parse(textBox1.Text));
                }
                if (doesRandomizeBossNames.IsChecked ?? false)
                {
                    if (bossNameConsistent.IsChecked ?? false)
                    {
                        RandomizeBossNames(1);
                    }
                    else
                    {
                        RandomizeBossNames(0);
                    }
                }
                if (doesRandomizeRarities.IsChecked ?? false)
                {
                    if (raritiesFair.IsChecked ?? false)
                    {
                        RandomizeRarities(1);
                    }
                    else
                    {
                        RandomizeRarities(0);
                    }
                }
                if (checkBox5.IsChecked ?? false)
                {
                    RemoveSpecialFlag();
                }
                if (doesRandomizeCharacterSprites.IsChecked ?? false)
                {
                    RandomizeCharacterSprites();
                }
                if (doesRandomizeMusic.IsChecked ?? false)
                {
                    RandomizeMusic();
                }
                float _ = 0.0f;
                if (doesMaxHp.IsChecked ?? false && float.TryParse(maxHp.Text, out _))
                {
                    RandomizeMaxHpGains(float.Parse(maxHp.Text));
                }
                if (doesHpGain.IsChecked ?? false && float.TryParse(hpGain.Text, out _))
                {
                    RandomizeHpGains(float.Parse(hpGain.Text));
                }
                if (doesBlackGain.IsChecked ?? false && float.TryParse(blackGain.Text, out _))
                {
                    RandomizeBlackGains(float.Parse(blackGain.Text));
                }
                if (doesSoulGain.IsChecked ?? false && float.TryParse(soulGain.Text, out _))
                {
                    RandomizeSoulGains(float.Parse(soulGain.Text));
                }
                if (doesFullGain.IsChecked ?? false && float.TryParse(fullGain.Text, out _))
                {
                    RandomizeFullGains(float.Parse(fullGain.Text));
                }
                if (doesKeyGain.IsChecked ?? false && float.TryParse(keyGain.Text, out _))
                {
                    RandomizeKeyGains(float.Parse(keyGain.Text));
                }
                if (doesBombGain.IsChecked ?? false && float.TryParse(bombGain.Text, out _))
                {
                    RandomizeBombGains(float.Parse(bombGain.Text));
                }
                if (doesCoinGain.IsChecked ?? false && float.TryParse(coinGain.Text, out _))
                {
                    RandomizeCoinGains(float.Parse(coinGain.Text));
                }
                if (checkBox7.IsChecked ?? false)
                {
                    if (activeChargesFair.IsChecked ?? false)
                    {
                        RandomizeActiveCharges(0);
                    }
                    else
                    {
                        RandomizeActiveCharges(1);
                    }
                }
                if (doesRandomizeCurses.IsChecked ?? false)
                {
                    RandomizeCurses();
                }
                if (doesRandomizePlayerItems.IsChecked ?? false)
                {
                    if (hasSpecificAmountItems.IsChecked ?? false)
                    {
                        RandomizeCharacterItems(ensureActiveItem.IsChecked ?? false, int.Parse(specificItemAmount.Text));
                    }
                    else
                    {
                        RandomizeCharacterItems(ensureActiveItem.IsChecked ?? false);
                    }
                }
                if (doesRandomizePlayerPickups.IsChecked ?? false)
                {
                    if (hasSpecificAmountPickups.IsChecked ?? false)
                    {
                        RandomizeCharacterPickups(coinMultiplier.IsChecked ?? false, specificAmount: int.Parse(pickupsEach.Text));
                    }
                    else if (hasTotalAmountPickups.IsChecked ?? false)
                    {
                        RandomizeCharacterPickups(coinMultiplier.IsChecked ?? false, totalAmount: int.Parse(totalPickup.Text));
                    }
                    else
                    {
                        RandomizeCharacterPickups(coinMultiplier.IsChecked ?? false);
                    }

                }
                if (removePauseMenuIcons.IsChecked ?? false)
                {
                    Directory.CreateDirectory(@"resources\gfx\ui");

                    File.Copy(@"Rec\death items.png", @"resources\gfx\ui\death items.png");
                }

                if (overwriteOriginalFiles.IsChecked ?? false)
                {
                    foreach (var dir in new DirectoryInfo(isaacPath).GetDirectories())
                    {
                        if (dir.Name != "packed")
                        {
                            RecursiveDelete(dir);
                        }
                    }

                    foreach (var file in new DirectoryInfo(isaacPath).GetFiles("*", SearchOption.TopDirectoryOnly))
                    {
                        file.Delete();
                    }

                    CopyFilesRecursively(new DirectoryInfo(@"resources"), new DirectoryInfo(isaacPath));
                    foreach (var file in new DirectoryInfo(@"resources").GetFiles("*", SearchOption.TopDirectoryOnly))
                    {
                        file.CopyTo(isaacPath + @"\" + file.Name, true);
                    }
                }

                button.Content = "Done!";
                this.FlashWindow();        
            }
            catch (Exception ex)
            {
                this.FlashWindow();
                if (ex.ToString().ToLower().Contains("wrong isaac path!"))
                {
                    MessageBox.Show("Your Isaac directory is wrong! Please enter the correct one and try again.");
                    return;
                }
                MessageBox.Show($"An error occurred! Please post this in the comments of the randomizer's mod page as a bug report:\n{ex.ToString()}");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //test!

            ImageSplit.SplitImage();
        }

        private void doesRandomizeSounds_Click(object sender, RoutedEventArgs e)
        {
            if (doesRandomizeSounds.IsChecked ?? false)
            {
                label3.IsEnabled = true;
                textBox1.IsEnabled = true;
            }
            else
            {
                label3.IsEnabled = false;
                textBox1.IsEnabled = false;
            }
        }

        private void doesRandomizeBossNames_Click(object sender, RoutedEventArgs e)
        {
            if (doesRandomizeBossNames.IsChecked ?? false)
            {
                bossNameConsistent.IsEnabled = true;
                bossNameRandom.IsEnabled = true;
            }
            else
            {
                bossNameConsistent.IsEnabled = false;
                bossNameRandom.IsEnabled = false;
            }
        }

        private void checkBox3_Copy_Checked(object sender, RoutedEventArgs e)
        {
            if (doesRandomizeRarities.IsChecked ?? false)
            {
                raritiesFair.IsEnabled = true;
                raritiesRandom.IsEnabled = true;
            }
            else
            {
                raritiesFair.IsEnabled = false;
                raritiesRandom.IsEnabled = false;
            }
        }

        private void checkBox6_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void checkBox9_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void doesMaxHp_Click(object sender, RoutedEventArgs e)
        {
            if (doesMaxHp.IsChecked ?? false)
            {
                maxHp.IsEnabled = true;
            }
            else
            {
                maxHp.IsEnabled = false;
            }
        }

        private void doesHpGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesHpGain.IsChecked ?? false)
            {
                hpGain.IsEnabled = true;
            }
            else
            {
                hpGain.IsEnabled = false;
            }
        }

        private void doesSoulGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesSoulGain.IsChecked ?? false)
            {
                soulGain.IsEnabled = true;
            }
            else
            {
                soulGain.IsEnabled = false;
            }
        }

        private void doesBlackGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesBlackGain.IsChecked ?? false)
            {
                blackGain.IsEnabled = true;
            }
            else
            {
                blackGain.IsEnabled = false;
            }
        }

        private void doesFullGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesFullGain.IsChecked ?? false)
            {
                fullGain.IsEnabled = true;
            }
            else
            {
                fullGain.IsEnabled = false;
            }
        }

        private void ItemsXmlCheckboxChecked(object sender, RoutedEventArgs e)
        {
            radioButton.IsEnabled = true;
            radioButton1.IsEnabled = true;
        }

        private void doesBombGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesBombGain.IsChecked ?? false)
            {
                bombGain.IsEnabled = true;
            }
            else
            {
                bombGain.IsEnabled = false;
            }
        }

        private void doesKeyGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesKeyGain.IsChecked ?? false)
            {
                keyGain.IsEnabled = true;
            }
            else
            {
                keyGain.IsEnabled = false;
            }
        }

        private void doesCoinGain_Click(object sender, RoutedEventArgs e)
        {
            if (doesCoinGain.IsChecked ?? false)
            {
                coinGain.IsEnabled = true;
            }
            else
            {
                coinGain.IsEnabled = false;
            }
        }

        private void checkBox7_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox7.IsChecked ?? false)
            {
                activeChargesFair.IsEnabled = true;
                activeChargesRandom.IsEnabled = true;
            }
            else
            {
                activeChargesFair.IsEnabled = false;
                activeChargesRandom.IsEnabled = false;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (presetList.SelectedItem == null)
            {
                return;
            }
            LoadPreset(@"Presets\" + presetList.SelectedItem.ToString() + ".boir");
        }

        private void LoadPreset(string path)
        {
            string[] preset = File.ReadAllLines(path);
            checkBox.IsChecked = bool.Parse(preset[0]);
            radioButton.IsEnabled = bool.Parse(preset[0]);
            radioButton1.IsEnabled = bool.Parse(preset[0]);


            radioButton.IsChecked = bool.Parse(preset[1]);
            radioButton1.IsChecked = bool.Parse(preset[2]);

            checkBox5.IsChecked = bool.Parse(preset[3]);
            checkBox7.IsChecked = bool.Parse(preset[4]);
            activeChargesFair.IsEnabled = bool.Parse(preset[4]);
            activeChargesRandom.IsEnabled = bool.Parse(preset[4]);

            activeChargesFair.IsChecked = bool.Parse(preset[5]);
            activeChargesRandom.IsChecked = bool.Parse(preset[6]);

            checkBox3.IsChecked = bool.Parse(preset[7]);
            checkBox6.IsChecked = bool.Parse(preset[8]);
            doesRandomizeRarities.IsEnabled = bool.Parse(preset[8]);

            doesRandomizeRarities.IsChecked = bool.Parse(preset[9]);
            raritiesFair.IsEnabled = bool.Parse(preset[9]);
            raritiesRandom.IsEnabled = bool.Parse(preset[9]);

            raritiesFair.IsEnabled = bool.Parse(preset[9]);
            raritiesRandom.IsEnabled = bool.Parse(preset[9]);
            raritiesFair.IsChecked = bool.Parse(preset[10]);
            raritiesRandom.IsChecked = bool.Parse(preset[11]);

            checkBox9.IsChecked = bool.Parse(preset[12]);
            checkBox10.IsChecked = bool.Parse(preset[13]);

            checkBox2.IsChecked = bool.Parse(preset[14]);
            doesRandomizeCurses.IsChecked = bool.Parse(preset[15]);

            doesRandomizeSounds.IsChecked = bool.Parse(preset[16]);
            textBox1.IsEnabled = bool.Parse(preset[16]);
            textBox1.Text = preset[17];

            doesRandomizeMusic.IsChecked = bool.Parse(preset[18]);

            doesRandomizeBossNames.IsChecked = bool.Parse(preset[19]);
            bossNameConsistent.IsEnabled = bool.Parse(preset[19]);
            bossNameRandom.IsEnabled = bool.Parse(preset[19]);
            bossNameConsistent.IsChecked = bool.Parse(preset[20]);
            bossNameRandom.IsChecked = bool.Parse(preset[21]);
            edgyWords.IsEnabled = bool.Parse(preset[20]);
            originalWords.IsEnabled = bool.Parse(preset[20]);
            jokeWords.IsEnabled = bool.Parse(preset[20]);

            doesRandomizeCharacterNames.IsChecked = bool.Parse(preset[22]);
            doesRandomizeCharacterSprites.IsChecked = bool.Parse(preset[23]);
            doesRandomizePlayerItems.IsChecked = bool.Parse(preset[24]);
            ensureActiveItem.IsEnabled = bool.Parse(preset[24]);
            hasSpecificAmountItems.IsEnabled = bool.Parse(preset[24]);
            hasSpecificAmountItems.IsChecked = bool.Parse(preset[25]);
            specificItemAmount.IsEnabled = bool.Parse(preset[25]);
            specificItemAmount.Text = preset[26];
            doesRandomizePlayerPickups.IsChecked = bool.Parse(preset[27]);
            radioButton2.IsEnabled = bool.Parse(preset[27]);
            coinMultiplier.IsEnabled = bool.Parse(preset[27]);
            hasSpecificAmountPickups.IsEnabled = bool.Parse(preset[27]);
            hasTotalAmountPickups.IsEnabled = bool.Parse(preset[27]);
            hasSpecificAmountPickups.IsChecked = bool.Parse(preset[28]);
            pickupsEach.IsEnabled = bool.Parse(preset[28]);
            pickupsEach.Text = preset[29];

            doesMaxHp.IsChecked = bool.Parse(preset[30]);
            maxHp.IsEnabled = bool.Parse(preset[30]);
            maxHp.Text = preset[31];
            doesHpGain.IsChecked = bool.Parse(preset[32]);
            hpGain.IsEnabled = bool.Parse(preset[32]);
            hpGain.Text = preset[33];
            doesSoulGain.IsChecked = bool.Parse(preset[34]);
            soulGain.IsEnabled = bool.Parse(preset[34]);
            soulGain.Text = preset[35];
            doesBlackGain.IsChecked = bool.Parse(preset[36]);
            blackGain.IsEnabled = bool.Parse(preset[36]);
            blackGain.Text = preset[37];
            doesFullGain.IsChecked = bool.Parse(preset[38]);
            fullGain.IsEnabled = bool.Parse(preset[38]);
            fullGain.Text = preset[39];
            doesBombGain.IsChecked = bool.Parse(preset[40]);
            bombGain.IsEnabled = bool.Parse(preset[40]);
            bombGain.Text = preset[41];
            doesKeyGain.IsChecked = bool.Parse(preset[42]);
            keyGain.IsEnabled = bool.Parse(preset[42]);
            keyGain.Text = preset[43];
            doesCoinGain.IsChecked = bool.Parse(preset[44]);
            coinGain.IsEnabled = bool.Parse(preset[44]);
            coinGain.Text = preset[45];
            doesRandomizeCurses.IsChecked = bool.Parse(preset[46]);
            ensureActiveItem.IsChecked = bool.Parse(preset[47]);
            hasTotalAmountPickups.IsChecked = bool.Parse(preset[48]);
            totalPickup.IsEnabled = bool.Parse(preset[48]);
            totalPickup.Text = preset[49];
            originalWords.IsChecked = bool.Parse(preset[50]);
            jokeWords.IsChecked = bool.Parse(preset[51]);
            edgyWords.IsChecked = bool.Parse(preset[52]);
            radioButton2.IsChecked = bool.Parse(preset[53]);
            fontIsaac.IsChecked = bool.Parse(preset[54]);
            fontReadable.IsChecked = bool.Parse(preset[55]);
            removePauseMenuIcons.IsChecked = bool.Parse(preset[56]);
        }

        private void savePreset_Click(object sender, RoutedEventArgs e)
        {
            string preset = "";
            preset += checkBox.IsChecked + "\n";
            preset += radioButton.IsChecked + "\n";
            preset += radioButton1.IsChecked + "\n";
            preset += checkBox5.IsChecked + "\n";
            preset += checkBox7.IsChecked + "\n";
            preset += activeChargesFair.IsChecked + "\n";
            preset += activeChargesRandom.IsChecked + "\n";
            preset += checkBox3.IsChecked + "\n";
            preset += checkBox6.IsChecked + "\n";
            preset += doesRandomizeRarities.IsChecked + "\n";
            preset += raritiesFair.IsChecked + "\n";
            preset += raritiesRandom.IsChecked + "\n";
            preset += checkBox9.IsChecked + "\n";
            preset += checkBox10.IsChecked + "\n";
            preset += checkBox2.IsChecked + "\n";
            preset += doesRandomizeCurses.IsChecked + "\n";
            preset += doesRandomizeSounds.IsChecked + "\n";
            preset += textBox1.Text + "\n";
            preset += doesRandomizeMusic.IsChecked + "\n";
            preset += doesRandomizeBossNames.IsChecked + "\n";
            preset += bossNameConsistent.IsChecked + "\n";
            preset += bossNameRandom.IsChecked + "\n";
            preset += doesRandomizeCharacterNames.IsChecked + "\n";
            preset += doesRandomizeCharacterSprites.IsChecked + "\n";
            preset += doesRandomizePlayerItems.IsChecked + "\n";
            preset += hasSpecificAmountItems.IsChecked + "\n";
            preset += specificItemAmount.Text + "\n";
            preset += doesRandomizePlayerPickups.IsChecked + "\n";
            preset += hasSpecificAmountPickups.IsChecked + "\n";
            preset += pickupsEach.Text + "\n";
            preset += doesMaxHp.IsChecked + "\n";
            preset += maxHp.Text + "\n";
            preset += doesHpGain.IsChecked + "\n";
            preset += hpGain.Text + "\n";
            preset += doesSoulGain.IsChecked + "\n";
            preset += soulGain.Text + "\n";
            preset += doesBlackGain.IsChecked + "\n";
            preset += blackGain.Text + "\n";
            preset += doesFullGain.IsChecked + "\n";
            preset += fullGain.Text + "\n";
            preset += doesBombGain.IsChecked + "\n";
            preset += bombGain.Text + "\n";
            preset += doesKeyGain.IsChecked + "\n";
            preset += keyGain.Text + "\n";
            preset += doesCoinGain.IsChecked + "\n";
            preset += coinGain.Text + "\n";
            preset += doesRandomizeCurses.IsChecked + "\n";
            preset += ensureActiveItem.IsChecked + "\n";
            preset += hasTotalAmountPickups.IsChecked + "\n";
            preset += totalPickup.Text + "\n";
            preset += originalWords.IsChecked + "\n";
            preset += jokeWords.IsChecked + "\n";
            preset += edgyWords.IsChecked + "\n";
            preset += radioButton2.IsChecked + "\n";
            preset += fontIsaac.IsChecked + "\n";
            preset += fontReadable.IsChecked + "\n";
            preset += removePauseMenuIcons.IsChecked + "\n";

            File.WriteAllText(@"Presets\" + presetName.Text + ".boir", preset);
            UpdatePresets();
        }

        private void doesRandomizePlayerItems_Click(object sender, RoutedEventArgs e)
        {
            if (doesRandomizePlayerItems.IsChecked ?? false)
            {
                hasSpecificAmountItems.IsEnabled = true;
                ensureActiveItem.IsEnabled = true;
            }
            else
            {
                hasSpecificAmountItems.IsEnabled = false;
                ensureActiveItem.IsEnabled = false;
            }
        }

        private void hasSpecificAmountItems_Click(object sender, RoutedEventArgs e)
        {
            if (hasSpecificAmountItems.IsChecked ?? false)
            {
                specificItemAmount.IsEnabled = true;
            }
            else
            {
                specificItemAmount.IsEnabled = false;
            }
        }

        private void bossNameConsistent_Checked(object sender, RoutedEventArgs e)
        {
            edgyWords.IsEnabled = true;
            originalWords.IsEnabled = true;
            jokeWords.IsEnabled = true;
        }

        private void doesRandomizePlayerPickups_Click(object sender, RoutedEventArgs e)
        {
            if (doesRandomizePlayerPickups.IsChecked ?? false)
            {
                hasSpecificAmountPickups.IsEnabled = true;
                hasTotalAmountPickups.IsEnabled = true;
                coinMultiplier.IsEnabled = true;
                radioButton2.IsEnabled = true;
            }
            else
            {
                hasSpecificAmountItems.IsEnabled = false;
                hasSpecificAmountPickups.IsEnabled = false;
                hasTotalAmountPickups.IsEnabled = false;
                coinMultiplier.IsEnabled = false;
                radioButton2.IsEnabled = false;
            }
        }

        private void hasSpecificAmountPickups_Click(object sender, RoutedEventArgs e)
        {
            pickupsEach.IsEnabled = true;
        }

        private void hasTotalAmountPickups_Click(object sender, RoutedEventArgs e)
        {
            totalPickup.IsEnabled = true;
        }

        private void bossNameConsistent_Unchecked(object sender, RoutedEventArgs e)
        {
            edgyWords.IsEnabled = false;
            originalWords.IsEnabled = false;
            jokeWords.IsEnabled = false;
        }

        private void hasSpecificAmountPickups_Unchecked(object sender, RoutedEventArgs e)
        {
            pickupsEach.IsEnabled = false;
        }

        private void hasTotalAmountPickups_Unchecked(object sender, RoutedEventArgs e)
        {
            totalPickup.IsEnabled = false;
        }

        private void deletePreset_Click(object sender, RoutedEventArgs e)
        {
            if (presetList.SelectedItem == null ||!File.Exists(@"Presets\" + presetList.SelectedItem.ToString() + ".boir"))
            {
                return;
            }
            File.Delete(@"Presets\" + presetList.SelectedItem.ToString() + ".boir");
            UpdatePresets();
        }

        private void updatePreset_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresets();
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void originalWords_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void doesBlackGain_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            string isaacPath = textBox.Text;
            if (!File.Exists(Path.GetDirectoryName(isaacPath) + @"\isaac-ng.exe") ||!Directory.Exists(isaacPath))
            {
                MessageBox.Show("Wrong Isaac path!");
            }
            else
            {
                foreach (var dir in new DirectoryInfo(isaacPath).GetDirectories())
                {
                    if (dir.Name != "packed")
                    {
                        RecursiveDelete(dir);
                    }
                }

                foreach (var file in new DirectoryInfo(isaacPath).GetFiles("*", SearchOption.TopDirectoryOnly))
                {
                    file.Delete();
                }
            }
        }

        private void presetList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (presetList.SelectedItem != null)
            {
                string selectedPreset = presetList.SelectedItem.ToString();
                LoadPreset(@"Presets\" + selectedPreset + ".boir");
            }
        }

        SoundPlayer player = new SoundPlayer(@"Rec\music.wav");

        private void music_Checked(object sender, RoutedEventArgs e)
        {
            player.PlayLooping();
        }

        private void music_Unchecked(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void ItemsXmlCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            radioButton.IsEnabled = false;
            radioButton1.IsEnabled = false;
        }
    }
}

