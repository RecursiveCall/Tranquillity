using System.Collections;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Defines the base class of particle systems
	/// </summary>
	/// <typeparam name="T">Particle type</typeparam>
    public abstract class ParticleSystem<T> : IParticleSystem where T : IParticle, new()
    {
        #region Fields


        /// <summary>
        /// A list of live particles
        /// </summary>
        protected List<T> liveParticles;

        /// <summary>
        /// A list of dead/reusable particles
        /// </summary>
        protected Stack<T> deadParticles;


        #endregion

        #region Properties


        Texture2D texture;
        /// <summary>
        /// Gets the system particle texture
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        Vector2 textureOrigin;
        /// <summary>
        /// Gets the origin of the particle texture
        /// </summary>
        public Vector2 TextureOrigin
        {
            get
            {
                return textureOrigin;
            }
        }

        /// <summary>
        /// Gets the particle at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the particle to get.</param>
        /// <returns>The particle at the specified index.</returns>
        public IParticle this[int index]
        {
            get
            {
                return liveParticles[index];
            }
        }

        /// <summary>
        /// Indicates whether this particle system should be updated and drawn
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the maximum capacity of the particle system
        /// </summary>
        public int Capacity
        {
            get
            {
                return liveParticles.Capacity;
            }
        }

        /// <summary>
        /// Gets the number of live particles
        /// </summary>
        public int ParticleCount
        {
            get
            {
                return liveParticles.Count;
            }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Initializes a new instance of a base particle system
        /// </summary>
        /// <param name="maxCapacity">Maximum capacity of the system</param>
        /// <param name="texture">Texture of the particle</param>
        public ParticleSystem(int maxCapacity, Texture2D texture)
        {
            liveParticles = new List<T>(maxCapacity);
            deadParticles = new Stack<T>(maxCapacity);

            for (int i = 0; i < maxCapacity; i++)
            {
                deadParticles.Push(new T());
            }

            this.texture = texture;
            this.textureOrigin = new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f);

            Enabled = true;
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Removes a particle at a given index
        /// </summary>
        /// <param name="index">Index of the particle to remove</param>
        /// <returns>True if the particle was removed; false if the given index is out of range</returns>
        public bool RemoveAt(int index)
        {
            if (index < liveParticles.Count)
            {
                deadParticles.Push(liveParticles[index]);
                liveParticles[index] = liveParticles[liveParticles.Count - 1];
                liveParticles.RemoveAt(liveParticles.Count - 1);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all particles from the system
        /// </summary>
        public void Clear()
        {
            for (int i = liveParticles.Count - 1; i >= 0; i--)
            {
                deadParticles.Push(liveParticles[i]);
                liveParticles.RemoveAt(i);
            }
        }


        #endregion
    }
}
