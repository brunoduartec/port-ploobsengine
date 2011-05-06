﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine;
using PloobsEngine.SceneControl;
using PloobsEngine.Utils;

namespace AdvancedDemo4._0
{
    /// <summary>
    /// Most basic sample about texture
    /// </summary>
    public class ProceduralTextureScreen : IScreen
    {
        Texture2D texture;

        protected override void LoadContent(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.GraphicFactory factory, IContentManager contentManager)
        {
            texture = factory.CreateTexture2D(800, 600);
            Color[] c = new Color[800 * 600];
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    c[i + j * 800] = new Color(((float)i) / 800, ((float)j) / 600, 1);
                }
            }
            texture.SetData<Color>(c);

            base.LoadContent(GraphicInfo, factory, contentManager);
        }

        protected override void  Draw(GameTime gameTime, RenderHelper render)
        {
            render.RenderTextureComplete(texture);
        }        
    }
        
}
