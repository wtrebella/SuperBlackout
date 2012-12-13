using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface ComponentInterface {
	void HandleComponentAdded(SBAbstractComponent component);
	void HandleComponentRemoved(SBAbstractComponent component);
}

public class SBEntity : FContainer, ComponentInterface {
	public FContainer rotatingContainer;
	
	public bool isBeingControlledBySittableComponent = false;
	public bool isBeingControlledByDirectionComponent = false;
	List<SBAbstractComponent> components;
	public string name;
	
	public SBEntity(string name) {
		rotatingContainer = new FContainer();
		AddChild(rotatingContainer);
		this.name = name;
		components = new List<SBAbstractComponent>();
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
	
	public SBSpriteComponent SpriteComponent() {
		return ComponentForType(ComponentType.Sprite) as SBSpriteComponent;	
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
	
	public void HandleUpdate() {
		foreach (SBAbstractComponent component in components) component.HandleUpdate();
	}
	
	public void HandleComponentAdded(SBAbstractComponent component) {
		if (component.componentType == ComponentType.Sprite) {
			SBSpriteComponent sc = component as SBSpriteComponent;
			if (sc.shouldBeInRotatingContainer) rotatingContainer.AddChild(sc.sprite);
			else AddChild(sc.sprite);
		}
		
		else if (component.componentType == ComponentType.ProgressBar) {
			SBProgressBarComponent pbc = component as SBProgressBarComponent;
			AddChild(pbc.progressBar);
		}
	}
}
