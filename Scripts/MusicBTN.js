#pragma strict

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