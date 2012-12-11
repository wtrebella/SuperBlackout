using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface ComponentInterface {
	void HandleComponentAdded(SBAbstractComponent component);
	void HandleComponentRemoved(SBAbstractComponent component);
}

public class SBEntity : FContainer, ComponentInterface {
	List<SBAbstractComponent> components;
	public string name;
	
	public SBEntity() {
		name = "entity";
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
	
	public SBSpriteComponent SpriteComponent() {
		return ComponentForType(ComponentType.Sprite) as SBSpriteComponent;	
	}
	
	public SBVelocityComponent VelocityComponent() {
		return ComponentForType(ComponentType.Velocity) as SBVelocityComponent;	
	}
	
	public void HandleComponentRemoved(SBAbstractComponent component) {
		switch (component.componentType) {
		case ComponentType.Sprite:
			SBSpriteComponent sc = component as SBSpriteComponent;
			RemoveChild(sc.sprite);
			break;
		case ComponentType.Velocity:
			break;
		}
	}
	
	public void HandleComponentAdded(SBAbstractComponent component) {
		switch (component.componentType) {
		case ComponentType.Sprite:
			SBSpriteComponent sc = component as SBSpriteComponent;
			AddChild(sc.sprite);
			break;
		case ComponentType.Velocity:
			break;
		}
	}
}
