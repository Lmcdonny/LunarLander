using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LunarLander.Lander
{
    internal class Terrain
    {
        private BasicEffect m_effect;
        private GraphicsDeviceManager m_graphics;
        private VertexPositionColor[] m_vertsTriStrip;
        private int[] m_indexTriStrip;
        private const int ITERATIONS = 4;
        private List<Vector3> m_verts;
        Random rnd = new Random();
        private Rectangle m_landingZone;

        public Terrain(GraphicsDeviceManager graphics)
        {
            m_graphics = graphics;

            m_graphics.GraphicsDevice.RasterizerState = new RasterizerState
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.CullCounterClockwiseFace,   // CullMode.None If you want to not worry about triangle winding order
                MultiSampleAntiAlias = true,
            };

            m_effect = new BasicEffect(m_graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),

                Projection = Matrix.CreateOrthographicOffCenter(
                    0, m_graphics.GraphicsDevice.Viewport.Width,
                    m_graphics.GraphicsDevice.Viewport.Height, 0,   // doing this to get it to match the default of upper left of (0, 0)
                    0.1f, 2)
            };
        }

        public Rectangle GetLandingZone()
        {
            return m_landingZone;
        }

        public List<Vector3> GetVerts()
        {
            return m_verts;
        }

        // Recursively modify the midpoint 
        private List<Vector3> ModifyTerrain(Vector3 start, Vector3 end, int iterations)
        {
            List<Vector3> verts = new List<Vector3>();
            if (iterations <= 0)
            {
                return verts;
            }

            Vector3 midpoint = (start + end) / 2;

            // generate normal value (ChatGPT helped me here)
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            const double STDDEV = 20;
            double randNormal = STDDEV * randStdNormal;

            midpoint += new Vector3(0, (float)randNormal, 0);
            verts.AddRange(ModifyTerrain(start, midpoint, iterations - 1));
            verts.Add(midpoint);
            verts.AddRange(ModifyTerrain(midpoint, end, iterations - 1));

            return verts;
        }
        public void GenerateTerrain()
        {
            int width = m_graphics.PreferredBackBufferWidth;
            int height = m_graphics.PreferredBackBufferHeight;

            Vector3 start = new Vector3(0, height - 200, 0);
            Vector3 end = new Vector3(width, height - 200, 0);

            List<Vector3> verts = new List<Vector3>();
            verts.Add(start);
            verts.AddRange(ModifyTerrain(start, end, ITERATIONS));
            verts.Add(end);

            int landingZone = rnd.Next(2, verts.Count - 4);
            verts[landingZone + 1] = new Vector3(verts[landingZone + 1].X, verts[landingZone].Y, 0);
            verts[landingZone + 2] = new Vector3(verts[landingZone + 2].X, verts[landingZone].Y, 0);
            int landingZoneWidth = Math.Abs((int)verts[landingZone + 2].X - (int)verts[landingZone].X);
            m_landingZone = new Rectangle((int)verts[landingZone].X, (int)verts[landingZone].Y, landingZoneWidth, 2);

            m_verts = verts;

            m_vertsTriStrip = new VertexPositionColor[verts.Count * 2 + 1];
            m_indexTriStrip = new int[verts.Count * 2 + 1];

            for (int i = 0; i < verts.Count; i++)
            {
                int triStripIndex = i * 2;
                m_vertsTriStrip[triStripIndex].Position = verts[i];
                m_vertsTriStrip[triStripIndex].Color = Color.Black;
                m_indexTriStrip[triStripIndex] = triStripIndex;

                m_vertsTriStrip[triStripIndex + 1].Position = new Vector3(verts[i].X, height, 0);
                m_vertsTriStrip[triStripIndex + 1].Color = new Color(59, 59, 59);
                m_indexTriStrip[triStripIndex + 1] = triStripIndex + 1;
            }
            m_vertsTriStrip[m_vertsTriStrip.Length - 1] = m_vertsTriStrip[0];

            m_vertsTriStrip = m_vertsTriStrip.Reverse().ToArray();


        }

        public void Draw(Texture2D texture, SpriteBatch sb)
        {
            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                // This is the all-important line that sets the effect, and all of its settings, on the graphics device
                pass.Apply();
                
                m_graphics.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleStrip,
                    m_vertsTriStrip, 0, m_vertsTriStrip.Length,
                    m_indexTriStrip, 0, m_indexTriStrip.Length - 2
                );
            }
            sb.Draw(texture, m_landingZone, Color.Red);
        }
    }
}
