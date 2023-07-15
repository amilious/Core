using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using Amilious.Core.Editor.SystemInfo;

namespace Amilious.Core.Editor {

	[InitializeOnLoad]
    public static class MultiPlay  {
	    
	    #if PLATFORM_STANDALONE_WIN

	    private static Action triggerAction;
	    private static bool _fullScreen;
	    private static bool _startServer;
	    private static bool _startClient;
	    private static int _displayScreen;
	    private static int _displayWindows;
	    private static bool Initialized;
	    private static Texture RunIcon;
	    private static Texture StopIcon;
	    private static Texture WindowIcon;
	    private static Texture DisplayIcon;
	    private static Texture FullScreen;
	    private static Texture ServerIcon;
	    private static Texture ClientIcon;
	    private static GUIContent StopPlaying;
	    private static GUIContent StartPlaying;
	    private static GUIContent FullScreenContent;
	    private static GUIContent StartServerContent;
	    private static GUIContent StartClientContent;
	    private static GUIStyle LeftButtonStyle;
	    private static GUIStyle MidButtonStyle;
	    private static GUIStyle RightButtonStyle;
	    private static GUIStyle MidTextButtonStyle;
	    private static readonly List<Process> Processes = new List<Process>();

	    private static bool FullScreenMode {
		    get => _fullScreen;
		    set {
			    if(value==_fullScreen) return;
			    _fullScreen = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(_fullScreen),value);
		    }
	    }

	    private static bool StartServer {
		    get => _startServer;
		    set {
			    if(value==_startServer) return;
			    _startServer = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(_startServer),value);
		    }
	    }

	    private static bool StartClient {
		    get => _startClient;
		    set {
			    if(value==_startClient) return;
			    _startClient = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(_startClient),value);
		    }
	    }

	    private static int DisplayWindows {
		    get => _displayWindows;
		    set {
			    if(value==_displayWindows) return;
			    _displayWindows = value;
			    EditorPrefs.SetInt(nameof(MultiPlay) + nameof(_displayWindows),value);
		    }
	    }
	    
	    private static int DisplayScreen {
		    get => _displayScreen;
		    set {
			    if(value==_displayScreen) return;
			    _displayScreen = value;
			    EditorPrefs.SetInt(nameof(MultiPlay) + nameof(_displayScreen),value);
		    }
	    }
	    
	    
        static MultiPlay() {
			ToolbarExtender.Editor.ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
			_fullScreen = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(_fullScreen), false);
			_startServer = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(_startServer), false);
			_startClient = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(_startClient), false);
			_displayWindows = EditorPrefs.GetInt(nameof(MultiPlay) + nameof(_displayWindows), 1);
			_displayScreen = EditorPrefs.GetInt(nameof(MultiPlay) + nameof(_displayScreen), 1);
			if(_displayWindows is not 1 and not 4 and not 9) _displayWindows = 1;
			if(_displayScreen<0||_displayScreen >= SystemScreenInfo.NumberOfDisplays) _displayScreen = 0;
			EditorApplication.update += DoWork;
        }

        private static void DoWork() {
	        if(triggerAction==null) return;
	        triggerAction?.Invoke();
	        triggerAction = null;
        }

        private static void OnToolbarGUI() {
			if(!Initialized) {
				RunIcon = EditorGUIUtility.IconContent(@"d_PlayButton On@2x").image;
				StopIcon  = EditorGUIUtility.IconContent(@"d_PreMatQuad@2x").image;
				FullScreen = EditorGUIUtility.IconContent(@"d_ScaleTool").image;
				WindowIcon = EditorGUIUtility.IconContent(@"d_winbtn_win_restore_a@2x").image;
				DisplayIcon = EditorGUIUtility.IconContent(@"BuildSettings.Standalone On").image;
				ServerIcon = Resources.Load<Texture>("Icons/server");
				ClientIcon = Resources.Load<Texture>("Icons/client");
				LeftButtonStyle = new GUIStyle("AppCommandLeft") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				MidButtonStyle = new GUIStyle("AppCommandMid") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				MidTextButtonStyle = new GUIStyle(MidButtonStyle) { imagePosition = ImagePosition.ImageLeft};
				RightButtonStyle = new GUIStyle("AppCommandRight") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				StartPlaying = new GUIContent(RunIcon, "Launch game windows!");
				StopPlaying = new GUIContent(StopIcon, "Stop the running game windows.");
				FullScreenContent = new GUIContent(FullScreen, "If toggled the windows will be fullscreen.");
				StartServerContent = new GUIContent(ServerIcon, "If toggled the server will be started for the first instance.");
				StartClientContent = new GUIContent(ClientIcon, "If toggled the clients will be started for all instances.");
				Initialized = true;
			}
			TryGetStartedProcesses();
			GUILayout.FlexibleSpace();
			var enabled = Processes.Count > 0;
			if(GUILayout.Toggle(enabled, enabled ? StopPlaying : StartPlaying, LeftButtonStyle) != enabled) {
				if(!enabled) triggerAction = RunInstances;
				else triggerAction = StopInstances;
			}
			if(GUILayout.Button(new GUIContent((DisplayWindows).ToString(),WindowIcon, "The number of windows to launch!"), MidTextButtonStyle)) {
				var windows = DisplayWindows;
				if(windows <= 1) DisplayWindows = 4;
				if(windows is >2 and <9) DisplayWindows = 9;
				if(windows >= 9) DisplayWindows = 1;
			}
			if(GUILayout.Button(new GUIContent((DisplayScreen+1).ToString(),DisplayIcon, "The display draw the game windows in."), MidTextButtonStyle)) {
				var value = DisplayScreen + 1;
				if(value >= SystemScreenInfo.NumberOfDisplays) value = 0;
				DisplayScreen = value;
			}
			FullScreenMode = GUILayout.Toggle(FullScreenMode, FullScreenContent, MidButtonStyle);
			StartServer = GUILayout.Toggle(StartServer, StartServerContent, MidButtonStyle);
			StartClient = GUILayout.Toggle(StartClient, StartClientContent, RightButtonStyle);
        }

        private static void StopInstances() {
			foreach(var process in Processes) {
				if(!process.HasExited) process.Kill();
				process.Dispose();
			}
			Processes.Clear();
		}

		private static void TryGetStartedProcesses() {
			if(Processes.Count != 0) return;
			var buildTarget = EditorUserBuildSettings.activeBuildTarget;
			var buildLocation = EditorUserBuildSettings.GetBuildLocation(buildTarget);
			var processName = Path.GetFileNameWithoutExtension(buildLocation);
			foreach(var process in Process.GetProcessesByName(processName)) {
				process.EnableRaisingEvents = true;
				process.Exited += OnProcessExited;
				Processes.Add(process);
			}
		}
		
		private static void RunInstances() {
			//get working area
			if(!SystemScreenInfo.TryGetWorkingArea(DisplayScreen, FullScreenMode, out var workingArea)) return;
			//build game
			if(!BuildDevelopmentGame(out var buildLocation)) return;
			//calculate screen values
			var sides = DisplayWindows == 1 ? 1 : Mathf.FloorToInt(Mathf.Sqrt(DisplayWindows));
			var windowWidth = workingArea.width / sides;
			var windowHeight = workingArea.height / sides;
			//start processes
			for(var y = 0; y < sides;y++)
			for(var x=0; x<sides;x++) {
				var process = new Process();
				Processes.Add(process);
				process.StartInfo.FileName = buildLocation;
				var args = LaunchArgs(Processes.Count, windowWidth, windowHeight,
					StartClient,x==0&&y==0&&StartServer);
				process.StartInfo.Arguments = args;
				process.EnableRaisingEvents = true;
				process.Exited += OnProcessExited;
				process.Start();
				var count = 0;
				while(process.MainWindowHandle==IntPtr.Zero&&count<20) {
					System.Threading.Thread.Sleep(100);
					count++;
				}
				var xPos = x * windowWidth + workingArea.x;
				var yPos = y * windowHeight + workingArea.y;
				SystemScreenInfo.SetWindowLocation(process,xPos,yPos,FullScreenMode);
			}
		}

		private static bool BuildDevelopmentGame(out string buildLocation) {
			//get the target values
			var buildTarget = EditorUserBuildSettings.activeBuildTarget;
			buildLocation = EditorUserBuildSettings.GetBuildLocation(buildTarget);

			//get the scenes
			var scenes = new string[EditorBuildSettings.scenes.Length];
			for(var i = 0; i< EditorBuildSettings.scenes.Length;i++) 
				scenes[i] = EditorBuildSettings.scenes[i].path;
			
			// Modify the build options with the desired resolution
			var buildPlayerOptions = new BuildPlayerOptions();
			buildPlayerOptions.scenes = scenes;
			buildPlayerOptions.locationPathName = buildLocation;
			buildPlayerOptions.target = buildTarget;
			buildPlayerOptions.options = BuildOptions.Development;
			try {
				// Build the project with default settings
				var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
				//only run the instances if the build was succeeded
				return report.summary.result == BuildResult.Succeeded;
			}catch(Exception) { return false; }
		}

		private static void OnProcessExited(object sender, EventArgs e) {
			if(Processes.Any(process => !process.HasExited)) return;
			StopInstances();
		}
		
		private static string LaunchArgs(int instance, int width, int height, bool startClient = false, 
			bool startServer = false) => $"-instance-id {instance} -screen-fullscreen 0 -screen-width {width} " +
			$"-screen-height {height} -popupwindow {(startServer?"-start-server ":"")}{(startClient?"-start-client":"")}";

		#endif

    }

}