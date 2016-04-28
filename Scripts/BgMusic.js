#pragma strict
var audiosource : AudioSource;

function Update()
{
	if(MainCode.Music == 0){
		audiosource.enabled = true;
	}
	if(MainCode.Music == 1){
		audiosource.enabled = false;
	}
	
}