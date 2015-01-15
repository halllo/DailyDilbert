var request = require("request");

var regex = /data-url="http:\/\/dilbert.com\/strip\/.*?" data-image="(.*?)"/;
var getUrlFromHtml = function(html) {
	var matches = html.match(regex);
	if (matches != null)
	{
		return matches[1];
	} else {
		return "cannot.find.url";
	}
}

var getHtmlFromDilbertCom = function(callback) {
	request("http://dilbert.com", function (error, response, html) {
		if (!error && response.statusCode == 200) {
			callback(html);
		}
	});
}

var getUrlFromDilbertCom = function(callback) {
	getHtmlFromDilbertCom(function(html){
		var url = getUrlFromHtml(html);
		callback(url);
	});
}

var getImageFromDilbertCom = function(callback) {
	getHtmlFromDilbertCom(function(html){
		var url = getUrlFromHtml(html);
		callback(request.get(url));
	});
}

exports.urlIn = getUrlFromHtml;
exports.urlOfToday = getUrlFromDilbertCom;
exports.imageOfToday = getImageFromDilbertCom;