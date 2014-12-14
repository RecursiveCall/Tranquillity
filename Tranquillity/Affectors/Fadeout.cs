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
    /// Fades out a particle based on its age
    /// </summary>
    public class Fadeout : IParticleAffector
    {
        /// <summary>
        /// Affects the alpha of the particle
        /// </summary>
        /// <param name="particle">Particle to affect</param>
        /// <param name="gameTime">Game time</param>
        public void Affect(GameTime gameTime, DynamicParticle particle)
        {
            if (particle.Age.HasValue)
            {
                particle.Color = Color.Lerp(particle.InitialColor, new Color((int)particle.InitialColor.R, particle.InitialColor.G, particle.InitialColor.B, 0), 1.0f - particle.Age.Value);
            }
        }
    }
}
