using System.Collections.Generic;
using System.Collections;
using System;

public class FSM
{
	List<System.Action> states;
	
	public FSM ()
	{
		this.states = new List<System.Action> ();
	}
	
	public void DoState ()
	{
		var currentStateFunction = GetCurrentState ();
		
		if (currentStateFunction != null)
			currentStateFunction ();
	}
	
	public System.Action PopState ()
	{
		return states.Pop ();
	}
	
	public void PushState (System.Action state)
	{
		if (GetCurrentState () != state)
			states.Push (state);
	}
	
	public System.Action GetCurrentState ()
	{
		return states.Count > 0 ? states [states.Count - 1] : null;
	}
}

public static class ListExtensions
{
	
	public static T Pop<T> (this List<T> theList)
	{
		var local = theList [theList.Count - 1];
		theList.RemoveAt (theList.Count - 1);
		return local;
	}
	
	public static void Push<T> (this List<T> theList, T item)
	{
		theList.Add (item);
	}
}