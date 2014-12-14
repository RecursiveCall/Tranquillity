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
    public class SmokeRingEmitter : IParticleEmitter
    {
        public DynamicParticleSystem ParticleSystem { get; set; }

        public int EmissionRate { get; set; }

		public Vector3 Position { get; set; }

		float particlesEmitted = 0.0f;

        public SmokeRingEmitter(Vector3 position, int emissionRate)
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
                        RandomPointOnCircle(),
                        Color.White,
                        new Vector3(RandomHelper.FloatBetween(0.0f, -0.5f), RandomHelper.FloatBetween(0.1f, 0.75f), 0.0f),
                        RandomHelper.FloatBetween(-0.01f, 0.1f),
                        TimeSpan.FromSeconds(RandomHelper.IntBetween(2, 5)),
                        true,
                        RandomHelper.FloatBetween(0.0f, MathUtil.TwoPi),
                        RandomHelper.FloatBetween(0.05f, 0.1f));
            }
        }

        /// <summary>
        /// Helper used by the UpdateFire method. Chooses a random location
        /// around a circle, at which a fire particle will be created.
        /// </summary>
        Vector3 RandomPointOnCircle()
        {
            const float radius = 30;
            const float height = 40;

            double angle = RandomHelper.Random.NextDouble() * MathUtil.TwoPi;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(x * radius, y * radius + height, 0);
        }
    }
}
