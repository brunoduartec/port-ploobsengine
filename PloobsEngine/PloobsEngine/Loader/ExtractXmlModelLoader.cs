﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PloobsEngine.Light;
using PloobsEngine.Utils;
using PloobsEngine.Engine;
using PloobsEngine.Modelo;
using PloobsEngine.Engine.Logger;

namespace PloobsEngine.Loader
{
    internal struct XmlModelMeshInfo
    {
        public XmlModelMeshInfo(string modeName, string collisionType, string difuseName, string bumpName, string specularName, string glowName)
        {
            this.modeName = modeName;
            this.collisionType = collisionType;
            this.difuseName = difuseName;
            this.bumpName = bumpName;
            this.specularName = specularName;
            this.glowName = glowName;
        }

        public string collisionType;
        public string modeName;
        public string difuseName;
        public string bumpName;
        public string specularName;
        public string glowName;        

    }

    internal struct targetInfo
    {
        public Vector3 targetPos;
        public string name;
    }

    internal struct SpotLightInformation
    {
        public Vector3 pos;        
        public Color color;
        public float decay;
        public float multiplier;
        public float angle;        
        public String name;
        public bool castShadow;
    }


    public class ExtractXmlModelLoader : IModelLoader
    {
        string path = "Content\\ModelInfos\\";
        string modelPath = "..\\Content\\Model\\";
        string texturePath = "..\\Content\\Textures\\";

        /// <summary>
        /// Combine the xmlBasePath + Name + .xml
        /// when using load, just pass the Name
        /// </summary>
        /// <param name="xmlBasePath">The XML base path.</param>
        /// <param name="modelPath">The model path.</param>
        /// <param name="texturePath">The texture path.</param>
        public ExtractXmlModelLoader(string xmlBasePath, string modelPath , string texturePath)
        {
            this.path = xmlBasePath;
            this.modelPath = modelPath;
            this.texturePath = texturePath;
        }

        /// <summary>
        /// Use the default Path for everything
        //string xmlpath = "Content\\ModelInfos\\";
        //string modelPath = "..\\Content\\Model\\";
        //string texturePath = "..\\Content\\Textures\\";
        /// </summary>
        public ExtractXmlModelLoader()
        {            
        }

        #region IModelLoader Members        

        public ModelLoaderData Load(GraphicFactory factory,GraphicInfo ginfo,String Name)
        {
            ModelLoaderData elements = new ModelLoaderData();
            Dictionary<String, XmlModelMeshInfo> infos = new Dictionary<string, XmlModelMeshInfo>();
            Dictionary<String, targetInfo> targets = new Dictionary<string, targetInfo>();            
            Dictionary<String, SpotLightInformation> spotLights = new Dictionary<string, SpotLightInformation>();
            Dictionary<String, CameraInfo> cameras = new Dictionary<string, CameraInfo>();

            SerializerHelper.ChangeDecimalSymbolToPoint();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path + Name + ".xml");
            XmlNodeList worldNode = xDoc["SCENE"].ChildNodes;

            foreach (XmlNode node in worldNode)
            {
                if (node.Name == "body")
                {
                    XmlModelMeshInfo info = new XmlModelMeshInfo();
                    info.modeName = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    XmlElement collision = node["collision"];
                    if (collision != null)
                    {
                        info.collisionType = SerializerHelper.DeserializeAttributeBaseType<String>("type", collision);
                    }
                    XmlElement material = node["material"];
                    if (material == null)
                    {
                        info.difuseName = "white";
                    }
                    else
                    {
                        XmlElement difuse = material["diffuse"];
                        if (difuse != null)
                        {
                            info.difuseName = removeExtension(SerializerHelper.DeserializeAttributeBaseType<String>("name", difuse));
                        }
                        XmlElement bump = material["bump"];
                        if (bump != null)
                        {
                            info.bumpName = removeExtension(SerializerHelper.DeserializeAttributeBaseType<String>("name", bump));
                        }
                        XmlElement specular = material["specular"];
                        if (specular != null)
                        {
                            info.specularName = removeExtension(SerializerHelper.DeserializeAttributeBaseType<String>("name", specular));
                        }
                        XmlElement glow = material["glow"];
                        if (glow != null)
                        {
                            info.glowName = removeExtension(SerializerHelper.DeserializeAttributeBaseType<String>("name", glow));
                        }
                    }
                    infos.Add(info.modeName, info);
                }
                else if (node.Name == "pointlight")
                {
                    String name = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    Vector3 pos = SerializerHelper.DeserializeVector3("position", node);
                    pos = new Vector3(pos.X, -pos.Y, -pos.Z);                    
                    Vector3 vColor = SerializerHelper.DeserializeVector3("color", node);
                    Color color = new Color(vColor.X / 255, vColor.Y / 255, vColor.Z / 255);
                    float amount = SerializerHelper.DeserializeAttributeBaseType<float>("amount", node["multiplier"]);
                    float decay = SerializerHelper.DeserializeAttributeBaseType<float>("value", node["decay"]);
                    PointLightPE pl = new PointLightPE(pos, color, 200, amount);
                    pl.Name = name;
                    pl.UsePointLightQuadraticAttenuation = true;
                    elements.LightsInfo.Add(pl);
                }
                else if (node.Name == "spotlight")
                {                    
                    String name = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    Vector3 pos = SerializerHelper.DeserializeVector3("position", node);
                    pos = new Vector3(pos.X, -pos.Y, -pos.Z);
                    Vector3 vColor = SerializerHelper.DeserializeVector3("color", node);
                    float fallof = SerializerHelper.DeserializeBaseType<float>("fallof", node);
                    Color color = new Color(vColor.X / 255, vColor.Y / 255, vColor.Z / 255);
                    float amount = SerializerHelper.DeserializeAttributeBaseType<float>("amount", node["multiplier"]);
                    float decay = SerializerHelper.DeserializeAttributeBaseType<float>("value", node["decay"]);
                    bool castShadow = SerializerHelper.DeserializeBaseType<bool>("castShadows", node);                    
                
                    SpotLightInformation spi = new SpotLightInformation();
                    spi.angle = MathHelper.ToRadians(fallof);
                    spi.color = color;
                    spi.decay = decay;
                    spi.multiplier = amount;
                    spi.name = name;
                    spi.pos = pos;
                    spi.castShadow = castShadow;          
                    spotLights.Add(spi.name, spi);


                }
                else if (node.Name == "target")
                {
                    String name = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    Vector3 pos = SerializerHelper.DeserializeVector3("position", node);
                    pos = new Vector3(pos.X, -pos.Y, -pos.Z);
                    targetInfo ti = new targetInfo();
                    ti.targetPos = pos;
                    ti.name = name;
                    targets.Add(ti.name, ti);                    
                }
                else if (node.Name == "camera")
                {
                    String name = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    Vector3 pos = SerializerHelper.DeserializeVector3("position", node);
                    pos = new Vector3(pos.X, -pos.Y, -pos.Z);
                    CameraInfo co = new CameraInfo();
                    co.Name = name;
                    co.Position = pos;
                    cameras.Add(co.Name,co);
                }
                else if (node.Name == "dummy")
                {
                    String name = SerializerHelper.DeserializeAttributeBaseType<String>("name", node);
                    Vector3 pos = SerializerHelper.DeserializeVector3("position", node);
                    pos = new Vector3(pos.X, -pos.Y, -pos.Z);
                    DummyInfo di = new DummyInfo();
                    di.Name = name;
                    di.Position = pos;
                    elements.DummyInfo.Add(di);
                }
            }

            ///////PROCCESS LIGHTS /////////////////////
            foreach (var item in spotLights)
            {
                SpotLightInformation si = item.Value;
                targetInfo ti =  targets[item.Key + ".Target"];
                SpotLightPE sl = new SpotLightPE(si.pos, Vector3.Normalize(ti.targetPos - si.pos), si.decay, (ti.targetPos - si.pos).Length() * 10f, si.color, (float)Math.Cos(si.angle / 2), si.multiplier);
                sl.CastShadown = si.castShadow;
                sl.Name = si.name;                
                elements.LightsInfo.Add(sl);

            }

            ///////PROCCESS CAMERAS/////////////////////
            foreach (var item in cameras)
            {
                CameraInfo ci = item.Value;
                targetInfo ti = targets[item.Key + ".Target"];
                ci.Target = ti.targetPos;
                elements.CameraInfo.Add(ci);
            }            


            Model model = factory.GetModel(modelPath + Name);
            Matrix[] m = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(m);

            ////////////EXTRAINDO MESHES
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                String name = model.Meshes[i].Name.Substring(5);
                if (infos.ContainsKey(name))
                {

                    XmlModelMeshInfo inf = infos[name];                    
                    Matrix tr = m[model.Meshes[i].ParentBone.Index];                    
                    
                    Vector3 scale;
                    Vector3 pos;
                    Quaternion ori;
                    tr.Decompose(out scale, out ori, out pos);

                    ModelInformation mi = new ModelInformation();
                    mi.ModelName = inf.modeName;
                    ModelBuilderHelper.Extract(m, model.Meshes[i], out mi.batchInformation);                    
                    
                    
                    mi.modelMesh = model.Meshes[i];
                    if(inf.difuseName!=null)
                        mi.difuse = factory.GetTexture2D(texturePath + inf.difuseName);

                    if (inf.glowName != null)
                        mi.glow = factory.GetTexture2D(texturePath + inf.glowName);

                    if (inf.specularName != null)
                        mi.specular = factory.GetTexture2D(texturePath + inf.specularName);

                    if (inf.bumpName != null)
                        mi.bump = factory.GetTexture2D(texturePath + inf.bumpName);

                    elements.ModelMeshesInfo.Add(mi);
                }
            }

            SerializerHelper.ChangeDecimalSymbolToSystemDefault();
            ///Clear Stuffs
            infos.Clear();
            targets.Clear();
            spotLights.Clear();
            cameras.Clear();

            return elements;
        }

        private string removeExtension(String str)
        {
            return str.Split('.')[0];
        }
    }

        #endregion    
}