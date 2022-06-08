using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace LifeformRadar.UI
{
	public class UiCursorElement : UIElement
	{
		private readonly List<NPC> _importantNpcs = new List<NPC>();
		private bool _drawCursors;

		public override void Update(GameTime gameTime)
		{
			if (Main.mapStyle == 2)
			{
				_drawCursors = false;
				return;
			}
			UpdateImportantNpcsList();
			_drawCursors = _importantNpcs.Count > 0;
		}
		
		private void UpdateImportantNpcsList()
		{
			var player = Main.player[Main.myPlayer];

			_importantNpcs.Clear();
			for (var i = 0; i < 200; i++)
			{
				var npc = Main.npc[i];
				if (npc.active && npc.rarity > 0 
				                && player.accCritterGuide
								&& !player.hideInfo[11])
				{
					_importantNpcs.Add(npc);
				}
			}
		}
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!_drawCursors)
			{
				return;
			}
			var center = Main.LocalPlayer.Center;
			var num = 1f / Main.UIScale;
			foreach (var npc in _importantNpcs)
			{
				if (Config.Instance.HideOnScreen)
				{
					var vector = npc.Center - Main.screenPosition;
					if (vector.X >= 0f && vector.Y >= 0f && vector.X <= Main.screenWidth * Main.UIScale && vector.Y <= Main.screenHeight * Main.UIScale)
					{
						continue;
					}
				}

				var vector2 = npc.Center - center;
				vector2.Y *= Main.LocalPlayer.gravDir;
				var num2 = Clamp(1.15f - 1f / (2f * Main.screenWidth) * vector2.Length(), 0.02f, 1f);
				var num4 = num2 * 1.2f;
				vector2.Normalize();
				var vector3 = center + vector2 * Config.Instance.CursorDistance - Main.screenPosition;
				vector3 *= num;
				var num5 = (float) Math.Atan2(vector2.Y, vector2.X);
				var cursorTexture = GetCursorTexture();
				spriteBatch.Draw(cursorTexture, vector3, null, Color.Yellow, num5, cursorTexture.Size() / 2f, 1.2f * Config.Instance.CursorSize, 0, 1f);
				var headTexture = GetHeadTexture(npc);
				if (headTexture != null)
				{
					var vector4 = center + vector2 * (Config.Instance.CursorDistance - 20f * Main.UIScale * Config.Instance.CursorSize) - Main.screenPosition;
					vector4 *= num;
					spriteBatch.Draw(headTexture, vector4, null, Color.White, 0f, headTexture.Size() * 0.5f, num4 * Config.Instance.CursorSize, npc.GetBossHeadSpriteEffects(), 0f);
				}
			}
		}
		
		private static Texture2D GetHeadTexture(NPC boss)
		{
			var bossHeadTextureIndex = boss.GetBossHeadTextureIndex();
			if (bossHeadTextureIndex < 0 || bossHeadTextureIndex >= Main.npcHeadBossTexture.Length)
			{
				return null;
			}
			return Main.npcHeadBossTexture[boss.GetBossHeadTextureIndex()];
		}
		
		private static Texture2D GetCursorTexture()
		{
			return ModContent.GetTexture("LifeformRadar/UI/Cursor");
		}
		
		private static float Clamp(float value, float min, float max)
		{
			if (value > max)
				return max;
			if (value < min)
				return min;
			return value;
		}
	}
}