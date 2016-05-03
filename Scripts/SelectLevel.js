#pragma strict

var target : GameObject;
var otherTarget : GameObject;

function OnMouseDown()
{
	
	var otherTargetState = otherTarget.GetComponent.<Animator>().GetCurrentAnimatorStateInfo(0);
	if(otherTargetState.IsName("ShownState")){
		otherTarget.GetComponent.<Animator>().SetTrigger("HideLevelInfo");
	}
	
	
	var animatorState = target.GetComponent.<Animator>().GetCurrentAnimatorStateInfo(0);
	Debug.Log(animatorState.IsName("ShownState"));
	if(!animatorState.IsName("ShownState")){
		target.GetComponent.<Animator>().SetTrigger("ShowLevelInfo");
	} else{
		target.GetComponent.<Animator>().SetTrigger("HideLevelInfo");
	}
}