var chai = require("chai")
var expect = chai.expect;
var fs = require("fs");

describe('DailyDilbert', function(){
  describe('#findUrl()', function(){
    
    it('should return the correct URL on 2013-10-18', function(done){
 		fs.readFile("test/dilbertcom_20131018.html", function (err, data) {
 			var dailyDilbertUrl = getDailyDilbertUrl(data);
			expect(dailyDilbertUrl).to.equal("http://dilbert.com/dyn/str_strip/000000000/00000000/0000000/100000/90000/9000/500/199595/199595.strip.gif");	
			done();
 		});
    })

  })
})

var getDailyDilbertUrl = function(html) {
	if (html.constructor !== String) {
		html = new String(html);
	}
	var regex = /title="Dilbert.com"><img src="(.*?)"/;
	return html.match(regex)[1];
}