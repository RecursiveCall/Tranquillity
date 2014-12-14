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
    /// This class demonstrates how to combine several different particle systems
    /// to build up a more sophisticated composite effect. It implements a rocket
    /// projectile, which arcs up into the sky using a ParticleEmitter to leave a
    /// steady stream of trail particles behind it. After a while it explodes,
    /// creating a sudden burst of explosion and smoke particles.
    /// </summary>
    public class Projectile
    {
        #region Constants


        const float trailParticlesPerSecond = 50;
        const int numExplosionParticles = 15;
        const int numExplosionSmokeParticles = 25;
        const float projectileLifespan = 1.5f;
        const float sidewaysVelocityRange = 60;
        const float verticalVelocityRange = 40;
        const float gravity = 15;


        #endregion

        #region Fields


        ExplosionParticleSystem explosionParticleSystem;
        ExplosionSmokeParticleSystem explosionSmokeParticleSystem;
        

        Vector3 position;
        Vector3 velocity;
        float age;


        #endregion

        public TrailEmitter TrailEmitter { get; set; }

        /// <summary>
        /// Constructs a new projectile.
        /// </summary>
        public Projectile(ExplosionParticleSystem explosionParticleSystem,
                          ExplosionSmokeParticleSystem explosionSmokeParticleSystem,
                          ProjectileTrailParticleSystem projectileTrailParticleSystem)
        {
            this.explosionParticleSystem = explosionParticleSystem;
            this.explosionSmokeParticleSystem = explosionSmokeParticleSystem;

            // Start at the origin, firing in a random (but roughly upward) direction.
            position = Vector3.Zero;

            velocity.X = (float)(RandomHelper.Random.NextDouble() - 0.5) * sidewaysVelocityRange;
            velocity.Y = (float)(RandomHelper.Random.NextDouble() + 0.5) * verticalVelocityRange;
            velocity.Z = (float)(RandomHelper.Random.NextDouble() - 0.5) * sidewaysVelocityRange;

            // Use the particle emitter helper to output our trail particles.
            TrailEmitter = new TrailEmitter(trailParticlesPerSecond, position);

            projectileTrailParticleSystem.AddEmitter(TrailEmitter);
        }

        /// <summary>
        /// Updates the projectile.
        /// </summary>
        public bool Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Simple projectile physics.
            position += velocity * elapsedTime;
            velocity.Y -= elapsedTime * gravity;
            age += elapsedTime;

            // Update the particle emitter, which will create our particle trail.
            TrailEmitter.Position = position;
            TrailEmitter.Update(gameTime);
            
            // If enough time has passed, explode! Note how we pass our velocity
            // in to the AddParticle method: this lets the explosion be influenced
            // by the speed and direction of the projectile which created it.
            if (age > projectileLifespan)
            {
                for (int i = 0; i < numExplosionParticles; i++)
                    explosionParticleSystem.AddParticle(
                        position,
                        RandomHelper.ColorBetween(Color.DarkGray, Color.Gray),
                        velocity * 0.01f + new Vector3(RandomHelper.FloatBetween(-30, 30), RandomHelper.FloatBetween(30, -10), RandomHelper.FloatBetween(-30, 30)) * 0.05f,
                        RandomHelper.FloatBetween(-0.01f, 0.01f),
                        TimeSpan.FromSeconds(RandomHelper.IntBetween(1, 2)),
                        true,
                        RandomHelper.FloatBetween(0.0f, MathUtil.Pi),
                        0.1f);

                for (int i = 0; i < numExplosionSmokeParticles; i++)
                    explosionSmokeParticleSystem.AddParticle(
                        position,
                        RandomHelper.ColorBetween(Color.LightGray, Color.White),
                        velocity * 0.01f + new Vector3(RandomHelper.FloatBetween(-50, 50), RandomHelper.FloatBetween(40, -40), RandomHelper.FloatBetween(-50, 50)) * 0.05f,
                        RandomHelper.FloatBetween(-0.01f, 0.01f),
                        TimeSpan.FromSeconds(1),
                        true,
                        RandomHelper.FloatBetween(0.0f, MathUtil.Pi),
                        0.25f);

                return false;
            }

            return true;
        }
    }
}
