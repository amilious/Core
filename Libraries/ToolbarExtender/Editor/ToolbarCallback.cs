using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;
#if !UNITY_2019_1_OR_NEWER
using UnityEngine.Experimental.UIElements;
#endif

namespace ToolbarExtender.Editor {

	public static class ToolbarCallback {
		static readonly Type m_toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
		static readonly Type m_guiViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GUIView");
		#if UNITY_2020_1_OR_NEWER
		static readonly Type m_iWindowBackendType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IWindowBackend");
		static PropertyInfo m_windowBackend = m_guiViewType.GetProperty("windowBackend",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		static PropertyInfo m_viewVisualTree = m_iWindowBackendType.GetProperty("visualTree",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		#else
		static PropertyInfo m_viewVisualTree = m_guiViewType.GetProperty("visualTree",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		#endif
		static FieldInfo m_imguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		static ScriptableObject m_currentToolbar;

		/// <summary>
		/// Callback for toolbar OnGUI method.
		/// </summary>
		public static Action OnToolbarGUI;
		public static Action OnToolbarGUILeft;
		public static Action OnToolbarGUIRight;
		
		static ToolbarCallback() {
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate() {
			// Relying on the fact that toolbar is ScriptableObject and gets deleted when layout changes
			if(m_currentToolbar != null) return;
			// Find toolbar
			var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
			m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
			if(m_currentToolbar == null) return;
			#if UNITY_2021_1_OR_NEWER
			var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
			if(root == null) {
				Debug.LogWarning("No root object found!");
				return;
			}
			var rawRoot = root.GetValue(m_currentToolbar);
			var mRoot = rawRoot as VisualElement;
			RegisterCallback(mRoot,"ToolbarZoneLeftAlign", OnToolbarGUILeft);
			RegisterCallback(mRoot,"ToolbarZoneRightAlign", OnToolbarGUIRight);
			#else
				#if UNITY_2020_1_OR_NEWER
				var windowBackend = m_windowBackend.GetValue(m_currentToolbar);

				// Get it's visual tree
				var visualTree = (VisualElement) m_viewVisualTree.GetValue(windowBackend, null);
				#else
				// Get it's visual tree
				var visualTree = (VisualElement) m_viewVisualTree.GetValue(m_currentToolbar, null);
				#endif

				// Get first child which 'happens' to be toolbar IMGUIContainer
				var container = (IMGUIContainer) visualTree[0];

				// (Re)attach handler
				var handler = (Action) m_imguiContainerOnGui.GetValue(container);
				handler -= OnGUI;
				handler += OnGUI;
				m_imguiContainerOnGui.SetValue(container, handler);
			#endif
			
		}

		private static void RegisterCallback(VisualElement mRoot,string root, Action cb) {
			var toolbarZone = mRoot.Q(root);
			var parent = new VisualElement() {
				style = { flexGrow = 1, flexDirection = FlexDirection.Row}

			};
			var container = new IMGUIContainer();
			container.style.flexGrow = 1;
			container.onGUIHandler += () => { cb?.Invoke(); };
			parent.Add(container);
			toolbarZone.Add(parent);
		}

		private static void OnGUI() {
			var handler = OnToolbarGUI;
			if (handler != null) handler();
		}
	}
}
