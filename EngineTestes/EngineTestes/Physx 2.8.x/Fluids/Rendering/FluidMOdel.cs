﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PloobsEngine.SceneControl;
using PloobsEngine.Engine;
using PloobsEngine.Engine.Logger;

namespace PloobsEngine.Modelo
{
    public class FluidMOdel : IModelo
    {
        int maximumaParticles;
        public FluidMOdel(GraphicFactory factory, String BilboardsName, String diffuseTextureName, int maximumParticles)
            : base(factory, BilboardsName, false)
        {
            this.maximumaParticles = maximumParticles;
            this.diffuseTextureName = diffuseTextureName;
            LoadModel(factory, out BatchInformations, out TextureInformations);
        }

        string diffuseTextureName;
        private float modelRadius = 0;
        public VertexPositionTexture[] billboardVertices;

        public void UpdateVertices(int primitives)
        {
            BatchInformations[0][0].VertexBuffer.SetData<VertexPositionTexture>(billboardVertices);
            BatchInformations[0][0].PrimitiveCount = primitives;
        }

        protected override void LoadModel(GraphicFactory factory, out BatchInformation[][] BatchInformations, out TextureInformation[][] TextureInformations)
        {
            billboardVertices = new VertexPositionTexture[maximumaParticles * 6];

            for (int i = 0; i < maximumaParticles*6;)
            {
                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(1, 1));

                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(1, 1));
                billboardVertices[i++] = new VertexPositionTexture(Vector3.Zero, new Vector2(0, 1));
            }


            VertexBuffer vertexBufferS = factory.CreateDynamicVertexBuffer(VertexPositionTexture.VertexDeclaration, billboardVertices.Count(), BufferUsage.WriteOnly);
            vertexBufferS.SetData(billboardVertices);
            int noVertices = billboardVertices.Count();
            int noTriangles = noVertices / 3;

            BatchInformations = new BatchInformation[1][];
            BatchInformation[] b = new BatchInformation[1];
            b[0] = new BatchInformation(0, 0, noTriangles, 0, 0, VertexPositionTexture.VertexDeclaration, VertexPositionTexture.VertexDeclaration.VertexStride, BatchType.NORMAL);
            b[0].VertexBuffer = vertexBufferS;
            b[0].IndexBuffer = null;
            BatchInformations[0] = b;

            TextureInformations = new TextureInformation[1][];
            TextureInformations[0] = new TextureInformation[1];
            TextureInformations[0][0] = new TextureInformation(false, factory, diffuseTextureName, null, null, null);
            TextureInformations[0][0].LoadTexture();
        }


        public override int MeshNumber
        {
            get { return 1; }
        }

        public override float GetModelRadius()
        {
            return modelRadius;
        }
    }

}
