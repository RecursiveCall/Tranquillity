using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Tranquillity.Helpers;

namespace TranquillityDemos
{
    public class FireParticleSystem : DynamicParticleSystem
    {
        #region Fields


        float particlesEmitted = 0.0f;


        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the rate (particles per second) at which particles are emitted in this system.
        /// </summary>
        public int EmissionRate { get; set; }


        #endregion

        public FireParticleSystem(int maxCapacity, Texture2D texture)
            : base(maxCapacity, texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            EmitParticles(gameTime);

            foreach (DynamicParticle particle in liveParticles)
            {
                particle.Color = Color.Lerp(particle.InitialColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f - particle.Age.Value);
                particle.Scale += 0.001f;
            }

            base.Update(gameTime);
        }

        private void EmitParticles(GameTime gameTime)
        {
            particlesEmitted += (float)gameTime.ElapsedGameTime.TotalSeconds * (float)EmissionRate;

            int emittedCount = (int)particlesEmitted;

            if (emittedCount > 0)
            {
                for (int i = 0; i < emittedCount; i++)
                {
                    AddParticle(
                        RandomPointOnCircle(),
                        new Color(255, 255, 255, 100),
                        RandomHelper.Vector3Between(new Vector3(-0.25f, 0.0f, 0.0f), new Vector3(0.25f, 1.0f, 0.0f)),
                        RandomHelper.FloatBetween(-0.01f, 0.1f),
                        TimeSpan.FromSeconds(RandomHelper.IntBetween(1, 2)),
                        true,
                        RandomHelper.FloatBetween(0.0f, MathUtil.TwoPi),
                        RandomHelper.FloatBetween(0.05f, 0.075f));
                }

                particlesEmitted -= emittedCount;
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
