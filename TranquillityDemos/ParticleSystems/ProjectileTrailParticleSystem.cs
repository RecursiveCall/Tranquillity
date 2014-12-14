using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace TranquillityDemos
{
    public class ProjectileTrailParticleSystem : DynamicParticleSystem
    {
        public ProjectileTrailParticleSystem(int maxCapacity, Texture2D texture)
            : base(maxCapacity, texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (DynamicParticle particle in liveParticles)
            {
                particle.Color = Color.Lerp(particle.InitialColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f - particle.Age.Value);
                particle.Scale += 0.001f;
            }

            base.Update(gameTime);
        }
    }
}
