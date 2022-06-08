using Terraria;
using Terraria.UI;

namespace LifeformRadar.UI
{
	public class UiCursor : UIState
	{
		public override void OnInitialize()
		{
			var uiCursorElement = new UiCursorElement();
			uiCursorElement.Width.Set((float) Main.screenWidth, 0f);
			uiCursorElement.Height.Set((float) Main.screenHeight, 0f);
			base.Append(uiCursorElement);
		}
	}
}