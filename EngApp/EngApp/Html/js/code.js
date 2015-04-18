// global variables, my programming professor would kill me for this
var answers = [];
var step = 0;
var topic = -1;
var letter = "";
var skips = 0;
var number = 0;
var currentPage = "menu";
var highscores = {"max":"-","min":"-","sum":"0", "tot":"0" };
var categories = categories_en;
var animation = false;
var totalgames = 0;
var totalpoints = 0;
var maxpoints = -999;
var minpoints = 999;


function cellBackButtonPressed(p) {

	if (p != "counter" && !animation) {
		if (p != "game") {
			changeView("menu");
		} else {
			if (confirm("Are you sure you want to leave the game?")) {
				$("#countline").stop(false,false);
				changeView("menu");
			}
		}
	}

}

function resetSettings() {

	if (confirm("Are you sure you want to delete your records?")) {

		highscores = {"max":"-","min":"-","sum":"0", "tot":"0"};
		
		totalgames = 0;
		totalpoints = 0;
		maxpoints = -999;
		minpoints = 999;

		window.external.notify('saveHighscores={"max":"-","min":"-","sum":"0", "tot":"0"}');

		displayScores();
	}
}

function loadHighscores() {
	window.external.notify("loadHighscores");
}

function loadSettings() {
	window.external.notify("loadSettings");
}

function displayScores() {
	
	if (highscores.tot/1 == 0) {
		highscores.avg = "-";
	} else {
		highscores.avg = Math.round(highscores.sum / highscores.tot);
	}

	$("#highscore_value_max").text("");
	if (highscores.max == "-") {
		$("#highscore_value_max").append("<img src='./images/minus.png' alt='' />");
	} else {
		var aux = highscores.max /1;
		if (highscores.max < 0) {
			$("#highscore_value_max").append("<img src='./images/minus.png' alt='' />");
			aux = -aux;
		}
		var cents = Math.floor(aux /100);
		var decs = Math.floor((aux - (cents * 100))/10);
		var units =  Math.floor(aux % 10);
		if (cents > 0) { $("#highscore_value_max").append("<img src='./images/" + cents + ".png' alt='' />"); }
		if (decs > 0 || cents > 0) { $("#highscore_value_max").append("<img src='./images/" + decs + ".png' alt='' />"); }
		$("#highscore_value_max").append("<img src='./images/" + units + ".png' alt='' />");
	}
	
	$("#highscore_value_min").text("");
	if (highscores.min == "-") {
		$("#highscore_value_min").append("<img src='./images/minus.png' alt='' />");
	} else {
		var aux = highscores.min /1;
		if (highscores.min < 0) {
			$("#highscore_value_min").append("<img src='./images/minus.png' alt='' />");
			aux = -aux;
		}
		var cents = Math.floor(aux /100);
		var decs = Math.floor((aux - (cents * 100))/10);
		var units =  Math.floor(aux % 10);
		if (cents > 0) { $("#highscore_value_min").append("<img src='./images/" + cents + ".png' alt='' />"); }
		if (decs > 0 || cents > 0) { $("#highscore_value_min").append("<img src='./images/" + decs + ".png' alt='' />"); }
		$("#highscore_value_min").append("<img src='./images/" + units + ".png' alt='' />");
	}
	
	$("#highscore_value_avg").text("");
	if (highscores.avg == "-") {
		$("#highscore_value_avg").append("<img src='./images/minus.png' alt='' />");
	} else {
		var aux = highscores.avg /1;
		if (highscores.avg < 0) {
			$("#highscore_value_avg").append("<img src='./images/minus.png' alt='' />");
			aux = -aux;
		}
		var cents = Math.floor(aux /100);
		var decs = Math.floor((aux - (cents * 100))/10);
		var units =  Math.floor(aux % 10);
		if (cents > 0) { $("#highscore_value_avg").append("<img src='./images/" + cents + ".png' alt='' />"); }
		if (decs > 0 || cents > 0) { $("#highscore_value_avg").append("<img src='./images/" + decs + ".png' alt='' />"); }
		$("#highscore_value_avg").append("<img src='./images/" + units + ".png' alt='' />");
	}
	
	
	
	
}

function changeLanguageToNew(language) {

	if (language == "en") {
		$(".flags.active").removeClass("active");
		$(".flag_" + language).addClass("active");
		eval("categories = categories_" + language);
		window.external.notify('saveLanguage=' + language);	
	} 

}

function changeMusicToNew(music) {

	if (music == "on") {
		$("#settings_music").attr("src", "./images/settings_sound_on.png");
		$("#gamemusic").trigger("play");
		window.external.notify('saveMusic=on');	
	} else if (music == "off") {
		$("#settings_music").attr("src", "./images/settings_sound_off.png");
		$("#gamemusic").trigger("pause");
		window.external.notify('saveMusic=off');	
	}

}

function SettingsLoaded(language, music) {

	changeLanguageToNew(language);
	changeMusicToNew(music);
}

function HighscoresLoaded(text) {

	if (text == "") {
		// no highscores
		// initialize scores to blank
		window.external.notify('saveHighscores={"max":"-","min":"-","sum":"0", "tot":"0"}');		 
		highscores = {"max":"-","min":"-","sum":"0", "tot":"0"};
	} else {
		// highscores saved :)
		highscores = jQuery.parseJSON(text);
	}

	totalgames = highscores.tot;
	totalpoints = highscores.sum;
	maxpoints = highscores.max;
	minpoints = highscores.min;
	

	displayScores();

}

function changeView(fname) {

	// display the correct frame
	$(".viewframe").hide();
	$("#" + fname).show();

	// save the name of the frame
	currentPage = fname;
	window.external.notify("frameChange=" + currentPage);

	switch(fname) {

		case "game":
			initializeGame();
			break;

		case "resultsframe":
			
			//$("#word_boxes").text(answers.length);
			writeResults();

			$("#word_boxes_frame").css({ top: 0, height: $("#word_boxes").height()*2 });
			$("#word_boxes").css("top", 0);

			break;
		case "counter":
			$("#count3").fadeIn(250, function() {
				setTimeout(function() {
					$("#count3").fadeOut(250, function() {
						setTimeout(function() {
							
							$("#count2").fadeIn(250, function() {
								setTimeout(function() {
									$("#count2").fadeOut(250, function() {
										setTimeout(function() {
							
											$("#count1").fadeIn(250, function() {
												setTimeout(function() {
													$("#count1").fadeOut(250, function() {
														setTimeout(function() {
							
															// initialize values
															$("#textbox_text").text("");

															// display game screen
															changeView("game");

															

														}, 200);
													});
												}, 500);
											});


										}, 200);
									});
								}, 500);
							});


						}, 200);
					});
				}, 500);
			});
			break;

	}

}





function prepare_results() {

	changeView("resultsframe");

}

function keyboard_pressed(l) {
	
	if (l.length == 1) {

		$("#textbox_text").text($("#textbox_text").text() + l);

	} else {

		// depending on the special button
		switch (l) {
			case "REM":
				$("#textbox_text").text("");
				break;
			case "DEL":
				$("#textbox_text").text( $("#textbox_text").text().substring(0, $("#textbox_text").text().length-1) );
				break;
			case "SKIP":
				skipValue();
				break;
			case "SEND":
				newAnswer();
				break;
			case "BIG_ARROW":
				if ($("#textbox_text").text().length > 0) {
					newAnswer();
				} else {
					skipValue();
				}
				break;
		}

	}

}

function initializeGame () {
		
	// reset game variables
	answers = [];
	step = 0;
	topic = -1;
	letter = "";
	skips = 0;
	number = 0;

	// set the field as blank
	$("#textbox_text").text("");

	// initialize timer
	$("#countline").css({width:"100%", backgroundColor:"#00ff00"}).animate(
		{backgroundColor:"#ff0000", width:0},
		90000, /****AMD*****/
		"linear",
		function () {
			prepare_results();
		}
	);

	// generate round
	generateRound();
		
}

function generateRound() {

	var randLevel = Math.floor(Math.random() * categories.length);
	var randWord  = Math.floor(Math.random() * categories[randLevel].values.length);
	
	letter = categories[randLevel].values[randWord].toLowerCase().charAt(0);
	topic  = randLevel;
	number++;
		
	//styling things
	$("#textbox_text").text("");
	$("#category_name_text").text(categories[topic].name);
	$("#game_letter").css("background-image", "url('./images/" + letter.toLowerCase() + ".png')");
	
}

function newAnswer() {
	if (step == 0) {
		
		answers.push({
			"topic":topic,
			"letter":letter,
			"answer": $("#textbox_text").text()
		});
	}
		
	generateRound();
}

function skipValue() {
	skips++;
	$("#textbox_text").text("")
	newAnswer();
}

function checkWord(word, topic, letter) {
		
	if (word.length < 1) { return 0; }
	var difference = 100;
	var position = -1;
	var x = 0;
	var found = false;
		
	while (x < categories[topic].values.length && !found) {
		
		var aux = fuzzySearch(word.toLowerCase(), categories[topic].values[x].toLowerCase());
		var tdw = word.length/1 + categories[topic].values[x].length/1;
		var dif = Math.round(aux.distance * 100 / tdw);
		
		if (dif <= difference) {
			difference = dif;
			position = x;
		}
		
		// advance if not found
		if (dif/1 == 0 && (word.length/1 < categories[topic].values[x].length/1 + 2 || word.length/1 + 2 > categories[topic].values[x].length/1) && letter.toLowerCase() == categories[topic].values[x].toLowerCase().charAt(0).toLowerCase()) { 
			found = true; 
		} else { 
			x++; 
		}

	}
		
	if (difference < 15 && (word.length/1 < categories[topic].values[position].length/1 + 2 || word.length/1 + 2 > categories[topic].values[position].length/1) && letter.toLowerCase() == categories[topic].values[position].toLowerCase().charAt(0).toLowerCase()) {
			
		var sizeArray = categories[topic].values.length;
			
		if (position <= sizeArray * 0.5) {
			return 1;
		} else if (position <= sizeArray * 0.85) {
			return 2
		} else {
			return 3;
		}
			
	} else {
		return 0;
	}
}




function writeResults() {

	$("#word_boxes").text("");
	$("#points").css("display","none");
	
	var text = "";
	var good = 0;
	var skips = 0;
	var consecutive = 0;
	var totalResults = 0;
	var totalConsecutives = 0;
	var arrayElementsNew = [];
		
	for (x = 0; x < answers.length; x++) {
		
		if (answers[x].answer != "") {
			var aux = checkWord(answers[x].answer.toLowerCase(), answers[x].topic, answers[x].letter.toLowerCase());
			if (aux > 0) {
				good = good/1 + aux/1;
				consecutive++;
				arrayElementsNew.push({ color:"green", type:"correct", points:aux, category:categories[answers[x].topic].name + " - " + answers[x].letter.toUpperCase(), answer: answers[x].answer });
					
				// bonus 5 consecutive corrects
				if (consecutive%5 == 0) {
					totalConsecutives++;
					totalResults++;
					arrayElementsNew.push({ color:"ESPECIAL", points:consecutive });
				}
					
			} else {
				consecutive = 0;
				arrayElementsNew.push({ color:"red", points:aux, type:"incorrect", category:categories[answers[x].topic].name + " - " + answers[x].letter.toUpperCase(), answer: answers[x].answer });
			}
		} else {
			consecutive = 0;
			skips++;
			arrayElementsNew.push({ color:"red", points:-1, type:"skipped", category:categories[answers[x].topic].name + " - " + answers[x].letter.toUpperCase(), answer: "SKIPPED" });
		}
			
		totalResults++;
		
	}
		
	// new way of calculating points
	var totalPointsRound = (good-skips);
		
	// reverse so they show up in order
	arrayElementsNew = arrayElementsNew.reverse();
		
	// add the elements to the view and render
	for (x = 0; x < arrayElementsNew.length; x++) {
		
		if (arrayElementsNew[x].color == "ESPECIAL") {
			$("#word_boxes").append('<div class="wordresult bonus wordResult' + x + '"><img src="./images/wordresult_bonus' + arrayElementsNew[x].points + '_76.png" /></div>');
			totalPointsRound = totalPointsRound/1 + arrayElementsNew[x].points;
		} else {
			$("#word_boxes").append('<div class="wordresult wordResult' + x + ' ' + arrayElementsNew[x].type + '"><div class="points">' + arrayElementsNew[x].points + '</div><div class="category">' + arrayElementsNew[x].category + '</div><div class="answer">' + arrayElementsNew[x].answer + '</div></div>');
		}
	}
		

	if (answers.length > 0) {
		animation = true;
		animateResults(totalResults-1);
	}
		
	if (totalPointsRound > maxpoints/1 || maxpoints == "-") {
		maxpoints = totalPointsRound;
		//enyo.setCookie("max",totalPointsRound);
	}
		
	if (totalPointsRound < minpoints/1 || minpoints == "-") {
		minpoints = totalPointsRound;
		//enyo.setCookie("min",totalPointsRound);
	}
		
	
		
	totalgames++;
	totalpoints = totalpoints/1 + totalPointsRound;

	highscores.max = maxpoints;
	highscores.min = minpoints;
	highscores.sum = totalpoints;
	highscores.tot = totalgames;
	

	displayScores();
	
	window.external.notify('saveHighscores={"max":"' + maxpoints + '","min":"' + minpoints + '","sum":"' + totalpoints + '", "tot":"' + totalgames  + '" }');
		
	
	

	var aux = totalPointsRound;
	
	$("#points").hide();
	$(".pointtype").hide(); 
	
	if (aux < 0) { 
		aux = -totalPointsRound;
		$("#pointsn").show();
	}

	var cents = Math.floor(aux/100);
	var decs = Math.floor((aux - (cents * 100))/10);
	var units =  Math.floor(aux%10);

	if (cents > 0) {
		$("#points0").attr("src", "./images/" + cents + ".png").show();
	}
	if (cents > 0 || decs > 0) {
		$("#points1").attr("src", "./images/" + decs + ".png").show();
	}
	$("#points2").attr("src", "./images/" + units + ".png").show();
	
}

function animateResults(num) {

	if (num == "ALL") {
		jQuery(".wordResult").fadeIn(1,
			function() {
				//$("#points").show();
				setTimeout('jQuery("#points").fadeIn(1000)', 512);
			}
		);	
	} else {
		
		if (num/1 < 0) {
			jQuery("#points").fadeIn(1000);
			animation = false;
		} else {
			jQuery(".wordResult" + num).fadeIn(
				256,
				function() {
					setTimeout('animateResults("' + (num/1-1) + '")', 512);
				}
			);
		}
	}
}


/*************************************/

function changeSettingMusic() {

	if ($("#settings_music").attr("src").indexOf("sound_off") > 0) {
		// music off => on
		changeMusicToNew("on") ;
	} else {
		// music on => off
		changeMusicToNew("off") ;
	}

}

/*************************************/


$(document).ready(function() {

	if ($(window).height() > 810) {
		$("#results").css("top", 200);
	} else {
		$("#results").css({ top: 235, height:440 });
	}

	// help scroller behaviour
	$("#word_boxes").draggable({ 
		axis: "y",
		stop: function() {
			var actHeight = $("#word_boxes").height();
			var posTop = $("#word_boxes").offset().top;
			
			if (posTop > $(window).height() - $("#word_boxes_box").height()) {
				$("#word_boxes").stop(false,false).animate( {top:0}, 200 ); 
			} else {
				if (posTop < 500 - actHeight) {
					if (actHeight < 500) {
						$("#word_boxes").stop(false,false).animate( {top:0}, 200 ); 
					} else {
						$("#word_boxes").stop(false,false).animate( {top: 500 - actHeight }, 200 ); 
					}
				}
			}
		}
	 });

	loadSettings();
	loadHighscores();


});