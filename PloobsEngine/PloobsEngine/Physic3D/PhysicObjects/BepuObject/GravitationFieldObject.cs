﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.UpdateableSystems.ForceFields;
using PloobsEngine.Physics;

namespace PloobsEngine.Physic.PhysicObjects.BepuObject
{
        public class GravitationalFieldObject : ForceField
        {
            /// <summary>
            /// Creates a gravitational field.
            /// </summary>
            /// <param name="shape">Shape representing the volume of the force field.</param>
            /// <param name="origin">Location that entities will be pushed toward.</param>
            /// <param name="multiplier">Represents the gravitational constant of the field times the effective mass at the center of the field.</param>
            /// <param name="maxForce">Maximum force the field can apply.</param>
            /// <param name="physicWorld">The physic world.</param>
            public GravitationalFieldObject(ForceFieldShape shape, Vector3 origin, float multiplier, float maxForce, BepuPhysicWorld physicWorld)
                : base(shape, physicWorld.Space.BroadPhase.QueryAccelerator)
            {
                this.Multiplier = multiplier;
                this.Origin = origin;
                this.MaxForce = maxForce;
            }

            /// <summary>
            /// Gets or sets the gravitational constant of the field times the effective mass at the center of the field.
            /// </summary>
            public float Multiplier { get; set; }

            /// <summary>
            /// Gets or sets the maximum force that can be applied by the field.
            /// </summary>
            public float MaxForce { get; set; }

            /// <summary>
            /// Gets or sets the center of the field that entities will be pushed toward.
            /// </summary>
            public Vector3 Origin { get; set; }


            /// <summary>
            /// Calculates the gravitational force to apply to the entity.
            /// </summary>
            /// <param name="e">Target of the impulse.</param>
            /// <param name="dt">Time since the last frame in simulation seconds.</param>
            /// <param name="impulse">Force to apply at the given position.</param>
            protected override void CalculateImpulse(BEPUphysics.Entities.Entity e, float dt, out Vector3 impulse)
            {
                Vector3 r = e.Position - Origin;
                float length = r.Length();
                float force = dt * Math.Min(MaxForce, Multiplier * e.Mass / (length * length * length));
                impulse = -force * r;

                //Could use a linear dropoff for a slightly faster calculation (divide by length^2 instead of length^3).
                //Vector3 r = e.Position - Origin;
                //float force = dt * Math.Min(MaxForce, Multiplier * e.Mass / (r.LengthSquared()));
                //impulse = -force * r;
            }
        }    
}