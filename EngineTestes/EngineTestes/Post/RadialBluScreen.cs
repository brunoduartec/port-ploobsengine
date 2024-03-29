﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.SceneControl;
using PloobsEngine.Physics;
using PloobsEngine.Modelo;
using PloobsEngine.Material;
using PloobsEngine.Engine;
using PloobsEngine.Physics.Bepu;
using Microsoft.Xna.Framework;
using PloobsEngine.Cameras;
using Microsoft.Xna.Framework.Input;

namespace ProjectTemplate
{
    /// <summary>
    /// Basic Forward Screen
    /// </summary>

    public class RadialBluScreen : IScene
    {
        /// <summary>
        /// Sets the world and render technich.
        /// </summary>
        /// <param name="renderTech">The render tech.</param>
        /// <param name="world">The world.</param>
        protected override void SetWorldAndRenderTechnich(out IRenderTechnic renderTech, out IWorld world)
        {
            ///create the IWorld
            world = new IWorld(new BepuPhysicWorld(-0.97f, true, 1), new SimpleCuller());

            ///Create the deferred technich
            ForwardRenderTecnichDescription desc = new ForwardRenderTecnichDescription();
            desc.UsePostEffect = true;
            renderTech = new ForwardRenderTecnich(desc);
        }

        /// <summary>
        /// Load content for the screen.
        /// </summary>
        /// <param name="GraphicInfo"></param>
        /// <param name="factory"></param>
        /// <param name="contentManager"></param>
        protected override void LoadContent(GraphicInfo GraphicInfo, GraphicFactory factory, IContentManager contentManager)
        {
            base.LoadContent(GraphicInfo, factory, contentManager);
                        
            SimpleModel simpleModel = new SimpleModel(factory, "Model/cenario");
            ///Physic info (position, rotation and scale are set here)
            TriangleMeshObject tmesh = new TriangleMeshObject(simpleModel, Vector3.Zero, Matrix.Identity, Vector3.One, MaterialDescription.DefaultBepuMaterial());
            ///Forward Shader (look at this shader construction for more info)
            ForwardXNABasicShader shader = new ForwardXNABasicShader();      
            ///Deferred material
            ForwardMaterial fmaterial = new ForwardMaterial(shader);            
            ///The object itself
            IObject obj = new IObject(fmaterial,simpleModel,tmesh);
            ///Add to the world
            this.World.AddObject(obj);

            this.RenderTechnic.AddPostEffect(RadialBlurPostEffect);

            ///add a camera
            this.World.CameraManager.AddCamera(new CameraFirstPerson(GraphicInfo));
        }

        RadialBlurPostEffect RadialBlurPostEffect = new RadialBlurPostEffect();

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                RadialBlurPostEffect.BlurStart += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                RadialBlurPostEffect.BlurStart -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                RadialBlurPostEffect.BlurWidth += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                RadialBlurPostEffect.BlurWidth -= 0.01f;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="render"></param>
        protected override void Draw(GameTime gameTime, RenderHelper render)
        {
            base.Draw(gameTime, render);

            ///Draw some text on the screen
            render.RenderTextComplete("Demo: Basic Screen Forward", new Vector2(GraphicInfo.Viewport.Width - 315, 15), Color.White, Matrix.Identity);
        }

    }
}

