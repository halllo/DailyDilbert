var http = require("http");

var dailyDilbert = require("./lib/dailydilbert.js");

http.createServer(function (req, res) {

	dailyDilbert.urlOfToday(function(url){
		res.writeHead(200, {"Content-Type": "text/html"});
  		res.write(""+
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
			'</html>');
  		res.end()
  	});

}).listen(process.env.VMC_APP_PORT || process.env.PORT || 1337);