using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// A system of dynamic particles. This system contains particles with changing properties,
	/// affectors that affect properties of particles and emitters that emit new particles.
	/// </summary>
    public class DynamicParticleSystem : ParticleSystem<DynamicParticle>
    {
        #region Fields


        /// <summary>
        /// Emitters that emit new particles in this system
        /// </summary>
        protected List<IParticleEmitter> emitters;

        /// <summary>
        /// Affectors that affect all affectable particles in this system
        /// </summary>
		protected List<IParticleAffector> affectors;


        #endregion

        #region Initialization


        /// <summary>
        /// Initializes a new instance of a dynamic particle system
        /// </summary>
        /// <param name="maxCapacity"></param>
        /// <param name="texture"></param>
        public DynamicParticleSystem(int maxCapacity, Texture2D texture)
            : base(maxCapacity, texture)
        {
            emitters = new List<IParticleEmitter>();
            affectors = new List<IParticleAffector>();
        }


        #endregion

        #region Update


		/// <summary>
		/// Updates all emitters, affectors and particles in this system
		/// </summary>
        public virtual void Update(GameTime gameTime)
        {
            foreach (IParticleEmitter emitter in emitters)
            {
                emitter.Update(gameTime);
            }

            for (int i = 0; i < liveParticles.Count; i++)
            {
                DynamicParticle particle = liveParticles[i];

                // Update RemainingLifetime
                if (particle.RemainingLifetime.HasValue)
                {
                    if (particle.RemainingLifetime.Value.TotalMilliseconds > 0.0)
                    {
                        particle.RemainingLifetime -= gameTime.ElapsedGameTime;
                    }
                    else
                    {
                        RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                // Update position
                if (particle.Velocity.HasValue)
                {
                    particle.Position += particle.Velocity.Value;
                }

                // Update rotation
                if (particle.Rotation.HasValue)
                {
                    particle.Angle += particle.Rotation.Value;
                }

                // Affect particle
                if (particle.IsAffectable)
                {
                    foreach (IParticleAffector affector in affectors)
                    {
                        affector.Affect(gameTime, particle);
                    }
                }
            }
        }


        #endregion

        #region Public Methods


		/// <summary>
		/// Adds an emitter
		/// </summary>
		/// <param name="emitter">Emitter to add</param>
        public void AddEmitter(IParticleEmitter emitter)
        {
            emitter.ParticleSystem = this;

            emitters.Add(emitter);
        }

        /// <summary>
        /// Removes an emitter
        /// </summary>
        /// <param name="emitter">Emitter to add</param>
        public void RemoveEmitter(IParticleEmitter emitter)
        {
            emitters.Remove(emitter);
        }
		
		/// <summary>
		/// Adds an affector
		/// </summary>
		/// <param name="affector">Affector to add</param>
        public void AddAffector(IParticleAffector affector)
        {
            affectors.Add(affector);
        }

		/// <summary>
		/// Adds a dynamic particle to the system
		/// </summary>
		/// <param name="position">Position of the particle</param>
		/// <param name="color">Color of the particle</param>
		/// <param name="velocity">Velocity of the particle</param>
        /// <param name="rotation">Rotation of the particle</param>
		/// <param name="lifespan">Lifespan of the particle</param>
        public void AddParticle(Vector3 position, Color color, Vector3? velocity, float? rotation, TimeSpan? lifespan)
        {
            AddParticle(position, color, velocity, rotation, lifespan, true, 0.0f, 1.0f);
        }

		/// <summary>
		/// Adds a dynamic particle to the system
		/// </summary>
		/// <param name="position">Position of the particle</param>
		/// <param name="color">Color of the particle</param>
		/// <param name="velocity">Velocity of the particle</param>
        /// <param name="rotation">Rotation of the particle</param>
		/// <param name="lifespan">Lifespan of the particle</param>
		/// <param name="isAffectable">A value indicating whether the particle is affectable or not</param>
		/// <param name="angle">Angle of the particle</param>
		/// <param name="scale">Scale of the particle</param>
        public void AddParticle(Vector3 position, Color color, Vector3? velocity, float? rotation, TimeSpan? lifespan, bool isAffectable, float angle, float scale)
        {
            if (deadParticles.Count != 0)
            {
                DynamicParticle particle = deadParticles.Pop();

                particle.InitialPosition = particle.Position = position;
                particle.InitialVelocity = particle.Velocity = velocity;
                particle.InitialColor = particle.Color = color;
                particle.InitialAngle = particle.Angle = angle;
                particle.InitialRotation = particle.Rotation = rotation;
                particle.InitialScale = particle.Scale = scale;
                particle.IsAffectable = isAffectable;
                particle.Lifespan = lifespan;

                liveParticles.Add(particle);
            }
        }


        #endregion
    }
}
