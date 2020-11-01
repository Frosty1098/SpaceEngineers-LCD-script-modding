using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;

namespace Frosty.TXTLCD
{
    [MyTextSurfaceScript("TXT LCD", "TXT to LCD")]
    public class MyTSSTestMod : MyTSSCommon
    {
        public static float ASPECT_RATIO = 1f;
        public static float DECORATION_RATIO = 0.25f;
        public static float TEXT_RATIO = 0.05f;
        private StringBuilder m_sb = new StringBuilder();
        private Vector2 m_innerSize;
        private Vector2 m_decorationSize;
        public override ScriptUpdate NeedsUpdate
        {
            get
            {
                return ScriptUpdate.Update100;
            }
        }

        public MyTSSTestMod(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
        {
            m_innerSize = new Vector2(ASPECT_RATIO, 1f);
            FitRect(size, ref m_innerSize);
            m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * this.m_innerSize.Y);
            m_sb.Clear();
            m_sb.Append("M");
            Vector2 vector2 = surface.MeasureStringInPixels(m_sb, m_fontId, 1f);
            m_fontScale = TEXT_RATIO * m_innerSize.Y / vector2.Y;
        }
		
        public override void Run()
        {
            using (MySpriteDrawFrame frame = m_surface.DrawFrame())
            {
                AddBackground(frame, new Color?(new Color(m_foregroundColor, 0.66f)));
                string str = "No file found";
				if (MyAPIGateway.Utilities.FileExistsInGlobalStorage("OutputHere.txt")){
				var read = MyAPIGateway.Utilities.ReadFileInGlobalStorage("OutputHere.txt");
				str = read.ReadToEnd();
				read.Dispose();
				}
				
                Vector2 vector2 = m_surface.MeasureStringInPixels(new StringBuilder(str), m_fontId, m_fontScale);
                MySprite sprite = new MySprite()
                {
                    Position = new Vector2?(new Vector2(0,m_halfSize.Y *2f - vector2.Y)),
                    Size = new Vector2?(new Vector2(m_innerSize.X, m_innerSize.Y)),
                    Type = SpriteType.TEXT,
                    FontId = m_fontId,
                    Alignment = TextAlignment.LEFT,
                    Color = new Color?(m_foregroundColor),
                    RotationOrScale = m_fontScale,
                    Data = str
                };
                frame.Add(sprite);
                float scale = (float)(m_innerSize.Y / 256.0 * 0.899999976158142);
                //AddBrackets(frame, new Vector2(64f, 256f), scale);
            }
        }
    }
}
