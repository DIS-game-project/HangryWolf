#pragma strict
var Button: Sprite;
var Button_Down: Sprite;

function OnMouseEnter()
{
    GetComponent(SpriteRenderer).sprite = Button_Down;
	if(MainCode.Sound == 1){
		GetComponent.<AudioSource>().Play();
	}
}

function OnMouseExit()
{
    GetComponent(SpriteRenderer).sprite = Button;
}

function OnMouseDown()
{
    Application.LoadLevel("LevelSelection");
}