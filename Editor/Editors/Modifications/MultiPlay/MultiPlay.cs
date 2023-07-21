using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

namespace Amilious.Core.Editor.Editors.Modifications.MultiPlay {

	/// <summary>
	/// This class is used to add multi play buttons to the editor toolbar.
	/// </summary>
	[InitializeOnLoad] public static class MultiPlay  {
	    
	    #if PLATFORM_STANDALONE_WIN

	    #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
	    
	    private static Action s_triggerAction;
	    private static int s_displayScreen, s_displayWindows;
	    private static readonly List<Process> Processes = new List<Process>();
	    private static bool s_fullScreen, s_startServer, s_startClient, s_initialized;
	    private static GUIStyle s_leftButtonStyle, s_midButtonStyle, s_rightButtonStyle, s_midTextButtonStyle;
	    private static Texture s_runIcon, s_stopIcon, s_windowIcon, s_displayIcon, s_fullScreenIcon, s_serverIcon,s_clientIcon;
	    private static GUIContent s_stopPlaying, s_startPlaying, s_fullScreenContent, s_startServerContent, s_startClientContent;

	    #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
	    
	    #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
	    
	    /// <summary>
	    /// This property is used to get or set the fullscreen editor pref.
	    /// </summary>
	    private static bool FullScreenMode {
		    get => s_fullScreen;
		    set {
			    if(value==s_fullScreen) return;
			    s_fullScreen = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(s_fullScreen),value);
		    }
	    }

	    /// <summary>
	    /// This property is used to get or set the start server editor pref.
	    /// </summary>
	    private static bool StartServer {
		    get => s_startServer;
		    set {
			    if(value==s_startServer) return;
			    s_startServer = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(s_startServer),value);
		    }
	    }

	    /// <summary>
	    /// This property is used to get or set the start client editor pref.
	    /// </summary>
	    private static bool StartClient {
		    get => s_startClient;
		    set {
			    if(value==s_startClient) return;
			    s_startClient = value;
			    EditorPrefs.SetBool(nameof(MultiPlay) + nameof(s_startClient),value);
		    }
	    }

	    /// <summary>
	    /// This property is used to get or set the display windows editor pref.
	    /// </summary>
	    private static int DisplayWindows {
		    get => s_displayWindows;
		    set {
			    if(value==s_displayWindows) return;
			    s_displayWindows = value;
			    EditorPrefs.SetInt(nameof(MultiPlay) + nameof(s_displayWindows),value);
		    }
	    }
	    
	    /// <summary>
	    /// This property is used to get or set the display screen editor pref.
	    /// </summary>
	    private static int DisplayScreen {
		    get => s_displayScreen;
		    set {
			    if(value==s_displayScreen) return;
			    s_displayScreen = value;
			    EditorPrefs.SetInt(nameof(MultiPlay) + nameof(s_displayScreen),value);
		    }
	    }
	    
	    #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
	    
	    #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
	    
	    /// <summary>
	    /// This constructor is called the first time any method is called.
	    /// </summary>
        static MultiPlay() {
			ToolbarExtender.Editor.ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
			s_fullScreen = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(s_fullScreen), false);
			s_startServer = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(s_startServer), false);
			s_startClient = EditorPrefs.GetBool(nameof(MultiPlay) + nameof(s_startClient), false);
			s_displayWindows = EditorPrefs.GetInt(nameof(MultiPlay) + nameof(s_displayWindows), 1);
			s_displayScreen = EditorPrefs.GetInt(nameof(MultiPlay) + nameof(s_displayScreen), 1);
			if(s_displayWindows is not 1 and not 4 and not 9) s_displayWindows = 1;
			if(s_displayScreen<0||s_displayScreen >= SystemScreenInfo.NumberOfDisplays) s_displayScreen = 0;
			EditorApplication.update += Update;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when drawing the editor toolbar.
        /// </summary>
        private static void OnToolbarGUI() {
			if(!s_initialized) {
				s_runIcon = EditorGUIUtility.IconContent(@"d_PlayButton On@2x").image;
				s_stopIcon  = EditorGUIUtility.IconContent(@"d_PreMatQuad@2x").image;
				s_fullScreenIcon = EditorGUIUtility.IconContent(@"d_ScaleTool").image;
				s_windowIcon = EditorGUIUtility.IconContent(@"d_winbtn_win_restore_a@2x").image;
				s_displayIcon = EditorGUIUtility.IconContent(@"BuildSettings.Standalone On").image;
				s_serverIcon = Resources.Load<Texture>("Icons/server");
				s_clientIcon = Resources.Load<Texture>("Icons/client");
				s_leftButtonStyle = new GUIStyle("AppCommandLeft") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				s_midButtonStyle = new GUIStyle("AppCommandMid") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				s_midTextButtonStyle = new GUIStyle(s_midButtonStyle) { imagePosition = ImagePosition.ImageLeft};
				s_rightButtonStyle = new GUIStyle("AppCommandRight") { margin = { top = -2 }, fixedHeight = 20, padding = {top = 2,bottom = 2}};
				s_startPlaying = new GUIContent(s_runIcon, "Launch game windows!");
				s_stopPlaying = new GUIContent(s_stopIcon, "Stop the running game windows.");
				s_fullScreenContent = new GUIContent(s_fullScreenIcon, "If toggled the windows will be fullscreen.");
				s_startServerContent = new GUIContent(s_serverIcon, "If toggled the server will be started for the first instance.");
				s_startClientContent = new GUIContent(s_clientIcon, "If toggled the clients will be started for all instances.");
				s_initialized = true;
			}
			TryGetStartedProcesses();
			GUILayout.FlexibleSpace();
			var enabled = Processes.Count > 0;
			if(GUILayout.Toggle(enabled, enabled ? s_stopPlaying : s_startPlaying, s_leftButtonStyle) != enabled) {
				if(!enabled) s_triggerAction = RunInstances;
				else s_triggerAction = StopInstances;
			}
			if(GUILayout.Button(new GUIContent((DisplayWindows).ToString(),s_windowIcon, "The number of windows to launch!"), s_midTextButtonStyle)) {
				var windows = DisplayWindows;
				if(windows <= 1) DisplayWindows = 4;
				if(windows is >2 and <9) DisplayWindows = 9;
				if(windows >= 9) DisplayWindows = 1;
			}
			if(GUILayout.Button(new GUIContent((DisplayScreen+1).ToString(),s_displayIcon, "The display draw the game windows in."), s_midTextButtonStyle)) {
				var value = DisplayScreen + 1;
				if(value >= SystemScreenInfo.NumberOfDisplays) value = 0;
				DisplayScreen = value;
			}
			FullScreenMode = GUILayout.Toggle(FullScreenMode, s_fullScreenContent, s_midButtonStyle);
			StartServer = GUILayout.Toggle(StartServer, s_startServerContent, s_midButtonStyle);
			StartClient = GUILayout.Toggle(StartClient, s_startClientContent, s_rightButtonStyle);
        }

        /// <summary>
        /// This method is called during the editor update cycle.
        /// </summary>
        private static void Update() {
	        if(s_triggerAction==null) return;
	        s_triggerAction?.Invoke();
	        s_triggerAction = null;
        }
        
        /// <summary>
        /// This method is used to run instances of the game.
        /// </summary>
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
        
		/// <summary>
		/// This method is used to create a development build of the game.
		/// </summary>
		/// <param name="buildLocation">The build location.</param>
		/// <returns>True if the build was successful, otherwise false.</returns>
		private static bool BuildDevelopmentGame(out string buildLocation) {
			//get the target values
			var buildTarget = EditorUserBuildSettings.activeBuildTarget;
			buildLocation = EditorUserBuildSettings.GetBuildLocation(buildTarget);

			//get the scenes
			var scenes = new string[EditorBuildSettings.scenes.Length];
			for(var i = 0; i< EditorBuildSettings.scenes.Length;i++) 
				scenes[i] = EditorBuildSettings.scenes[i].path;
			
			// Modify the build options with the desired resolution
			var buildPlayerOptions = new BuildPlayerOptions {
				scenes = scenes, locationPathName = buildLocation, target = buildTarget,
				options = BuildOptions.Development
			};
			try {
				// Build the project with default settings
				var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
				//only run the instances if the build was succeeded
				return report.summary.result == BuildResult.Succeeded;
			}catch(Exception) { return false; }
		}
		
		/// <summary>
		/// This method is used to get the launch args.
		/// </summary>
		/// <param name="instance">The instance id.</param>
		/// <param name="width">The width of the window.</param>
		/// <param name="height">The height of the window.</param>
		/// <param name="startClient">True if network clients should be started.</param>
		/// <param name="startServer">True if a network server should be started.</param>
		/// <returns>The launch args created from the given values.</returns>
		private static string LaunchArgs(int instance, int width, int height, bool startClient = false, 
			bool startServer = false) {
			var launchArgs = $"-instance-id {instance} -screen-fullscreen 0 -screen-width {width} " +
				$"-screen-height {height} -popupwindow";
			if(startServer) launchArgs += " -start-server";
			if(startClient) launchArgs += " -start-client";
			return launchArgs;
		}

		#region Process Methods ////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// This method is used to try get the processes that are already running.
		/// </summary>
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
		
		/// <summary>
		/// This method is used to stop all the running instances of the build.
		/// </summary>
		private static void StopInstances() {
			foreach(var process in Processes) {
				if(!process.HasExited) process.Kill();
				process.Dispose();
			}
			Processes.Clear();
		}
		
		/// <summary>
		/// This method is called when an instance stops running.
		/// </summary>
		/// <param name="sender">The process.</param>
		/// <param name="e">The event args.</param>
		private static void OnProcessExited(object sender, EventArgs e) {
			if(Processes.Any(process => !process.HasExited)) return;
			StopInstances();
		}
		
		#endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

		#endif

    }

}