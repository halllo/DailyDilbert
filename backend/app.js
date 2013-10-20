var http = require("http"),
	director = require("director");

var dailyDilbert = require("./lib/dailydilbert.js");


var router = new director.http.Router();

router.get(/url/, function() {
	var response = this.res;
	dailyDilbert.urlOfToday(function(url){
		response.writeHead(200, { 'Content-Type': 'text/plain' })
    	response.end(url);
	});
});

http.createServer(function (req, res) {
	router.dispatch(req, res, function (err) {
		if (err) {
			dailyDilbert.urlOfToday(function(url){
				res.writeHead(200, {"Content-Type": "text/html"});
		  		res.write(getIndexHtml(url));
		  		res.end()
  			});
		}
	});
}).listen(process.env.VMC_APP_PORT || process.env.PORT || 1337);


var getIndexHtml = function(url) {
	return ''+
		'<!DOCTYPE html>'+
		'<html>'+
			'<head>'+
			'</head>'+
			'<body>'+
				'<div class="container">'+
					'<h1>Daily Dilbert</h1>'+
					'<div>'+
						'url: <a href="' + url + '">' + url + '</a>'+
					'</div>'+
					'<div>'+
						'<img src="' + url + '"></img>'+
					'</div>'+
				'</div>'+
			'</body>'+
		'</html>';
}