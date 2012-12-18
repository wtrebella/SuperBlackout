using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface ComponentInterface {
	void HandleComponentAdded(SBAbstractComponent component);
	void HandleComponentRemoved(SBAbstractComponent component);
}

public class SBEntity : FContainer, ComponentInterface {
	public bool isBeingControlledBySittableComponent = false;
	public bool isBeingControlledByDirectionComponent = false;
	public FContainer rotatingContainer;
	List<SBAbstractComponent> components;
	List<SBSpriteComponent> spriteComponents;
	public string name;
	public int tag = -1;
	
	public SBEntity(string name) {
		this.name = name;
		rotatingContainer = new FContainer();
		AddChild(rotatingContainer);
		components = new List<SBAbstractComponent>();
		spriteComponents = new List<SBSpriteComponent>();
	}
	
	public void AddComponent(SBAbstractComponent component) {
		components.Add(component);
		component.owner = this;
		HandleComponentAdded(component);
	}
	
	public void RemoveComponent(SBAbstractComponent component) {
		components.Remove(component);
		component.owner = null;
		HandleComponentRemoved(component);
	}
	
	public SBAbstractComponent ComponentForType(ComponentType componentType) {
		foreach (SBAbstractComponent component in components) {
			if (component.componentType == componentType) return component;	
		}
		
		return null;
	}
	
	public SBCollideComponent CollideComponent() {
		return ComponentForType(ComponentType.Collide) as SBCollideComponent;	
	}
	
	public SBSittableComponent SittableComponent() {
		return ComponentForType(ComponentType.Sittable) as SBSittableComponent;	
	}
	
	public SBSpriteComponent SpriteComponent(int spriteComponentIndex = 0) {
		return spriteComponents[spriteComponentIndex];	
	}
	
	public SBVelocityComponent VelocityComponent() {
		return ComponentForType(ComponentType.Velocity) as SBVelocityComponent;	
	}
	
	public SBDirectionComponent DirectionComponent() {
		return ComponentForType(ComponentType.Direction) as SBDirectionComponent;	
	}
	
	public SBProgressBarComponent ProgressBarComponent() {
		return ComponentForType(ComponentType.ProgressBar) as SBProgressBarComponent;	
	}
	
	public SBTimerComponent TimerComponent() {
		return ComponentForType(ComponentType.Timer) as SBTimerComponent;	
	}
	
	public void HandleComponentAdded(SBAbstractComponent component) {
		if (component.componentType == ComponentType.Sprite) {
			SBSpriteComponent sc = component as SBSpriteComponent;
			spriteComponents.Add(sc);
			if (sc.shouldBeInRotatingContainer) rotatingContainer.AddChild(sc.sprite);
			else AddChild(sc.sprite);
		}
		else if (component.componentType == ComponentType.ProgressBar) {
			SBProgressBarComponent pbc = component as SBProgressBarComponent;
			AddChild(pbc.progressBar);	
		}
	}
	
	public void HandleComponentRemoved(SBAbstractComponent component) {
		if (component.componentType == ComponentType.Sprite) {
			SBSpriteComponent sc = component as SBSpriteComponent;
			if (sc.shouldBeInRotatingContainer) rotatingContainer.RemoveChild(sc.sprite);
			else RemoveChild(sc.sprite);
		}
		else if (component.componentType == ComponentType.ProgressBar) {
			SBProgressBarComponent pbc = component as SBProgressBarComponent;
			RemoveChild(pbc.progressBar);	
		}
	}
	
	virtual public void HandleUpdate() {
		foreach (SBAbstractComponent component in components) component.HandleUpdate();
	}
}
