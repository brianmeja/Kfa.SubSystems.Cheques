using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Kfa.SubSystems.Cheques.Contracts;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity;

namespace Kfa.SubSystems
{
    public static class Extensions
    {
        public static Action LoadAsDialog(this Control ctrl)
        {
            try
            {
                if (Declarations.MainWindow == null)
                    throw new InvalidOperationException("Main page is not set.");

                var mainPage = Declarations.MainWindow as dynamic;
                var previousContent = mainPage.Content as Control;
                var previousPage = ctrl as Window;

                if (null != previousPage)
                {
                    if (previousPage.Content is Control ctr)
                    {
                        ctrl = ctr;
                    }
                    else
                    {
                        throw new InvalidOperationException("Try to add a window as content to another window");
                    }
                }

                ctrl.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                ctrl.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;

                void controlClosing()
                {
                    Functions.RunOnMain(() =>
                    {
                        try
                        {
                            if (previousPage != null)
                                previousPage.Content = mainPage.Content;

                            mainPage.Content = previousContent;
                        }
                        catch { }
                    });
                }

                Functions.RunOnMain(() =>
                {
                    // try
                    // {
                    //    mainPage.Content = ctrl;
                    // }
                    // catch (Exception ex)
                    // {
                    //    Contracts.Messaging.Notifier.NotifyError(ex);
                    //    if (previousPage != null)
                    //        previousPage.Content = mainPage.Content;
                    //    mainPage.Content = previousContent;
                    // }
                });

                return controlClosing;
            }
            catch (Exception ex)
            {
                Notifier.NotifyError(ex);
            }

            return () => { };
        }

        public static string StrimLineObjectName(this string name)
        {
            return name.Where(chr => char.IsLetterOrDigit(chr) || chr == ' ' || chr == '_')
                .Aggregate("", (current, chr) => current + chr);
        }

        public static Exception InnerError(this Exception error)
        {
            return error?.InnerException ?? error;
        }

        public static DateTimeOffset? ToDateTimeOffset(this DateTime? date)
        {
            if (date == null)
                return null;

            var dateTime = date.Value;
            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                    ? DateTimeOffset.MinValue
                    : new DateTimeOffset(dateTime);
        }

        public static DateTime? FromDateTimeOffset(this DateTimeOffset? date)
        {
            if (date == null)
                return null;

            return ((DateTimeOffset)date).DateTime;
        }

        private static readonly Stack<Window> CurrentLoadedDialogWindows = new();

        public static async Task ShowAsDialog(this IControl control)
        {
            try
            {
                if (!Dispatcher.UIThread.CheckAccess())
                {
                    Functions.RunOnMain(async () => await ShowAsDialog(control));
                    return;
                }
                if (control is Window page)
                {
                    if (!CurrentLoadedDialogWindows.Any())
                        CurrentLoadedDialogWindows.Push(Declarations.MainWindow);

                    Functions.RunOnMain(() => page.Focus(), 1000);
                    if (Declarations.MainWindow != null)
                    {
                        page.ShowInTaskbar = false;
                        page.Topmost = false;
                        page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        void Page_Closed(object sender, EventArgs e)
                        {
                            Functions.RunOnMain(() =>
                            {
                                try
                                {
                                    if (Declarations.MainWindow?.Content is Avalonia.VisualTree.IVisual ctrl2)
                                        ctrl2.Opacity = 1;

                                    var initialPage = CurrentLoadedDialogWindows.Pop();

                                    if (sender != initialPage)
                                        return;

                                    var current = CurrentLoadedDialogWindows.Peek();
                                    if (current != Declarations.MainWindow)
                                        current.Closed += Page_Closed;
                                    if (current?.Content is Avalonia.VisualTree.IVisual ctrl)
                                        ctrl.Opacity = 1;

                                    initialPage.Closed -= Page_Closed;
                                }
                                catch
                                {
                                    if (Declarations.MainWindow?.Content is Avalonia.VisualTree.IVisual ctrl)
                                        ctrl.Opacity = 1;
                                }
                            });
                        }
                        foreach (var item in CurrentLoadedDialogWindows)
                        {
                            try
                            {
                                if (item?.Content is Avalonia.VisualTree.IVisual ctrl)
                                    ctrl.Opacity = 0.002;
                                page.Closed -= Page_Closed;
                            }
                            catch { }
                        }
                        page.Closed += Page_Closed;
                        page.Focus();
                        CurrentLoadedDialogWindows.Push(page);
                        await page.ShowDialog(Declarations.MainWindow);
                    }
                    else
                    {
                        page.Topmost = true;
                        page.Focus();
                        page.Show();
                    }
                }
                else
                {
                    Functions.RunOnMain(() => Declarations.MainWindow
                     .NewPageContentDialog(control, null, "Close"));
                }
            }
            catch (Exception ex)
            {
                Notifier.NotifyError(ex);
            }
        }

        private static Brush TextBackColor;// = (Brush) (new BrushConverter()).ConvertFromString("#161515");

        public static void BlinkForError(this TextBox box)
        {
            //var color = Brushes.Black;// new TextBox().Background;
            Functions.RunOnMain(() =>
            {
                if (TextBackColor == null)
                    TextBackColor = (Brush)(new BrushConverter()).ConvertFromString("#161515");
                box.Background = Brushes.DarkRed;
            });
            Functions.RunOnMain(() => box.Background = TextBackColor, 1000);
            Functions.RunOnMain(() => box.Background = Brushes.DarkRed, 1300);
            Functions.RunOnMain(() => box.Background = TextBackColor, 1600);
            Functions.RunOnMain(() => box.Background = Brushes.DarkRed, 2000);
            Functions.RunOnMain(() => box.Background = TextBackColor, 3000);
        }

        public static void BlinkForError(this AutoCompleteBox box)
        {
            Functions.RunOnMain(() =>
            {
                if (TextBackColor == null)
                    TextBackColor = (Brush)new BrushConverter().ConvertFromString("#161515");
                box.Background = Brushes.DarkRed;
            });
            Functions.RunOnMain(() => box.Background = TextBackColor, 1000);
            Functions.RunOnMain(() => box.Background = Brushes.DarkRed, 1300);
            Functions.RunOnMain(() => box.Background = TextBackColor, 1600);
            Functions.RunOnMain(() => box.Background = Brushes.DarkRed, 2000);
            Functions.RunOnMain(() => box.Background = TextBackColor, 3000);
        }

        public static decimal GetNumber(this string str)
        {
            if (str == null)
                return 0;

            if (decimal.TryParse(new string(str.Where(char.IsDigit).ToArray()), out decimal ans))
                return ans;
            return 0;
        }

        public static decimal[] GetNumbers(this string str)
        {
            if (str == null)
                return Array.Empty<decimal>();

            return Regex.Matches(str, @"[0-9]+(\.[0-9]+)*").OfType<Match>()
                 .Select(x => decimal.TryParse(x.Value, out decimal obj) ? obj : 0m)
                 .ToArray();
        }

        public static string CreateMd5Hash(this string password)
        {
            using var md5 = MD5.Create();
            byte[] retVal = md5.ComputeHash(Encoding.Unicode.GetBytes(password));
            var sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));

            return sb.ToString();
        }

        public static string CreateSHA512Hash(this string password)
        {
            byte[] data = Encoding.Unicode.GetBytes(password);
            SHA512 shaM = new SHA512Managed();
            var retVal = shaM.ComputeHash(data);
            var sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));

            return sb.ToString();
        }

        private static SortedDictionary<string, IBitmap> resourceImages = new();
        private static IAssetLoader assetLoader;

        public static IBitmap GetImageIcon(this string name)
        {
            if (resourceImages.ContainsKey(name))
                return resourceImages[name];

            if (assetLoader == null)
                assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            if (name.StartsWith("resm:") || name.StartsWith("avares:"))
            {
                var bitmap = new Bitmap(assetLoader.Open(new Uri(name)));
                resourceImages.Add(name, bitmap);
                return bitmap;
            }
            try
            {
                if (name.ToLower().EndsWith(".png"))
                    name = name?.Substring(0, name.LastIndexOf('.'));

                var url = $"avares://Kfa.SubSystems/Assets/Icons/actions/{name}.png";

                var bitmap = new Bitmap(assetLoader.Open(new Uri(url)));
                resourceImages.Add(name, bitmap);
                return bitmap;
            }
            catch (Exception)
            {
                try
                {
                    var url = "avares://Kfa.SubSystems/Assets/Icons/actions/appointment-new.png";
                    var bitmap = new Bitmap(assetLoader.Open(new Uri(url)));
                    resourceImages.Add(name, bitmap);
                    return bitmap;
                }
                catch (Exception)
                { }
            }

            return null;
        }

        public static T ResolveObject<T>(this IContainerProvider obj) => obj.Resolve<T>();

        public static T ResolveObject<T>(this string name)
        {
            return (T)Declarations.DiContainer?.Resolve(typeof(T), name);
        }

        public static void RegisterObject<T>(this T value, string name)
        {
            Declarations.DiContainer?.RegisterInstance(typeof(T),
                   name,
                   value,
                   InstanceLifetime.Singleton);
        }

        /// <summary>
        /// The Chunk.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="source">The source<see cref="IEnumerable{T}"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="List{List{T}}"/>.</returns>
        public static List<List<T>> Chunk<T>(this IEnumerable<T> source, int size)
        {
            var chunks = new List<List<T>>();
            var enumerable = source as T[] ?? source.ToArray();
            var xCount = (enumerable.Length / size) + 2;

            for (var i = 0; i < xCount; i++)
            {
                var temp = enumerable.Skip(i * size).Take(size).ToList();
                if (temp.Any()) chunks.Add(temp);
            }

            return chunks;
        }

        /// <summary>
        /// The Randomize.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="values">The values<see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="List{T}"/>.</returns>
        public static List<T> Randomize<T>(this IEnumerable<T> values)
        {
            var list = values as List<T> ?? values.ToList();
            var n = list.Count;
            var rnd = new Random();

            while (n > 1)
            {
                var k = rnd.Next(0, n) % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}