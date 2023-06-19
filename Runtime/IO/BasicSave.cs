/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Amilious.Core.IO {
    
    // ReSharper disable MemberCanBePrivate.Global
    public static class BasicSave {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        private const string SAVE_NAME = "BasicSave";
        private const string LABEL_COLOR = "#FFA500";
        private const string MENU_NAME = "Amilious/Core/Basic Save";
        private const string FILE_NAME = "basic";
        private const string CURRENT_VERSION_STRING = "1.0.0.0";
        //don't edit below here ////////////////////////////////////////////////////////////////////////////////////////
        private const string BUILT_SAVE_FILE = FILE_NAME+".data";
        private const string EDITOR_SAVE_FILE = FILE_NAME+".editor.data";
        private const string DEVELOPER_SAVE_FILE = FILE_NAME+".developer.data";
        private const string DEVELOPER_X_SAVE_FILE = FILE_NAME+".developer{0}.data";
        private const string VERSION_KEY = "**save_version**";
        private const string TITLE = "<b><color="+LABEL_COLOR+">["+SAVE_NAME+"]</color></b>";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary contains all of the save data.
        /// </summary>
        private static Dictionary<string, object> _data = new Dictionary<string, object>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered after the data has been loaded or reloaded.
        /// </summary>
        public static event Action OnAfterLoad;

        /// <summary>
        /// This event is triggered before the data is saved.
        /// </summary>
        public static event Action OnBeforeSave;

        /// <summary>
        /// This event is triggered when the data is being reset.
        /// </summary>
        public static event Action OnResetting;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the newest version of the console data.
        /// </summary>
        public static Version Version { get; }

        /// <summary>
        /// This property contains the save path.
        /// </summary>
        public static string SavePath { get; }

        /// <summary>
        /// If this property is true the data will be saved whenever a value is updated.
        /// </summary>
        public static bool SaveWhenUpdated { get; set; } = false;

        /// <summary>
        /// If this property is true loading and saving will be logged.
        /// </summary>
        public static bool ShowSaveAndLoadLogs { get; set; } = true;

        /// <summary>
        /// If this property is true the data has been updated since the last time it was loaded.
        /// </summary>
        public static bool DataChanged { get; private set; } = false;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Constructor /////////////////////////////////////////////////////////////////////////////////////
        
        static BasicSave() {
            Version = Version.Parse(CURRENT_VERSION_STRING);
            SavePath = BUILT_SAVE_FILE;
            if(Application.isEditor) SavePath = EDITOR_SAVE_FILE;
            else if(Debug.isDebugBuild) {
                SavePath = !AmiliousCore.TryGetInstanceId(out var id) ? 
                    DEVELOPER_SAVE_FILE : string.Format(DEVELOPER_X_SAVE_FILE, id);
            }
            SavePath = FileHelper.CreatePath(Application.persistentDataPath,SavePath);
            //load
            Load();
            //automatically save when the application is quitting
            Application.quitting += Save;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Saving And Loading /////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to load the console data.
        /// </summary>
        #if UNITY_EDITOR
        [MenuItem(MENU_NAME+"/Reload Data",false,AmiliousCore.PACKAGE_ID)]
        #endif
        public static void Load() {
            //create new save data
            if(!File.Exists(SavePath)) {Reset(areYouSure: true);}
            //load existing data
            using(var fileStream = File.Open(SavePath, FileMode.Open)) {
                var formatter = new BinaryFormatter();
                _data = (Dictionary<string, object>)formatter.Deserialize(fileStream);
            }
            //get the version
            if(_data.TryGetCastValue(VERSION_KEY, out string versionText)) {
                var version = new Version(versionText);
                if(version != Version) UpgradeData(version, _data);
            }
            //load
            if(ShowSaveAndLoadLogs) Debug.Log($"{TITLE} <color=#00FF00>Loaded the {FILE_NAME} save data!</color>");
            OnAfterLoad?.Invoke();
        }

        #if UNITY_EDITOR
        [MenuItem(MENU_NAME+"/Reset Data",false,AmiliousCore.PACKAGE_ID+2)]
        public static void Reset() {
            var sure = EditorUtility.DisplayDialog($"Reset {FILE_NAME.ToUpperFirst()} Save Data",
                "Are you sure that you want to delete the currently saved data?"
                , "Yes, I am sure!", "Cancel");
            Reset(sure);
        }
        #endif

        /// <summary>
        /// This method is used to reset the save data.
        /// </summary>
        /// <param name="areYouSure">If true the data will be reset.</param>
        public static void Reset(bool areYouSure) {
            if(!areYouSure) return;
            _data.Clear();
            //data containers
            OnResetting?.Invoke();
            DataChanged = true;
            Debug.Log($"{TITLE} <color=#FF0000>Reset the {FILE_NAME} save data!</color>");
            Save();
        }

        /// <summary>
        /// This method is used to save the console data.
        /// </summary>
        #if UNITY_EDITOR
        [MenuItem(MENU_NAME+"/Force Save",false,AmiliousCore.PACKAGE_ID+1)]
        #endif
        public static void Save() {
            OnBeforeSave?.Invoke();
            if(!DataChanged) return;
            using(var fileStream = File.Open(SavePath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, _data);
                fileStream.Close();
            }
            DataChanged = false;
            if(ShowSaveAndLoadLogs) Debug.Log($"{TITLE} <color=#00FF00>Saved the {FILE_NAME} save data!</color>");
        }
        
        /// <summary>
        /// This method will be called if the data structure has been altered.
        /// </summary>
        /// <param name="oldVersion">The version that was loaded.</param>
        /// <param name="data">The loaded data that needs to be modified.</param>
        private static void UpgradeData(Version oldVersion, Dictionary<string, object> data) {}

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Data Specific Methods //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to save data to the save file.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="value">The data's value.</param>
        /// <param name="forceSave">If true the data will be saved immediately if it has been updated.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        public static void StoreData<T>(string key, T value, bool forceSave = false) {
            //we do not need to save the data if it has not changed.
            if(_data.TryGetCastValue(key,out T value2)&&value2.Equals(value)) return;
            _data[key] = value;
            DataChanged = true;
            if(SaveWhenUpdated||!Application.isPlaying||forceSave) Save();
        }

        /// <summary>
        /// This method is used to read saved data from the save file.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="value">The data's value.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        /// <returns>True if the data exists and is the correct type, otherwise false.</returns>
        public static bool TryReadData<T>(string key, out T value) {
            return _data.TryGetCastValue(key, out value);
        }

        /// <summary>
        /// This method is used to read saved data from the save file.
        /// </summary>
        /// <param name="key">The data's key.</param>
        /// <param name="defaultValue">This value will be returned and added to the save file if no value exists.</param>
        /// <param name="forceSave">If true the data will be saved immediately if it has been updated.</param>
        /// <typeparam name="T">The type of data being stored.</typeparam>
        /// <returns>The value of the given key or the default value.</returns>
        public static T ReadData<T>(string key, T defaultValue = default, bool forceSave = false) {
            if(_data.TryGetCastValue<T>(key, out var value)) return value;
            StoreData(key,defaultValue,forceSave);
            return defaultValue;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}