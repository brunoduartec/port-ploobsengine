﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.SceneControl;
using PloobsEngine.Material;
using PloobsEngine.Physics;
using PloobsEngine.Modelo;
using BehaviorTrees;

namespace EngineTestes.AI.Behavior_Tree
{
    public class RVO3DObject : IObject
    {
        public RVO3DObject(int id, IMaterial mat, IModelo model, IPhysicObject py)
            : base(mat,model,py)
        {
            this.RVOID = id;
        }

        public int RVOID
        {
            get;
            set;
        }
        
    }
}