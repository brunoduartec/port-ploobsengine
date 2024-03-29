#region License
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
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bnoerj.AI.Steering
{
	public enum SeenFromState
	{
		Outside,
		Inside,
		Both
	}

	/// <summary>
	/// Obstacle: a pure virtual base class for an abstract shape in space, to be
	/// used with obstacle avoidance.
	/// 
	/// XXX this should define generic methods for querying the obstacle shape
	/// </summary>
	public interface IObstacle
	{
		SeenFromState SeenFrom { get; set; }

		// XXX 4-23-03: Temporary work around (see comment above)
        Vector3 SteerToAvoid(IVehicle v, float minTimeToCollision);
	}
}
