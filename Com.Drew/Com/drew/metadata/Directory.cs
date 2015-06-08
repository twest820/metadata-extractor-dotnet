/*
 * Copyright 2002-2015 Drew Noakes
 *
 *    Modified by Yakov Danilov <yakodani@gmail.com> for Imazen LLC (Ported from Java to C#)
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * More information about this project is available at:
 *
 *    https://drewnoakes.com/code/exif/
 *    https://github.com/drewnoakes/metadata-extractor
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Com.Drew.Lang;
using JetBrains.Annotations;
using Sharpen;

namespace Com.Drew.Metadata
{
    /// <summary>
    /// Abstract base class for all directory implementations, having methods for getting and setting tag values of various
    /// data types.
    /// </summary>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public abstract class Directory
    {
        /// <summary>Map of values hashed by type identifiers.</summary>
        [NotNull]
        protected readonly IDictionary<int?, object> TagMap = new Dictionary<int?, object>();

        /// <summary>A convenient list holding tag values in the order in which they were stored.</summary>
        /// <remarks>
        /// A convenient list holding tag values in the order in which they were stored.
        /// This is used for creation of an iterator, and for counting the number of
        /// defined tags.
        /// </remarks>
        [NotNull]
        protected readonly ICollection<Tag> DefinedTagList = new AList<Tag>();

        [NotNull]
        private readonly ICollection<string> _errorList = new AList<string>(4);

        /// <summary>The descriptor used to interpret tag values.</summary>
        protected ITagDescriptor Descriptor;

        // ABSTRACT METHODS
        /// <summary>Provides the name of the directory, for display purposes.</summary>
        /// <remarks>Provides the name of the directory, for display purposes.  E.g. <c>Exif</c></remarks>
        /// <returns>the name of the directory</returns>
        [NotNull]
        public abstract string GetName();

        /// <summary>Provides the map of tag names, hashed by tag type identifier.</summary>
        /// <returns>the map of tag names</returns>
        [NotNull]
        protected abstract Dictionary<int?, string> GetTagNameMap();

        protected Directory()
        {
        }

        // VARIOUS METHODS
        /// <summary>Gets a value indicating whether the directory is empty, meaning it contains no errors and no tag values.</summary>
        public virtual bool IsEmpty()
        {
            return _errorList.IsEmpty() && DefinedTagList.IsEmpty();
        }

        /// <summary>Indicates whether the specified tag type has been set.</summary>
        /// <param name="tagType">the tag type to check for</param>
        /// <returns>true if a value exists for the specified tag type, false if not</returns>
        public virtual bool ContainsTag(int tagType)
        {
            return TagMap.ContainsKey(Extensions.ValueOf(tagType));
        }

        /// <summary>Returns an Iterator of Tag instances that have been set in this Directory.</summary>
        /// <returns>an Iterator of Tag instances</returns>
        [NotNull]
        public virtual ICollection<Tag> GetTags()
        {
            return Collections.UnmodifiableCollection(DefinedTagList);
        }

        /// <summary>Returns the number of tags set in this Directory.</summary>
        /// <returns>the number of tags set in this Directory</returns>
        public virtual int GetTagCount()
        {
            return DefinedTagList.Count;
        }

        /// <summary>Sets the descriptor used to interpret tag values.</summary>
        /// <param name="descriptor">the descriptor used to interpret tag values</param>
        public virtual void SetDescriptor([NotNull] ITagDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("cannot set a null descriptor");
            }
            Descriptor = descriptor;
        }

        /// <summary>Registers an error message with this directory.</summary>
        /// <param name="message">an error message.</param>
        public virtual void AddError([NotNull] string message)
        {
            _errorList.Add(message);
        }

        /// <summary>Gets a value indicating whether this directory has any error messages.</summary>
        /// <returns>true if the directory contains errors, otherwise false</returns>
        public virtual bool HasErrors()
        {
            return _errorList.Count > 0;
        }

        /// <summary>Used to iterate over any error messages contained in this directory.</summary>
        /// <returns>an iterable collection of error message strings.</returns>
        [NotNull]
        public virtual IEnumerable<string> GetErrors()
        {
            return Collections.UnmodifiableCollection(_errorList);
        }

        /// <summary>Returns the count of error messages in this directory.</summary>
        public virtual int GetErrorCount()
        {
            return _errorList.Count;
        }

        // TAG SETTERS
        /// <summary>Sets an <c>int</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as an int</param>
        public virtual void SetInt(int tagType, int value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets an <c>int[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="ints">the int array to store</param>
        public virtual void SetIntArray(int tagType, [NotNull] int[] ints)
        {
            SetObjectArray(tagType, ints);
        }

        /// <summary>Sets a <c>float</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a float</param>
        public virtual void SetFloat(int tagType, float value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>float[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="floats">the float array to store</param>
        public virtual void SetFloatArray(int tagType, [NotNull] float[] floats)
        {
            SetObjectArray(tagType, floats);
        }

        /// <summary>Sets a <c>double</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a double</param>
        public virtual void SetDouble(int tagType, double value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>double[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="doubles">the double array to store</param>
        public virtual void SetDoubleArray(int tagType, [NotNull] double[] doubles)
        {
            SetObjectArray(tagType, doubles);
        }

        /// <summary>Sets a <c>String</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a String</param>
        public virtual void SetString(int tagType, [NotNull] string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("cannot set a null String");
            }
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>String[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="strings">the String array to store</param>
        public virtual void SetStringArray(int tagType, [NotNull] string[] strings)
        {
            SetObjectArray(tagType, strings);
        }

        /// <summary>Sets a <c>boolean</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a boolean</param>
        public virtual void SetBoolean(int tagType, bool value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>long</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a long</param>
        public virtual void SetLong(int tagType, long value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>java.util.Date</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag as a java.util.Date</param>
        public virtual void SetDate(int tagType, DateTime value)
        {
            SetObject(tagType, value);
        }

        /// <summary>Sets a <c>Rational</c> value for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="rational">rational number</param>
        public virtual void SetRational(int tagType, [NotNull] Rational rational)
        {
            SetObject(tagType, rational);
        }

        /// <summary>Sets a <c>Rational[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="rationals">the Rational array to store</param>
        public virtual void SetRationalArray(int tagType, [NotNull] Rational[] rationals)
        {
            SetObjectArray(tagType, rationals);
        }

        /// <summary>Sets a <c>byte[]</c> (array) for the specified tag.</summary>
        /// <param name="tagType">the tag identifier</param>
        /// <param name="bytes">the byte array to store</param>
        public virtual void SetByteArray(int tagType, [NotNull] sbyte[] bytes)
        {
            SetObjectArray(tagType, bytes);
        }

        /// <summary>Sets a <c>Object</c> for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="value">the value for the specified tag</param>
        /// <exception cref="System.ArgumentNullException">if value is <c>null</c></exception>
        public virtual void SetObject(int tagType, [NotNull] object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("cannot set a null object");
            }
            if (!TagMap.ContainsKey(Extensions.ValueOf(tagType)))
            {
                DefinedTagList.Add(new Tag(tagType, this));
            }
            //        else {
            //            final Object oldValue = _tagMap.get(tagType);
            //            if (!oldValue.equals(value))
            //                addError(String.format("Overwritten tag 0x%s (%s).  Old=%s, New=%s", Integer.toHexString(tagType), getTagName(tagType), oldValue, value));
            //        }
            TagMap.Put(tagType, value);
        }

        /// <summary>Sets an array <c>Object</c> for the specified tag.</summary>
        /// <param name="tagType">the tag's value as an int</param>
        /// <param name="array">the array of values for the specified tag</param>
        public virtual void SetObjectArray(int tagType, [NotNull] object array)
        {
            // for now, we don't do anything special -- this method might be a candidate for removal once the dust settles
            SetObject(tagType, array);
        }

        // TAG GETTERS
        /// <summary>Returns the specified tag's value as an int, if possible.</summary>
        /// <remarks>
        /// Returns the specified tag's value as an int, if possible.  Every attempt to represent the tag's value as an int
        /// is taken.  Here is a list of the action taken depending upon the tag's original type:
        /// <list type="bullet">
        /// <item> int - Return unchanged.
        /// <item> Number - Return an int value (real numbers are truncated).
        /// <item> Rational - Truncate any fractional part and returns remaining int.
        /// <item> String - Attempt to parse string as an int.  If this fails, convert the char[] to an int (using shifts and OR).
        /// <item> Rational[] - Return int value of first item in array.
        /// <item> byte[] - Return int value of first item in array.
        /// <item> int[] - Return int value of first item in array.
        /// </list>
        /// </remarks>
        /// <exception cref="MetadataException">if no value exists for tagType or if it cannot be converted to an int.</exception>
        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public virtual int GetInt(int tagType)
        {
            int? integer = GetInteger(tagType);
            if (integer != null)
            {
                return (int)integer;
            }
            object o = GetObject(tagType);
            if (o == null)
            {
                throw new MetadataException("Tag '" + GetTagName(tagType) + "' has not been set -- check using containsTag() first");
            }
            throw new MetadataException("Tag '" + tagType + "' cannot be converted to int.  It is of type '" + o.GetType() + "'.");
        }

        /// <summary>Returns the specified tag's value as an Integer, if possible.</summary>
        /// <remarks>
        /// Returns the specified tag's value as an Integer, if possible.  Every attempt to represent the tag's value as an
        /// Integer is taken.  Here is a list of the action taken depending upon the tag's original type:
        /// <list type="bullet">
        /// <item> int - Return unchanged
        /// <item> Number - Return an int value (real numbers are truncated)
        /// <item> Rational - Truncate any fractional part and returns remaining int
        /// <item> String - Attempt to parse string as an int.  If this fails, convert the char[] to an int (using shifts and OR)
        /// <item> Rational[] - Return int value of first item in array if length &gt; 0
        /// <item> byte[] - Return int value of first item in array if length &gt; 0
        /// <item> int[] - Return int value of first item in array if length &gt; 0
        /// </list>
        /// If the value is not found or cannot be converted to int, <c>null</c> is returned.
        /// </remarks>
        [CanBeNull]
        public virtual int? GetInteger(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            if (o.IsNumber())
            {
                return Number.GetInstance(o).IntValue();
            }
            var value = o as string;
            if (value != null)
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch (FormatException)
                {
                    // convert the char array to an int
                    string s = value;
                    sbyte[] bytes = Runtime.GetBytesForString(s);
                    long val = 0;
                    foreach (sbyte aByte in bytes)
                    {
                        val = val << 8;
                        val += (aByte & unchecked((int)(0xff)));
                    }
                    return (int)val;
                }
            }
            var rationals = o as Rational[];
            if (rationals != null)
            {
                if (rationals.Length == 1)
                {
                    return rationals[0].IntValue();
                }
            }
            else
            {
                var bytes = o as sbyte[];
                if (bytes != null)
                {
                    if (bytes.Length == 1)
                    {
                        return (int)bytes[0];
                    }
                }
                else
                {
                    var ints = o as int[];
                    if (ints != null)
                    {
                        if (ints.Length == 1)
                        {
                            return ints[0];
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>Gets the specified tag's value as a String array, if possible.</summary>
        /// <remarks>
        /// Gets the specified tag's value as a String array, if possible.  Only supported
        /// where the tag is set as String[], String, int[], byte[] or Rational[].
        /// </remarks>
        /// <param name="tagType">the tag identifier</param>
        /// <returns>the tag's value as an array of Strings. If the value is unset or cannot be converted, <c>null</c> is returned.</returns>
        [CanBeNull]
        public virtual string[] GetStringArray(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var strings = o as string[];
            if (strings != null)
            {
                return strings;
            }
            var s = o as string;
            if (s != null)
            {
                return new string[] { s };
            }
            var ints = o as int[];
            if (ints != null)
            {
                strings = new string[ints.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i] = Extensions.ConvertToString(ints[i]);
                }
                return strings;
            }
            var bytes = o as sbyte[];
            if (bytes != null)
            {
                strings = new string[bytes.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i] = Extensions.ConvertToString(bytes[i]);
                }
                return strings;
            }
            var rationals = o as Rational[];
            if (rationals != null)
            {
                strings = new string[rationals.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i] = rationals[i].ToSimpleString(false);
                }
                return strings;
            }
            return null;
        }

        /// <summary>Gets the specified tag's value as an int array, if possible.</summary>
        /// <remarks>
        /// Gets the specified tag's value as an int array, if possible.  Only supported
        /// where the tag is set as String, Integer, int[], byte[] or Rational[].
        /// </remarks>
        /// <param name="tagType">the tag identifier</param>
        /// <returns>the tag's value as an int array</returns>
        [CanBeNull]
        public virtual int[] GetIntArray(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var ints = o as int[];
            if (ints != null)
            {
                return ints;
            }
            var rationals = o as Rational[];
            if (rationals != null)
            {
                ints = new int[rationals.Length];
                for (int i = 0; i < ints.Length; i++)
                {
                    ints[i] = rationals[i].IntValue();
                }
                return ints;
            }
            var shorts = o as short[];
            if (shorts != null)
            {
                ints = new int[shorts.Length];
                for (int i = 0; i < shorts.Length; i++)
                {
                    ints[i] = shorts[i];
                }
                return ints;
            }
            var bytes = o as sbyte[];
            if (bytes != null)
            {
                ints = new int[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    ints[i] = bytes[i];
                }
                return ints;
            }
            var str = o as CharSequence;
            if (str != null)
            {
                ints = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    ints[i] = str[i];
                }
                return ints;
            }
            var nullableInt = o as int?;
            if (nullableInt != null)
            {
                return new int[] { (int)o };
            }
            return null;
        }

        /// <summary>Gets the specified tag's value as an byte array, if possible.</summary>
        /// <remarks>
        /// Gets the specified tag's value as an byte array, if possible.  Only supported
        /// where the tag is set as String, Integer, int[], byte[] or Rational[].
        /// </remarks>
        /// <param name="tagType">the tag identifier</param>
        /// <returns>the tag's value as a byte array</returns>
        [CanBeNull]
        public virtual sbyte[] GetByteArray(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            sbyte[] bytes;

            var rationals = o as Rational[];
            if (rationals != null)
            {
                bytes = new sbyte[rationals.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = rationals[i].ByteValue();
                }
                return bytes;
            }
            bytes = o as sbyte[];
            if (bytes != null)
            {
                return bytes;
            }
            var ints = o as int[];
            if (ints != null)
            {
                bytes = new sbyte[ints.Length];
                for (int i = 0; i < ints.Length; i++)
                {
                    bytes[i] = unchecked((sbyte)ints[i]);
                }
                return bytes;
            }
            var shorts = o as short[];
            if (shorts != null)
            {
                bytes = new sbyte[shorts.Length];
                for (int i = 0; i < shorts.Length; i++)
                {
                    bytes[i] = unchecked((sbyte)shorts[i]);
                }
                return bytes;
            }
            var str = o as CharSequence;
            if (str != null)
            {
                bytes = new sbyte[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    bytes[i] = unchecked((sbyte)str[i]);
                }
                return bytes;
            }
            var nullableInt = o as int?;
            if (nullableInt != null)
            {
                return new sbyte[] { nullableInt.ByteValue() };
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a double, if possible.</summary>
        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public virtual double GetDouble(int tagType)
        {
            double? value = GetDoubleObject(tagType);
            if (value != null)
            {
                return (double)value;
            }
            object o = GetObject(tagType);
            if (o == null)
            {
                throw new MetadataException("Tag '" + GetTagName(tagType) + "' has not been set -- check using containsTag() first");
            }
            throw new MetadataException("Tag '" + tagType + "' cannot be converted to a double.  It is of type '" + o.GetType() + "'.");
        }

        /// <summary>Returns the specified tag's value as a Double.</summary>
        /// <remarks>Returns the specified tag's value as a Double.  If the tag is not set or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual double? GetDoubleObject(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var s = o as string;
            if (s != null)
            {
                try
                {
                    return double.Parse(s);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            if (o.IsNumber())
            {
                return Number.GetInstance(o).DoubleValue();
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a float, if possible.</summary>
        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public virtual float GetFloat(int tagType)
        {
            float? value = GetFloatObject(tagType);
            if (value != null)
            {
                return (float)value;
            }
            object o = GetObject(tagType);
            if (o == null)
            {
                throw new MetadataException("Tag '" + GetTagName(tagType) + "' has not been set -- check using containsTag() first");
            }
            throw new MetadataException("Tag '" + tagType + "' cannot be converted to a float.  It is of type '" + o.GetType() + "'.");
        }

        /// <summary>Returns the specified tag's value as a float.</summary>
        /// <remarks>Returns the specified tag's value as a float.  If the tag is not set or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual float? GetFloatObject(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var s = o as string;
            if (s != null)
            {
                try
                {
                    return float.Parse(s);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            if (o.IsNumber())
            {
                return Number.GetInstance(o).FloatValue();
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a long, if possible.</summary>
        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public virtual long GetLong(int tagType)
        {
            long? value = GetLongObject(tagType);
            if (value != null)
            {
                return (long)value;
            }
            object o = GetObject(tagType);
            if (o == null)
            {
                throw new MetadataException("Tag '" + GetTagName(tagType) + "' has not been set -- check using containsTag() first");
            }
            throw new MetadataException("Tag '" + tagType + "' cannot be converted to a long.  It is of type '" + o.GetType() + "'.");
        }

        /// <summary>Returns the specified tag's value as a long.</summary>
        /// <remarks>Returns the specified tag's value as a long.  If the tag is not set or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual long? GetLongObject(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var s = o as string;
            if (s != null)
            {
                try
                {
                    return Convert.ToInt64(s);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            if (o.IsNumber())
            {
                return Number.GetInstance(o).LongValue();
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a boolean, if possible.</summary>
        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public virtual bool GetBoolean(int tagType)
        {
            bool? value = GetBooleanObject(tagType);
            if (value != null)
            {
                return (bool)value;
            }
            object o = GetObject(tagType);
            if (o == null)
            {
                throw new MetadataException("Tag '" + GetTagName(tagType) + "' has not been set -- check using containsTag() first");
            }
            throw new MetadataException("Tag '" + tagType + "' cannot be converted to a boolean.  It is of type '" + o.GetType() + "'.");
        }

        /// <summary>Returns the specified tag's value as a boolean.</summary>
        /// <remarks>Returns the specified tag's value as a boolean.  If the tag is not set or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual bool? GetBooleanObject(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var b = o as bool?;
            if (b != null)
            {
                return b;
            }
            var s = o as string;
            if (s != null)
            {
                try
                {
                    return bool.Parse(s);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            if (o.IsNumber())
            {
                return Number.GetInstance(o).DoubleValue() != 0;
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a java.util.Date.</summary>
        /// <remarks>
        /// Returns the specified tag's value as a java.util.Date.  If the value is unset or cannot be converted, <c>null</c> is returned.
        /// <para>
        /// If the underlying value is a <see cref="string"/>, then attempts will be made to parse the string as though it is in
        /// the current <see cref="System.TimeZoneInfo"/>.  If the <see cref="System.TimeZoneInfo"/>
        /// is known, call the overload that accepts one as an argument.
        /// </remarks>
        [CanBeNull]
        public virtual DateTime? GetDate(int tagType)
        {
            return GetDate(tagType, null);
        }

        /// <summary>Returns the specified tag's value as a java.util.Date.</summary>
        /// <remarks>
        /// Returns the specified tag's value as a java.util.Date.  If the value is unset or cannot be converted, <c>null</c> is returned.
        /// <para>
        /// If the underlying value is a <see cref="string"/>, then attempts will be made to parse the string as though it is in
        /// the <see cref="System.TimeZoneInfo"/> represented by the <paramref name="timeZone"/> parameter (if it is non-null).  Note that this parameter
        /// is only considered if the underlying value is a string and parsing occurs, otherwise it has no effect.
        /// </remarks>
        [CanBeNull]
        public virtual DateTime? GetDate(int tagType, [CanBeNull] TimeZoneInfo timeZone)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            if (o is DateTime)
            {
                return (DateTime)o;
            }
            var s = o as string;
            if (s != null)
            {
                // This seems to cover all known Exif date strings
                // Note that "    :  :     :  :  " is a valid date string according to the Exif spec (which means 'unknown date'): http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif/datetimeoriginal.html
                string[] datePatterns = new string[] { "yyyy:MM:dd HH:mm:ss", "yyyy:MM:dd HH:mm", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy.MM.dd HH:mm:ss", "yyyy.MM.dd HH:mm" };
                string dateString = s;
                foreach (string datePattern in datePatterns)
                {
                    try
                    {
                        DateFormat parser = new SimpleDateFormat(datePattern);
                        if (timeZone != null)
                        {
                            parser.SetTimeZone(timeZone);
                        }
                        return parser.Parse(dateString);
                    }
                    catch (ParseException)
                    {
                    }
                }
            }
            // simply try the next pattern
            return null;
        }

        /// <summary>Returns the specified tag's value as a Rational.</summary>
        /// <remarks>Returns the specified tag's value as a Rational.  If the value is unset or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual Rational GetRational(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var rational = o as Rational;
            if (rational != null)
            {
                return rational;
            }
            if (o is int?)
            {
                return new Rational((int)o, 1);
            }
            if (o is long?)
            {
                return new Rational((long)o, 1);
            }
            // NOTE not doing conversions for real number types
            return null;
        }

        /// <summary>Returns the specified tag's value as an array of Rational.</summary>
        /// <remarks>Returns the specified tag's value as an array of Rational.  If the value is unset or cannot be converted, <c>null</c> is returned.</remarks>
        [CanBeNull]
        public virtual Rational[] GetRationalArray(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var rationals = o as Rational[];
            if (rationals != null)
            {
                return rationals;
            }
            return null;
        }

        /// <summary>Returns the specified tag's value as a String.</summary>
        /// <remarks>
        /// Returns the specified tag's value as a String.  This value is the 'raw' value.  A more presentable decoding
        /// of this value may be obtained from the corresponding Descriptor.
        /// </remarks>
        /// <returns>
        /// the String representation of the tag's value, or
        /// <c>null</c> if the tag hasn't been defined.
        /// </returns>
        [CanBeNull]
        public virtual string GetString(int tagType)
        {
            object o = GetObject(tagType);
            if (o == null)
            {
                return null;
            }
            var rational = o as Rational;
            if (rational != null)
            {
                return rational.ToSimpleString(true);
            }
            if (o is DateTime)
            {
                return Extensions.ConvertToString((DateTime)o);
            }
            if (o is bool)
            {
                return (bool)o ? "true" : "false";
            }
            if (o.GetType().IsArray)
            {
                // handle arrays of objects and primitives
                int arrayLength = Runtime.GetArrayLength(o);
                Type componentType = o.GetType().GetElementType();
                bool isObjectArray = typeof(object).IsAssignableFrom(componentType);
                bool isFloatArray = componentType.FullName.Equals("float");
                bool isDoubleArray = componentType.FullName.Equals("double");
                bool isIntArray = componentType.FullName.Equals("int");
                bool isLongArray = componentType.FullName.Equals("long");
                bool isByteArray = componentType.FullName.Equals("byte");
                bool isShortArray = componentType.FullName.Equals("short");
                StringBuilder @string = new StringBuilder();
                for (int i = 0; i < arrayLength; i++)
                {
                    if (i != 0)
                    {
                        @string.Append(' ');
                    }
                    if (isObjectArray)
                    {
                        @string.Append(Extensions.ConvertToString(Runtime.GetArrayValue(o, i)));
                    }
                    else
                    {
                        if (isIntArray)
                        {
                            @string.Append(Runtime.GetInt(o, i));
                        }
                        else
                        {
                            if (isShortArray)
                            {
                                @string.Append(Runtime.GetShort(o, i));
                            }
                            else
                            {
                                if (isLongArray)
                                {
                                    @string.Append(Runtime.GetLong(o, i));
                                }
                                else
                                {
                                    if (isFloatArray)
                                    {
                                        @string.Append(Runtime.GetFloat(o, i));
                                    }
                                    else
                                    {
                                        if (isDoubleArray)
                                        {
                                            @string.Append(Runtime.GetDouble(o, i));
                                        }
                                        else
                                        {
                                            if (isByteArray)
                                            {
                                                @string.Append(Runtime.GetByte(o, i));
                                            }
                                            else
                                            {
                                                AddError("Unexpected array component type: " + componentType.FullName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return Extensions.ConvertToString(@string);
            }
            // Note that several cameras leave trailing spaces (Olympus, Nikon) but this library is intended to show
            // the actual data within the file.  It is not inconceivable that whitespace may be significant here, so we
            // do not trim.  Also, if support is added for writing data back to files, this may cause issues.
            // We leave trimming to the presentation layer.
            return Extensions.ConvertToString(o);
        }

        [CanBeNull]
        public virtual string GetString(int tagType, string charset)
        {
            sbyte[] bytes = GetByteArray(tagType);
            if (bytes == null)
            {
                return null;
            }
            try
            {
                return Runtime.GetStringForBytes(bytes, charset);
            }
            catch (UnsupportedEncodingException)
            {
                return null;
            }
        }

        /// <summary>Returns the object hashed for the particular tag type specified, if available.</summary>
        /// <param name="tagType">the tag type identifier</param>
        /// <returns>the tag's value as an Object if available, else <c>null</c></returns>
        [CanBeNull]
        public virtual object GetObject(int tagType)
        {
            return TagMap.Get(Extensions.ValueOf(tagType));
        }

        // OTHER METHODS
        /// <summary>Returns the name of a specified tag as a String.</summary>
        /// <param name="tagType">the tag type identifier</param>
        /// <returns>the tag's name as a String</returns>
        [NotNull]
        public virtual string GetTagName(int tagType)
        {
            Dictionary<int?, string> nameMap = GetTagNameMap();
            if (!nameMap.ContainsKey(tagType))
            {
                string hex = Extensions.ToHexString(tagType);
                while (hex.Length < 4)
                {
                    hex = "0" + hex;
                }
                return "Unknown tag (0x" + hex + ")";
            }
            return nameMap.Get(tagType);
        }

        /// <summary>Gets whether the specified tag is known by the directory and has a name.</summary>
        /// <param name="tagType">the tag type identifier</param>
        /// <returns>whether this directory has a name for the specified tag</returns>
        public virtual bool HasTagName(int tagType)
        {
            return GetTagNameMap().ContainsKey(tagType);
        }

        /// <summary>
        /// Provides a description of a tag's value using the descriptor set by
        /// <c>setDescriptor(Descriptor)</c>.
        /// </summary>
        /// <param name="tagType">the tag type identifier</param>
        /// <returns>the tag value's description as a String</returns>
        [CanBeNull]
        public virtual string GetDescription(int tagType)
        {
            Debug.Assert((Descriptor != null));
            return Descriptor.GetDescription(tagType);
        }

        public override string ToString()
        {
            return Extensions.StringFormat("%s Directory (%d %s)", GetName(), TagMap.Count, TagMap.Count == 1 ? "tag" : "tags");
        }
    }
}