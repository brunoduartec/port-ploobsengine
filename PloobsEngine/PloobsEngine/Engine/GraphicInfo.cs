﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PloobsEngine.Engine
{
    /// <summary>
    /// When GraphicInfo changes
    /// </summary>
    /// <param name="newGraphicInfo">The new graphic info.</param>
    public delegate void OnGraphicInfoChange(GraphicInfo newGraphicInfo);

    /// <summary>
    /// Contains Graphics Informations about the current execution
    /// </summary>
    public class GraphicInfo
    {
        internal GraphicInfo(int BackBufferHeight, int BackBufferWidth, Rectangle FullScreenRectangle, Vector2 halfPixel, GraphicsDevice device, int MultiSample, DepthFormat DepthFormat)
        {
            this.BackBufferHeight = BackBufferHeight;
            this.BackBufferWidth = BackBufferWidth;
            this.FullScreenRectangle = FullScreenRectangle;
            this.HalfPixel = halfPixel;
            OnGraphicInfoChange = null;
            this.device = device;
            Viewport = device.Viewport;
            this.MultiSample = MultiSample;
            this.DepthFormat = DepthFormat;
        }

        private GraphicsDevice device;

        internal void FireEvent(GraphicInfo gi)
        {
            OnGraphicInfoChange(gi);
        }

        /// <summary>
        /// BackBuffer Depth Format
        /// </summary>
        public readonly DepthFormat DepthFormat;

        /// <summary>
        /// BAck Buffer Multisample
        /// </summary>
        public readonly int MultiSample;

        /// <summary>
        /// Occurs when [on graphic info change].
        /// </summary>
        public event OnGraphicInfoChange OnGraphicInfoChange;

        /// <summary>
        /// BackBufferHeight
        /// </summary>
        public readonly int BackBufferHeight;
        /// <summary>
        /// BackBufferWidth
        /// </summary>
        public readonly int BackBufferWidth;
        /// <summary>
        /// FullScreenRectangle
        /// </summary>
        public readonly Rectangle FullScreenRectangle;
        /// <summary>
        /// HalfPixel (used in DX 9 shaders)
        /// </summary>
        public readonly Vector2 HalfPixel;

        /// <summary>
        /// Viewport
        /// </summary>
        public readonly Viewport Viewport;
    }
}
