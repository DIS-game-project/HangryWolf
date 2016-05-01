#pragma strict
import UnityEngine.UI;

var Button: Sprite;
var ButtonDown: Sprite;
var setState = false;

function Start(){
	changeSprite();
}

function OnMouseEnter(){
	if(MainCode.Sound == 1){
		GetComponent.<AudioSource>().Play();
	}
}

function OnMouseDown()
{
	if(MainCode.Music == 0){
		MainCode.Music = 1;
		return;
	}
	if(MainCode.Music == 1){
		MainCode.Music = 0;
		return;
	}
	
}

function changeSprite(){
	if(MainCode.Music == 1){
		GetComponent(Image).sprite = ButtonDown;
	}
	if(MainCode.Music == 0){
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