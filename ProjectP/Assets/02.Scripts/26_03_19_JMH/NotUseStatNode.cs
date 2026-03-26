/*
using XNode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatNode : Node
{

	[Input] public bool Input;
	[Output] public bool Output;

	[SerializeField] private string _nodeName;
	[SerializeField] private int _value;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "output") return this;
		return null; // Replace this
	}
	
	public string GetNodeName() {
		return _nodeName;
	}
	
	public int GetValue() {
		return _value;
	}
}
*/