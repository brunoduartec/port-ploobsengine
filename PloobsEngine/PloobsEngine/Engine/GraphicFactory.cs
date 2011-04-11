﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.SceneControl;
using PloobsEngine.Utils;
using Microsoft.Xna.Framework;
using XNAnimation;

namespace PloobsEngine.Engine
{
    /// <summary>
    /// Creates everything related to Graphics
    /// </summary>
    public class GraphicFactory
    {
        GraphicsDevice device;
        GraphicInfo ginfo;
        IContentManager contentManager;
        SpriteBatch SpriteBatch;
        RenderTargetPool RenderTargetPool;
        TextureCreator texCreator;
             

        public GraphicFactory(GraphicInfo ginfo, GraphicsDevice device,IContentManager contentManager)
        {
            this.device = device;
            this.ginfo = ginfo;
            this.contentManager = contentManager;
            SpriteBatch = new SpriteBatch(device);
            RenderTargetPool = new RenderTargetPool(device);
            texCreator = new TextureCreator(ginfo, this);
        }

        public BasicEffect GetBasicEffect()
        {
            return new BasicEffect(device);
        }

        public Effect GetEffect(String name, bool clone = false, bool isInternal = false)
        {
            Effect effect = contentManager.GetAsset<Effect>(name, isInternal);
            if (clone)
                return effect.Clone();
            return effect;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return SpriteBatch;
        }
        public Model GetModel(String name,bool isinternal = false)
        {
            return contentManager.GetAsset<Model>(name, isinternal);
        }
        public SkinnedModel GetAnimatedModel(String name, bool isinternal = false)
        {
            return contentManager.GetAsset<SkinnedModel>(name, isinternal);
        }

        public RenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat SurfaceFormat = SurfaceFormat.Color, bool mipmap = false, DepthFormat DepthFormat = DepthFormat.Depth24Stencil8, int preferedMultisampleCount = 0, RenderTargetUsage RenderTargetUsage = RenderTargetUsage.PreserveContents)
        {
            return new RenderTarget2D(device, width, height, mipmap, SurfaceFormat, DepthFormat, preferedMultisampleCount, RenderTargetUsage);
        }

        public IntermediateRenderTarget GetRenderTargetFromPool(int width, int height, SurfaceFormat SurfaceFormat = SurfaceFormat.Color, bool mipmap = false, DepthFormat DepthFormat = DepthFormat.Depth24Stencil8, int preferedMultisampleCount = 0, RenderTargetUsage RenderTargetUsage = RenderTargetUsage.PreserveContents)
        {
            return RenderTargetPool.GetIntermediateTexture(width, height, mipmap, SurfaceFormat, DepthFormat, preferedMultisampleCount, RenderTargetUsage);
        }

        public Texture2D GetTexture2D(String textureName,bool isInternal = false)
        {
            return contentManager.GetAsset<Texture2D>(textureName, isInternal);
        }

        public TextureCube GetTextureCube(String textureName, bool isInternal = false)
        {
            return contentManager.GetAsset<TextureCube>(textureName, isInternal);
        }


        public Texture2D CreateTexture2D(int width, int height,bool mipmap = false,SurfaceFormat format = SurfaceFormat.Color)
        {
            return new Texture2D(device, width, height, mipmap, format);
        }
        public Texture2D CreateTexture2DColor(int width, int height, Color color,bool mipmap = false)
        {
            return texCreator.CreateColorTexture(width, height, color, mipmap);
        }
        public Texture2D CreateTexture2DRandom(int width, int height, bool mipmap = false)
        {
            return texCreator.CreateCompleteRandomColorTexture(width, height, mipmap);
        }

        public Texture2D CreateTexture2DBlackAndWhite(int width, int height, bool mipmap = false)
        {            
            return texCreator.CreateBlackAndWhiteRandomTexture(width, height, mipmap);
        }

        public Texture2D CreateTexture2DPerlinNoise(int width, int height, float frequencia, float amplitude, float persistence, int octave, bool mipmap = false)
        {
            return texCreator.CreatePerlinNoiseTexture(width, height, frequencia,amplitude,persistence,octave,mipmap);
        }

        public IndexBuffer CreateIndexBuffer(IndexElementSize elementSize, int indexCount,BufferUsage usage)
        {
            return new IndexBuffer(device, elementSize, indexCount, usage);
        }

        public VertexBuffer CreateVertexBuffer(VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage)
        {
            return new VertexBuffer(device, vertexDeclaration, vertexCount, usage);
        }

        public T GetAsset<T>(String assetName, bool isInternal = false)
        {
            return contentManager.GetAsset<T>(assetName, isInternal);
        }

    }


}