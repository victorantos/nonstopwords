$(function(){
	
    var ts = new Date(2012, 0, 1),
        untilGameIsOver = true;
	 
	
	//if((new Date()) > ts){
		 
		ts =  0;
		untilGameIsOver = false;
	//}
		 

	$('#countdown').stopwatch({
		timestamp	: ts,
		callback	: function(days, hours, minutes, seconds){
		    
			 
		}
	});
	
});
