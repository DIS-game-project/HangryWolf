#pragma strict

function Update(){
	
}
function OnMouseEnter(){
	if(MainCode.Sound == 1){
		GetComponent.<AudioSource>().Play();
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