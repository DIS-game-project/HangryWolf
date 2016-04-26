#pragma strict

var image: GUITexture;
var Image_01: Texture2D;
var Image_02: Texture2D;

function Update(){
	if(MainCode.Sound == 0){
		image.texture = Image_01;
	}
	if(MainCode.Sound == 1){
		image.texture = Image_02;
	}
}
function OnMouseEnter(){
	if(MainCode.Sound == 0){
		GetComponent.<AudioSource>().Play();
	}
}

function OnMouseDown()
{
	if(MainCode.Sound == 0){
		MainCode.Sound = 1;
		return;
	}
	if(MainCode.Sound == 1){
		MainCode.Sound = 0;
		return;
	}
	
}