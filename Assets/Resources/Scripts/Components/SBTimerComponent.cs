using UnityEngine;
using System.Collections;

public class SBTimerComponent : SBAbstractComponent {
	private bool isRunning_ = false;
	private float timer_ = 0;
	
	public SBTimerComponent() {
		name = "timer component";
		componentType = ComponentType.Timer;
	}
	
	public void Reset() {
		timer_ = 0;	
	}
	
	public void Start() {
		isRunning_ = true;
		//Futile.instance.SignalUpdate += HandleUpdate;
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
		isRunning_ = false;
		//Futile.instance.SignalUpdate -= HandleUpdate;
	}
		
	override public void HandleUpdate() {
		if (!isRunning_) return;
		
		base.HandleUpdate();
		timer_ += Time.fixedDeltaTime;
	}
	
	public float timer {
		get {return timer_;}
	}
	
	public bool isRunning {
		get {return isRunning_;}	
	}
}
