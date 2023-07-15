using System;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
#if UNITY_EDITOR_WIN
using System.Runtime.InteropServices;
#endif

namespace Amilious.Core.Editor.SystemInfo {
    
    public static class SystemScreenInfo {

	    private static RectInt[] displayInfo;
	    

	    static SystemScreenInfo() => ForceUpdate();

	    public static int NumberOfDisplays {
		    get {
			    #if !UNITY_EDITOR_WIN
			    return 1;
			    #else
			    if(displayInfo == null) return 0;
			    return displayInfo.Length;
			    #endif
		    }
	    }

	    public static bool TryGetWorkingArea(int screen, bool fullscreen, out RectInt workingArea) {
		    #if !UNITY_EDITOR_WIN
		    workingArea = fullscreen? 
			    new RectInt(0,0,Screen.currentResolution.width, Screen.currentResolution.height): 
			    Screen.mainWindowDisplayInfo.workArea;
		    return true;
		    #else 
		    return TryWinGetWorkingArea(screen,fullscreen,out workingArea);
		    #endif
	    }

	    public static void SetWindowLocation(Process process, int x, int y, bool topMost = false) {
		    #if !UNITY_EDITOR_WIN
		    return;
		    #else
		    WinSetWindowLocation(process,x,y,topMost);
		    #endif
	    }

	    public static bool TryGetScreenRect(int screen, out RectInt screenRect) {
		    screenRect = default;
		    if(displayInfo == null || screen < 0 || screen >= displayInfo.Length) return false;
		    screenRect = displayInfo[screen];
		    return true;
	    }

	    public static void ForceUpdate() {
			#if !UNITY_EDITOR_WIN
		    displayInfo = new RectInt[Display.displays.Length];
		    var i = 0;
		    foreach(var display in Display.displays) {
			    var rect = new RectInt(0, 0, display.systemWidth, display.systemHeight);
			    displayInfo[i] = rect;
			    i++;
		    }
		    #else
		    displayInfo = WinGetDisplayInfo().ToArray();
		    #endif
	    }

	    #region Windows Methods ////////////////////////////////////////////////////////////////////////////////////////
	    #if UNITY_EDITOR_WIN
	    
	    private const int SWP_NO_SIZE = 0x0001;
		private const int SWP_NO_Z_ORDER = 0x0004;
		private const int SWP_SHOW_WINDOW = 0x0040;
		private const int HWND_TOPMOST = -1;
		private const int ABM_GET_TASK_BAR_POS = 0x00000005;

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

		[DllImport("shell32.dll")]
		private static extern IntPtr SHAppBarMessage(int dwMessage, ref AppBarData pData);
		
		[DllImport("user32.dll")]
		private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

		[DllImport("user32.dll")]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);
		
		private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

		[StructLayout(LayoutKind.Sequential)]
		private struct AppBarData {
			public int cbSize;
			public IntPtr hWnd;
			public uint uCallbackMessage;
			public uint uEdge;
			public SystemRect rc;
			public int lParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct SystemRect {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		private struct MonitorInfo {
			public int cbSize;
			public SystemRect rcMonitor;
			public SystemRect rcWork;
			public uint dwFlags;
		}

		private static SystemRect GetTaskbarInfo() {
			var appBarData = new AppBarData();
			appBarData.cbSize = Marshal.SizeOf(appBarData);
			var result = SHAppBarMessage(ABM_GET_TASK_BAR_POS, ref appBarData);
			return result != IntPtr.Zero ? appBarData.rc : new SystemRect();
		}
		
		private static IEnumerable<RectInt> WinGetDisplayInfo() {
			var displayRects = new List<SystemRect>();
			bool Callback(IntPtr hMonitor, IntPtr intPtr, IntPtr intPtr1, IntPtr intPtr2) {
				var monitorInfo = new MonitorInfo();
				monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
				if(GetMonitorInfo(hMonitor, ref monitorInfo)) {
					displayRects.Add(monitorInfo.rcMonitor);
				}
				return true;
			}

			EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, Callback, IntPtr.Zero);

			// Iterate over the displayRects list to access information about each display
			for (int i = 0; i < displayRects.Count; i++) {
				var systemRect = displayRects[i];
				var width = systemRect.Right - systemRect.Left;
				var height = systemRect.Bottom - systemRect.Top;
				yield return new RectInt(systemRect.Left, systemRect.Top, width, height);
			}
		}
		
		private static bool TryWinGetWorkingArea(int screen, bool fullscreen, out RectInt workingArea) {
			var taskBarInfo = GetTaskbarInfo();
			var passed = TryGetScreenRect(screen, out var screenArea);
			workingArea = new RectInt(screenArea.x,screenArea.y,screenArea.width,screenArea.height);
			if(!passed) return false;
			if(fullscreen) return true;
			var taskWidth = taskBarInfo.Right - taskBarInfo.Left;
			var taskHeight = taskBarInfo.Bottom - taskBarInfo.Top;
			//the taskbar is on the bottom
			if(taskBarInfo.Left == 0 && taskBarInfo.Top != 0 && taskBarInfo.Right == Screen.currentResolution.width) 
				workingArea.height -= taskHeight;
			//the taskbar is on the top
			if(taskBarInfo.Left == 0 && taskBarInfo.Top == 0 && taskBarInfo.Right == Screen.currentResolution.width) 
				workingArea.y += taskHeight;
			//the taskbar is on the left
			if(taskBarInfo.Left == 0 && taskBarInfo.Bottom == Screen.currentResolution.height && taskBarInfo.Top == 0) 
				workingArea.x += taskWidth;
			//the taskbar is on the right
			if(taskBarInfo.Left != 0) workingArea.width -= taskWidth;
			return true;
		}

		private static void WinSetWindowLocation(Process process, int x, int y, bool topMost = false) {
			if(process.MainWindowHandle == IntPtr.Zero) return;
			var handle = process.MainWindowHandle;
			if(topMost) SetWindowPos(handle, HWND_TOPMOST, x, y, 0, 0, SWP_NO_SIZE | SWP_SHOW_WINDOW);
			else SetWindowPos(handle, 0, x, y, 0, 0, SWP_NO_SIZE | SWP_NO_Z_ORDER);
		}

		#endif
		#endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
		
    }
    
}