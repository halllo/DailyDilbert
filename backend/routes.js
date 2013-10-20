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
			'<body bgcolor="#FF69B4">'+
				'<div style="width:200px; margin:0 auto;">'+
					'<h1>Daily Dilbert</h1>'+
					'<div>'+
						'get the <a href="/url">url</a>'+
					'</div>'+
					'<div>'+
						'get the <a href="/image">image</a>'+
					'</div>'+
				'</div>'+
			'</body>'+
		'</html>';
}