using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using DeviceId;
using DeviceId.Encoders;
using DeviceId.Formatters;
using Kfa.SubSystems.Cheques.Contracts.Classes;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using Kfa.SubSystems.Cheques.Contracts.MvvmNavigation;
using Kfa.SubSystems.Cheques.Contracts.MvvmNavigation.Abstractions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kfa.SubSystems.Cheques.Contracts
{
    public static class Functions
    {
        private static readonly object BackgroundlockObject = new object();

        public static DataTable SortDataTable(DataTable codes, params string[] parameters)
        {
            try
            {
                if (!parameters.Any()) return codes;
                codes.DefaultView.Sort = string.Join(", ", parameters);
                return codes.DefaultView.ToTable();
            }
            catch (Exception)
            {
                return codes;
            }
        }

        public static Func<int, int, string> AsMonthString =
       (yy, mm) => GetMonthsFormated(yy, mm);

        private static IBrush textboxBackground;

        public static void AlertifyError(AutoCompleteBox textbox, string message)
        {
            if (textboxBackground == null
                && textbox.Background != Brushes.Red)
                textboxBackground = textbox.Background;

            textbox.Background = Brushes.Red;
            // textbox.Focus();
            RunOnMain(() => textbox.Background = textboxBackground, 2000);
            MessageBox.Show(message);
        }

        public static string GetMonthsFormated(int? year, int? month)
        {
            if (year < 100)
            {
                year = year > (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)))
                    ? Convert.ToInt32(string.Format("19{0}", year))
                    : Convert.ToInt32(string.Format("20{0}", year));
            }

            return string.Format("{0}-{1}", year, month?.ToString("00"));
        }

        public static int AsMonth(string month)
        {
            return Convert.ToInt32(new string(month.Where(char.IsDigit).ToArray()));
        }

        public static string[] GenerateMonths(string monthFrom, string monthTo)
        {
            var noFrom = monthFrom.GetNumbers();
            var noTo = monthTo.GetNumbers();
            return GenerateMonths((byte)noFrom[1], (short)noFrom[0], (byte)noTo[1], (short)noTo[0]);
        }

        public static string GetMonthName(string monthFrom, bool inShortForm = false)
        {
            var noFrom = monthFrom.GetNumbers();
            if (noFrom?.Length == 2)
            {
                var mth = (byte)noFrom[1];
                return inShortForm ? $"{(Declarations.Months.ContainsKey(mth) ? Declarations.Months[mth] : null)?.Substring(0, 3)} {noFrom[0].ToString("0000").Substring(2, 2)}" : $"{(Declarations.Months.ContainsKey(mth) ? Declarations.Months[mth] : null)} {noFrom[0]:0000}";
            }
            return null;
        }

        public static string[] GenerateMonths(byte monthFrom, short yearFrom, byte monthTo, short yearTo)
        {
            var startDate = new DateTime(yearFrom, monthFrom, 1);
            var endDate = new DateTime(yearTo, monthTo, 1);

            if (endDate < startDate)
            {
                var d = endDate;
                endDate = startDate;
                startDate = d;
            }

            var months = new List<string>();
            for (int i = 0; i < 2000; i++)
            {
                var date = startDate.AddMonths(i);
                if (date <= endDate)
                    months.Add(date.ToString("yyyy-MM"));
                else
                    break;
            }

            return months.ToArray();
        }

        public static int AsMonth(int year, int month) => AsMonth(AsMonthString(year, month));

        private static readonly List<Action> Actions = new List<Action>();

        public static object RegisterView { get; set; }
        public static Func<IDbConnection> GetDBConnection { get; set; }

        public static async Task RegisterAsLoginFunction(Action a)
        {
            try
            {
                async Task ax() => await
            Task.Factory.StartNew(() =>
            {
                try
                {
                    a();
                }
                catch (Exception ex)
                {
                    Functions.NotifyError(ex.Message, "Login Functions Error");
                }
            });

                if (Declarations.LoggedInUser.Value.UserName != null)
                    await ax();
                else
                    Actions.Add(a);
            }
            catch (Exception ex)
            {
                Notifier.NotifyError("Error refreshing the primary key values", "Refreshing Keys", ex);
            }
        }

        private static IDbDataAdapter GetAdapter(IDbConnection connection)
        {
            var assembly = connection.GetType().Assembly;
            var @namespace = connection.GetType().Namespace;

            // Assumes the factory is in the same namespace
            var factoryType = assembly.GetTypes()
                                .Where(x => x.Namespace == @namespace)
                                .Where(x => x.IsSubclassOf(typeof(DbProviderFactory)))
                                .Single();

            // SqlClientFactory and OleDbFactory both have an Instance field.
            var instanceFieldInfo = factoryType.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
            var factory = (DbProviderFactory)instanceFieldInfo.GetValue(null);

            return factory.CreateDataAdapter();
        }

        public static DataSet GetDbDataSet(string sql)
        {
            using var con = Functions.GetDBConnection();
            using var cmd = con.CreateCommand();
            if (con.State != ConnectionState.Open)
                con.Open();

            var ds = new DataSet();
            var adapter = GetAdapter(con);
            IDbCommand dbCommand = con.CreateCommand();
            dbCommand.CommandText = sql;
            dbCommand.CommandType = CommandType.Text;
            adapter.SelectCommand = dbCommand;
            adapter.Fill(ds);
            return ds;
        }

        public static string MakeAllFirstLetterCapital(string myStr, bool lowerOthers)
        {
            if (string.IsNullOrWhiteSpace(myStr))
                return myStr;

            short i;
            var makeFirstUCase = "";

            var strArr = myStr.Split(' ');
            for (i = 0; i <= strArr.Length - 1; i++)
                try
                {
                    if (strArr[i] == string.Empty) continue;
                    var others = lowerOthers ? strArr[i][1..].ToLower() : strArr[i][1..];
                    var ser = strArr[i].Substring(0, 1);
                    strArr[i] = ser.ToUpper() + others;
                    makeFirstUCase = makeFirstUCase + strArr[i] + " ";
                }
                catch
                {
                    // ignored
                }

            return makeFirstUCase.Trim();
        }

        public static void RecordPreviousLogs((DateTime, string, object, string)[] ps)
        {
            if (!ps.Any())
                return;

            Functions.RunOnBackground(() =>
            {
                try
                {
                    var dir = Path.Combine(Declarations.AppFolder, "AppLogs");
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    try
                    {
                        var files = Directory.GetFiles(dir, "*.zip");
                        if (files.Length > 10)
                        {
                            var filesToRetain = files.OrderByDescending(v => v).Take(9);
                            foreach (var fil in files)
                            {
                                try
                                {
                                    if (filesToRetain.Contains(fil))
                                        continue;
                                    File.Delete(fil);
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Notifier.NotifyError(ex);
                    }

                    var sb = new StringBuilder();
                    ps.ToList().ForEach(tt =>
                    {
                        sb.Append("******************************************************************");
                        sb.Append($"\r\n\r\nTIME: {tt.Item1:yyyy dd, MMMM => HH:mm:ss} => ");
                        sb.Append(tt.Item2);
                        sb.Append("\r\n");
                        sb.Append("___________________________________________________________________");
                        sb.Append("\r\n");
                        sb.Append(tt.Item3);
                        sb.Append("\r\n");
                        sb.Append("###################################################################\r\n");
                        sb.Append(tt.Item4);
                        sb.Append("\r\n");
                        sb.Append("___________________________________________________________________");
                        sb.Append($"\r\n\r\n\r\n\r\n");
                    });
                    var path = Path.Combine(dir, $"General_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.zip");
                    var file = Path.Combine(dir, "data.txt");
                    File.WriteAllText(file, sb.ToString());
                    //ZipMaker.Create(path, new string[] { file }, "KfaLogs654321Eliud");
                    ZipMaker.Create(path, new string[] { file }, "KfaSubSystem_654321_Logs");
                }
                catch (Exception ex)
                {
                    var dir = Path.Combine(Declarations.AppFolder, "AppLogs");
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var file = Path.Combine(dir, "error.txt");
                    File.WriteAllText(file, ex.ToString());
                }
            });
        }

        public static void RunLoginFunctions()
        {
            Actions.Reverse();

            RunOnMain(() =>
            {
                Declarations.LoggedInUser.OnNext(Declarations.LoggedInUser.Value);

                Actions.ToList().ForEach(async tt =>
                {
                    await Task.Factory
                     .StartNew(() => Dispatcher.UIThread.InvokeAsync(tt, DispatcherPriority.SystemIdle))
                     .ConfigureAwait(false);
                });
            });
            Functions.RunOnMain(() => Declarations.NavigationManager?.Navigate("HomePage"), 7000);
            // var objs = Actions.Select(x => Task.Factory.StartNew(x)).ToList();
            // Actions.ForEach(x => await x());
        }

        public static string ReadManifestData<T>(string fileName) where T : class
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;

            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(s => s.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new InvalidOperationException("Could not load the specified resource " + fileName);

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static Dictionary<string, object> dynamicGeneratedObjects = new Dictionary<string, object>();

        public static void RegisterAndNavigate(string path, Func<object> getViewModel, Func<object> getView)
        {
            var nav = Declarations.NavigationManager;

            if (nav == null)
                throw new NullReferenceException("Can't resolve navigation manager");

            if (!nav.CanNavigate(path))
                RegisterNavigation(path, getViewModel, getView);

            NavigateTo(path);
        }

        public static string OperatingSystem = System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;

        public static void NavigateTo(string path)
        {
            var nav = Declarations.NavigationManager;

            if (nav == null)
                throw new NullReferenceException("Can't resolve navigation manager");

            if (nav.CanNavigate(path)) nav.Navigate(path);
            else throw new NullReferenceException($"Navigation path '{path}' has not been registered");
        }

        public static object Pluralize(string tableName)
        {
            throw new NotImplementedException();
        }

        public static void RegisterNavigation(string path, Func<object> getViewModel, Func<object> getView)
        {
            try
            {
                var nav = Declarations.NavigationManager;

                if (nav == null)
                    throw new NullReferenceException("Can't resolve navigation manager");

                if (nav.CanNavigate(path)) return;

                nav.Register(path, getViewModel, getView);
            }
            catch { }
        }

        public static void RegisterNavigation<T>(string path, Func<object> getViewModel) where T : Control, new()
        {
            var nav = Declarations.NavigationManager;

            if (nav == null)
                throw new NullReferenceException("Can't resolve navigation manager");

            nav.Register<T>(path, getViewModel);
        }

        public static Exception ExtractInnerException(Exception exception)
        {
            try
            {
                static Exception getInnerError(Exception ex)
                {
                    // if (ex is DbEntityValidationException objEx)
                    // {
                    //    var mm = string.Join("\r\n\r\n", objEx.EntityValidationErrors
                    //        .Select(x => string.Join("\r\n\r\n",
                    // x.ValidationErrors.Select(y => y.PropertyName + ": " + y.ErrorMessage))));

                    //    return new ValidationException(mm);
                    // }
                    // else
                    if (ex.InnerException != null)
                    {
                        return ex.InnerException;
                    }

                    return ex;
                }

                var mEx = exception;

                while (true)
                {
                    var mm = getInnerError(mEx);
                    if (mm == mEx) break;
                    mEx = mm;
                }

                return mEx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T GetApiService<T>(bool h = false)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidDate(out DateTime? date, object year, object month, object day)
        {
            date = null;
            try
            {
                var xYear = Convert.ToInt32(year);
                var xMonth = Convert.ToInt32(month);
                var xDay = Convert.ToInt32(day);

                static string getYear(string ans)
                {
                    if (short.TryParse(ans, out short oo))
                    {
                        if (100 > oo)
                        {
                            var mm = Convert.ToInt32((DateTime.Now.Year + 3).ToString().Substring(2));
                            return oo < mm ? "20" + oo : "19" + oo;
                        }
                        else
                        {
                            var mm = Convert.ToInt32($"{ans}0000".Substring(0, 4));
                            return mm.ToString("0000");
                        }
                    }
                    return ans;
                }

                try
                {
                    date = new DateTime(xYear, xMonth, xDay);
                    return true;
                }
                catch { }
                try
                {
                    var text = xDay.ToString();
                    if (text.Length == 2 && xDay > 28)
                    {
                        date = new DateTime(xYear, Convert.ToInt32(text[1]), Convert.ToInt32(text[0]));
                        return true;
                    }
                    else if (text.Length == 3)
                    {
                        try
                        {
                            date = new DateTime(xYear, Convert.ToInt32(text[2]), Convert.ToInt32(text.Substring(0, 2)));
                            return true;
                        }
                        catch { }
                        try
                        {
                            date = new DateTime(xYear, Convert.ToInt32(text.Substring(1, 2)), Convert.ToInt32(text[0]));
                            return true;
                        }
                        catch { }
                    }
                    if (byte.TryParse(text.Substring(0, 2), out byte mn) && mn > 0 && mn < 32)
                    {
                        if (byte.TryParse(text.Substring(2, 2), out byte kn) && kn > 0 && kn < 13)
                        {
                            date = new DateTime(Convert.ToInt32(getYear(text[4..])),
                                Convert.ToInt32(text.Substring(2, 2)), Convert.ToInt32(text.Substring(0, 2)));
                            return true;
                        }
                        else if (byte.TryParse(text.Substring(2, 1), out byte kn1) && kn1 > 0 && kn1 < 13)
                        {
                            date = new DateTime(Convert.ToInt32(getYear(text[3..])), kn1,
                                Convert.ToInt32(text.Substring(0, 2)));
                            return true;
                        }
                    }
                    else if (byte.TryParse(text.Substring(0, 1), out byte mn1) && mn1 > 0 && mn1 < 32)
                    {
                        if (byte.TryParse(text.Substring(1, 2), out byte kn) && kn > 0 && kn < 13)
                        {
                            date = new DateTime(Convert.ToInt32(getYear(text[3..])), Convert.ToInt32(text.Substring(1, 2)), Convert.ToInt32(mn1));
                            return true;
                        }
                        else if (byte.TryParse(text.Substring(1, 1), out byte kn1) && kn1 > 0 && kn1 < 13)
                        {
                            date = new DateTime(Convert.ToInt32(getYear(text[2..])), kn1, mn1);
                            return true;
                        }
                    }
                }
                catch { }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetComputerId()
        {
            var res = new DeviceIdBuilder()
              //.AddMachineName()
              //.AddOSVersion()
              //.AddUserName()
              .AddProcessorId()
              .AddMacAddress()
              //.AddSystemDriveSerialNumber()
              .AddMotherboardSerialNumber()
              //.AddSystemDriveSerialNumber()
              .UseFormatter(new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder()))
              .ToString();

            return Regex.Replace(res.ToUpper(global::System.Globalization.CultureInfo.CurrentCulture), ".{4}", "$0 ").Trim();
        }

        public static string GetEncKey() => "54389bhjfdsh-dsmnjek _cndfmgnoriy3io  @IO#o";

        public static string GetMonthFormated(int? year, int? month)
        {
            if (year < 100)
            {
                year = year > Convert.ToInt32(DateTime.Now.Year.ToString()[2..])
                    ? Convert.ToInt32(string.Format("19{0}", ((int)year).ToString("00")))
                    : Convert.ToInt32(string.Format("20{0}", ((int)year).ToString("00")));
            }
            return string.Format("{0}-{1}", year, month?.ToString("00"));
        }

        public static string StrimLineObjectName(string name)
        {
            var _name = "";

            return name.Where(chr => char.IsLetterOrDigit(chr) || chr == ' ' || chr == '_')
                .Aggregate(_name, (current, chr) => current + chr);
        }

        public static void AddToken(string token) => token.RegisterObject("token");

        public static void RunOnBackground(Action action)
        {
            RunOnBackground(out BackgroundWorker worker, action);
        }

        public static void RunOnBackground(out BackgroundWorker worker, Action action)
        {
            lock (BackgroundlockObject)
            {
                try
                {
                    var action1 = action;

                    action = () =>
                    {
                        try
                        {
                            action1();
                        }
                        catch (Exception ex)
                        {
                            NotifyError(ex.Message, "Unhandled Background Error", ex);
                        }
                    };

                    var helper = new BackgroundWorkHelper();
                    worker = helper.BackgroundWorker;
                    var actions = new List<Action> { action };
                    helper.SetActionsTodo(actions);
                    helper.IsParallel = true;

                    if (helper.BackgroundWorker.IsBusy)
                        helper.SetActionsTodo(actions);
                    else
                        helper.BackgroundWorker.RunWorkerAsync();
                }
                catch (Exception e)
                {
                    NotifyError(e.Message, "Unhandled Background Error", e);
                    worker = null;
                }
            }
        }

        internal static string GetSupplierCodeSuffix(string supplierCode)
        {
            if (string.IsNullOrWhiteSpace(supplierCode))
                return supplierCode;

            if (supplierCode?.Length <= 6 && supplierCode?.Length > 3)
            {
                return supplierCode.Substring(3);
            }

            return "";
        }

        public static void RunOnBackground(Action action, int sleepTime)
        {
            RunOnBackground(() =>
            {
                try
                {
                    Thread.Sleep(sleepTime);
                    action();
                }
                catch (Exception ex)
                {
                    NotifyError(ex.InnerError().Message, "Unhandled Background Error", ex);
                }
            });
        }

        public static void RunOnMain(Action action, int sleepTime = 0, Dispatcher dispatcher = null)
        {
            RunOnBackground(async () =>
            {
                try
                {
                    void ax()
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            NotifyError(ex.InnerError().Message, "Unhandled Background Error", ex);
                        }
                    }

                    if (sleepTime > 0) Thread.Sleep(sleepTime);

                    await (dispatcher ?? Dispatcher.UIThread)
                    .InvokeAsync(ax, DispatcherPriority.ApplicationIdle)
                    .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    NotifyError(ex.InnerError().Message, "Unhandled Background Error", ex);
                }
            });
        }

        public static string GetRandomString(int length, int trial = 0)
        {
            var rnd = new Random();
            var sb = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var nn = rnd.Next(0, 35);

                if (nn < 10)
                    sb.Append(nn);
                else
                {
                    const int CON = (byte)'A' - 10;
                    sb.Append((char)(CON + nn));
                }
            }

            var ans = sb.ToString();

            if (trial++ < 10 && (ans.Contains("I") || ans.Contains("O")))
            {
                return GetRandomString(length, trial);
            }

            return ans;
        }

        public static void NotifyError(string message, string title, Exception ex = null)
        {
            Notifier.NotifyError(message, title, ex);
        }
    }
}