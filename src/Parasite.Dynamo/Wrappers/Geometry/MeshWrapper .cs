﻿using System;
using System.Drawing;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace ParasiteIO.Dynamo.Wrappers.Geometry
{
    public class MeshWrapper : IGraphicItem
    {

         Vector[] m_normals;
         Autodesk.DesignScript.Geometry.Point[] m_vertices;
         Color[] m_vertexColors;

        [IsVisibleInDynamoLibrary(false)]
        public MeshWrapper(Vector[] normals, Autodesk.DesignScript.Geometry.Point[] vertices, Color[] vertexColors)
        {
            m_normals = normals;
            m_vertices = vertices;
            m_vertexColors = vertexColors;
        }
    


        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            // Dynamo's renderer uses IRenderPackage objects
            // to store data for rendering. The Tessellate method
            // give you an IRenderPackage object which you can fill
            // with render data.

            // Set RequiresPerVertexColoration to let the renderer
            // know that you need to use a per-vertex color shader.
            package.RequiresPerVertexColoration = true;
            AddColoredQuadToPackage(package);
        }


        private  void AddColoredQuadToPackage(IRenderPackage package)
        {
            for (int i = 0; i < m_vertices.Length; i++)
            {
                package.AddTriangleVertex(m_vertices[i].X, m_vertices[i].Y, m_vertices[i].Z);
                package.AddTriangleVertexColor(m_vertexColors[i].R, m_vertexColors[i].G, m_vertexColors[i].B, m_vertexColors[i].A);
                package.AddTriangleVertexNormal(m_normals[i].X, m_normals[i].Y, m_normals[i].Z);
                package.AddTriangleVertexUV(0, 0);
            }
        }




    }
}
