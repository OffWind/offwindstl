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

namespace Elevation
{
    public class Surface
    {
        public LatLonAlt Center { get; set; }
        public double DistanceNS { get; set; }
        public double DistanceEW { get; set; }
        
        /// <summary>
        /// Constructor using default settings (10x10 grid, 100m bearing)
        /// </summary>
        /// <param name="center">The center of the map</param>
        public Surface(LatLonAlt center)
        {
            Center = center;
            DistanceEW = 100.0;
            DistanceNS = 100.0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="center">The center of the map</param>
        /// <param name="distanceNS">Length of the map from South to North (in meters)</param>
        /// <param name="distanceEW">Length of the map from East to West (in meters)</param>
        /// <param name="ni">Number of grid cells in I - direction (EW)</param>
        /// <param name="nj">Number of grid cells in J - direction (NS)</param>
        /// <author>Thomas F. Hagelien</author>
        public Surface(LatLonAlt center, double distanceNS, double distanceEW)
        {
            Center = center;
            DistanceNS = distanceNS;
            DistanceEW = distanceEW;
        }

        public void GenerateSurface(ref LatLonAlt [,] locations)
        {
            var west = Geometry.CalculateDerivedPosition(Center, DistanceNS / 2.0, 180.0);
            var southWest = Geometry.CalculateDerivedPosition(west, DistanceEW / 2.0, 240.0);

            var ni = locations.GetLength(0);
            var nj = locations.GetLength(1);

            double dNS = DistanceNS / (double)ni;
            double dEW = DistanceEW / (double)nj;

            // Fill in the geographics from southWest up to northEast.
            var position = southWest;
            for (int J = 0; J < nj; ++J)
            {
                position = Geometry.CalculateDerivedPosition(position, dEW * (double)J, 90.0);
                locations[0, J] = position;

                for (int I = 1; I < ni; I++)
                {
                    locations[I, J] = Geometry.CalculateDerivedPosition(position, dNS * (double)I, 0.0);
                }
            }
        }
    }
}
