using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PInvokeScreenResolutionChanger
{
    class Program
    {
        public struct POINTL
        {
            public Int32 x;
            public Int32 y;
        }

        [Flags]
        public enum DM : int
        {
            Orientation = 0x1,
            PaperSize = 0x2,
            PaperLength = 0x4,
            PaperWidth = 0x8,
            Scale = 0x10,
            Position = 0x20,
            NUP = 0x40,
            DisplayOrientation = 0x80,
            Copies = 0x100,
            DefaultSource = 0x200,
            PrintQuality = 0x400,
            Color = 0x800,
            Duplex = 0x1000,
            YResolution = 0x2000,
            TTOption = 0x4000,
            Collate = 0x8000,
            FormName = 0x10000,
            LogPixels = 0x20000,
            BitsPerPixel = 0x40000,
            PelsWidth = 0x80000,
            PelsHeight = 0x100000,
            DisplayFlags = 0x200000,
            DisplayFrequency = 0x400000,
            ICMMethod = 0x800000,
            ICMIntent = 0x1000000,
            MediaType = 0x2000000,
            DitherType = 0x4000000,
            PanningWidth = 0x8000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [FieldOffset(0)]
            public string dmDeviceName;
            [FieldOffset(32)]
            public Int16 dmSpecVersion;
            [FieldOffset(34)]
            public Int16 dmDriverVersion;
            [FieldOffset(36)]
            public Int16 dmSize;
            [FieldOffset(38)]
            public Int16 dmDriverExtra;
            [FieldOffset(40)]
            public DM dmFields;

            [FieldOffset(44)]
            Int16 dmOrientation;
            [FieldOffset(46)]
            Int16 dmPaperSize;
            [FieldOffset(48)]
            Int16 dmPaperLength;
            [FieldOffset(50)]
            Int16 dmPaperWidth;
            [FieldOffset(52)]
            Int16 dmScale;
            [FieldOffset(54)]
            Int16 dmCopies;
            [FieldOffset(56)]
            Int16 dmDefaultSource;
            [FieldOffset(58)]
            Int16 dmPrintQuality;

            [FieldOffset(44)]
            public POINTL dmPosition;
            [FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;

            [FieldOffset(60)]
            public short dmColor;
            [FieldOffset(62)]
            public short dmDuplex;
            [FieldOffset(64)]
            public short dmYResolution;
            [FieldOffset(66)]
            public short dmTTOption;
            [FieldOffset(68)]
            public short dmCollate;
            [FieldOffset(72)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [FieldOffset(102)]
            public Int16 dmLogPixels;
            [FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [FieldOffset(108)]
            public Int32 dmPelsWidth;
            [FieldOffset(112)]
            public Int32 dmPelsHeight;
            [FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [FieldOffset(116)]
            public Int32 dmNup;
            [FieldOffset(120)]
            public Int32 dmDisplayFrequency;
        }

        private enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

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

            Console.WriteLine(ChangeResolution(ref dm, 600, 800)
                ? "Screen resolution is changed succesfully"
                : "Something went wrong");

            Thread.Sleep(5000);
            dm.dmDisplayOrientation = 0;
            RestoreDefaultResolution(ref dm);
        }
    }
}
