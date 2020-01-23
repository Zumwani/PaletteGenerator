using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using static OpenShell.Screens.WindowsDesktopAPI.WindowsDesktopAPI;

namespace OpenShell.Screens
{
    
    public class Screen
    {

        public string Name { get; }
        public string DeviceName { get; }
        public string Adapter { get; }
        public Rectangle Bounds { get; }
        public Rectangle WorkArea { get; }
        public bool IsPrimary { get; }
        public IntPtr Handle { get; }

        int? index;
        public int Index
        { get { return index ?? (index = All().ToList().FindIndex(s => s.Handle == Handle)).Value; } }

        private Screen(IntPtr handle)
        {

            MonitorInfoEx info = new MonitorInfoEx();
            GetMonitorInfo(new HandleRef(null, handle), info);

            Bounds = info.rcMonitor;
            WorkArea = info.rcWork;
            IsPrimary = ((info.dwFlags & MONITORINFOF_PRIMARY) != 0);
            DeviceName = new string(info.szDevice).TrimEnd((char)0).TrimStart(@"\\.\".ToCharArray());
            Handle = handle;

            var display = GetDisplayInfo();
            Name = display.name;
            Adapter = display.adapter;

        }

        public static Screen FromIndex(int index)
        {
            var screen = All()?.ElementAtOrDefault(index);
            if (screen != null)
                return screen;
            else
                throw new IndexOutOfRangeException();
        }

        public static Screen FromWindowHandle(IntPtr handle)
        {

            if (handle == IntPtr.Zero)
                throw new ArgumentNullException(nameof(handle));

            handle = MonitorFromWindow(handle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            return FromScreenHandle(handle);

        }

        public static Screen FromScreenHandle(IntPtr handle)
        {

            if (handle == IntPtr.Zero)
                return default;

            return new Screen(handle);

        }

        public static Screen[] All()
        {

            var l = new List<Screen>();
            bool EnumMonitor(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
            {
                var screen = FromScreenHandle(hMonitor);
                if (screen != null)
                    l.Add(screen);
                return true;
            }

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, new EnumMonitors(EnumMonitor), IntPtr.Zero);

            return l.ToArray();

        }

        (string adapter, string name) GetDisplayInfo()
        {

            string adapter = "";
            DISPLAY_DEVICE device = new DISPLAY_DEVICE(0);

            int i = 0;
            while (EnumDisplayDevices(null, i, ref device, 0))
            {
                if (device.DeviceName.EndsWith(DeviceName))
                {
                    adapter = device.DeviceString;
                    break;
                }
                i += 1;
            }

            EnumDisplayDevices(device.DeviceName, 0, ref device, 0);
            string deviceName = device.DeviceString;

            return (adapter, deviceName);

        }

    }

}
