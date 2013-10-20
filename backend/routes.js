var director = require("director");
var dailyDilbert = require("./lib/dailydilbert.js");

var router = new director.http.Router({
	'/': {
		get: function() {
			var response = this.res;
			dailyDilbert.urlOfToday(function(url){
				response.writeHead(200, {"Content-Type": "text/html"});
		  		response.end(getIndexHtml(url))
  			});
		}
	},
	'/url': {
		get: function() {
			var response = this.res;
			dailyDilbert.urlOfToday(function(url){
				response.writeHead(200, { 'Content-Type': 'text/plain' })
		    	response.end(url);
			});
		}
	},
	'/image': {
		get: function() {
			var response = this.res;
			dailyDilbert.imageOfToday(function(requested){
				requested.pipe(response);
			});
		}
	}
});

var dispatchRequest = function(req, res) {
	router.dispatch(req, res, function (err) {
		if (err) {
			res.writeHead(404, { 'Content-Type': 'text/plain' })
		    res.end("404");
		}
	});
}

exports.router = router;
exports.dispatch = dispatchRequest;



var getIndexHtml = function(url) {
	return ''+
		'<!DOCTYPE html>'+
		'<html>'+
			'<head>'+
			'</head>'+
			'<body bgcolor="#FF0080">'+
				'<div style="width:200px; height:100px; position:absolute; margin-top:-80px; margin-left:-100px; top:50%; left:50%;">'+
					'<h1>Daily Dilbert</h1>'+
					'<div>'+
						'get the <a href="/url">url</a>'+
					'</div>'+
					'<div>'+
						'get the <a href="/image">image</a>'+
					'</div>'+
				'</div>'+
				'<div style="width:100%; position:fixed; bottom:0px">'+
					'This website proxies the daily dilbert image from <a href="http://dilbert.com/">dilbert.com</a>. All images are copyrighted by their respective owners. No images, ip addresses or anything at all are stored. This proxy is copyrighted by <a href="http://njks.de">Manuel Naujoks</a>.'+
				'</div>'+
			'</body>'+
		'</html>';
}