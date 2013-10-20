var chai = require("chai");
var expect = chai.expect;

var dailyDilbert = require("../lib/dailydilbert.js");

describe("INTEGRATIONTEST", function(){
	describe("daily dilbert url", function(){
		it("should be a gif", function(done){
			dailyDilbert.urlOfToday(function(url){
				expect(url).to.match(/http:\/\/.*?.gif/);
				done();
			});
		})
	})
})