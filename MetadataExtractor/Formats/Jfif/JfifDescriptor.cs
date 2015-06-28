#region License
//
// Copyright 2002-2015 Drew Noakes
// Ported from Java to C# by Yakov Danilov for Imazen LLC in 2014
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// More information about this project is available at:
//
//    https://github.com/drewnoakes/metadata-extractor-dotnet
//    https://drewnoakes.com/code/exif/
//
#endregion

using JetBrains.Annotations;

namespace MetadataExtractor.Formats.Jfif
{
    /// <summary>Provides human-readable string versions of the tags stored in a JfifDirectory.</summary>
    /// <remarks>
    /// Provides human-readable string versions of the tags stored in a JfifDirectory.
    /// <para />
    /// More info at: http://en.wikipedia.org/wiki/JPEG_File_Interchange_Format
    /// </remarks>
    /// <author>Yuri Binev, Drew Noakes</author>
    public sealed class JfifDescriptor : TagDescriptor<JfifDirectory>
    {
        public JfifDescriptor([NotNull] JfifDirectory directory)
            : base(directory)
        {
        }

        public override string GetDescription(int tagType)
        {
            switch (tagType)
            {
                case JfifDirectory.TagResX:
                {
                    return GetImageResXDescription();
                }

                case JfifDirectory.TagResY:
                {
                    return GetImageResYDescription();
                }

                case JfifDirectory.TagVersion:
                {
                    return GetImageVersionDescription();
                }

                case JfifDirectory.TagUnits:
                {
                    return GetImageResUnitsDescription();
                }

                default:
                {
                    return base.GetDescription(tagType);
                }
            }
        }

        [CanBeNull]
        public string GetImageVersionDescription()
        {
            int value;
            if (!Directory.TryGetInt32(JfifDirectory.TagVersion, out value))
                return null;
            return string.Format("{0}.{1}",
                (value & 0xFF00) >> 8,
                 value & 0x00FF);
        }

        [CanBeNull]
        public string GetImageResYDescription()
        {
            int value;
            if (!Directory.TryGetInt32(JfifDirectory.TagResY, out value))
                return null;
            return string.Format("{0} dot{1}", value, value == 1 ? string.Empty : "s");
        }

        [CanBeNull]
        public string GetImageResXDescription()
        {
            int value;
            if (!Directory.TryGetInt32(JfifDirectory.TagResX, out value))
                return null;
            return string.Format("{0} dot{1}", value, value == 1 ? string.Empty : "s");
        }

        [CanBeNull]
        public string GetImageResUnitsDescription()
        {
            int value;
            if (!Directory.TryGetInt32(JfifDirectory.TagUnits, out value))
                return null;
            switch (value)
            {
                case 0:
                {
                    return "none";
                }

                case 1:
                {
                    return "inch";
                }

                case 2:
                {
                    return "centimetre";
                }

                default:
                {
                    return "unit";
                }
            }
        }
    }
}