using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MouseAhead
{
	static class InputInjector
	{
		const uint INPUT_MOUSE = 0;
		const uint INPUT_KEYBOARD = 1;
		const uint INPUT_HARDWARE = 2;

		const uint MOUSEEVENTF_MOVE = 0x1;
		const uint MOUSEEVENTF_LEFTDOWN = 0x2;
		const uint MOUSEEVENTF_LEFTUP = 0x4;
		const uint MOUSEEVENTF_RIGHTDOWN = 0x8;
		const uint MOUSEEVENTF_RIGHTUP = 0x10;
		const uint MOUSEEVENTF_MIDDLEDOWN = 0x20;
		const uint MOUSEEVENTF_MIDDLEUP = 0x40;
		const uint MOUSEEVENTF_XDOWN = 0x80;
		const uint MOUSEEVENTF_XUP = 0x100;
		const uint MOUSEEVENTF_WHEEL = 0x800;
		const uint MOUSEEVENTF_HWHEEL = 0x1000;
		const uint MOUSEEVENTF_NOCOALESCE = 0x2000;
		const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
		const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

		[StructLayout(LayoutKind.Sequential)]
		struct INPUTMOUSEINPUT
		{
			public uint type;
			public MOUSEINPUT mi;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct MOUSEINPUT
		{
			public int dx, dy;
			public uint mouseData, dwFlags, time;
			public IntPtr dwExtraInfo;
		}

		[DllImport("user32")]
		static extern uint SendInput(uint nInputs, INPUTMOUSEINPUT[] pInputs, int cbSize);

		public static void MouseEvent(MouseButtons button, bool down)
		{
			INPUTMOUSEINPUT input = new INPUTMOUSEINPUT();
			input.type = INPUT_MOUSE;
			switch (button)
			{
				case MouseButtons.Left:
					input.mi.dwFlags = down ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP;
					break;
				case MouseButtons.Right:
					input.mi.dwFlags = down ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP;
					break;
				case MouseButtons.Middle:
					input.mi.dwFlags = down ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP;
					break;
				default:
					throw new ArgumentException();
			}
			SendInput(1, new INPUTMOUSEINPUT[] { input }, Marshal.SizeOf(input));
		}
	}
}
