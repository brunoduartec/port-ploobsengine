﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PloobsEngine.Physics;
using System.Runtime.Serialization;

namespace PloobsEngine.Physic.Constraints
{
      /// <summary>
    /// Specification of a physic Constraint
    /// </summary>
    public abstract class IPhysicConstraint : ISerializable
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public abstract Vector3 Position { set; get; }
        public abstract IPhysicObject BodyA { set; get; }
        public abstract IPhysicObject BodyB { set; get; }
        public abstract PhysicConstraintTypes PhysicConstraintType { get;}



        #region ISerializable Members

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        #endregion
    }


    /// <summary>
    /// Physic Types allowed
    /// </summary>
    public enum PhysicConstraintTypes
    { 
    
        POINTPOINT
    
    }
}