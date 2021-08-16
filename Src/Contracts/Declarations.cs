using Avalonia.Controls;
using Kfa.SubSystems.Cheques.Contracts.Classes;
using Kfa.SubSystems.Cheques.Contracts.Messaging;
using Kfa.SubSystems.Cheques.Contracts.MvvmNavigation;
using MonkeyCache;
using MonkeyCache.LiteDB;
using Prism.Events;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Unity;
using Usb.Events;

namespace Kfa.SubSystems.Cheques.Contracts
{
    public static class Declarations
    {
        public static EventAggregator EventAggregator { get => eventAggregator ??= new EventAggregator(); }
        public static IUnityContainer DiContainer { get; set; }
        public static BehaviorSubject<UserObject> LoggedInUser { get; } = new BehaviorSubject<UserObject>(new UserObject());
        public static BehaviorSubject<bool> ShutingDown { get; } = new BehaviorSubject<bool>(false);
        public static bool IsInDevelopmentProcess { get; internal set; }
        public static Func<string, Stream> GetStream { get; set; }

        public static CompositeDisposable AppDisposable { get; set; }

        public static string GetReportsFolder() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kfa Reports");

        public static BehaviorSubject<double> WorkingAreaHeight { get; } = new BehaviorSubject<double>(0);
        public static BehaviorSubject<double> AppHeight { get; } = new BehaviorSubject<double>(0);
        public static BehaviorSubject<string> CurrentTitle { get; } = new BehaviorSubject<string>("KFA Data Entries System");
        public static BehaviorSubject<string> DialogsCurrentTitle { get; } = new BehaviorSubject<string>("KFA Data Entries System");
        public static BehaviorSubject<double> WorkingAreaWidth { get; } = new BehaviorSubject<double>(0);
        public static BehaviorSubject<double> AppWidth { get; } = new BehaviorSubject<double>(0);
        public static BehaviorSubject<double> Fontsize { get; } = new BehaviorSubject<double>(18);
        public static BehaviorSubject<bool> ShowSideMenu { get; } = new BehaviorSubject<bool>(true);

        public static Dictionary<byte, string> Months = new()
        {
            { 1, "January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };

        public static string DeviceNumber
        {
            get
            {
                try
                {
                    static string GetId()
                    {
                        return Barrel.Current.Get<string>("DeviceNumber") ?? "AAA";
                    }
                    return LoggedInUser.Value.Prefix ?? GetId();
                }
                catch (Exception)
                {
                    return "AAA";
                }
            }
        }

        private static bool initialized;

        static Declarations()
        {
            IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();
            usbEventWatcher.UsbDeviceRemoved += (_, device) => Declarations.EventAggregator
                               .GetEvent<UsbDeviceUpdateEvent>()
                               .Publish(new UsbUpdateObj(device, false));

            usbEventWatcher.UsbDeviceAdded += (_, device) => Declarations.EventAggregator
                               .GetEvent<UsbDeviceUpdateEvent>()
                               .Publish(new UsbUpdateObj(device, true));

            usbEventWatcher.UsbDriveEjected += (_, path) => Declarations.EventAggregator
                               .GetEvent<UsbDeviceMountEvent>()
                               .Publish(new UsbMountObj(path, false));

            usbEventWatcher.UsbDriveMounted += (_, path) => Declarations.EventAggregator
                               .GetEvent<UsbDeviceMountEvent>()
                               .Publish(new UsbMountObj(path, true));

            CurrentTitle.Subscribe(tt => DialogsCurrentTitle.OnNext(tt));
        }

        public static string ServerUrl { get; set; } = "https://localhost:5001";
        public static long OriginatorId { get; set; }
        public static IBarrel CachedData { get; set; } = Barrel.Current;
        public static string CurrentUserLoginId { get; set; }
        public static Window MainWindow { get; set; }
        public static string AppFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Private Data");
        public static string BackUpsFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kfa Backups");
        public static NavigationManager NavigationManager { get; set; }
        public static Func<Type, dynamic> GetFetchService { get; set; }
        public static Func<Type, dynamic> GetUpdateService { get; set; }
        public static Logger DbLogger { get; set; }
        public static Logger PermanentTableDbLogger { get; set; }

        public static string DatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                     "KfaSubSystem", "Database", "Data.mdb");

        public static string ApplicationDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KfaSubSystem", "Data.mdb");
        private static EventAggregator eventAggregator;
        private static Dictionary<string, string> branchCodes;

        public static short[] Years => Enumerable.Range(1992, DateTime.Now.Year).Select(v => (short)v).ToArray();
        public static double AllowedIdleMinutes { get; set; } = 30;
        public static BehaviorSubject<bool> NavigationViewIsOpen { get; set; } = new BehaviorSubject<bool>(false);
    }
}