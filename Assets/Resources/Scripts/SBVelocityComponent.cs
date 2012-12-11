using UnityEngine;
using System.Collections;

public class SBVelocityComponent : SBAbstractComponent {
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public SBVelocityComponent() {
		componentType = ComponentType.Velocity;
	}
}
