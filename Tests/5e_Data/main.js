const fs = require('fs-extra')
const replaceAll = require('string.prototype.replaceall');

const datafolder = 'data';
const outputfolder = 'output_data'; 
//main2();

splitjson(datafolder, outputfolder)

function splitjson(datalocation, outputlocation){
  fs.readdir(datalocation, { withFileTypes: true }, (err, files) => {
    if (err) {
      console.trace("Here I am!");
      throw err;
      
    }
    files.forEach(file => {
      if (file.isDirectory()) {
        var tempData = `${datalocation}/${file.name}`;
        var tempLocation = `${outputlocation}/${file.name}`;
        splitjson(tempData, tempLocation);
        
      } else if(file.name.endsWith(".json")){
        fs.readFile(`./${datalocation}/${file.name}`, 'utf8', function(err, data) {
          if (err) throw err;
  
          var filename = file.name.replace(".json", "");
          //console.log('OK: ' + file.name);
          
          temp = JSON.parse(data);
          
          for (const [key, value] of Object.entries(temp)) {
            //console.log(`${key}:`);
            if(Array.isArray(value)){
              value.forEach(entry => { 
                //console.log(entry);
                try{
                  var firstentry = Object.entries(entry)[0][1];
                  var naam = firstentry.replace(/[\<\>\:\"\/\\\|\?\*\s]/g , "_")
                  var location = `${outputlocation}/${filename}/${key}/${naam}.json`.toLowerCase();
                  var output = JSON.stringify(entry, null, "\t");
                  fs.outputFileSync(location, output);
                } catch (e) {
                  console.warn(`FAILED ${datalocation}/${file.name}`);
                }     
              });
            }
          }
          
        });
      }
    });
  });

}

function main2(){

  fs.readdir(datafolder, { withFileTypes: true }, (err, files) => {
    console.log(files);
    files.forEach(file => {
      
      if (file.endsWith(".json")){
        fs.readFile(`./${datafolder}/${file}`, 'utf8', function(err, data) {
          if (err) throw err;
  
          var filename = file.replace(".json", "");
          //console.log('OK: ' + file);
          
          temp = JSON.parse(data);
          
          for (const [key, value] of Object.entries(temp)) {
            //console.log(`${key}:`);
            if(Array.isArray(value)){
              value.forEach(entry => { 
                var firstentry = Object.entries(entry)[0][1];
                var naam = firstentry.replace(/[\<\>\:\"\/\\\|\?\*\s]/g , "_")
                var location = `${outputfolder}/${filename}/${key}/${naam}.json`.toLowerCase();
                var output = JSON.stringify(entry, null, "\t");
                //console.log(entry);
                fs.outputFileSync(location, output);
              });
            }
          }
          
        });
      }
    });
  });

}

