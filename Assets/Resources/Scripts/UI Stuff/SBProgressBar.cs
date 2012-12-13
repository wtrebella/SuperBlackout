using UnityEngine;
using System.Collections;

public enum ProgressBarType {
	FillLeftToRight,
	FillRightToLeft
}

public class SBProgressBar : FContainer {
	public float maxWidth;
	public float height;
	public ProgressBarType type;
	
	private float percent_;
	private FSprite sprite_;
	
	public SBProgressBar(float width, float height, Color color, ProgressBarType type) {
		this.type = type;
		this.height = height;
		maxWidth = width;
		percent_ = 1;
		
		sprite_ = WTSquareMaker.Square(width, height);
		sprite_.color = color;
		if (type == ProgressBarType.FillLeftToRight) {
			sprite_.anchorX = 0;
			sprite_.x = -maxWidth / 2f;
		}
		else if (type == ProgressBarType.FillRightToLeft) {
			sprite_.anchorX = 1;
			sprite_.x = maxWidth / 2f;
		}
		AddChild(sprite_);
	}
	
	public float percent {
		get {return percent;}
		set {
			percent_ = value;
			if (percent_ < 0) percent_ = 0;
			if (percent_ > 1) percent_ = 1;
			sprite_.width = percent_ * maxWidth;
		}
	}
}
