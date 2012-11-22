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
    /// A vertex is a point in space defined by cartesian coordinates. The internal representation
    /// is a vector of doubles with three components: x, y and z.
    /// <author>Thomas F. Hagelien</author>
    /// </summary>
    public struct Vertex
    {
        private Vector3 Position;
        public double X { get { return Position.X; } set { Position.X = value; } }
        public double Y { get { return Position.Y; } set { Position.Y = value; } }
        public double Z { get { return Position.Z; } set { Position.Z = value; } }

        public Vertex(Vector3 Pos)
        {
            Position = Pos;
        }

        public Vertex(double x, double y, double z)
        {
            Position = new Vector3(x, y, z);
        }

        public static Vertex ToVertex(LatLonAlt pos)
        {
            var vertex = new Vertex(pos.Latitude, pos.Longitude, pos.Altitude);
            return vertex;            
        }

        public static Vertex[,] ToVertex(ref LatLonAlt[,] pos)
        {            
            var vertex = new Vertex[pos.GetLength(0), pos.GetLength(1)];
            for (var j = 0; j < pos.GetLength(1); j++)
            {
                for (var i = 0; i < pos.GetLength(1); i++)
                {
                    vertex[i, j] = ToVertex(pos[i, j]);
                }
            }

            return vertex;
        }
    }
}
