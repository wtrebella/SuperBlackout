using UnityEngine;
using System.Collections;

public class SBBladderBar : SBProgressBar {	
	public SBBladderBar(int playerNum) : base(190 + 10, 34 + 10, new Color(0.92f, 0.91f, 0.07f), ProgressBarType.FillLeftToRight) {
		/*FSprite backgroundSprite = new FSprite("bladderBar.psd");
		backgroundSprite.color = new Color(0.12f, 0.12f, 0.12f);
		AddChild(backgroundSprite);*/
		
		FSprite background = WTSquareMaker.Square(maxWidth, height);
		background.alpha = 0.5f;
		AddChild(background);
		
		FLabel label = new FLabel("Silkscreen", "Bladder");//string.Format("Bladder {0}", playerNum));
		label.color = new Color(0.12f, 0.12f, 0.12f);
		label.scale = 0.35f;
		//label.y += 2f;
		label.x -= 2f;
		AddChild(label);
		
		this.percent = 0f;
	}
}
