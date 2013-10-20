var request = require("request");

var regex = /title="Dilbert.com"><img src="(.*?)"/;
var getUrlFromHtml = function(html) {
	return html.match(regex)[1];
}

var getHtmlFromDilbertCom = function(callback) {
	request("http://dilbert.com", function (error, response, html) {
		if (!error && response.statusCode == 200) {
			callback(html);
		}
	})
}

var getUrlFromDilbertCom = function(callback) {
	getHtmlFromDilbertCom(function(html){
		var url = getUrlFromHtml(html);
		callback(url);
	});
}

exports.urlIn = getUrlFromHtml;
exports.urlOfToday = getUrlFromDilbertCom;