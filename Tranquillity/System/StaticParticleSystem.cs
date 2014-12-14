using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// A system of static particles. This system is optimal for particles
	/// with properties that don't change such as stars, billboards, etc.
	/// </summary>
    public class StaticParticleSystem : ParticleSystem<StaticParticle>
    {
		/// <summary>
		/// Initializes a new instance of a static particle system
		/// </summary>
		/// <param name="maxCapacity"></param>
		/// <param name="texture"></param>
        public StaticParticleSystem(int maxCapacity, Texture2D texture)
            : base(maxCapacity, texture)
        {
			
        }

        /// <summary>
        /// Adds a static particle
        /// </summary>
        /// <param name="position">Position of the particle</param>
        /// <param name="color">Color of the particle</param>
        public void AddParticle(Vector3 position, Color color)
        {
            AddParticle(position, color, 0.0f, 1.0f);
        }

        /// <summary>
        /// Adds a static particle
        /// </summary>
		/// <param name="position">Position of the particle</param>
		/// <param name="color">Color of the particle</param>
        /// <param name="angle">Angle of the particle</param>
		/// <param name="scale">Scale of the particle</param>
        public void AddParticle(Vector3 position, Color color, float angle, float scale)
        {
            if (deadParticles.Count != 0)
            {
                StaticParticle particle = deadParticles.Pop();

                particle.Position = position;
                particle.Color = color;
                particle.Angle = angle;
                particle.Scale = scale;

                liveParticles.Add(particle);
            }
        }
    }
}
