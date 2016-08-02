function initAccordion1(){
	  var pm_accordion1 = new JSAccordion1("js_accordion1");
		pm_accordion1.init();
	}
function JSAccordion1(id) {
	if (!document.getElementById || !document.getElementsByTagName)
		return false;
	this.accordion1 = document.getElementById(id);
	this.subaccordion1s = this.accordion1.getElementsByTagName("ul");
}
JSAccordion1.prototype.init = function() {
	var mainInstance = this;
	for (var i = 0; i < this.subaccordion1s.length; i++)
		this.subaccordion1s[i].getElementsByTagName("li")[0].onclick = function() {
			mainInstance.toggleAccordion(this.parentNode);
		};
	this.expandOne();
};
JSAccordion1.prototype.expandOne = function() {
	for (var i = 0; i < this.subaccordion1s.length; i++) {
		var links = this.subaccordion1s[i].getElementsByTagName("div");
		for (var j = 0; j < links.length; j++){
			if (links[j].className == "current")
			this.expandAccordion(this.subaccordion1s[i]);
			}
		}
};
JSAccordion1.prototype.toggleAccordion = function(subaccordion1) {
	if (subaccordion1.className == "collapsed")
		this.expandAccordion(subaccordion1);
	else
		this.collapseAccordion(subaccordion1);
};
JSAccordion1.prototype.expandAccordion = function(subaccordion1) {
	var fullHeight = subaccordion1.getElementsByTagName("li")[0].offsetHeight;
	var links = subaccordion1.getElementsByTagName("div");
	for (var i = 0; i < links.length; i++)
		fullHeight += links[i].offsetHeight;
	var moveBy = Math.round(5 * links.length);
	
	var mainInstance = this;
	var intId = setInterval(function() {
		var curHeight = subaccordion1.offsetHeight;
		var newHeight = curHeight + moveBy;
		if (newHeight < fullHeight)
			subaccordion1.style.height = newHeight + "px";
		else {
			clearInterval(intId);
			subaccordion1.style.height= "";
			subaccordion1.className = "current";
		}
	}, 10);
	this.collapseOthers(subaccordion1);
};
JSAccordion1.prototype.collapseAccordion = function(subaccordion1) {
	var minHeight = subaccordion1.getElementsByTagName("li")[0].offsetHeight;
	var moveBy = Math.round(5 * subaccordion1.getElementsByTagName("div").length);
	var mainInstance = this;
	var intId = setInterval(function() {
		var curHeight = subaccordion1.offsetHeight;
		var newHeight = curHeight - moveBy;
		if (newHeight > minHeight)
			subaccordion1.style.height = newHeight + "px";
		else {
			clearInterval(intId);
			subaccordion1.style.height = "";
			subaccordion1.className = "collapsed";
		}
	}, 10);
};
JSAccordion1.prototype.collapseOthers = function(subaccordion1) {
		for (var i = 0; i < this.subaccordion1s.length; i++)
			if (this.subaccordion1s[i] != subaccordion1 && this.subaccordion1s[i].className != "collapsed")
			
				this.collapseAccordion(this.subaccordion1s[i]);
};