#pragma strict

var image: GUITexture;
var Image_01: Texture2D;
var Image_02: Texture2D;

function Update(){
	if(MainCode.Music == 0){
		image.texture = Image_01;
	}
	if(MainCode.Music == 1){
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
	if(MainCode.Music == 0){
		MainCode.Music = 1;
		return;
	}
	if(MainCode.Music == 1){
		MainCode.Music = 0;
		return;
	}
	
}