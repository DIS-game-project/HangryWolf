#pragma strict
import UnityEngine.UI;

var Button: Sprite;
var ButtonDown: Sprite;
var Audio: AudioSource;
var setState = false;

function Start(){
	changeSprite();
}

function OnMouseEnter(){
	if(MainCode.Sound == 1){
		GetComponent.<AudioSource>().PlayOneShot(Audio.clip);
		/* return;  */
	}
}

function OnMouseDown()
{
	if(MainCode.Sound == 1){
		MainCode.Sound = 0;
		return;
	}
	if(MainCode.Sound == 0){
		MainCode.Sound = 1;
		return;
	}
}
	
function changeSprite(){
	if(MainCode.Sound == 0){
		GetComponent(Image).sprite = ButtonDown;
	}
	if(MainCode.Sound == 1){
		GetComponent(Image).sprite = Button;
	}
}

function checkState(){
	if(GetComponent.<Button>().interactable){
		GetComponent.<Button>().interactable = false;
	} else{
		GetComponent.<Button>().interactable = true;
	}
}