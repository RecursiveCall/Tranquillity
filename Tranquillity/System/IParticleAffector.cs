using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Defines a method to affect the properties of a dynamic particle
	/// </summary>
    public interface IParticleAffector
    {
		/// <summary>
		/// Affects properties of a dynamic particle
		/// </summary>
		/// <param name="particle">Particle to affect</param>
        /// <param name="gameTime">Game time</param>
        void Affect(GameTime gameTime, DynamicParticle particle);
    }
}
