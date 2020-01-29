console.log("START: Hello there. General Kenobi..");

window.onload = function() {
     console.log("I AM READING THE SCRIPT");
    document.getElementById('bodytext').innerHTML = "I AM READING THE SCRIPT";
};

// Load Survey Data
d3.csv("/data/pre_survey.csv").then(function(data) {
  console.log(data[0]);
});

// Load Project Data
/*Promise.all([
  d3.csv("/data/cities.csv"),
  d3.tsv("/data/animals.tsv")
]).then(function(data) {
  console.log(data[0][0])  // first row of cities
  console.log(data[1][0])  // first row of animals
});*/

// Combine Survey and Project Data