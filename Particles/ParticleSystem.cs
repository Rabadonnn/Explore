using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Explore.Particles
{
    public class ParticleSystem
    {
        public bool enabled = true;
        public Settings settings;
        public Rectangle rectangle;
        private Texture2D texture;
        private List<Particle> particles;
        private Random rand = new Random();

        private bool oneTime = false;
        private float lifetime;

        public ParticleSystem(Settings _settings, Rectangle _rectangle) {
            settings = _settings;
            rectangle = _rectangle;
            particles = new List<Particle>();
            if (settings.oneTime) {
                lifetime = settings.lifespan;
                oneTime = true;
            }
        }

        public void Update() {
            settings.CheckValues();
            if (enabled) {

                if (oneTime) {
                    if (lifetime >= 0) {
                        for (int i = 0; i < settings.number_per_frame; i++) {
                            Vector2 pos = new Vector2(rand.Next(rectangle.Left, rectangle.Right), rand.Next(rectangle.Top, rectangle.Bottom));
                            Particle particleToAdd = new Particle(settings, pos);
                            particles.Add(particleToAdd);
                        }

                        for (int i = 0; i < particles.Count; i++) {
                            particles[i].Update();

                            if (particles[i].IsDead) {
                                particles.RemoveAt(i);
                            }
                        }
                        lifetime -= GameManager.DeltaTime;
                    } else {
                        enabled = false;
                    }
                } else {
                    for (int i = 0; i < settings.number_per_frame; i++) {
                        Vector2 pos = new Vector2(rand.Next(rectangle.Left, rectangle.Right), rand.Next(rectangle.Top, rectangle.Bottom));
                        Particle particleToAdd = new Particle(settings, pos);
                        particles.Add(particleToAdd);
                    }

                    for (int i = 0; i < particles.Count; i++) {
                        particles[i].Update();

                        if (particles[i].IsDead) {
                            particles.RemoveAt(i);
                        }
                    }
                }
            }
            
        }

        public void SetTexture(Texture2D _texture) {
            texture = _texture;
        }

        public void SetTexture() {
            texture = GameManager.Assets["circle"];
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (enabled) {
                for (int i = 0; i < particles.Count; i++) {
                    spriteBatch.Draw(texture, destinationRectangle: particles[i].rectangle, color: particles[i].color * particles[i].Alpha);
                }
            }
        }
    }
}