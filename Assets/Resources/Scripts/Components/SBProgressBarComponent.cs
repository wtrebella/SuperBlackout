using UnityEngine;
using System.Collections;

public class SBProgressBarComponent : SBAbstractComponent {
	public SBProgressBar progressBar;
	
	public SBProgressBarComponent(float xOffset, float yOffset, float width, float height, Color color, ProgressBarType type) {
		name = "progress bar component";
		componentType = ComponentType.ProgressBar;
		progressBar = new SBProgressBar(width, height, color, type);
		progressBar.x = xOffset;
		progressBar.y = yOffset;
	}
}
