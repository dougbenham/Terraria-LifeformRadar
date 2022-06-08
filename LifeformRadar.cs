using System.Collections.Generic;
using LifeformRadar.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace LifeformRadar
{
	public class LifeformRadar : Mod
	{
		private UiCursor _uiCursor;
		private UserInterface _userInterface;

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				this._uiCursor = new UiCursor();
				this._uiCursor.Activate();
				this._userInterface = new UserInterface();
				this._userInterface.SetState(this._uiCursor);
			}
		}
		
		public override void Unload()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				this._userInterface = null;
				this._uiCursor = null;
			}
		}
		
		public override void UpdateUI(GameTime gameTime)
		{
			this._userInterface.Update(gameTime);
		}
		
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int num = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars")) + 1;
			if (num != -1)
			{
				layers.Insert(num, new LegacyGameInterfaceLayer("LifeformRadar: UI", delegate()
				{
					this._userInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				}, InterfaceScaleType.UI));
			}
		}
	}
}