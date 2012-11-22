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
using System.IO;


namespace Elevation
{
    /// <summary>
    /// STL (STereoLithography) is a file format native to the stereolithography CAD software created 
    /// by 3D Systems. STL is also known as Standard Tessellation Language[1] This file format is supported 
    /// by many other software packages; it is widely used for rapid prototyping and computer-aided manufacturing. 
    /// STL files describe only the surface geometry of a three dimensional object without any representation of 
    /// color, texture or other common CAD model attributes. The STL format specifies both ASCII and binary 
    /// representations. Binary files are more common, since they are more compact.[2]
    /// </summary>
    /// <author>Thomas F. Hagelien</author>
    public class Stereolithography
    {
        private Vertex[,] Vertices;
        private StreamWriter writer;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vertices">A map of structured N*M vertices </param>
        public Stereolithography(Vertex[,] vertices)
        {
            Vertices = vertices;
        }

        /// <summary>
        /// Write the STL facet field.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="v1">Vertex 1</param>
        /// <param name="v2">Vertex 2</param>
        /// <param name="v3">Vertex 3</param>
        private void WriteFacet(Vertex v1, Vertex v2, Vertex v3)
        {
            var normal = Geometry.CalcFacetNormal(v1, v2, v3);
            writer.WriteLine(String.Format("facet normal {0} {1} {2}", normal.X, normal.Y, normal.Z));
            writer.WriteLine("outer loop");
            writer.WriteLine(String.Format("vertex {0} {1} {2}", v1.X, v1.Y, v1.Z));
            writer.WriteLine(String.Format("vertex {0} {1} {2}", v2.X, v2.Y, v2.Z));
            writer.WriteLine(String.Format("vertex {0} {1} {2}", v3.X, v3.Y, v3.Z));
            writer.WriteLine("endloop");
            writer.WriteLine("endfacet");
        }

        /// <summary>
        /// Generate STL based on a map of structured mesh of vertices.
        /// </summary>
        /// <param name="Filename"> Name of output file </param>
        public void Write(string Filename)
        {
            var I = Vertices.GetLength(0);
            var J = Vertices.GetLength(1);

            writer = new StreamWriter(Filename,false);
            writer.WriteLine("solid offshore");
            for (int i = 0; i < I-1; i++)
            {
                for (int j = 0; j < J-1; j++)
                {
                    var v1 = Vertices[i, j];
                    var v2 = Vertices[i + 1, j];
                    var v3 = Vertices[i, j + 1];
                    var v4 = Vertices[i + 1, j + 1];
                    WriteFacet(v1, v2, v3);
                    WriteFacet(v2, v4, v3);
                }
            }
            writer.WriteLine("endsolid offshore");

            writer.Close();
        }
    }
}
