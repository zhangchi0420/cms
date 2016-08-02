// JScript ÇÐ»»

function changebutton(num)
{
	var as = document.getElementById("mastermenu").children;
	for(var i=1;i<as.length;i++)
	{
		as[i].className="";	
	}
	as[num].className="hot";
	
}
function changebutton1(num)
{
	var as = document.getElementById("mastermenu1").children;
	for(var i=1;i<as.length;i++)
	{
		as[i].className="";	
	}
	as[num].className="hot";
	
}
function changebutton2(num)
{
	var as = document.getElementById("mastermenu2").children;
	for(var i=1;i<as.length;i++)
	{
		as[i].className="";	
	}
	as[num].className="hot";
	
}