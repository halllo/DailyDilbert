var http = require("http");

var routes = require("./routes.js");

http.createServer(function (req, res) {

	routes.dispatch(req, res);
	
}).listen(process.env.VMC_APP_PORT || process.env.PORT || 1337);