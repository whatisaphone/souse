using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MouseAhead
{
	static class InputState
	{
		const int VK_SHIFT =    0x10;
		const int VK_CONTROL =  0x11;
		const int VK_ALT =      0x12;
		const int VK_ADD =      0x6b;
		const int VK_F6 =       0x75;
		const int VK_NUMLOCK =  0x90;
		const int VK_LSHIFT =   0xa0;
		const int VK_RSHIFT =   0xa1;
		const int VK_LCONTROL = 0xa2;
		const int VK_RCONTROL = 0xa3;
		const int VK_LMENU =    0xa4;
		const int VK_RMENU =    0xa5;

		[DllImport("user32")]
		private static extern int AttachThreadInput(uint idAttach, uint idAttachTo, int fAttach);
		[DllImport("user32")]
		private static extern short GetAsyncKeyState(int vKey);
		//[DllImport("user32")]
		//private static extern uint GetCurrentThreadId();
		[DllImport("user32")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32")]
		private static extern int GetKeyboardState(byte[] lpKeyStats);
		[DllImport("user32")]
		private static extern short GetKeyState(int vKey);
		[DllImport("user32")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

		public static int WhichNonModKeyIsDown() {
			GetKeyState(0);  // flush cache, or something like that
			for (var i = 0x20; i < 0x8f; ++i)
				if (i != VK_SHIFT && i != VK_CONTROL && i != VK_ALT &&
						i != VK_LSHIFT && i != VK_RSHIFT && i != VK_LCONTROL && i != VK_RCONTROL &&
						i != VK_NUMLOCK && i != VK_ADD &&
						i != VK_LMENU && i != VK_RMENU)
				{
					System.Diagnostics.Debug.WriteLine(i.ToString() + " - " + GetAsyncKeyState(i).ToString());
					if (GetAsyncKeyState(i) != 0)
						return i;
				}
			return 0;
		}

		private static uint GetCurrentThreadId()
		{
			return 0;
		}

		private static byte[] ReadAllKeys()
		{
			byte[] keys = new byte[256];
			IntPtr hwnd = GetForegroundWindow();
			uint tid = GetWindowThreadProcessId(hwnd, IntPtr.Zero);
			
			if (AttachThreadInput(GetCurrentThreadId(), tid, 1) != 0)
			{
				GetKeyState(0);
				GetKeyboardState(keys);
				AttachThreadInput(GetCurrentThreadId(), tid, 0);
			}
			else
			{
				GetKeyboardState(keys);
			}
			return keys;
		}
	}
}
