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
using JetBrains.Annotations;
using Sharpen;

namespace Com.Drew.Imaging.Jpeg
{
    /// <summary>An enumeration of the known segment types found in JPEG files.</summary>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    [Serializable]
    public sealed class JpegSegmentType
    {
        /// <summary>APP0 JPEG segment identifier -- JFIF data (also JFXX apparently).</summary>
        public static readonly JpegSegmentType App0 = new JpegSegmentType(unchecked((sbyte)0xE0), true);

        /// <summary>APP1 JPEG segment identifier -- where Exif data is kept.</summary>
        /// <remarks>APP1 JPEG segment identifier -- where Exif data is kept.  XMP data is also kept in here, though usually in a second instance.</remarks>
        public static readonly JpegSegmentType App1 = new JpegSegmentType(unchecked((sbyte)0xE1), true);

        /// <summary>APP2 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App2 = new JpegSegmentType(unchecked((sbyte)0xE2), true);

        /// <summary>APP3 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App3 = new JpegSegmentType(unchecked((sbyte)0xE3), true);

        /// <summary>APP4 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App4 = new JpegSegmentType(unchecked((sbyte)0xE4), true);

        /// <summary>APP5 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App5 = new JpegSegmentType(unchecked((sbyte)0xE5), true);

        /// <summary>APP6 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App6 = new JpegSegmentType(unchecked((sbyte)0xE6), true);

        /// <summary>APP7 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App7 = new JpegSegmentType(unchecked((sbyte)0xE7), true);

        /// <summary>APP8 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App8 = new JpegSegmentType(unchecked((sbyte)0xE8), true);

        /// <summary>APP9 JPEG segment identifier.</summary>
        public static readonly JpegSegmentType App9 = new JpegSegmentType(unchecked((sbyte)0xE9), true);

        /// <summary>APPA (App10) JPEG segment identifier -- can hold Unicode comments.</summary>
        public static readonly JpegSegmentType Appa = new JpegSegmentType(unchecked((sbyte)0xEA), true);

        /// <summary>APPB (App11) JPEG segment identifier.</summary>
        public static readonly JpegSegmentType Appb = new JpegSegmentType(unchecked((sbyte)0xEB), true);

        /// <summary>APPC (App12) JPEG segment identifier.</summary>
        public static readonly JpegSegmentType Appc = new JpegSegmentType(unchecked((sbyte)0xEC), true);

        /// <summary>APPD (App13) JPEG segment identifier -- IPTC data in here.</summary>
        public static readonly JpegSegmentType Appd = new JpegSegmentType(unchecked((sbyte)0xED), true);

        /// <summary>APPE (App14) JPEG segment identifier.</summary>
        public static readonly JpegSegmentType Appe = new JpegSegmentType(unchecked((sbyte)0xEE), true);

        /// <summary>APPF (App15) JPEG segment identifier.</summary>
        public static readonly JpegSegmentType Appf = new JpegSegmentType(unchecked((sbyte)0xEF), true);

        /// <summary>Start Of Image segment identifier.</summary>
        public static readonly JpegSegmentType Soi = new JpegSegmentType(unchecked((sbyte)0xD8), false);

        /// <summary>Define Quantization Table segment identifier.</summary>
        public static readonly JpegSegmentType Dqt = new JpegSegmentType(unchecked((sbyte)0xDB), false);

        /// <summary>Define Huffman Table segment identifier.</summary>
        public static readonly JpegSegmentType Dht = new JpegSegmentType(unchecked((sbyte)0xC4), false);

        /// <summary>Start-of-Frame (0) segment identifier.</summary>
        public static readonly JpegSegmentType Sof0 = new JpegSegmentType(unchecked((sbyte)0xC0), true);

        /// <summary>Start-of-Frame (1) segment identifier.</summary>
        public static readonly JpegSegmentType Sof1 = new JpegSegmentType(unchecked((sbyte)0xC1), true);

        /// <summary>Start-of-Frame (2) segment identifier.</summary>
        public static readonly JpegSegmentType Sof2 = new JpegSegmentType(unchecked((sbyte)0xC2), true);

        /// <summary>Start-of-Frame (3) segment identifier.</summary>
        public static readonly JpegSegmentType Sof3 = new JpegSegmentType(unchecked((sbyte)0xC3), true);

        /// <summary>Start-of-Frame (5) segment identifier.</summary>
        public static readonly JpegSegmentType Sof5 = new JpegSegmentType(unchecked((sbyte)0xC5), true);

        /// <summary>Start-of-Frame (6) segment identifier.</summary>
        public static readonly JpegSegmentType Sof6 = new JpegSegmentType(unchecked((sbyte)0xC6), true);

        /// <summary>Start-of-Frame (7) segment identifier.</summary>
        public static readonly JpegSegmentType Sof7 = new JpegSegmentType(unchecked((sbyte)0xC7), true);

        /// <summary>Start-of-Frame (8) segment identifier.</summary>
        public static readonly JpegSegmentType Sof8 = new JpegSegmentType(unchecked((sbyte)0xC8), true);

        /// <summary>Start-of-Frame (9) segment identifier.</summary>
        public static readonly JpegSegmentType Sof9 = new JpegSegmentType(unchecked((sbyte)0xC9), true);

        /// <summary>Start-of-Frame (10) segment identifier.</summary>
        public static readonly JpegSegmentType Sof10 = new JpegSegmentType(unchecked((sbyte)0xCA), true);

        /// <summary>Start-of-Frame (11) segment identifier.</summary>
        public static readonly JpegSegmentType Sof11 = new JpegSegmentType(unchecked((sbyte)0xCB), true);

        /// <summary>Start-of-Frame (13) segment identifier.</summary>
        public static readonly JpegSegmentType Sof13 = new JpegSegmentType(unchecked((sbyte)0xCD), true);

        /// <summary>Start-of-Frame (14) segment identifier.</summary>
        public static readonly JpegSegmentType Sof14 = new JpegSegmentType(unchecked((sbyte)0xCE), true);

        /// <summary>Start-of-Frame (15) segment identifier.</summary>
        public static readonly JpegSegmentType Sof15 = new JpegSegmentType(unchecked((sbyte)0xCF), true);

        /// <summary>JPEG comment segment identifier.</summary>
        public static readonly JpegSegmentType Com = new JpegSegmentType(unchecked((sbyte)0xFE), true);

        public static readonly ICollection<JpegSegmentType> canContainMetadataTypes;

        static JpegSegmentType()
        {
            //    /** Start-of-Frame (4) segment identifier. */
            //    SOF4((byte)0xC4, true),
            //    /** Start-of-Frame (12) segment identifier. */
            //    SOF12((byte)0xCC, true),
            IList<JpegSegmentType> segmentTypes = new AList<JpegSegmentType>();
            foreach (JpegSegmentType segmentType in typeof(JpegSegmentType).GetEnumConstants<JpegSegmentType>())
            {
                if (segmentType.canContainMetadata)
                {
                    segmentTypes.Add(segmentType);
                }
            }
            canContainMetadataTypes = segmentTypes;
        }

        public readonly sbyte byteValue;

        public readonly bool canContainMetadata;

        internal JpegSegmentType(sbyte byteValue, bool canContainMetadata)
        {
            this.byteValue = byteValue;
            this.canContainMetadata = canContainMetadata;
        }

        [CanBeNull]
        public static JpegSegmentType FromByte(sbyte segmentTypeByte)
        {
            foreach (JpegSegmentType segmentType in typeof(JpegSegmentType).GetEnumConstants<JpegSegmentType>())
            {
                if (segmentType.byteValue == segmentTypeByte)
                {
                    return segmentType;
                }
            }
            return null;
        }

        public static JpegSegmentType ValueOf(string segmentName)
        {
            return Extensions.GetEnumConstantByName<JpegSegmentType>(segmentName);
        }
    }
}