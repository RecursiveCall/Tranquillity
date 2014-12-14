using System.Collections;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;
using System;

namespace Tranquillity
{
	/// <summary>
	/// Represents a collection of particles that share a texture
	/// </summary>
    public interface IParticleSystem
    {
        /// <summary>
        /// Gets the particle texture
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// Gets the origin of the particle texture
        /// </summary>
        Vector2 TextureOrigin { get; }

        /// <summary>
        /// Indicates whether this particle system should be updated and drawn
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets the maximum capacity of the particle system
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Gets the number of live particles
        /// </summary>
        int ParticleCount { get; }

        /// <summary>
        /// Gets the particle at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the particle to get.</param>
        /// <returns>The particle at the specified index.</returns>
        IParticle this[int index] { get; }

		/// <summary>
		/// Removes a live particle at a given index.
		/// </summary>
		/// <param name="index">Index of the particle to remove.</param>
		/// <returns>True if the particle was removed; false if the given index is out of range.</returns>
        bool RemoveAt(int index);

		/// <summary>
		/// Removes all live particles from the system
		/// </summary>
        void Clear();
    }
}
