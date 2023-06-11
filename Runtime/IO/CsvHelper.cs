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
using System.Linq;
using System.Text;
using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Amilious.Core.IO {
    
    /// <summary>
    /// This class is used to read and write to a csv file.
    /// </summary>
    public static class CsvHelper { 
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Regex ValueMatcher = new Regex(@"(?<=^|,)\s*""((?:[^""\\]|\\.)*)""(?=,|$)");
        private static readonly Encoding Encoding = Encoding.UTF8;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when loading a key pair file with duplicate keys.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">The duplicate key.</param>
        public delegate void DuplicateKeyDelegate(string path, string key);

        /// <summary>
        /// This method is called when a read or write error occurs.
        /// </summary>
        /// <param name="path">The path of the file where the error occured.</param>
        /// <param name="methodName">The name of the method where the error occured.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public delegate void ReadWriteErrorDelegate(string path, string methodName, Exception exception);

        /// <summary>
        /// This method is used to return the correct value for the duplicate key.
        /// </summary>
        /// <param name="key">The key that has duplicate values.</param>
        /// <param name="value1">One of the key's values.</param>
        /// <param name="value2">The next key value.</param>
        /// <returns>The correct value for the key.</returns>
        public delegate string ChooseValueDelegate(string key, string value1, string value2);

        /// <summary>
        /// This delegate is used for the method that will be called when reading key value pairs.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public delegate void ReadKeyValueDelegate(string key, string value);

        /// <summary>
        /// This delegate is used for the method that will be called when the values from a line are loaded.
        /// </summary>
        /// <param name="values">The values.</param>
        public delegate void ReadValuesDelegate(IEnumerable<string> values);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when loading key pairs if a duplicate key is found.
        /// </summary>
        public static event DuplicateKeyDelegate OnDuplicateKey;

        public static event ReadWriteErrorDelegate OnReadWriteError;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try load values from a <see cref="TextAsset"/>.
        /// </summary>
        /// <param name="asset">The <see cref="TextAsset"/> that you want to load the values from.</param>
        /// <param name="values">The values read from the asset.</param>
        /// <returns>True if the asset is not null and it was successfully read, otherwise false.</returns>
        public static bool TryLoadValues(TextAsset asset,out string[][] values) {
            if(asset == null) {
                values = null;
                return false;
            }
            var tmpVals = new List<string[]>();
            try {
                using var memoryStream = new MemoryStream(asset.bytes);
                using var streamReader = new StreamReader(memoryStream, Encoding);
                while(!streamReader.EndOfStream) {
                    tmpVals.Add(GetLineValues(streamReader.ReadLine()).ToArray());
                }

                streamReader.Close();
                memoryStream.Close();
            } catch(Exception e) {
                OnReadWriteError?.Invoke($"asset:{asset.name}",nameof(TryLoadValues),e);
                values = null;
                return false;
            }
            values = tmpVals.ToArray();
            return true;
        }

        /// <summary>
        /// This method is used to try load values from a <see cref="TextAsset"/>.
        /// </summary>
        /// <param name="asset">The <see cref="TextAsset"/> that you want to load the values from.</param>
        /// <param name="readValues">The method that will be called for each line of values.</param>
        /// <returns>True if the asset is not null and it was successfully read, otherwise false.</returns>
        public static bool TryLoadValues(TextAsset asset,ReadValuesDelegate readValues) {
            if(asset == null) {
                return false;
            }
            try {
                using var memoryStream = new MemoryStream(asset.bytes);
                using var streamReader = new StreamReader(memoryStream, Encoding);
                while(!streamReader.EndOfStream) {
                    readValues?.Invoke(GetLineValues(streamReader.ReadLine()).ToArray());
                }
                streamReader.Close();
                memoryStream.Close();
            } catch(Exception e) {
                OnReadWriteError?.Invoke($"asset:{asset.name}",nameof(TryLoadValues),e);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try load key values from a <see cref="TextAsset"/>.
        /// </summary>
        /// <param name="asset">The <see cref="TextAsset"/> that you want to load the key values from.</param>
        /// <param name="values">The values read from the file.</param>
        /// <param name="skipMissingValues">If true keys with no values will be skipped, otherwise they will use
        /// string.Empty.</param>
        /// <returns>True if the asset is not null and it was successfully read, otherwise false.</returns>
        public static bool TryLoadKeyValuePairs(TextAsset asset, out Dictionary<string, string> values, 
            bool skipMissingValues = false) {
            values = new Dictionary<string, string>();
            if(asset == null) return false;
            try {
                using var memoryStream = new MemoryStream(asset.bytes);
                using var streamReader = new StreamReader(memoryStream, Encoding);
                while(!streamReader.EndOfStream) {
                    var line = streamReader.ReadLine();
                    var tmp = GetLineValues(line).ToArray();
                    if(tmp.Length<1) continue; //no values on this line
                    if(skipMissingValues&&tmp.Length<2) continue;
                    values[tmp[0]] = tmp.Length>1?tmp[1] : string.Empty;
                }
                streamReader.Close();
                memoryStream.Close();
            } catch(Exception e) {
                OnReadWriteError?.Invoke($"asset:{asset.name}",nameof(TryLoadKeyValuePairs),e);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try load key values from a <see cref="TextAsset"/>.
        /// </summary>
        /// <param name="asset">The <see cref="TextAsset"/> that you want to load the key values from.</param>
        /// <param name="readKeyValue">The method that will be called for each key value pair.</param>
        /// <param name="skipMissingValues">If true keys with no values will be skipped, otherwise they will use
        /// string.Empty.</param>
        /// <returns>True if the asset is not null and it was successfully read, otherwise false.</returns>
        public static bool TryLoadKeyValuePairs(TextAsset asset, ReadKeyValueDelegate readKeyValue, 
            bool skipMissingValues = false) {
            if(asset == null) return false;
            try {
                using var memoryStream = new MemoryStream(asset.bytes);
                using var streamReader = new StreamReader(memoryStream, Encoding);
                while(!streamReader.EndOfStream) {
                    var line = streamReader.ReadLine();
                    var tmp = GetLineValues(line).ToArray();
                    if(tmp.Length<1) continue; //no values on this line
                    if(skipMissingValues&&tmp.Length<2) continue;
                    readKeyValue?.Invoke(tmp[0], tmp.Length > 1 ? tmp[1] : string.Empty);
                }
                streamReader.Close();
                memoryStream.Close();
            } catch(Exception e) {
                OnReadWriteError?.Invoke($"asset:{asset.name}",nameof(TryLoadKeyValuePairs),e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to try load values from the given path.
        /// </summary>
        /// <param name="path">The path you want to read values from.</param>
        /// <param name="values">The values read from the path.</param>
        /// <returns>True if the path is valid and it was successfully read, otherwise false.</returns>
        public static bool TryLoadValues(string path,out string[][] values) {
            if(!File.Exists(path)) {
                values = null;
                return false;
            }
            var tmpVals = new List<string[]>();
            try {
                using var streamReader = new StreamReader(path, Encoding);
                while(!streamReader.EndOfStream) {
                    tmpVals.Add(GetLineValues(streamReader.ReadLine()).ToArray());
                }

                streamReader.Close();
            }catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryLoadValues),e);
                values = null;
                return false;
            }
            values = tmpVals.ToArray();
            return true;
        }
        
        /// <summary>
        /// This method is used to try load values from the given path.
        /// </summary>
        /// <param name="path">The path you want to read values from.</param>
        /// <param name="readValues">The method that will be called for each line of values.</param>
        /// <returns>True if the path is valid and it was successfully read, otherwise false.</returns>
        public static bool TryLoadValues(string path,ReadValuesDelegate readValues) {
            if(!File.Exists(path)) {
                return false;
            }
            try {
                using var streamReader = new StreamReader(path, Encoding);
                while(!streamReader.EndOfStream) {
                    readValues?.Invoke(GetLineValues(streamReader.ReadLine()).ToArray());
                }
                streamReader.Close();
            }catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryLoadValues),e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to try load key pair values from the given path.
        /// </summary>
        /// <param name="path">The path that you want to read key values from.</param>
        /// <param name="values">The values read from the file.</param>
        /// <param name="skipMissingValues">If true keys with no values will be skipped, otherwise they will use
        /// string.Empty.</param>
        /// <returns>True if the path is valid and it was successfully read, otherwise false.</returns>
        public static bool TryLoadKeyValuePairs(string path, out Dictionary<string, string> values,
            bool skipMissingValues = false) {
            values = new Dictionary<string, string>();
            if(!File.Exists(path)) return false;
            try {
                using var streamReader = new StreamReader(path, Encoding);
                while(!streamReader.EndOfStream) {
                    var line = streamReader.ReadLine();
                    var tmp = GetLineValues(line).ToArray();
                    if(tmp.Length<1) continue; //no values on this line
                    if(skipMissingValues&&tmp.Length<2) continue;
                    if(values.ContainsKey(tmp[0])) OnDuplicateKey?.Invoke(path,tmp[0]);
                    values[tmp[0]] = tmp.Length>1?tmp[1] : string.Empty;
                }
                streamReader.Close();
            }catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryLoadKeyValuePairs),e);
                values = null;
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try load key pair values from the given path.
        /// </summary>
        /// <param name="path">The path that you want to read key values from.</param>
        /// <param name="readKeyValue">The method that will be called for each key value pair.</param>
        /// <param name="skipMissingValues">If true keys with no values will be skipped, otherwise they will use
        /// string.Empty.</param>
        /// <returns>True if the path is valid and it was successfully read, otherwise false.</returns>
        public static bool TryLoadKeyValuePairs(string path, ReadKeyValueDelegate readKeyValue,
            bool skipMissingValues = false) {
            if(!File.Exists(path)) return false;
            try {
                using var streamReader = new StreamReader(path, Encoding);
                while(!streamReader.EndOfStream) {
                    var line = streamReader.ReadLine();
                    var tmp = GetLineValues(line).ToArray();
                    if(tmp.Length<1) continue; //no values on this line
                    if(skipMissingValues&&tmp.Length<2) continue;
                    readKeyValue?.Invoke(tmp[0],tmp.Length>1?tmp[1] : string.Empty);
                }
                streamReader.Close();
            }catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryLoadKeyValuePairs),e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to try write values to a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="values">The values.</param>
        /// <returns>True if able to write the values to the file, otherwise false.</returns>
        public static bool TryWriteValues(string path, IEnumerable<ICollection<string>> values) {
            var dir = Path.GetDirectoryName(path);
            try {
                if(!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                using var writer = new StreamWriter(path, false, Encoding);
                //add the values
                foreach(var line in values)
                    writer.WriteLine(FormatLine(line));
                writer.Close();
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryWriteValues),e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to try write key values to a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="values">The keys and values.</param>
        /// <returns>True if able to write the keys and values to the file, otherwise false.</returns>
        public static bool TryWriteKeyValuePairs(string path, IDictionary<string, string> values) {
            var dir = Path.GetDirectoryName(path);
            var tmpVals = new List<string>();
            try {
                if(!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                using var writer = new StreamWriter(path, false, Encoding);
                //add the values
                foreach(var line in values) {
                    tmpVals.Clear();
                    tmpVals.Add(line.Key);
                    tmpVals.Add(line.Value ?? string.Empty);
                    writer.WriteLine(FormatLine(tmpVals));
                }

                writer.Close();
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryWriteKeyValuePairs),e);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try write key values to a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="values">The keys and values.</param>
        /// <param name="valueFunction">The function to convert the values to strings.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>True if able to write the keys and values to the file, otherwise false.</returns>
        public static bool TryWriteKeyValuePairs<T>(string path, IDictionary<string, T> values, 
            Func<T,string> valueFunction) {
            var dir = Path.GetDirectoryName(path);
            var tmpVals = new List<string>();
            try {
                if(!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                using var writer = new StreamWriter(path, false, Encoding);
                //add the values
                foreach(var line in values) {
                    tmpVals.Clear();
                    tmpVals.Add(line.Key);
                    var value = valueFunction != null ? valueFunction(line.Value) : line.Value.ToString();
                    tmpVals.Add(value ?? string.Empty);
                    writer.WriteLine(FormatLine(tmpVals));
                }

                writer.Close();
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryWriteKeyValuePairs),e);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try append the given values to the end of a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="values">The values for the line that you want to append to the file.</param>
        /// <returns>True if able to append the values as a line to the file, otherwise false.</returns>
        public static bool TryAppendValues(string path, ICollection<string> values) {
            var dir = Path.GetDirectoryName(path);
            try {
                if(!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                using var writer = new StreamWriter(path, true, Encoding);
                //add the values
                writer.WriteLine(FormatLine(values));
                writer.Close();
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryAppendValues),e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to try append a key value pair to the end of a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if able to append the key and value to the file, otherwise false.</returns>
        public static bool TryAppendKeyValuePair(string path, string key, string value) {
            var dir = Path.GetDirectoryName(path);
            var tmpVals = new List<string>();
            try {
                if(!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                using var writer = new StreamWriter(path, true, Encoding);
                //add the values
                tmpVals.Add(key);
                tmpVals.Add(value ?? string.Empty);
                writer.WriteLine(FormatLine(tmpVals));
                writer.Close();
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryAppendKeyValuePair), e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to remove the lines that contain the given keys.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">A key that you want to remove.</param>
        /// <returns>True if there are no issues modifying the file, otherwise false.</returns>
        public static bool TryRemoveKeys(string path, params string[] key) {
            var dir = Path.GetDirectoryName(path);
            if(!File.Exists(path)||!Directory.Exists(dir)) return false;
            var tmpPath = Path.Combine(dir, Path.GetFileName(path) + ".tmp");
            try {
                // create a temporary file
                using var writer = new StreamWriter(tmpPath, false, Encoding);
                // read from the original file and write to the temporary file, excluding the line with the key
                using var reader = new StreamReader(path, Encoding.Unicode);
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if(!key.Contains(GetLineKey(line))) writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
                // overwrite the original file with the temporary file
                File.Delete(path);
                File.Move(tmpPath, path);
            }
            catch (Exception e) { 
                OnReadWriteError?.Invoke(path,nameof(TryRemoveKeys), e);
                return false; 
            }
            return true;
        }

        /// <summary>
        /// This method is used to try clean up duplicate key values.
        /// </summary>
        /// <param name="path">The path of the file that you want to clean up.</param>
        /// <param name="chooseDuplicateValue">The method to handle duplicates.</param>
        /// <returns>True if able to clean up the duplicate keys, otherwise false.</returns>
        public static bool TryCleanDuplicateKeys(string path, ChooseValueDelegate chooseDuplicateValue) {
            var values = new Dictionary<string, string>();
            if(!File.Exists(path)) return false;
            var hasDuplicates = false;
            try {
                using var streamReader = new StreamReader(path, Encoding);
                while(!streamReader.EndOfStream) {
                    var line = streamReader.ReadLine();
                    if(TryGetLineKeyValue(line, out var key, out var value)) continue;
                    if(values.TryGetValueFix(key, out var oldValue)) {
                        values[key] = chooseDuplicateValue(key, oldValue, value);
                        hasDuplicates = true;
                    } else {
                        values[key] = value;
                    }
                }
                streamReader.Close();
                if(!hasDuplicates) return true;
                using var writer = new StreamWriter(path, false, Encoding);
                //add the values
                foreach(var line in values) {
                    writer.WriteLine(FormatLine(line.Key,line.Value));
                }
                writer.Close();
            } catch(Exception e) {
                OnReadWriteError?.Invoke(path, nameof(TryCleanDuplicateKeys), e);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// This method is used to try edit the value of a key value pair.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">The key that you want to edit.</param>
        /// <param name="value">The new value of the key.</param>
        /// <returns>True if the file was edited, otherwise false.</returns>
        public static bool TryEditValue(string path, string key, string value) {
            var dir = Path.GetDirectoryName(path);
            if(!File.Exists(path)||!Directory.Exists(dir)) return false;
            var tmpPath = Path.Combine(dir, Path.GetFileName(path) + ".tmp");
            var edited = false;
            try {
                // create a temporary file
                using var writer = new StreamWriter(tmpPath, false, Encoding);
                // read from the original file and write to the temporary file, excluding the line with the key
                using var reader = new StreamReader(path, Encoding.Unicode);
                while(!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if(GetLineKey(line) == key) {
                        writer.WriteLine(FormatLine(key, value));
                        edited = true;
                    }
                    else writer.WriteLine(line);
                }

                reader.Close();
                writer.Close();
                if(edited) {
                    // overwrite the original file with the temporary file
                    File.Delete(path);
                    File.Move(tmpPath, path);
                }
                else {
                    //delete the temp file
                    File.Delete(tmpPath);
                }
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryEditValue), e);
                return false;
            }
            return edited;
        }

        /// <summary>
        /// This method is used to try rename the key of a key value pair.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="oldKeyName">The key that you want to edit.</param>
        /// <param name="newKeyName">The new key name of the key value pair.</param>
        /// <returns>True if the file was edited, otherwise false.</returns>
        public static bool TryEditKeyName(string path, string oldKeyName, string newKeyName) {
            var dir = Path.GetDirectoryName(path);
            if(!File.Exists(path)||!Directory.Exists(dir)) return false;
            var tmpPath = Path.Combine(dir, Path.GetFileName(path) + ".tmp");
            var edited = false;
            try {
                // create a temporary file
                using var writer = new StreamWriter(tmpPath, false, Encoding);
                // read from the original file and write to the temporary file, excluding the line with the key
                using var reader = new StreamReader(path, Encoding.Unicode);
                while(!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if(TryGetLineKeyValue(line,out var lineKey, out var lineValue) && lineKey == oldKeyName) {
                        writer.WriteLine(FormatLine(newKeyName, lineValue));
                        edited = true;
                    }
                    else writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
                if(edited) {
                    // overwrite the original file with the temporary file
                    File.Delete(path);
                    File.Move(tmpPath, path);
                }
                else {
                    //delete the temp file
                    File.Delete(tmpPath);
                }
            }
            catch(Exception e) {
                OnReadWriteError?.Invoke(path,nameof(TryEditKeyName), e);
                return false;
            }
            return edited;
        }

        /// <summary>
        /// This method is used to try edit a key if it exists otherwise it adds the key to the end of the file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">The key that you want to edit or add.</param>
        /// <param name="value">The new value of the key.</param>
        /// <returns>True if the value was updated or added!</returns>
        public static bool TryEditAddValue(string path, string key, string value) {
            if(TryEditValue(path, key, value)) return true;
            return TryAppendKeyValuePair(path, key, value);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to read the key and value from a line of text.
        /// </summary>
        /// <param name="line">The text for the line that you want to get values from.</param>
        /// <returns>A collection of strings that exist in the line.</returns>
        private static IEnumerable<string> GetLineValues(string line) {
            foreach (Match match in ValueMatcher.Matches(line)) {
                yield return DesanitizeValue(match.Groups[1].Value.Trim('"'));
            }
        }

        /// <summary>
        /// This method is used to get the key and value for the given line if it exists.
        /// </summary>
        /// <param name="line">The line that you want to get the key and value from.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the line contains a key, otherwise false.</returns>
        private static bool TryGetLineKeyValue(string line, out string key, out string value) {
            var values = GetLineValues(line).ToArray();
            if(values.Length < 1) {
                key = null;
                value = null;
                return false;
            }
            key = values[0];
            value = values.Length > 1 ? values[1] : string.Empty;
            return true;
        }

        /// <summary>
        /// This method is used to get the first value in the line.
        /// </summary>
        /// <param name="line">The text line that you want to get the key from.</param>
        /// <returns>The key of the given line, otherwise null.</returns>
        private static string GetLineKey(string line) {
            var match = ValueMatcher.Match(line);
            return match.Success ? DesanitizeValue(match.Groups[1].Value.Trim('"')) : null;
        }

        /// <summary>
        /// This method is used to clean values by adding backslash escapes.
        /// </summary>
        /// <param name="value">The value that you want to clean.</param>
        /// <returns>The cleaned value.</returns>
        private static string SanitizeValue(string value) {
            if(string.IsNullOrEmpty(value)) return value;
            var builder = StringBuilderPool.Rent;
            foreach (char c in value) {
                if (c == '\\') builder.Append("\\\\");
                else if (c == '\"') builder.Append("\\\"");
                else if(c == '\n') builder.Append(@"\n");
                else if(c == '\t') builder.Append(@"\t");
                else if(c == '\r') builder.Append(@"\r");
                else if(c == '\f') builder.Append(@"\f");
                else builder.Append(c);
            }
            return builder.ToStringAndReturnToPool();
        }
        
        /// <summary>
        /// This method is used to desanitize a read value.
        /// </summary>
        /// <param name="sanitizedValue">The value that you want to desanitize.</param>
        /// <returns>The desanitized string.</returns>
        private static string DesanitizeValue(string sanitizedValue) {
            if (string.IsNullOrEmpty(sanitizedValue)) return sanitizedValue;
            var builder = StringBuilderPool.Rent;
            var index = 0;
            while (index < sanitizedValue.Length) {
                var currentChar = sanitizedValue[index];
                if (currentChar == '\\') {
                    if (index + 1 < sanitizedValue.Length) {
                        var nextChar = sanitizedValue[index + 1];
                        if (nextChar == '\\') { builder.Append('\\'); index += 2; }
                        else if (nextChar == '\"') { builder.Append('\"'); index += 2; }
                        else if (nextChar == 'n') { builder.Append('\n'); index += 2; }
                        else if(nextChar == 't') { builder.Append('\t'); index += 2; }
                        else if(nextChar == 'r') { builder.Append('\r'); index += 2; }
                        else if(nextChar == 'f') { builder.Append('\f'); index += 2; }
                        else { builder.Append(currentChar); index++; }
                    } else { builder.Append(currentChar); index++; }
                } else { builder.Append(currentChar); index++; }
            }
            return builder.ToStringAndReturnToPool();
        }

        /// <summary>
        /// This method is used to format a line to be written to a file.
        /// </summary>
        /// <param name="values">The values for the line.</param>
        /// <returns>The formatted line text.</returns>
        private static string FormatLine(params string[] values) {
            var sb = StringBuilderPool.Rent;
            var first = true;
            foreach(var value in values) {
                if(!first) sb.Append(", ");
                sb.Append("\"").Append(SanitizeValue(value)).Append("\"");
                first = false;
            }
            return sb.ToStringAndReturnToPool();
        }
        
        /// <summary>
        /// This method is used to format a line to be written to a file.
        /// </summary>
        /// <param name="values">The values for the line.</param>
        /// <returns>The formatted line text.</returns>
        private static string FormatLine(ICollection<string> values) {
            var sb = StringBuilderPool.Rent;
            var first = true;
            foreach(var value in values) {
                if(!first) sb.Append(", ");
                sb.Append("\"").Append(SanitizeValue(value)).Append("\"");
                first = false;
            }
            return sb.ToStringAndReturnToPool();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}