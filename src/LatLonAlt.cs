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
using System.Globalization;

namespace Elevation
{
    /// <summary>
    /// LatLonAlt is a class representing a geographic position.
    /// The lat/lon is given in decimal representations.
    /// </summary>
    /// <author>Thomas F. Hagelien</author>
    [Serializable]
    public class LatLonAlt
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public void SetDMSLatitude(double deg, double min, double sec)
        {
            Latitude = deg + min/60F + sec/3600;
        }

        public void SetDMSLongitude(double deg, double min, double sec)
        {
            Longitude = deg + min/60F + sec/3600;
        }

        public LatLonAlt()
        {
            Latitude = 0.0;
            Longitude = 0.0;
            Altitude = 0.0;
        }

        public LatLonAlt(string dmsLat, string dmsLon)
        {
            try
            {
                var ci = new CultureInfo("en-US");
                var dmsLatQuery = Array.ConvertAll(dmsLat.Split(':'), p => Double.Parse(p, ci));
                var dmsLonQuery = Array.ConvertAll(dmsLon.Split(':'), p => Double.Parse(p, ci));
                Latitude = dmsLatQuery[0] + dmsLatQuery[1] / 60F + dmsLatQuery[2] / 3600F;
                Longitude = dmsLonQuery[0] + dmsLonQuery[1] / 60F + dmsLonQuery[2] / 3600F;
            }
            catch( Exception e ) 
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

        }

        public LatLonAlt(double lat, double lon, double alt)
        {
            Latitude = lat;
            Longitude = lon;
            Altitude = alt;
        }

        public LatLonAlt(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
            Altitude = 0.0;
        }

        public override string ToString()
        {
            return string.Format(
                "{0},{1}",
                Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat),
                Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}
