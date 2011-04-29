﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Engine;
using PloobsEngine.Modelo;
using PloobsEngine.SceneControl;
using Microsoft.Xna.Framework;
using XNAnimation.Effects;

namespace PloobsEngine.Material
{

    /// <summary>
    /// Description that helps building the XNABasicShader
    /// </summary>
    public struct XNABasicShaderDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XNABasicShaderDescription"/> struct.
        /// </summary>
        /// <param name="AmbientColor">Color of the ambient.</param>
        /// <param name="EmissiveColor">Color of the emissive.</param>
        /// <param name="SpecularColor">Color of the specular.</param>
        /// <param name="specularPower">The specular power.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="EnableLightning">if set to <c>true</c> [enable lightning].</param>
        /// <param name="EnableTexture">if set to <c>true</c> [enable texture].</param>
        public XNABasicShaderDescription(Color AmbientColor, Color EmissiveColor, Color SpecularColor, float specularPower = 50, float alpha = 1, bool EnableLightning = true, bool EnableTexture = true)
        {
            this.AmbientColor = AmbientColor.ToVector3();
            this.EmissiveColor = EmissiveColor.ToVector3();
            this.SpecularColor = SpecularColor.ToVector3();
            this.SpecularPower = specularPower;
            this.alpha = alpha;
            this.EnableLightning = EnableLightning;
            this.EnableTexture = EnableTexture;
            DefaultLightning = false;
        }

        /// <summary>
        /// Defaults this instance.
        /// </summary>
        /// <returns></returns>
        public static XNABasicShaderDescription Default()
        {
            XNABasicShaderDescription desc = new XNABasicShaderDescription(Color.White, Color.Black, Color.White, 0, 1, false, true);
            return desc;
        }
        
        public bool DefaultLightning;
        public Vector3 AmbientColor;
        public bool EnableLightning;
        public bool EnableTexture;
        public Vector3 EmissiveColor;
        public Vector3 SpecularColor;
        public float SpecularPower;
        public float alpha;
             
    }

    /// <summary>
    /// XNA basic shader
    /// Wrapper to xna basic effect
    /// </summary>
    public class XNABasicShader : IShader
    {
        XNABasicShaderDescription desc;
        /// <summary>
        /// Initializes a new instance of the <see cref="XNABasicShader"/> class.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public XNABasicShader(XNABasicShaderDescription desc)
        {
            this.desc = desc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XNABasicShader"/> class.
        /// </summary>
        public XNABasicShader()
            : this(XNABasicShaderDescription.Default())
        {            
        }

        private BasicEffect effect;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="ginfo"></param>
        /// <param name="factory"></param>
        /// <param name="obj"></param>
        public override void Initialize(GraphicInfo ginfo, GraphicFactory factory, IObject obj)
        {
            effect = factory.GetBasicEffect();            
            base.Initialize(ginfo,factory,obj);
            effect.PreferPerPixelLighting = true;
            SetDescription(desc);
            base.Initialize(ginfo, factory, obj);
        }

        public XNABasicShaderDescription GetDescription()
        {
            return desc;
        }

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void SetDescription(XNABasicShaderDescription desc)
        {
            this.desc = desc;
            if (desc.DefaultLightning)
            {
                effect.EnableDefaultLighting();
            }
            else
            {
                if (effect.LightingEnabled)
                {
                    effect.LightingEnabled = true;
                    effect.SpecularColor = desc.SpecularColor;
                    effect.SpecularPower = desc.SpecularPower;
                    effect.EmissiveColor = desc.EmissiveColor;
                    effect.AmbientLightColor = desc.AmbientColor;                    
                }
            }
            
            effect.TextureEnabled = desc.EnableTexture;
            effect.Alpha = desc.alpha;
            
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gt">gametime</param>
        /// <param name="obj">the obj</param>
        /// <param name="render">The render.</param>
        /// <param name="cam">The cam.</param>
        /// <param name="lights"></param>
        public override void Draw(GameTime gt, IObject obj, RenderHelper render, Cameras.ICamera cam, IList<Light.ILight> lights)
        {
            base.Draw(gt, obj, render, cam, lights);
            effect.Texture = obj.Modelo.getTexture(Modelo.TextureType.DIFFUSE);
            effect.View = cam.View;
            effect.Projection = cam.Projection;

            for (int i = 0; i < obj.Modelo.MeshNumber; i++)
            {
                BatchInformation[] bi = obj.Modelo.GetBatchInformation(i);
                for (int j = 0; j < bi.Count(); j++)
                {
                    effect.World = bi[j].ModelLocalTransformation * obj.WorldMatrix;
                    effect.CurrentTechnique.Passes[0].Apply();
                    render.RenderBatch(ref bi[j]);
                }
            }
        }

        /// <summary>
        /// Gets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        public override MaterialType MaterialType
        {
            get { return Material.MaterialType.FORWARD; }
        }
    }
}
