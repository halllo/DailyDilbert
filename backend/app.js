var http = require('http');

http.createServer(function (req, res) {

  res.writeHead(200, {'Content-Type': 'text/plain'});
  res.end('Hello World\n');

}).listen(process.env.VMC_APP_PORT || process.env.PORT || 1337);