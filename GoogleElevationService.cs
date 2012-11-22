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
using System.Xml;

namespace Elevation
{
    
    /// <summary>
    /// Uses the Google Elevation API to fill in elevation data for all given locations.
    /// </summary>
    /// <note>
    /// Use of the Google Elevation API is subject to a limit of 2,500 requests per day 
    /// (Maps API for Business users may send up to 100,000 requests per day). In each given 
    /// request you may query the elevation of up to 512 locations, but you may not exceed 
    /// 25,000 total locations per day (1,000,000 for Maps API for Business users). This 
    /// limit is enforced to prevent abuse and/or repurposing of the Elevation API, and this 
    /// limit may be changed in the future without notice. Additionally, we enforce a request 
    /// rate limit to prevent abuse of the service. If you exceed the 24-hour limit or otherwise 
    /// abuse the service, the Elevation API may stop working for you temporarily. If you continue
    /// to exceed this limit, your access to the Elevation API may be blocked.
    /// </note>    
    /// <author>Thomas F. Hagelien</author>

    public class GoogleElevationService : IElevationService    
    {
        /// <summary>
        /// <param name="locations">A 2D map of locations on the surface of earth. </param>
        /// </summary>
        public void FillElevationData(ref LatLonAlt[,] locations)
        {
            CultureInfo ci = new CultureInfo("en-US");
            List<string> loc = new List<string>();
            for (int J = 0; J < locations.GetLength(0); J++)
            {
                for (int I = 0; I < locations.GetLength(1); I++)
                {
                    loc.Add(String.Format(ci, "{0:0.0000},{1:0.0000}", locations[I, J].Latitude, locations[I, J].Longitude));                    
                }
            }
            var url = String.Format("http://maps.googleapis.com/maps/api/elevation/xml?sensor=false&locations={0}", loc.Aggregate((i, j) => i + "|" + j));

            Console.WriteLine("Fetching elevation data from maps.googleapis.com...");
            XmlDocument elevationData = new XmlDocument();
            elevationData.Load(url);
            
            XmlNodeList xmlNodeList = elevationData.SelectNodes("/ElevationResponse/result/elevation");
                        
            var di = 0; // A local counter
            int x = 0; // the I-index of the matrix
            int y = 0; // the J-index of the matrix.
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                // Update locations directly as we're spinning through all XML nodes.
                locations[x, y].Altitude = double.Parse(xmlNode.InnerText, ci.NumberFormat);                
                di++;
                x = di % locations.GetLength(0);
                if (x == 0) y++;
            }
        }
    }
}
