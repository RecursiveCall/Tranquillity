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
    /// Decelerates a particle based on its age
    /// </summary>
    public class Decelerate : IParticleAffector
    {
        /// <summary>
        /// Affects the velocity of the particle
        /// </summary>
        /// <param name="particle">Particle to affect</param>
        /// <param name="gameTime">Game time</param>
        public void Affect(GameTime gameTime, DynamicParticle particle)
        {
            if (particle.Age.HasValue && particle.Velocity.HasValue && particle.InitialVelocity.HasValue)
            {
                particle.Velocity = Vector3.Lerp(particle.InitialVelocity.Value, Vector3.Zero, 1.0f - particle.Age.Value);
            }
        }
    }
}
