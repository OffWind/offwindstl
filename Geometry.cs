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
    /// <summary>
    /// Geometric utility class.    
    /// </summary>
    /// <author>Thomas F. Hagelien</author>
    public class Geometry
    {
        const double DegreesToRadians = Math.PI / 180.0;
        const double RadiansToDegrees = 180.0 / Math.PI;
        const double EarthRadius = 6378137.0;

        /// <summary>
        /// Calculates the end-point from a given source at a given range (meters) and bearing (degrees).
        /// This methods uses simple geometry equations to calculate the end-point.
        /// </summary>
        /// <param name="source">Point of origin</param>
        /// <param name="range">Range in meters</param>
        /// <param name="bearing">Bearing in degrees</param>
        /// <returns>End-point from the source given the desired range and bearing.</returns>
        /// 
        public static LatLonAlt CalculateDerivedPosition(LatLonAlt source, double distance, double bearing)
        {
            double latA = source.Latitude * DegreesToRadians;
            double lonA = source.Longitude * DegreesToRadians;
            double angularDistance = distance / EarthRadius;
            double trueCourse = bearing * DegreesToRadians;

            double lat = Math.Asin(Math.Sin(latA) * Math.Cos(angularDistance) + Math.Cos(latA) * Math.Sin(angularDistance) * Math.Cos(trueCourse));
            double dlon = Math.Atan2(Math.Sin(trueCourse) * Math.Sin(angularDistance) * Math.Cos(latA),
                                          Math.Cos(angularDistance) - Math.Sin(latA) * Math.Sin(lat));
            double lon = ((lonA + dlon + Math.PI) % (Math.PI * 2.0)) - Math.PI;

            return new LatLonAlt(lat * RadiansToDegrees, lon * RadiansToDegrees, source.Altitude);
        }

        /// <summary>
        /// Compute the Facet Normal between three vertices in space. The direction is given by rhs-rule
        /// v1v2 x v1v3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector3 CalcFacetNormal(Vertex v1, Vertex v2, Vertex v3)
        {
            var M = new Vector3(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z);
            var N = new Vector3(v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z);
            var normalVector = M.CrossProduct(N);
            normalVector.Normalize();

            return normalVector;
        }
    }
}
