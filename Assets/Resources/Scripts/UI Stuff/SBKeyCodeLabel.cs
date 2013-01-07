using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBKeyCodeLabel : FContainer {
	public FLabel label;
	public bool isFlashing = true;
	float flashingTimer = 0;
	bool labelIsColored = false;
	Color baseColor;
	Color flashColor;
	
	public SBKeyCodeLabel(string text, Color baseColor, Color flashColor) {				
		this.baseColor = baseColor;
		this.flashColor = flashColor;
		
		label = new FLabel("Silkscreen", text);
		label.anchorX = 0;
		label.anchorY = 1;
		
		label.y -= 68f;
		
		AddChild(label);
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {
		if (!isFlashing) return;
		
		flashingTimer += Time.fixedDeltaTime;
		if (flashingTimer >= 0.25f) {
			flashingTimer = 0;
			if (labelIsColored) label.color = flashColor;
			else label.color = baseColor;
			labelIsColored = !labelIsColored;
		}
	}
}
