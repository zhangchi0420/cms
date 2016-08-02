function initAccordion(){
	  var pm_accordion = new JSAccordion("js_accordion");
		pm_accordion.init();
	}
function JSAccordion(id) {
	if (!document.getElementById || !document.getElementsByTagName)
		return false;
	this.accordion = document.getElementById(id);
	this.subaccordions = this.accordion.getElementsByTagName("ul");
}
JSAccordion.prototype.init = function() {
	var mainInstance = this;
	for (var i = 0; i < this.subaccordions.length; i++)
		this.subaccordions[i].getElementsByTagName("li")[0].onclick = function() {
			mainInstance.toggleAccordion(this.parentNode);
		};
	this.expandOne();
};
JSAccordion.prototype.expandOne = function() {
	for (var i = 0; i < this.subaccordions.length; i++) {
		var links = this.subaccordions[i].getElementsByTagName("a");
		for (var j = 0; j < links.length; j++){
			if (links[j].className == "current")
			this.expandAccordion(this.subaccordions[i]);
			}
		}
};
JSAccordion.prototype.toggleAccordion = function(subaccordion) {
	if (subaccordion.className == "collapsed")
		this.expandAccordion(subaccordion);
	else
		this.collapseAccordion(subaccordion);
};
JSAccordion.prototype.expandAccordion = function(subaccordion) {
	var fullHeight = subaccordion.getElementsByTagName("li")[0].offsetHeight;
	var links = subaccordion.getElementsByTagName("a");
	for (var i = 0; i < links.length; i++)
		fullHeight += links[i].offsetHeight;
	var moveBy = Math.round(5 * links.length);
	
	var mainInstance = this;
	var intId = setInterval(function() {
		var curHeight = subaccordion.offsetHeight;
		var newHeight = curHeight + moveBy;
		if (newHeight < fullHeight)
			subaccordion.style.height = newHeight + "px";
		else {
			clearInterval(intId);
			subaccordion.style.height = "";
			subaccordion.style.overflow= "hidden";
			subaccordion.className = "current";
		}
	}, 30);
	this.collapseOthers(subaccordion);
};
JSAccordion.prototype.collapseAccordion = function(subaccordion) {
	var minHeight = subaccordion.getElementsByTagName("li")[0].offsetHeight;
	var moveBy = Math.round(5 * subaccordion.getElementsByTagName("a").length);
	var mainInstance = this;
	var intId = setInterval(function() {
		var curHeight = subaccordion.offsetHeight;
		var newHeight = curHeight - moveBy;
		if (newHeight > minHeight)
			subaccordion.style.height = newHeight + "px";
		else {
			clearInterval(intId);
			subaccordion.style.height = "";
			subaccordion.style.overflow= "hidden";
			subaccordion.className = "collapsed";
		}
	}, 30);
};
JSAccordion.prototype.collapseOthers = function(subaccordion) {
		for (var i = 0; i < this.subaccordions.length; i++)
			if (this.subaccordions[i] != subaccordion && this.subaccordions[i].className != "collapsed")
			
				this.collapseAccordion(this.subaccordions[i]);
};