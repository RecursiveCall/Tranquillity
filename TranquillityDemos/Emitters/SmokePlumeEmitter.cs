using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Tranquillity;
using Tranquillity.Helpers;

namespace TranquillityDemos
{
    /// <summary>
    /// A fire emitter
    /// </summary>
    public class SmokePlumeEmitter : IParticleEmitter
    {
        public DynamicParticleSystem ParticleSystem { get; set; }

        public int EmissionRate { get; set; }

		public Vector3 Position { get; set; }

		float particlesEmitted = 0.0f;

        public SmokePlumeEmitter(Vector3 position, int emissionRate)
        {
            Position = position;
            EmissionRate = emissionRate;
        }

        public void Update(GameTime gameTime)
        {
			particlesEmitted += (float)gameTime.ElapsedGameTime.TotalSeconds * (float)EmissionRate;

			int emittedCount = (int)particlesEmitted;

			if (emittedCount > 0)
			{
				Emit(emittedCount);

				particlesEmitted -= emittedCount;
			}
        }

        public void Emit(int particlesToEmit)
        {
			for (int i = 0; i < particlesToEmit; i++)
            {
                ParticleSystem.AddParticle(
                        Position,
                        Color.White,
                        new Vector3(RandomHelper.FloatBetween(0.0f, -1.0f), RandomHelper.FloatBetween(0.2f, 1.5f), 0.0f),
                        RandomHelper.FloatBetween(-0.01f, 0.1f),
                        TimeSpan.FromSeconds(RandomHelper.IntBetween(3, 5)),
                        true,
                        0.0f,
                        RandomHelper.FloatBetween(0.05f, 0.1f));
            }
        }
    }
}
