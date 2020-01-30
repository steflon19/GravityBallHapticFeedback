/* window.onload = function() {
     console.log("I AM READING THE SCRIPT");
    document.getElementById('bodytext').innerHTML = "I AM READING THE SCRIPT";
}; */

var preSurveyData = null;
var postSurveyData = null;
var uId = 'Unique ID';
var pId = 'ParticipantID';
var mrBaseFolder = '../data/project/fieldMR/';
var vrBaseFolder = '../data/project/labVR/';
let unifiedData = {};

// Load Survey Data and adapt format preprocessing
async function loadUnifiedData() {
    await d3.json('../data/unifiedData.json').then(function(data) {
        // Here we have now the complete JSON object with all data.
        // keys: sceneType,postSurveyData,preSurveyData,id,projectData
        console.log(Object.getOwnPropertyNames(data[0]) + ' - ' + data[0].id + ' - ' + Object.getOwnPropertyNames(data[0]['preSurveyData'])) //  sample access
        // Find keys of [object Object] with Object.getOwnPropertyNames(data[0]['preSurveyData'])
        // e.g. do stuff  with for (entry in data[0]['preSurveyData']) { ... }
        // Haven't thought about how to properly do that yet.
        //console.log(data[1].length)  // first row of postSurvey
    });
}

loadUnifiedData().then(function () {
    // Data loaded and everything in previous function done. Further steps here?
});
