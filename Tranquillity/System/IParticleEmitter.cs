using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Represents a particle-emitting entity
	/// </summary>
    public interface IParticleEmitter
    {
        /// <summary>
        /// The particle system that this emitter belongs to.
        /// The emitter will emit particles only in this system.
        /// </summary>
        DynamicParticleSystem ParticleSystem { get; set; }

        /// <summary>
        /// Updates the emitter
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Emits a given number of particles
        /// </summary>
        /// <param name="particlesToEmit">Number of particles to emit</param>
        void Emit(int particlesToEmit);
    }
}
