#region License
//
// OffWind elevation maps
//
// Author:
//   Thomas F. Hagelien (thomas.f.hagelien@sintef.no)
//
// Copyright (C) 2012 SINTEF Materials & Chemistry
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Elevation
{
    /// <summary>
    /// Option class for program command line arguments.
    /// Uses the Command Line Library 'CommandLine' by Giacomo Stelluti Scala    
    /// </summary>
    /// <author>Thomas F. Hagelien</author>
    class Options
    {
        [Option("a", "latitude", HelpText = "Set Latitudal position of POI.")]
        public double Latitude { get; set; }

        [Option("g", "longitude", HelpText = "Set Longitudal position of POI.")]
        public double Longitude { get; set; }

        [Option(null, "dms-latitude", Required = true, HelpText = "Set DMS Latitudal position of POI. (dd:mm:ssss)")]
        public string dmsLatitude { get; set; }

        [Option(null, "dms-longitude", Required = true, HelpText = "Set DMS Longitudal position of POI. (dd:mm:ssss)")]
        public string dmsLongitude { get; set; }

        [Option("o", "output", Required = true, HelpText = "Set STL output filename.")]
        public string Output { get; set; }

        [Option("i", "num-cells-I", DefaultValue = 10, HelpText = "The number of I elements (West-East).")]
        public int NI { get; set; }

        [Option("j", "num-cells-J", DefaultValue = 10, HelpText = "The number of J elements (South-North).")]
        public int NJ { get; set; }

        [Option(null, "distance-ew", DefaultValue = 100F, HelpText = "Distance of surface from East to West [m].")]
        public double distEW { get; set; }
 
        [Option(null, "distance-ns", DefaultValue = 100F, HelpText = "Distance of surface from South to North [m].")]
        public double distNS{ get; set; }

        [Option(null, "name", DefaultValue = "OffWind", HelpText = "Name of the solid (surface).")]
        public string solidName{ get; set; }



        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("OffWind STL generator v0.1");
            return usage.ToString();
        }

    }
}
