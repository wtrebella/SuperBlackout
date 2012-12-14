using UnityEngine;
using System.Collections;

public class SBTimerComponent : SBAbstractComponent {
	private float timer_ = 0;
	
	public SBTimerComponent() {
		name = "timer component";
		componentType = ComponentType.Timer;
	}
	
	public void Reset() {
		timer_ = 0;	
	}
	
	public void Start() {
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	public void Restart() {
		Reset();
		Start();
	}
	
	public void Stop() {
		Pause();
		Reset();
	}
	
	public void Pause() {
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		timer_ += Time.fixedDeltaTime;
	}
	
	public float timer {
		get {return timer_;}
	}
}
