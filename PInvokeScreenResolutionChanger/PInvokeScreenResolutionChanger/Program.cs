using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PInvokeScreenResolutionChanger;

namespace PInvokeScreenResolutionChanger
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern DISP_CHANGE ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        private const int EnumCurrentSettings = -1;
        private const int CdsUpdateregistry = 0x01;
        private const int CdsTest = 0x02;

        private static int _defaultHeigth;
        private static int _defaultWidth;

        public static void GetCurrentRes(ref DEVMODE dm)
        {
            dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(dm);
            EnumDisplaySettings(null, EnumCurrentSettings, ref dm);
            _defaultHeigth = dm.dmPelsHeight;
            _defaultWidth = dm.dmPelsWidth;
        }

        private static bool SetDisplayMode(ref DEVMODE dm)
        {
            if (ChangeDisplaySettings(ref dm, CdsTest) == 0 && ChangeDisplaySettings(ref dm, CdsUpdateregistry) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeResolution(ref DEVMODE dm, int height, int width)
        {
            dm.dmPelsHeight = height;
            dm.dmPelsWidth = width;
            if (SetDisplayMode(ref dm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void RestoreDefaultResolution(ref DEVMODE dm)
        {
            dm.dmPelsHeight = _defaultHeigth;
            dm.dmPelsWidth = _defaultWidth;
            SetDisplayMode(ref dm);
        }

        static void Main(string[] args)
        {
            DEVMODE dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
            GetCurrentRes(ref dm);
            Console.WriteLine("Press Enter to change your screen resolution for 5 seconds.");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine("Enter screen width:");
                //Argument format check required
                int width = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter screen height:");
                int height = int.Parse(Console.ReadLine());
                Console.WriteLine(ChangeResolution(ref dm, height, width)
                    ? "Screen resolution is changed succesfully"
                    : "Something went wrong");
                Thread.Sleep(5000);
                RestoreDefaultResolution(ref dm);
            }
        }
    }
}
