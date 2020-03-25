﻿<?php

if ( isset($_POST["imageData"]) && !empty($_POST["imageData"]) ) {    
//define('UPLOAD_DIR', 'E:/LogiTeks/vMTS/vMTS/images/carousel/');
define('UPLOAD_DIR', 'E:/web/learntor/images/carousel/');

    // get the dataURL
    $dataURL = $_POST["imageData"];  
    $dataName = $_POST["imageName"];  

    // the dataURL has a prefix (mimetype+datatype) 
    // that we don't want, so strip that prefix off
    $parts = explode(',', $dataURL);  
    $data = $parts[1];  

    // Decode base64 data, resulting in an image
    $data = base64_decode($data);  

    // create a temporary unique file name
    $uid = uniqid();
    $file = UPLOAD_DIR . $dataName . '.jpg';
   
    // write the file to the upload directory
    $success = file_put_contents($file, $data);

    // return the temp file name (success)
    // or return an error message just to frustrate the user (kidding!)
    echo $success ? "Good" : "Fail";
    }else{
    echo "Fail";
    }
 ?> 