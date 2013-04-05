using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MouseAhead
{
	static class InputInjector
	{
		const uint INPUT_MOUSE = 0;
		const uint INPUT_KEYBOARD = 1;
		const uint INPUT_HARDWARE = 2;

		const uint MAPVK_VK_TO_VSC = 0;
		const uint MAPVK_VSC_TO_VK = 1;
		const uint MAPVK_VK_TO_CHAR = 2;
		const uint MAPVK_VSC_TO_VK_EX = 3;

		const uint KEYEVENTF_KEYUP = 2;

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

		[StructLayout(LayoutKind.Explicit)]
		struct INPUT
		{
			[FieldOffset(0)]
			public uint type;
			[FieldOffset(4)]
			public MOUSEINPUT mi;
			[FieldOffset(4)]
			public KEYBDINPUT ki;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct KEYBDINPUT
		{
			public ushort wVk;
			public ushort wScan;
			public uint dwFlags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct MOUSEINPUT
		{
			public int dx, dy;
			public uint mouseData, dwFlags, time;
			public IntPtr dwExtraInfo;
		}

		[DllImport("user32")]
		static extern uint MapVirtualKey(uint uCode, uint uMapType);
		[DllImport("user32")]
		static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
		
		public static void KeyEvent(Keys keys, bool down)
		{
			var input = new INPUT();
			input.type = INPUT_KEYBOARD;
			if ((keys & Keys.Modifiers) != 0)
			{
				input.ki.wVk = (ushort)(((keys & Keys.Shift) != 0 ? Keys.ShiftKey : 0) |
					((keys & Keys.Control) != 0 ? Keys.ControlKey : 0) |
					((keys & Keys.Alt) != 0 ? Keys.Menu : 0));
			}
			else
			{
				input.ki.wVk = (ushort)keys;
			}
			input.ki.wScan = (ushort)MapVirtualKey(input.ki.wVk, MAPVK_VK_TO_VSC);
			input.ki.dwFlags = down ? 0 : KEYEVENTF_KEYUP;
			SendInput(1, new[] {input}, Marshal.SizeOf(input));
		}

		public static void MouseEvent(MouseButtons button, bool down)
		{
			INPUT input = new INPUT();
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
			SendInput(1, new[] {input}, Marshal.SizeOf(input));
		}
	}
}
