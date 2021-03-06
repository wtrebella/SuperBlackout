using UnityEngine;
using System.Collections;
using System;

public enum ComponentType {
	Abstract,
	Sprite,
	Velocity,
	Sittable,
	Collide,
	Direction,
	ProgressBar,
	Timer
}

public class SBAbstractComponent {
	public ComponentType componentType;
	public SBEntity owner;
	public string name;
	
	public SBAbstractComponent() {
		componentType = ComponentType.Abstract;
		name = "abstract";
	}
	
	virtual public void HandleUpdate() {
		
	}
}
