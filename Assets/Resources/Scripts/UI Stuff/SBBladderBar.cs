using UnityEngine;
using System.Collections;

public class SBBladderBar : SBProgressBar {
	FSprite backgroundSprite;
	
	public SBBladderBar() : base(190, 34, new Color(0.92f, 0.91f, 0.07f), ProgressBarType.FillLeftToRight) {
		backgroundSprite = new FSprite("bladderBar.psd");
		backgroundSprite.color = new Color(0.12f, 0.12f, 0.12f);
		AddChild(backgroundSprite);
		
		this.percent = 0.42f;
	}
}
