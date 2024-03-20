using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LunarLander.Lander
{
    internal class Lander
    {
        private Vector2 m_velocity;
        private float m_orientation = (float)Math.PI / 4; // in Degrees
        private Rectangle m_model;
        private Vector2 m_position;
        private float GRAVITY = 1f;
        private Texture2D m_texture;

        public Lander()
        {
            m_position = new Vector2(50, 50);
            m_velocity = Vector2.Zero;
            m_model = new Rectangle((int)m_position.X, (int)m_position.Y, 40, 30);
        }

        public void LoadContent(Texture2D landerTexture)
        {
            m_texture = landerTexture;
        }

        public Rectangle GetRectangle()
        {
            return m_model;
        }
        public float GetOrientation()
        {
            return m_orientation;
        }
        public Vector2 GetVelocity()
        {
            return m_velocity;
        }
        private void UpdateVelocity(TimeSpan deltaTime, int thrustMultiplier) //thustMultiplier is 1 or 0 based on user input
        {
            double gravity = GRAVITY * deltaTime.TotalSeconds; // Calculate gravity using seconds
            m_velocity.Y += (float)gravity; // Update Y velocity with gravity

            const float THRUSTPOWER = 5f;
            float totalThrust = THRUSTPOWER * thrustMultiplier * (float)deltaTime.TotalSeconds;
            Vector2 thrust = new Vector2(
                (float)Math.Cos(m_orientation - Math.PI / 2) * totalThrust,
                (float)Math.Sin(m_orientation - Math.PI / 2) * totalThrust
                );
            m_velocity += thrust;
        }
        private void UpdatePosition(TimeSpan deltaTime)
        {
            // Update X and Y position using velocity and deltaTime
            m_position += m_velocity;

            m_model = new Rectangle((int)m_position.X, (int)m_position.Y, 30, 20);
        }
        private void UpdateOrientation(TimeSpan deltaTime, int multiplier) //multiplier parameter controls rotation direction
        {
            float rotationSpeed = (float)Math.PI;
            m_orientation += rotationSpeed * (float)deltaTime.TotalSeconds * multiplier;
            if (m_orientation > Math.PI * 2) m_orientation -= (float)(Math.PI * 2);
            else if (m_orientation < 0) m_orientation += (float)(Math.PI * 2);

        }

        private void ParseCommands(TimeSpan deltaTime, Keys[] keys)
        {
            int thrustMultiplier = 0;
            foreach (ConsoleKey key in keys)
            {
                if (key == ConsoleKey.LeftArrow)
                {
                    UpdateOrientation(deltaTime, -1);
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    UpdateOrientation(deltaTime, 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    thrustMultiplier = 1;
                }
            }

            UpdateVelocity(deltaTime, thrustMultiplier);
        }
        public void Update(TimeSpan deltaTime, Keys[] keys)
        {
            ParseCommands(deltaTime, keys);
            UpdatePosition(deltaTime);
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 origin = new Vector2(m_model.Width / 2, m_model.Height / 2);
            sb.Draw(
                texture: m_texture,
                position: m_position,
                sourceRectangle: null,
                color: Color.White,
                rotation: m_orientation,
                origin: origin,
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: 0
                );
        }
    }
}
