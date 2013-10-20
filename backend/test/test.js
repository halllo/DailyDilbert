var chai = require("chai");
var expect = chai.expect;
var fs = require("fs");

var dailyDilbert = require("../lib/dailydilbert.js");

describe("dailyDilbert", function(){
	describe("urlIn()", function(){
		it("should return the correct URL on 2013-10-18", function(done){
			expectDailyDilbertUrl(
				"test/data/dilbertcom_20131018.html",
				"http://dilbert.com/dyn/str_strip/000000000/00000000/0000000/100000/90000/9000/500/199595/199595.strip.gif",
				done);
		})
		it("should return the correct URL on 2013-10-20", function(done){
			expectDailyDilbertUrl(
				"test/data/dilbertcom_20131020.html",
				"http://dilbert.com/dyn/str_strip/000000000/00000000/0000000/100000/90000/6000/000/196048/196048.strip.sunday.gif",
				done);
		})
	})
})


var expectDailyDilbertUrl = function(htmlFile, expectedUrl, done) {
	fs.readFile(htmlFile, function (err, data) {
		var html = new String(data);
		expect(dailyDilbert.urlIn(html)).to.equal(expectedUrl);
		done();
	});
}