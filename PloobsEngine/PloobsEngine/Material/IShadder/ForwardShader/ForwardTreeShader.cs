﻿#region License
/*
    PloobsEngine Game Engine Version 0.3 Beta
    Copyright (C) 2011  Ploobs

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion 
#if !WINDOWS_PHONE && !REACH && !XBOX360
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Modelo;
using LTreesLibrary.Trees;

namespace PloobsEngine.Material
{
    /// <summary>
    /// Tree Shader, used by the Tree Material, interacts with the LTree
    /// </summary>
    public class ForwardTreeShader : IShader
    {
        /// <summary>
        /// Gets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        public override MaterialType MaterialType
        {
            get { return MaterialType.FORWARD; }
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gt, SceneControl.IObject obj, SceneControl.RenderHelper render, Cameras.ICamera cam, IList<Light.ILight> lights)        
 	    {
            SimpleTree tree = (obj.Modelo as TreeModel).Tree;            
            tree.DrawTrunk(obj.WorldMatrix, cam.View, cam.Projection,true);
            tree.DrawLeaves(obj.WorldMatrix, cam.View, cam.Projection, true);            
            render.ResyncStates();
        }

        public override void DepthExtractor(Microsoft.Xna.Framework.GameTime gt, SceneControl.IObject obj, ref Microsoft.Xna.Framework.Matrix View, ref Microsoft.Xna.Framework.Matrix projection, SceneControl.RenderHelper render)
        {
            SimpleTree tree = (obj.Modelo as TreeModel).Tree;
            tree.DrawTrunk(obj.WorldMatrix, View, projection, false, true);
            render.ResyncStates();
            tree.DrawLeaves(obj.WorldMatrix, View, projection, false, true);
            render.ResyncStates();
        }         
    }
}
#endif