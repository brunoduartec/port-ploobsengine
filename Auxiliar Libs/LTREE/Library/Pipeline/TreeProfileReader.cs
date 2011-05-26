﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using LTreesLibrary.Trees;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;
using Library;

namespace LTreesLibrary.Pipeline
{
    public class TreeProfileReader : ContentTypeReader<TreeProfile>
    {
        public String DefaultTreeShaderAssetName = "Trunk";
        public String DefaultLeafShaderAssetName = "Leaves";
        ResourceContentManager cnt ;

        protected override TreeProfile Read(ContentReader input, TreeProfile existingInstance)
        {
            // brace yourself for the simple and intuitive way of retrieving the graphics device
            IGraphicsDeviceService deviceService = input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            GraphicsDevice device = deviceService.GraphicsDevice;

            cnt = new ResourceContentManager(input.ContentManager.ServiceProvider, Resource1.ResourceManager);

            if (existingInstance == null)
                existingInstance = new TreeProfile(device);

            ContentManager content = input.ContentManager;

            existingInstance.Generator = ReadGenerator(input);
            existingInstance.TrunkTexture = content.Load<Texture2D>(input.ReadString());
            existingInstance.LeafTexture = content.Load<Texture2D>(input.ReadString());

            string trunkEffect = input.ReadString();
            string leafEffect = input.ReadString();

            if (trunkEffect == "")
                trunkEffect = DefaultTreeShaderAssetName;
            if (leafEffect == "")
                leafEffect = DefaultLeafShaderAssetName;

            existingInstance.TrunkEffect = cnt.Load<Effect>(trunkEffect);
            existingInstance.LeafEffect = cnt.Load<Effect>(leafEffect);

            return existingInstance;
        }

        private TreeGenerator ReadGenerator(ContentReader input)
        {
            String source = input.ReadString();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(source);
            return TreeGenerator.CreateFromXml(xml);
        }
    }
}
