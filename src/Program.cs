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
using System.Threading;
using System.Globalization;

namespace Elevation
{
    /// <summary>
    /// Main program
    /// </summary>
    /// <author>Thomas F. Hagelien</author>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Set the Culture Info for the entire main thread.
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

                // Using the "Command Line Parser Library" from CodePlex
                // The MIT License (MIT)
                // Copyright (c) 2005 - 2012 Giacomo Stelluti Scala
                var options = new Options();
                ICommandLineParser parser = new CommandLineParser();

                /// Example                
                /// elevation --dms-latitude 61:53:37.20 --dms-longitude 9:51:43.92 --num-cells-I 10 --num-cells-J 10 --distance-ew 2000.0 --distance-ns 2000.0 --output rondane.stl
                if (parser.ParseArguments(args, options))
                {
                    // consume Options type properties
                    var lat = options.dmsLatitude;
                    var lon = options.dmsLongitude;
                    var ni = options.NI;
                    var nj = options.NJ;
                    var distSN = options.distNS;
                    var distEW = options.distEW;

                    // Generate a surface, i.e. a map of LatLonAlt types surrounding the initial
                    // central location given by the user.
                    var surface = new Surface(new LatLonAlt(lat, lon), distSN, distEW);
                    var locations = new LatLonAlt[ni, nj];
                    surface.GenerateSurface(ref locations);

                    // Fetch elevation data from Google
                    var service = new GoogleElevationService();
                    service.FillElevationData(ref locations);

                    // Generate a matrix of vertices using cartesian coordinates with
                    // the point of interest in the center (0,0)
                    var vertices = GenerateVerticesMatrix(ref locations, ni, nj, distEW, distSN);
         
                    // Write the map to an STL output file
                    var fileName = options.Output;
                    //var vertices = Vertex.ToVertex(ref locations);
                    var stl = new Stereolithography(vertices); // vertices);
                    stl.Write(fileName);

                    Console.WriteLine(String.Format("Output written to {0}", fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception Error", e);
            }

        }

        /// <summary>
        /// Generates a cartesian mesh by inserting relative X and Y coordinates based on the given distance
        /// and Z (elevation) based on a locations map.
        /// </summary>
        /// <param name="locations">Location data containing elevation information</param>
        /// <param name="ni">Number of horizontal (X) grid cells</param>
        /// <param name="nj">Number of vertical (Y) grid cells</param>
        /// <param name="distEW">Total horizontal distance</param>
        /// <param name="distSN">Total vertical distance</param>
        /// <returns></returns>
        private static Vertex[,] GenerateVerticesMatrix(ref LatLonAlt[,] locations, int ni, int nj, double distEW, double distSN)
        {
            var x0 = -distEW / 2F;
            var y0 = -distSN / 2F;

            var y = y0;
            double dx = distEW / (double)ni;
            double dy = distSN / (double)nj;
            var vertices = new Vertex[ni, nj];

            for (var j = 0; j < nj; j++)
            {
                var x = x0;
                for (var i = 0; i < ni; i++)
                {
                    vertices[i, j] = new Vertex(x, y, locations[i, j].Altitude);
                    x += dx;
                }
                y += dy;
            }
            return vertices;
        }
    }
}
